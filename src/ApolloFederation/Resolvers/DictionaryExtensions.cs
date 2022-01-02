using System;
using System.Collections.Generic;

namespace HotChocolate.Extensions.ApolloFederation;

/// <summary>
/// Provides extension methods to support interacting with entity representations.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Tries to get the value associated with the specified <see cref="key"/> in the dictionary.
    /// </summary>
    /// <param name="source">The dictionary.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <returns>
    /// The value associated with <paramref name="key"/>, cast to <typeparamref name="TValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="key"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// <paramref name="key"/> was not found.
    /// </exception>
    public static TValue GetValue<TValue>(this IReadOnlyDictionary<string, object?> source, string key)
    {
        if (!source.TryGetValue(key, out var result))
        {
            throw new KeyNotFoundException(key);
        }
        return (TValue)result!;
    }

    /// <summary>
    /// Tries to get the value associated with the specified <see cref="key"/> in the dictionary.
    /// </summary>
    /// <param name="source">The dictionary.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <returns>
    /// When successful, the value associated with <paramref name="key"/>, safely casted to
    /// <typeparamref name="TValue"/>; otherwise the <see langword="default"/> value of <typeparamref name="TValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="key"/> is <see langword="null"/>.
    /// </exception>
    public static TValue? GetValueOrDefault<TValue>(this IReadOnlyDictionary<string, object?> source, string key)
    {
        return !source.TryGetValue(key, out var untyped) ? default : (TValue)untyped!;
    }

    internal static bool TryGetValue<TResult>(
        this IDictionary<string, object?> source,
        string key,
        out TResult? result)
    {
        if (source.TryGetValue(key, out var untyped))
        {
            result = (TResult?)untyped;
            return true;
        }
        result = default;
        return false;
    }
}