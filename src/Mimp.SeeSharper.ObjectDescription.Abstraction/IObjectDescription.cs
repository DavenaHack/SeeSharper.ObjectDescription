using System.Collections.Generic;
//#if NullableAttributes
//using System.Diagnostics.CodeAnalysis;
//#endif

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{
    public interface IObjectDescription
    {


        public bool HasValue { get; }

//#if NullableAttributes
//        [MemberNotNullWhen(true, nameof(HasValue))]
//#endif
        public object? Value { get; }

//#if NullableAttributes
//        [MemberNotNullWhen(false, nameof(HasValue))]
//#endif
        public IEnumerable<KeyValuePair<string?, IObjectDescription>>? Children { get; }


    }
}
