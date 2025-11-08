using System;
using System.Collections.Generic;

public static class CollectionExt
{
    public static IEnumerable<U> SelectNotNull<T, U>(
        this IEnumerable<T> items, Func<T, U?> selector) where U : class
    {
        foreach (var item in items)
        {
            var mapped = selector(item);
            if (mapped != null) yield return mapped;
        }
    }
}