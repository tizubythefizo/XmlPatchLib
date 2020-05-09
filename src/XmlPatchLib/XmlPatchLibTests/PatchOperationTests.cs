using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tizuby.XmlPatchLib;
using Wmhelp.XPath2;

namespace XmlPatchLibTests
{
    [TestClass]
    public class PatchOperationTests
    {
        [TestMethod]
        public void AddSingleChildElement()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\AddSingleChildTestGood.xml");

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var newChild = source.Element("original").Element("main").Element("child3");

            // Assert
            Assert.IsNotNull(newChild);
        }

        [TestMethod]
        public void AddComplexChildElement()
        {
            //Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\AddComplexChildRelement.xml");

            var patcher = new XmlPatcher();

            //Act
            patcher.PatchXml(source, diff);

            //Assert
            var newChildParent = source.Element("original").Element("main").Element("child3");

            Assert.IsNotNull(newChildParent);
            Assert.AreEqual(2, newChildParent.Elements().Count());

            var innerChild = newChildParent.Element("child3Inner2");

            Assert.IsNotNull(innerChild);
            Assert.AreEqual(1, innerChild.Elements().Count());

            var attributedChild = innerChild.Element("child3Inner2Inner1");

            Assert.IsNotNull(attributedChild);
            Assert.AreEqual(1, attributedChild.Attributes().Count());
        }

        [TestMethod]
        public void AddAttribute()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\AddAttributes.xml");
            var expected = diff.Descendants("add").First().Value.Trim();

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var mainElement = source.Descendants("main").First();
            var attribute = mainElement.Attribute("testAttribute");

            // Assert
            Assert.IsNotNull(attribute);
            Assert.IsTrue(expected.Equals(attribute.Value));
        }

        [TestMethod]
        public void ReplaceElement()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\ReplaceElement.xml");
            var expectedText = "Replaced test!";

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var targetNode = source.Root.Element("main").Element("child1");

            // Assert
            Assert.AreEqual(expectedText, targetNode.Value);
        }

        [TestMethod]
        public void ReplaceAttribute()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\ReplaceAttribute.xml");
            var expectedText = "NewTestValue";

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var targetNode = source.Root.Element("attributedNode");
            var attributeVal = targetNode.Attribute("test").Value;

            // Assert
            Assert.AreEqual(expectedText, attributeVal);
        }

        [TestMethod]
        public void RemoveElement()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\RemoveElement.xml");

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var targetNode = source.Root.Element("main").Element("child2");

            // Assert
            Assert.IsNull(targetNode);
        }

        [TestMethod]
        public void RemoveAttribute()
        {
            // Arrange
            var source = XDocument.Load(@"TestingXmls\TestOriginalSource1.xml");
            var diff = XDocument.Load(@"TestingXmls\RemoveAttribute.xml");

            var patcher = new XmlPatcher();

            // Act
            patcher.PatchXml(source, diff);

            var targetNode = source.Root.Element("attributedNode");

            // Assert
            Assert.IsFalse(targetNode.HasAttributes);
        }
    }
}
