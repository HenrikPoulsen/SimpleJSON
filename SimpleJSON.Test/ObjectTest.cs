using NUnit.Framework;

namespace SimpleJSON.Test
{
    [TestFixture]
    public class ObjectTest
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
