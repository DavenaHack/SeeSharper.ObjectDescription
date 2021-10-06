using System;

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{

    [Serializable]
    public class ObjectDescribeException : Exception
    {


        public ObjectDescribeException() { }

        public ObjectDescribeException(string? message)
            : base(message) { }

        public ObjectDescribeException(string? message, Exception? inner)
            : base(message, inner) { }

        protected ObjectDescribeException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context
            ) : base(info, context) { }


        public static InvalidOperationException GetValueDescriptionException(IObjectDescription description) =>
            new InvalidOperationException($"{description} is a value description!");

        public static InvalidOperationException GetNonValueDescriptionException(IObjectDescription description) =>
            new InvalidOperationException($"{description} is a non value description!");


    }
}
