using System;
using System.Globalization;
using System.IO;
using NUnit.Framework;

namespace SimpleJSON.Test
{
    [TestFixture]
    public class TestSerializer
    {
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
            ""long"": {
                ""positive"": 1234567890000,
                ""explicitPositive"": +1234567890000,
                ""negative"" : -1234567890000
            },
            ""floating"": {
                ""positive"": 3.14,
                ""explicitPositive"": +3.14,
                ""negative"": -3.14
            },
            ""double"": {
                ""positive"": 3.14159265359,
                ""explicitPositive"": +3.14159265359,
                ""negative"": -3.14159265359
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

        [Test]
        public void Serialize_NullObject_IntegerNull()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["integer"].IsNull);
        }

        [Test]
        public void Serialize_NullObject_StringNull()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);


            // assert
            Assert.AreEqual("null", node["string"].Value);
        }

        [Test]
        public void Serialize_NullObject_BoolNull()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["boolean"].IsNull);
        }

        [Test]
        public void Serialize_NullObject_FloatNull()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["floating"].IsNull);
        }

        [Test]
        public void Serialize_NullObject_ArrayNull()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["floating"].IsNull);
        }

        [Test]
        public void Serialize_SimpleObject_TrueBoolSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(true, node["boolean"]["positive"].AsBool);
        }

        [Test]
        public void Serialize_SimpleObject_FalseBoolSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(false, node["boolean"]["negative"].AsBool);
        }

        [Test]
        public void Serialize_SimpleObject_NullSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["null"].IsNull);
        }

        [Test]
        public void Serialize_SimpleObject_EmptyArrayCountZero()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(0, node["array"]["empty"].AsArray.Count);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayCountSix()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(6, node["array"]["populated"].AsArray.Count);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayIntAtZero()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1, node["array"]["populated"].AsArray[0].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayFloatAtOne()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1.0, node["array"]["populated"].AsArray[1].AsFloat, 0.00001);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayNullAtTwo()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(node["array"]["populated"].AsArray[2].IsNull);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayStringAtThree()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual("string", node["array"]["populated"].AsArray[3].Value);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayBoolAtFour()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsFalse(node["array"]["populated"].AsArray[4].AsBool);
        }

        [Test]
        public void Serialize_SimpleObject_PopulatedArrayObjectAtFive()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsNotNull(node["array"]["populated"].AsArray[5].AsObject);
        }

        [Test]
        public void Serialize_SimpleObject_NormalStringSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual("this is a string", node["string"]["normal"].Value);
        }

        [Test]
        public void Serialize_SimpleObject_SpecialStringSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(@":,[]{}""\\t\n\r\bA\f\m/", node["string"]["special"].Value);
        }

        [Test]
        public void Serialize_SimpleObject_PlainIntegerSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1, node["integer"]["positive"].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_ExplicitPositiveIntegerSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1, node["integer"]["explicitPositive"].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_NegativeIntegerSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(-1, node["integer"]["negative"].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_PlainLongSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1234567890000, node["long"]["positive"].AsLong);
        }

        [Test]
        public void Serialize_SimpleObject_ExplicitPositiveLongSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(1234567890000, node["long"]["explicitPositive"].AsLong);
        }

        [Test]
        public void Serialize_SimpleObject_NegativeLongSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(-1234567890000, node["long"]["negative"].AsLong);
        }

        [Test]
        public void Serialize_SimpleObject_FloatingSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(3.14, node["floating"]["positive"].AsFloat, 0.000001);
        }

        [Test]
        public void Serialize_SimpleObject_FloatingExplicitPositiveSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(3.14, node["floating"]["explicitPositive"].AsFloat, 0.000001);
        }

        [Test]
        public void Serialize_SimpleObject_FloatingNegativeSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(-3.14, node["floating"]["negative"].AsFloat, 0.000001);
        }

        [Test]
        public void Serialize_SimpleObject_DoubleSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(3.14159265359, node["double"]["positive"].AsDouble, 0.000000000001);
        }

        [Test]
        public void Serialize_SimpleObject_DoubleExplicitPositiveSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(3.14159265359, node["double"]["explicitPositive"].AsDouble, 0.000000000001);
        }

        [Test]
        public void Serialize_SimpleObject_DoubleNegativeSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(-3.14159265359, node["double"]["negative"].AsDouble, 0.000000000001);
        }

        [Test]
        public void Serialize_SimpleObject_PlainExponentialSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(30000, node["exponential"]["positive"].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_ExplicitPositiveExponentialSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(30000, node["exponential"]["explicitPositive"].AsInt);
        }

        [Test]
        public void Serialize_SimpleObject_NegativeExponentialSuccess()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            node = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.AreEqual(0.0003, node["exponential"]["negative"].AsFloat, 0.000001);
        }

        [Test]
        public void Serialize_SimpleObject_Base64()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);

            // act
            var base64 = node.SaveToBase64();
            var loaded = JSONClass.LoadFromBase64(base64).ToString();

            // assert
            Assert.IsTrue(
                String.Compare(JsonObjectStringWithAllNull, loaded, CultureInfo.CurrentCulture,
                    CompareOptions.IgnoreSymbols) ==
                0);
        }
    }
}