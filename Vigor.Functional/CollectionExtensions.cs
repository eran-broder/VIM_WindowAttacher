using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigor.Functional.Option;

namespace Vigor.Functional;
public static class MyCollectionExtensions
{
    public static IEnumerable<T> HeadTail<T>(T head, IEnumerable<T> tail) => new[] { head }.Concat(tail);
    public static IEnumerable<T> ConcatLast<T>(this IEnumerable<T> list, T last) => list.Concat(new[] { last });

    public static IEnumerable<T> FirstItems<T, U>(this IEnumerable<(T, U)> items) => items.Select(p => p.Item1);
    public static IEnumerable<U> SecondItems<T, U>(this IEnumerable<(T, U)> items) => items.Select(p => p.Item2);

    public static IEnumerable<(T, Z)> MapSeconds<T, U, Z>(this IEnumerable<(T, U)> items, Func<U, Z> mapFunc) =>
        items.Select(p => (p.Item1, mapFunc(p.Item2)));

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
            action(element);
    }
    public static IReadOnlySet<T> AsSet<T>(this IEnumerable<T> source)
    {
        return new HashSet<T>(source);
    }

    public static U GetOrSetIfEmpty<T, U>(this IDictionary<T, U> dict, T key, U defaultValueToSet)
    {
        if (!dict.ContainsKey(key))
            dict[key] = defaultValueToSet;
        return dict[key];
    }
    
    public static IReadOnlyDictionary<T, U> AsDict<T, U>(this IEnumerable<(T, U)> listOfValueTuples) where T : notnull => 
        listOfValueTuples.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

    public static IReadOnlyDictionary<T, U> AsDict<T, U>(this IEnumerable<KeyValuePair<T, U>> listOfValueTuples) where T : notnull =>
        listOfValueTuples.ToDictionary(tuple => tuple.Key, tuple => tuple.Value);

    public static IReadOnlyDictionary<U, T> Reverse<T, U>(this IReadOnlyDictionary<T, U> dict) where U : notnull =>
        dict.ToDictionary(pair => pair.Value, pair => pair.Key);

    public static IReadOnlyDictionary<T, U> Add<T, U>(this IReadOnlyDictionary<T, U> dict, T key, U value) where T : notnull
    {
        return dict.Append(new(key, value)).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static Option<T> GetOrNone<U, T>(this IReadOnlyDictionary<U, T> dict, U key)
    {
        if (dict.ContainsKey(key))
            return Optional.Some(dict[key]);
        return Optional.None<T>();
    }

    public static (IEnumerable<T>, IEnumerable<U>) Split<T, U>(this IEnumerable<(T, U)> enumerable)
    {
        var valueTuples = enumerable.ToList();
        return (valueTuples.Select(p => p.Item1), valueTuples.Select(p => p.Item2));
    }

    public static IEnumerable<Option<T>> OnlySomes<T>(this IEnumerable<Option<T>> enumerable) =>
        enumerable.Where(e => e.HasValue);
    
}
