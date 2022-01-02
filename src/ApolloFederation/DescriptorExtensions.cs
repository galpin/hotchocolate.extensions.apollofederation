using System;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to type system descriptors.
/// </summary>
public static partial class DescriptorExtensions
{
    internal static void SetContextData<T>(this IDescriptor<T> descriptor, string key, object? value)
        where T : DefinitionBase
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor.Extend().OnBeforeCreate(x => x.ContextData[key] = value);
    }
}