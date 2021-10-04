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
                new ObjectDescription(new List<KeyValuePair<string?, IObjectDescription>> {
                    new KeyValuePair<string?, IObjectDescription>( "Value1", new ObjectDescription(1)),
                    new KeyValuePair<string?, IObjectDescription>("Value2", ObjectDescriptions.NullDescription)
                }), new ObjectDescription(new List<KeyValuePair<string?, IObjectDescription>> {
                    new KeyValuePair<string?, IObjectDescription>( "Value1", new ObjectDescription("1") ),
                    new KeyValuePair<string?, IObjectDescription>(  "Value3", new ObjectDescription((object?)null) )
                })));

        }
    }
}
