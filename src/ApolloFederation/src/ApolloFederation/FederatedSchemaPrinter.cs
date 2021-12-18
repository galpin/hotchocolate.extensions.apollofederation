using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Types;

namespace HotChocolate.Extensions.ApolloFederation;

internal sealed class FederatedSchemaPrinter : IFederatedSchemaPrinter
{
    public string Print(ISchema schema)
    {
        if (schema is null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        return SerializeSchema(schema).ToString();
    }

    private DocumentNode SerializeSchema(ISchema schema)
    {
        var typeDefinitions = GetSchemaTypes(schema)
            .Select(SerializeTypeDefinition)
            .Where(x => x is not null)
            .OfType<IDefinitionNode>()
            .ToList();
        return new DocumentNode(null, typeDefinitions);
    }

    private static IDefinitionNode? SerializeTypeDefinition(INamedType namedType)
    {
        return namedType switch
        {
            ObjectType type => SerializeObjectType(type),
            InterfaceType type => SerializeInterfaceType(type),
            InputObjectType type => SerializeInputObjectType(type),
            UnionType type => SerializeUnionType(type),
            EnumType type => SerializeEnumType(type),
            ScalarType type => SerializeScalarType(type),
            _ => throw new NotSupportedException()
        };
    }

    private static IDefinitionNode? SerializeObjectType(ObjectType objectType)
    {
        var directives = objectType.Directives.Select(SerializeDirective).ToList();
        var interfaces = objectType.Implements.Select(SerializeNamedType).ToList();
        var fields = objectType.Fields
            .Where(x => !x.IsIntrospectionField)
            .Where(x => !IsApolloTypeAddition(x.Type.NamedType()))
            .Select(SerializeObjectField)
            .ToList();
        if (fields.Count == 0)
        {
            return null;
        }
        return new ObjectTypeDefinitionNode(
            null,
            new NameNode(objectType.Name),
            SerializeDescription(objectType.Description),
            directives,
            interfaces,
            fields);
    }

    private static InterfaceTypeDefinitionNode SerializeInterfaceType(InterfaceType interfaceType)
    {
        var directives = interfaceType.Directives.Select(SerializeDirective).ToList();
        var fields = interfaceType.Fields.Select(SerializeObjectField).ToList();
        return new InterfaceTypeDefinitionNode(
            null,
            new NameNode(interfaceType.Name),
            SerializeDescription(interfaceType.Description),
            directives,
            Array.Empty<NamedTypeNode>(),
            fields);
    }

    private static InputObjectTypeDefinitionNode SerializeInputObjectType(InputObjectType inputObjectType)
    {
        var directives = inputObjectType.Directives.Select(SerializeDirective).ToList();
        var fields = inputObjectType.Fields.Select(SerializeInputField).ToList();
        return new InputObjectTypeDefinitionNode(
            null,
            new NameNode(inputObjectType.Name),
            SerializeDescription(inputObjectType.Description),
            directives,
            fields);
    }

    private static UnionTypeDefinitionNode SerializeUnionType(UnionType unionType)
    {
        var directives = unionType.Directives.Select(SerializeDirective).ToList();
        var types = unionType.Types.Values.Select(SerializeNamedType).ToList();
        return new UnionTypeDefinitionNode(
            null,
            new NameNode(unionType.Name),
            SerializeDescription(unionType.Description),
            directives,
            types);
    }

    private static EnumTypeDefinitionNode SerializeEnumType(EnumType enumType)
    {
        var directives = enumType.Directives.Select(SerializeDirective).ToList();
        var values = enumType.Values.Select(SerializeEnumValue).ToList();
        return new EnumTypeDefinitionNode(
            null,
            new NameNode(enumType.Name),
            SerializeDescription(enumType.Description),
            directives,
            values);
    }

    private static EnumValueDefinitionNode SerializeEnumValue(IEnumValue enumValue)
    {
        var directives = enumValue.Directives.Select(SerializeDirective).ToList();
        if (enumValue.IsDeprecated)
        {
            directives.Add(SerializeDeprecatedNode(enumValue.DeprecationReason));
        }
        return new EnumValueDefinitionNode(
            null,
            new NameNode(enumValue.Name),
            SerializeDescription(enumValue.Description),
            directives);
    }

    private static FieldDefinitionNode SerializeObjectField(IOutputField field)
    {
        var arguments = field.Arguments.Select(SerializeInputField).ToList();
        var directives = field.Directives.Select(SerializeDirective).ToList();
        if (field.IsDeprecated)
        {
            directives.Add(SerializeDeprecatedNode(field.DeprecationReason));
        }
        return new FieldDefinitionNode(
            null,
            new NameNode(field.Name),
            SerializeDescription(field.Description),
            arguments,
            SerializeType(field.Type),
            directives);
    }

    private static InputValueDefinitionNode SerializeInputField(IInputField field)
    {
        var directives = field.Directives.Select(SerializeDirective).ToList();
        return new InputValueDefinitionNode(
            null,
            new NameNode(field.Name),
            SerializeDescription(field.Description),
            SerializeType(field.Type),
            field.DefaultValue,
            directives);
    }

    private static IDefinitionNode SerializeScalarType(ScalarType scalar)
    {
        var directives = scalar.Directives.Select(SerializeDirective).ToList();
        return new ScalarTypeDefinitionNode(
            null,
            new NameNode(scalar.Name),
            SerializeDescription(scalar.Description),
            directives);
    }

    private static ITypeNode SerializeType(IType type)
    {
        return type switch
        {
            NonNullType x => new NonNullTypeNode(null, (INullableTypeNode)SerializeType(x.Type)),
            ListType x => new ListTypeNode(null, SerializeType(x.ElementType)),
            INamedType x => SerializeNamedType(x),
            _ => throw new NotSupportedException()
        };
    }

    private static IEnumerable<INamedType> GetSchemaTypes(ISchema schema)
    {
        return schema.Types
            .Where(type => !FederatedTypes.IsBuiltInOrFederatedType(type.Name))
            .OrderBy(x => x.Name.ToString(), StringComparer.Ordinal)
            .GroupBy(x => (int)x.Kind)
            .OrderBy(x => x.Key)
            .SelectMany(x => x);
    }

    private static bool IsApolloTypeAddition(INamedType type)
    {
        return type is EntityType or ServiceType or AnyType or FieldSetType;
    }

    private static NamedTypeNode SerializeNamedType(INamedType namedType)
    {
        return new NamedTypeNode(null, new NameNode(namedType.Name));
    }

    private static DirectiveNode SerializeDeprecatedNode(string? reason)
    {
        return new DirectiveNode(
            WellKnownDirectives.Deprecated,
            new ArgumentNode(
                "reason",
                reason ?? WellKnownDirectives.DeprecationDefaultReason));
    }

    private static DirectiveNode SerializeDirective(IDirective directiveType)
    {
        return directiveType.ToNode(removeNullArguments: true);
    }

    private static StringValueNode? SerializeDescription(string? description)
    {
        return string.IsNullOrWhiteSpace(description) ? null : new StringValueNode(description!);
    }
}