using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigor.Functional.Option
{
    public static class Optional
    {
        public static Option<T> Some<T>(T value) => new SomeClass<T>(value);
        public static Option<T> None<T>() => new NoneClass<T>();
    }

    public interface Option<out T>
    {
        public Option<U> Map<U>(Func<T, U> func);
        public Option<U> FlatMap<U>(Func<T, Option<U>> func);
        public U Match<U>(Func<T, U> some, Func<U> none);
        public void MatchSome(Action<T> some);
        public void Match(Action<T> some, Action none);
        public bool HasValue { get; }

        public T ValueOrFailure();
    }

    public abstract record OptionBase<T>(bool HasValue) : Option<T>
    {
        public abstract Option<U> Map<U>(Func<T, U> func);
        public abstract Option<U> FlatMap<U>(Func<T, Option<U>> func);
        public abstract U Match<U>(Func<T, U> some, Func<U> none);
        public abstract void MatchSome(Action<T> some);
        public abstract void Match(Action<T> some, Action none);
        public abstract T ValueOrFailure();
    }

    public interface Some<out T>: Option<T>
    {
        T Value { get; }
    }

    public record SomeClass<T>(T Value): OptionBase<T>(true), Some<T>
    {
        public override Option<U> Map<U>(Func<T, U> func) => new SomeClass<U>(func(Value));
        public override Option<U> FlatMap<U>(Func<T, Option<U>> func) => func(Value);
        public override U Match<U>(Func<T, U> some, Func<U> none) => some(Value);
        public override void MatchSome(Action<T> some) => some(Value);

        public override void Match(Action<T> some, Action none) => some(Value);
        public override T ValueOrFailure() => Value;
    }

    public interface None<out T> : Option<T>
    {

    }

    public record NoneClass<T>() : OptionBase<T>(false)
    {
        public override Option<U> Map<U>(Func<T, U> func) => new NoneClass<U>();
        public override Option<U> FlatMap<U>(Func<T, Option<U>> func) => new NoneClass<U>();
        public override U Match<U>(Func<T, U> some, Func<U> none) => none();
        public override void MatchSome(Action<T> some) { /* do nothing */ }

        public override void Match(Action<T> some, Action none) => none();
        public override T ValueOrFailure() => throw new Exception("None has no value");
        
    }

    public static class OptionalExtensions
    {
        public static Option<T> SomeNotNull<T>(this T? value) where T : class
        {
            return value == null ? Optional.None<T>() : Optional.Some(value);
        }

        public static Option<T> When<T>(this Option<T> option, Func<T, bool> pred)
        {
            return option.HasValue && pred(option.ValueOrFailure()) ? option : Optional.None<T>();
        }

        public static void MatchSome<T>(this Option<T> option, Action<T> action)
        {
            if (option.HasValue)
                action(option.ValueOrFailure());
        }
        public static T? ValueOrDefault<T>(this Option<T> option, T? @default = default)
        {
            if (option.HasValue)
                return ((Some<T>)option).Value;
            return @default;
        }


        public static Option<T> Flatten<T>(this Option<Option<T>> option) =>
            option.FlatMap(innerOption => innerOption);

        // ReSharper disable once PossibleMultipleEnumeration
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> collection)
        {
            var firstValue = collection.FirstOrDefault();
            if (firstValue == null)
                return Optional.None<T>();
            return Optional.Some(firstValue);
        }

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> collection, Func<T, bool> pred)
        {
            var firstValue = collection.FirstOrDefault(pred);
            if (firstValue == null)
                return Optional.None<T>();
            return Optional.Some(firstValue);
        }
    }
}
