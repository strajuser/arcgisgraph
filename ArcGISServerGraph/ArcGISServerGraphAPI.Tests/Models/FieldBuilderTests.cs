using ArcGISServerGraphAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArcGISServerGraphAPI.Tests.Models
{
    [TestClass]
    public class FieldBuilderTests
    {
        [TestMethod]
        public void TestFieldBuilder()
        {
            var fields = new[] {"*", "layers", "layers/*"};
            var rez = FieldBuilder.GetFields(fields);

            Assert.AreEqual(rez.Count, 2);
            Assert.IsTrue(rez[0].Name == "*");
            Assert.AreEqual(rez[1].Fields.Count, 1);
        }
    }
}