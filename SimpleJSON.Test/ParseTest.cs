using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleJSON.Test
{
    [TestClass]
    public class ParseTest
    {
        public static string JsonStringEqualsInsteadOfColon = @"
        {
            ""integer"": {
                ""negative"" = 1
        }}";
        public static string JsonStringMissingComma = @"
        {
            ""integer"": {
                ""negative"": 1
                ""positive"": 1
        }}";
        public static string JsonStringMissingClosingBracket = @"
        {
            ""integer"": {
                ""negative"": 1,
                ""positive"": 1
            },
            ""floating"": 1.0";
        public static string JsonStringMissingOpeningBracket = @"
        
            ""integer"": {
                ""negative"": 1,
                ""positive"": 1
        }}";
        public static string JsonObjectStringWithAllTypes = @"
        {
            ""string"": {
                ""normal"": ""this is a string"",
                ""special"": "":,[]{}\""\\\t\n\r\b\u0041\f\m\/""
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
            ""array"": {
                ""empty"": [],
                ""populated"": [1, 1.0, null, ""string"", false, {}]
            },
            ""null"": null
        }";
        public static string JsonObjectStringWithAllNull = @"
        {
            ""string"": null,
            ""integer"": null,
            ""floating"": null,
            ""exponential"": null,
            ""boolean"": null,
            ""emptyArray"": null,
            ""null"": null
        }";

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void Parse_EqualsInsteadOfColon_ThrowsException()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonStringEqualsInsteadOfColon);

            // assert
            Assert.Fail("Should have thrown an exception");
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void Parse_MissingClosingBracket_ThrowsException()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonStringMissingClosingBracket);

            // assert
            Assert.Fail("Should have thrown an exception");
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void Parse_MissingOpeningBracket_ThrowsException()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonStringMissingOpeningBracket);

            // assert
            Assert.Fail("Should have thrown an exception");
        }

        [TestMethod]
        [ExpectedException(typeof (Exception))]
        public void Parse_MissingComma_ThrowsException()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonStringMissingComma);

            // assert
            Assert.Fail("Should have thrown an exception");
        }

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
            Assert.AreEqual(@":,[]{}""\\t\n\r\bA\f\m/", node["string"]["special"].Value);
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
            Assert.AreEqual(3.14, node["floating"]["positive"].AsFloat.Value, 0.000001);
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
            Assert.AreEqual(0.0003, node["exponential"]["negative"].AsFloat.Value, 0.000001);
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

        [TestMethod]
        public void Parse_SimpleObject_NullSuccess()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.IsTrue(node["null"].IsNull);
        }

        [TestMethod]
        public void Parse_SimpleObject_EmptyArrayCountZero()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(0, node["array"]["empty"].AsArray.Count);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayCountSix()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(6, node["array"]["populated"].AsArray.Count);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayIntAtZero()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(1, node["array"]["populated"].AsArray[0].AsInt);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayFloatAtOne()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual(1.0, node["array"]["populated"].AsArray[1].AsFloat.Value, 0.00001);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayNullAtTwo()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.IsTrue(node["array"]["populated"].AsArray[2].IsNull);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayStringAtThree()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.AreEqual("string", node["array"]["populated"].AsArray[3].Value);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayBoolAtFour()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.IsFalse(node["array"]["populated"].AsArray[4].AsBool.Value);
        }

        [TestMethod]
        public void Parse_SimpleObject_PopulatedArrayObjectAtFive()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllTypes);

            // assert
            Assert.IsNotNull(node["array"]["populated"].AsArray[5].AsObject);
        }

        [TestMethod]
        public void Parse_NullObject_IntegerNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // assert
            Assert.AreEqual(null, node["integer"].AsInt);
        }

        [TestMethod]
        public void Parse_NullObject_StringNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // assert
            Assert.AreEqual("null", node["string"].Value);
        }

        [TestMethod]
        public void Parse_NullObject_BoolNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // assert
            Assert.AreEqual(null, node["boolean"].AsBool);
        }

        [TestMethod]
        public void Parse_NullObject_FloatNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // assert
            Assert.AreEqual(null, node["floating"].AsFloat);
        }

        [TestMethod]
        public void Parse_NullObject_ArrayNull()
        {
            // arrange
            // nothing

            // act
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // assert
            Assert.AreEqual(null, node["floating"].AsArray);
        }
    }
}