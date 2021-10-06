using System.Collections.Generic;

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{
    public interface IObjectDescription
    {


        public bool HasValue { get; }

        public object? Value { get; }

        public IEnumerable<KeyValuePair<string?, IObjectDescription>> Children { get; }


    }
}
