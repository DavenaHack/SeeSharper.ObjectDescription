using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription
{
    public class ConcatObjectDescription : IObjectDescription
    {


        public IObjectDescription Head { get; }

        public IObjectDescription Tail { get; }


        public bool HasValue => false;

        public object? Value => throw ObjectDescribeException.GetNonValueDescriptionException(this);

        public IEnumerable<KeyValuePair<string?, IObjectDescription>> Children =>
            (Head.HasValue ? new KeyValuePair<string?, IObjectDescription>[] {
                new KeyValuePair<string?, IObjectDescription>(ObjectDescriptions.ValueKey, new ConstantObjectDescription(Head.Value))
            } : Head.Children)
                .Concat(Tail.HasValue ? new KeyValuePair<string?, IObjectDescription>[] {
                    new KeyValuePair<string?, IObjectDescription>(ObjectDescriptions.ValueKey, new ConstantObjectDescription(Tail.Value))
                } : Tail.Children);


        public ConcatObjectDescription(IObjectDescription head, IObjectDescription tail)
        {
            Head = head ?? throw new ArgumentNullException(nameof(head));
            Tail = tail ?? throw new ArgumentNullException(nameof(tail));
        }


        public override string? ToString() =>
            this.Constant(true).ToString();


    }
}
