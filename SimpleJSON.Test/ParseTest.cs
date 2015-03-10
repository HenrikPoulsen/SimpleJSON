using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleJSON.Test
{
    [TestClass]
    public class ParseTest
    {
        public static string JsonBuggyString = @"
        {
            ""integer"": {
                ""negative"" = 1
        }}";
        public static string JsonObjectStringWithAllTypes = @"
        {
            ""string"": {
                ""normal"": ""this is a string"",
                ""special"": "":,[]{}\t\n\r\b\u0041\f""
            },
            ""integer"": {
                ""positive"": 1,
                ""explicitPositive"": +1,
                ""negative"" : -1
            },
            ""floating"": {
                ""positive"": 3.14,
                ""explicitPositive"": +3.14,
                ""negative"": -3.14
            },
            ""exponential"": {
                ""positive"": 3E4,
                ""explicitPositive"": 3E+4,
                ""negative"": 3E-4
            },
            ""boolean"": {
                ""positive"": true,
                ""negative"": false
            },
            ""emptyArray"": [],
            ""null"": ""null currently unsupported!!!""
        }";

        [TestMethod]
        public void Parse_EmptyJsonObject_NotNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse("{}");

            // assert
            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void Parse_EmptyString_ReturnsNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse("");

            // assert
            Assert.IsNull(node);
        }

        [TestMethod]
        public void Parse_SimpleObject_NormalStringSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual("this is a string", node["string"]["normal"].Value);
        }

        [TestMethod]
        public void Parse_SimpleObject_SpecialStringSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(@":,[]{}\t\n\r\bA\f", node["string"]["special"].Value);
        }

        [TestMethod]
        public void Parse_SimpleObject_PlainIntegerSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(1, node["integer"]["positive"].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_ExplicitPositiveIntegerSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(1, node["integer"]["explicitPositive"].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_NegativeIntegerSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(-1, node["integer"]["negative"].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_FloatingSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(3.14, node["floating"]["positive"].AsFloat, 0.000001);
        }

        [TestMethod]
        public void Parse_SimpleObject_PlainExponentialSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(30000, node["exponential"]["positive"].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_ExplicitPositiveExponentialSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(30000, node["exponential"]["explicitPositive"].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_NegativeExponentialSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(0.0003, node["exponential"]["negative"].AsFloat, 0.000001);
        }

        [TestMethod]
        public void Parse_SimpleObject_ArraySuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(0, node["array"].AsArray.Count);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Parse_Null_ThrowsException()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(null);

            // assert
            // Expectes ArgumentNullException
        }

        [TestMethod]
        public void Parse_SimpleObject_TrueBoolSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(true, node["boolean"]["positive"].AsBool);
        }

        [TestMethod]
        public void Parse_SimpleObject_FalseBoolSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(false, node["boolean"]["negative"].AsBool);
        }
    }
}