using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public class FallbackObjectDescription : IObjectDescription
    {


        public IObjectDescription Source { get; }

        public IObjectDescription Fallback { get; }


        public bool HasValue => Source.HasValue;

        public object? Value => HasValue ? Source.Value ?? Fallback.Value
            : throw ObjectDescribeException.GetNonValueDescriptionException(this);

        public IEnumerable<KeyValuePair<string?, IObjectDescription>> Children
        {
            get
            {
                if (HasValue)
                    throw ObjectDescribeException.GetValueDescriptionException(this);

                var sources = Source.Children.ToList();
                var targets = Fallback.Children.ToList();

                foreach (var pairS in sources)
                {
                    var pairsT = targets.Where(pairT => pairT.Key == pairS.Key);
                    if (pairsT.Any())
                    {
                        var pairT = pairsT.First();
                        yield return new KeyValuePair<string?, IObjectDescription>(pairS.Key, pairS.Value.Fallback(pairT.Value));
                        targets.Remove(pairT);
                    }
                    else
                        yield return pairS;
                }
                foreach (var pairT in targets)
                    yield return pairT;
            }
        }


        public FallbackObjectDescription(IObjectDescription source, IObjectDescription fallback)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
        }


        public override string? ToString() =>
            this.Constant(true).ToString();


    }
}
