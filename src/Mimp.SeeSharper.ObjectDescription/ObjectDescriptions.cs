using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{
    public static class ObjectDescriptions
    {


        public static string? ValueKey => null;

        public static IObjectDescription NullDescription { get; }
            = Constant((object?)null);

        public static IObjectDescription EmptyDescription { get; }
            = new ConstantObjectDescription(Array.Empty<KeyValuePair<string?, IObjectDescription>>());


        public static IObjectDescription Constant(object? value) =>
            new ConstantObjectDescription(value);

        public static IObjectDescription Constant(IEnumerable<KeyValuePair<string?, IObjectDescription>> children) =>
            new ConstantObjectDescription(children ?? throw new ArgumentNullException(nameof(children)));

        public static IObjectDescription Constant(KeyValuePair<string?, IObjectDescription> descriptionPair) =>
            Constant(new[] { descriptionPair });

        public static IObjectDescription Constant(string? key, IObjectDescription description) =>
            Constant(new KeyValuePair<string?, IObjectDescription>(key, description));

        public static IObjectDescription Constant(string? key, object? value) =>
            Constant(key, Constant(value));


        public static bool Equals(IObjectDescription descA, IObjectDescription descB) =>
            Equals(descA, descB, Equals);


        public static bool EqualsValuesToString(IObjectDescription descA, IObjectDescription descB) =>
            Equals(descA, descB, (a, b) => Equals(a?.ToString(), b?.ToString()));


        public static bool Equals(IObjectDescription descA, IObjectDescription descB, Func<object?, object?, bool> valueEquals)
        {
            if (descA is null)
                throw new ArgumentNullException(nameof(descA));
            if (descB is null)
                throw new ArgumentNullException(nameof(descB));
            if (valueEquals is null)
                throw new ArgumentNullException(nameof(valueEquals));

            if (ReferenceEquals(descA, descB))
                return true;
            if (descA.HasValue != descB.HasValue)
                return false;

            if (descA.HasValue)
                return valueEquals(descA.Value, descB.Value);

            var valuesA = descA.Children.ToList();
            var valuesB = descB.Children.ToList();

            foreach (var pairA in valuesA)
            {
                var pairBs = valuesB.Where(pairB => pairB.Key == pairA.Key);
                if (pairBs.Any())
                {
                    var pairB = pairBs.First();
                    valuesB.Remove(pairB);
                    if (!Equals(pairA.Value, pairB.Value, valueEquals))
                        return false;
                }
                else
                {
                    if (!Equals(pairA.Value, NullDescription, valueEquals))
                        return false;
                }
            }
            foreach (var pairB in valuesB)
                if (!Equals(NullDescription, pairB.Value, valueEquals))
                    return false;

            return true;
        }


    }
}
