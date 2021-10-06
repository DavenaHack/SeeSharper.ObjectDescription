using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System.Collections.Generic;

namespace Mimp.SeeSharper.ObjectDescription.Test
{
    [TestClass]
    public class ObjectDescriptionsTest
    {
        [TestMethod]
        public void TestEqualsValuesToString()
        {

            Assert.IsTrue(ObjectDescriptions.EqualsValuesToString(
                ObjectDescriptions.Constant(new List<KeyValuePair<string?, IObjectDescription>> {
                    new KeyValuePair<string?, IObjectDescription>("Value1", ObjectDescriptions.Constant(1)),
                    new KeyValuePair<string?, IObjectDescription>("Value2", ObjectDescriptions.NullDescription)
                }), ObjectDescriptions.Constant(new List<KeyValuePair<string?, IObjectDescription>> {
                    new KeyValuePair<string?, IObjectDescription>("Value1", ObjectDescriptions.Constant("1") ),
                    new KeyValuePair<string?, IObjectDescription>("Value3", ObjectDescriptions.Constant((object?)null) )
                })));

        }
    }
}
