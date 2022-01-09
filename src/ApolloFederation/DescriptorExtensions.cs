using System;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extensions to type system descriptors.
/// </summary>
public static partial class DescriptorExtensions
{
    internal static void SetContextData<T>(this IDescriptor<T> descriptor, string key, object value)
        where T : DefinitionBase
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor.Extend().OnBeforeCreate(x => x.ContextData[key] = value);
    }

    internal static void AppendContextData<T, TValue>(this IDescriptor<T> descriptor, string key, TValue value)
        where T : DefinitionBase
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor.Extend().OnBeforeCreate(x =>
        {
            if (x.ContextData.TryGetValue<List<TValue>>(key, out var existing))
            {
                existing!.Add(value);
            }
            else
            {
                x.ContextData.Add(key, new List<TValue> { value });
            }
        });
    }
}