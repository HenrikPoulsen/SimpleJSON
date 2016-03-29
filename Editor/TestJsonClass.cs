using NUnit.Framework;

namespace SimpleJSON.Test
{
    [TestFixture]
    public class TestJsonClass
    {
        [Test]
        public void JsonClass_Set_EscapeNewLine()
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
        public void JsonClass_Set_EsapeCarriageReturn()
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
        public void JsonClass_Set_EscapeForwardSlash()
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
        public void JsonClass_Set_EscapeBackSlash()
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
        public void JsonClass_Set_EscapeQuote()
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
        public void JsonClass_Set_EscapeTab()
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
        public void JsonClass_Set_EscapeB()
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
        public void JsonClass_Set_EscapeF()
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
        public void JsonClass_Set_DontEscapeG()
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
        public void JsonClass_Set_WriteOverItemUpdatesValue()
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
        public void JsonClass_Set_WriteOverDoesNotAddItem()
        {
            // arrange
            var node = new JSONClass();
            node["value"] = new JSONData("string");

            // act
            node["value"] = new JSONData("string2");

            // assert
            Assert.AreEqual(1, node.Count);
        }

        [Test]
        public void JsonClass_Remove_NonExistantNode()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");

            // act
            var removed = node.Remove(toRemove);

            // assert
            Assert.IsNull(removed);
        }

        [Test]
        public void JsonClass_Remove_NonNode()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node.Add(toRemove);

            // act
            var removed = node.Remove(toRemove);

            // assert
            Assert.AreEqual(toRemove, removed);
        }

        [Test]
        public void JsonClass_Remove_NegativeIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node.Add(toRemove);

            // act
            var removed = node.Remove(-1);

            // assert
            Assert.IsNull(removed);
        }

        [Test]
        public void JsonClass_Remove_IndexTooHigh()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node.Add(toRemove);

            // act
            var removed = node.Remove(2);

            // assert
            Assert.IsNull(removed);
        }

        [Test]
        public void JsonClass_Remove_ExistingIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node.Add(toRemove);

            // act
            var removed = node.Remove(0);

            // assert
            Assert.AreEqual(toRemove, removed);
        }

        [Test]
        public void JsonClass_Remove_NonExistantKey()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node.Add(toRemove);

            // act
            var removed = node.Remove("value");

            // assert
            Assert.IsNull(removed);
        }

        [Test]
        public void JsonClass_Remove_ExistingKey()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toRemove = new JSONData("string");
            node["value"] = toRemove;

            // act
            var removed = node.Remove("value");

            // assert
            Assert.AreEqual(toRemove, removed);
        }

        [Test]
        public void JsonClass_Get_AtNegativeIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");
            node.Add(toAccess);

            // act
            var accessed = node[-1];

            // assert
            Assert.IsNull(accessed);
        }

        [Test]
        public void JsonClass_Get_AtOutOfRangeIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");
            node.Add(toAccess);

            // act
            var accessed = node[2];

            // assert
            Assert.IsNull(accessed);
        }

        [Test]
        public void JsonClass_Get_AtExistingIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");
            node.Add(toAccess);

            // act
            var accessed = node[0];

            // assert
            Assert.AreEqual(toAccess, accessed);
        }

        [Test]
        public void JsonClass_Set_AtNegativeIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");

            // act
            node[-1] = toAccess;

            // assert
            Assert.AreEqual(0, node.Count);
        }

        [Test]
        public void JsonClass_Set_AtOutOfRangeIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");

            // act
            node[2] = toAccess;

            // assert
            Assert.AreEqual(0, node.Count);
        }

        [Test]
        public void JsonClass_Set_AtExistingIndex()
        {
            // arrange
            var node = new JSONClass();
            JSONNode toAccess = new JSONData("string");
            node.Add(new JSONData(null));

            // act
            node[0] = toAccess;

            // assert
            Assert.AreEqual(toAccess, node[0]);
        }

        [Test]
        public void JsonClass_GetEnumerator_TwoItems()
        {
            // arrange
            var node = new JSONClass();
            node.Add(new JSONData("string"));
            node.Add(new JSONData(null));

            // act
            var i = 0;
            foreach (var item in node)
            {
                ++i;
            }

            // assert
            Assert.AreEqual(2, i);
        }

        [Test]
        public void JsonClass_Children_TwoItems()
        {
            // arrange
            var node = new JSONClass();
            node.Add(new JSONData("string"));
            node.Add(new JSONData(null));

            // act
            var i = 0;
            foreach (var item in node.Children)
            {
                ++i;
            }

            // assert
            Assert.AreEqual(2, i);
        }
    }
}