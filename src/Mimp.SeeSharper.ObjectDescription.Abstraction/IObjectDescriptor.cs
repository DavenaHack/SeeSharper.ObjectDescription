using System;

namespace Mimp.SeeSharper.ObjectDescription.Abstraction
{
    public interface IObjectDescriptor
    {


        public bool Describable(Type type, object? instance);

        public IObjectDescription Describe(Type type, object? instance);


    }
}
