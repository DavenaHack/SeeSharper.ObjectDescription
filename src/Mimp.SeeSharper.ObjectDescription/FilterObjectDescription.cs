using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public class FilterObjectDescription : IObjectDescription
    {


        public IObjectDescription Source { get; }

        public Func<KeyValuePair<string?, IObjectDescription>, bool> Filter { get; }


        public bool HasValue => Source.HasValue;

        public object? Value => Filter(new KeyValuePair<string?, IObjectDescription>(ObjectDescriptions.ValueKey, new ConstantObjectDescription(Source.Value)))
            ? Source.Value : null;

        public IEnumerable<KeyValuePair<string?, IObjectDescription>> Children => Source.Children
            .Where(Filter);


        public FilterObjectDescription(IObjectDescription source, Func<KeyValuePair<string?, IObjectDescription>, bool> filter)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }


        public override string? ToString() =>
            this.Constant(true).ToString();


    }
}
