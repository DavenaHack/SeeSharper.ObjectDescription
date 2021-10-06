using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public sealed class ConstantObjectDescription : IObjectDescription
    {


        public bool HasValue { get; }


        private readonly object? _value;
        public object? Value => HasValue ? _value : throw new InvalidOperationException();


        private readonly IEnumerable<KeyValuePair<string?, IObjectDescription>>? _children;
        public IEnumerable<KeyValuePair<string?, IObjectDescription>> Children =>
            HasValue ? throw new InvalidOperationException() : _children!;


        public ConstantObjectDescription(object? value)
        {
            HasValue = true;
            _value = value;
        }

        public ConstantObjectDescription(IEnumerable<KeyValuePair<string?, IObjectDescription>> children)
        {
            HasValue = false;
            _children = children?.ToArray() ?? throw new ArgumentNullException(nameof(children));
        }

        public ConstantObjectDescription(IEnumerable<IObjectDescription> children)
            : this(children?.Select((v, i) => new KeyValuePair<string?, IObjectDescription>(i.ToString(), v))!) { }


        public override string? ToString()
        {
            static string ToString(string? key) =>
                key is null ? "null" : $@"""{key}""";
            return HasValue ?
                Value is string or null ? ToString((string?)Value) : Value.ToString()
                : "{ " + string.Join(", ", Children.Select(c => $@"{ToString(c.Key)}: {c.Value}")) + " }";
        }


    }
}
