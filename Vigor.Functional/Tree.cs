using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigor.Functional;

public record Tree<T>(T Value, IEnumerable<Tree<T>> Children);

public record BidirectionalTree<T>(T Value, IEnumerable<BidirectionalTree<T>> Children, BidirectionalTree<T> Parent);

public static class TreeUtils
{
    public static Tree<T> Create<T,U>(U seed, Func<U, T> valueExtractor, Func<U, IEnumerable<U>> childrenExtractor)
    {
        return new Tree<T>(valueExtractor(seed),
            childrenExtractor(seed).Select(child => Create(child, valueExtractor, childrenExtractor)));
    }

    public static IEnumerable<T> Flatten<T>(Tree<T> tree)
    {
        yield return tree.Value;
        foreach (var next in tree.Children.SelectMany(Flatten))
        {
            yield return next;
        }
    }

    public static Tree<U> Map<T, U>(Tree<T> tree, Func<T, U> map)
    {
        return TreeUtils.Create(tree, t => map(t.Value), t => t.Children);
    }
}



