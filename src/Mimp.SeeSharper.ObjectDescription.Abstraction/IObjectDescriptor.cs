using System;

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{
    public interface IObjectDescriptor
    {


        public IObjectDescription Describe(Type type, object? instance);


    }
}
