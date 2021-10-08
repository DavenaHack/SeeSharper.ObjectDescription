using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public static class ObjectDescriptionExtensions
    {


        public static bool IsNull(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (ReferenceEquals(description, ObjectDescriptions.NullDescription))
                return true;

            if (description.HasValue)
                return description.Value is null;

            if (description.IsWrappedValue())
                return description.UnwrapValue().Value is null;

            return false;
        }

        public static bool IsEmpty(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (ReferenceEquals(description, ObjectDescriptions.EmptyDescription))
                return true;

            return !description.HasValue && !description.Children.Any();
        }

        public static bool IsNullOrEmpty(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            return description.IsEmpty() || description.IsNull();
        }

        public static bool IsWrappedValue(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (description.HasValue)
                return false;

            var children = description.Children;
            if (!children.Any())
                return false;

            if (children.Skip(1).Any())
                return false;
            if (children.First().Key != ObjectDescriptions.ValueKey)
                return false;

            return true;
        }

        public static IObjectDescription WrapValue(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (!description.HasValue)
            {
                if (description.IsWrappedValue())
                    return description;

                throw new ObjectDescribeException($"Can't wrap a non value description: {description}");
            }

            return ObjectDescriptions.Constant(ObjectDescriptions.ValueKey, description);
        }

        public static IObjectDescription UnwrapValue(this IObjectDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if (description.HasValue)
                return description;

            if (!description.IsWrappedValue())
                throw new ObjectDescribeException($"Can't unwrap description: {description}");

            return ObjectDescriptions.Constant(description.Children.First().Value.Value);
        }


        public static IObjectDescription Constant(this IObjectDescription description, bool recursive)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            if ((!recursive || description.HasValue) && description is ConstantObjectDescription)
                return description;

            if (description.HasValue)
                return ObjectDescriptions.Constant(description.Value);

            var children = description.Children;
            if (recursive)
                children = children.Select(c => new KeyValuePair<string?, IObjectDescription>(c.Key, c.Value.Constant(recursive)));

            return ObjectDescriptions.Constant(children);
        }

        public static IObjectDescription Constant(this IObjectDescription description) =>
            description.Constant(false);


        public static IObjectDescription Fallback(this IObjectDescription description, IObjectDescription fallback) =>
            new FallbackObjectDescription(description ?? throw new ArgumentNullException(nameof(description)),
                fallback ?? throw new ArgumentNullException(nameof(fallback)));


        public static IObjectDescription Filter(this IObjectDescription description, Func<KeyValuePair<string?, IObjectDescription>, bool> filter)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            return new FilterObjectDescription(description, filter);
        }

        public static IObjectDescription Filter(this IObjectDescription description, Func<string?, bool> filter)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            return description.Filter(pair => filter(pair.Key));
        }

        public static IObjectDescription Remove(this IObjectDescription description, Func<KeyValuePair<string?, IObjectDescription>, bool> remove)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (remove is null)
                throw new ArgumentNullException(nameof(remove));

            return description.Filter(pair => !remove(pair));
        }

        public static IObjectDescription Remove(this IObjectDescription description, Func<string?, bool> remove)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (remove is null)
                throw new ArgumentNullException(nameof(remove));

            return description.Filter(pair => !remove(pair.Key));
        }

        public static IObjectDescription Remove(this IObjectDescription description, IEnumerable<KeyValuePair<string?, IObjectDescription>> pairs)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            pairs = pairs.ToArray();
            if (!pairs.Any())
                return description;

            return description.Filter(pair => !pairs.Contains(pair));
        }

        public static IObjectDescription Remove(this IObjectDescription description, params KeyValuePair<string?, IObjectDescription>[] pairs) =>
            description.Remove((IEnumerable<KeyValuePair<string?, IObjectDescription>>)pairs);


        public static IObjectDescription Remove(this IObjectDescription description, IEnumerable<string?> keys, StringComparison comparison)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (keys is null)
                throw new ArgumentNullException(nameof(keys));

            keys = keys.ToArray();
            if (!keys.Any())
                return description;

            return description.Filter(a => !keys.Any(b => string.Equals(a, b, comparison)));
        }

        public static IObjectDescription Remove(this IObjectDescription description, IEnumerable<string?> keys) =>
            description.Remove(keys, StringComparison.InvariantCultureIgnoreCase);

        public static IObjectDescription Remove(this IObjectDescription description, params string?[] keys) =>
            description.Remove((IEnumerable<string?>)keys);

        public static IObjectDescription RemoveValue(this IObjectDescription description) =>
            description.Remove(ObjectDescriptions.ValueKey);


        public static IObjectDescription Concat(this IObjectDescription head, IObjectDescription tail) =>
            new ConcatObjectDescription(head ?? throw new ArgumentNullException(nameof(head)),
                tail ?? throw new ArgumentNullException(nameof(tail)));


        public static IObjectDescription Append(this IObjectDescription head, KeyValuePair<string?, IObjectDescription> descriptionPair)
        {
            if (head is null)
                throw new ArgumentNullException(nameof(head));

            return head.Concat(ObjectDescriptions.Constant(descriptionPair));
        }

        public static IObjectDescription Append(this IObjectDescription head, string? key, IObjectDescription value) =>
            head.Append(new KeyValuePair<string?, IObjectDescription>(key, value));

        public static IObjectDescription Append(this IObjectDescription head, string? key, object? value) =>
            head.Append(key, ObjectDescriptions.Constant(value));

        public static IObjectDescription Append(this IObjectDescription head, object? value) =>
            head.Append(ObjectDescriptions.ValueKey, value);


        public static IObjectDescription Prepend(this IObjectDescription tail, KeyValuePair<string?, IObjectDescription> descriptionPair)
        {
            if (tail is null)
                throw new ArgumentNullException(nameof(tail));

            return ObjectDescriptions.Constant(descriptionPair).Concat(tail);
        }

        public static IObjectDescription Prepend(this IObjectDescription tail, string? key, IObjectDescription value) =>
            tail.Prepend(new KeyValuePair<string?, IObjectDescription>(key, value));

        public static IObjectDescription Prepend(this IObjectDescription tail, string? key, object? value) =>
            tail.Prepend(key, ObjectDescriptions.Constant(value));

        public static IObjectDescription Prepend(this IObjectDescription head, object? value) =>
            head.Prepend(ObjectDescriptions.ValueKey, value);


    }
}
