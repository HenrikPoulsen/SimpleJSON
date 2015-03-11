using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleJSON.Test
{
    [TestClass]
    public class ObjectTest
    {
        [TestMethod]
        public void Object_Set_EscapeNewLine()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\n");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\n"));
        }

        [TestMethod]
        public void Object_Set_EsapeCarriageReturn()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\r");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\r"));
        }

        [TestMethod]
        public void Object_Set_EscapeForwardSlash()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("/");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\/"));
        }

        [TestMethod]
        public void Object_Set_EscapeBackSlash()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\\");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\\\"));
        }

        [TestMethod]
        public void Object_Set_EscapeQuote()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\"\"");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\\"\\\""));
        }

        [TestMethod]
        public void Object_Set_EscapeTab()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\t");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\t"));
        }

        [TestMethod]
        public void Object_Set_EscapeB()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\b");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\b"));
        }

        [TestMethod]
        public void Object_Set_EscapeF()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\f");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\f"));
        }

        [TestMethod]
        public void Object_Set_DontEscapeG()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("\\g");


            // act
            var result = node.ToString();

            // assert
            Assert.IsTrue(result.Contains("\\g"));
        }

        [TestMethod]
        public void Object_Set_WriteOverItemUpdatesValue()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("string");

            // act
            node["value"] = new JSONData("string2");

            // assert
            Assert.AreEqual("string2", node["value"].Value);
        }

        [TestMethod]
        public void Object_Set_WriteOverDoesNotAddItem()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("string");

            // act
            node["value"] = new JSONData("string2");

            // assert
            Assert.AreEqual(1, node.Count);
        }
    }
}
