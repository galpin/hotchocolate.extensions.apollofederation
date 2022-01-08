using System.Reflection;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

internal static class ContextExtensions
{
    public static NameString GetTypeName(this ITypeSystemObjectContext context, ITypeDefinition typeDefinition)
    {
        return typeDefinition.ExtendsType switch
        {
            null => typeDefinition.Name,
            _ => context.DescriptorContext.Naming.GetTypeName(typeDefinition.ExtendsType)
        };
    }

    public static NameString GetMemberName(this IDescriptorContext context, MemberInfo member, MemberKind kind)
    {
        return context.Naming.GetMemberName(member, kind);
    }
}