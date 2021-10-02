using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public class ObjectDescriptions
    {


        public static string? ValueKey => null;

        public static IObjectDescription NullDescription { get; }
            = new ObjectDescription((object?)null);


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

            var valuesA = descA.Children!.ToList();
            var valuesB = descB.Children!.ToList();

            foreach (var pairA in valuesA)
            {
                var pairBs = valuesB.Where(pairB => pairB.Key == pairA.Key);
                if (pairBs.Any())
                {
                    var pairB = pairBs.First();
                    valuesB.Remove(pairB);
                    if (!Equals(pairA.Value, pairB.Value))
                        return false;
                }
                else
                {
                    if (!Equals(pairA.Value, NullDescription))
                        return false;
                }
            }
            foreach (var pairB in valuesB)
                if (!Equals(NullDescription, pairB.Value))
                    return false;

            return true;
        }


    }
}
