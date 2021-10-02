using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public sealed class ObjectDescription : IObjectDescription
    {


        public bool HasValue { get; }

        public object? Value { get; }

        public IEnumerable<KeyValuePair<string?, IObjectDescription>>? Children { get; }


        public ObjectDescription(object? value)
        {
            HasValue = true;
            Value = value;
        }

        public ObjectDescription(IEnumerable<KeyValuePair<string?, IObjectDescription>> children)
        {
            HasValue = false;
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public ObjectDescription(IEnumerable<IObjectDescription> children)
            : this(children?.Select((v, i) => new KeyValuePair<string?, IObjectDescription>(i.ToString(), v))!) { }


        public override string? ToString()
        {
            string ToString(string? key) =>
                key is null ? "null" : $@"""{key}""";
            return HasValue ? 
                Value is string or null ? ToString((string?)Value) : Value.ToString()
                : "{ " + string.Join(", ", Children!.Select(c => $@"{ToString(c.Key)}: {c.Value}")) + " }";
        }


    }
}
