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

        [Test]
        public void Serialize_NullData_RoundTrip()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllNull);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var result = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(
                String.Compare(JsonObjectStringWithAllNull, result.ToString(), CultureInfo.CurrentCulture,
                    CompareOptions.IgnoreSymbols) == 0);
        }

        [Test]
        public void Serialize_SimpleObject_RoundTrip()
        {
            // arrange
            var node = JSON.Parse(JsonObjectStringWithAllTypes);
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            node.Serialize(binaryWriter);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var result = JSONNode.Deserialize(binaryReader);

            // assert
            Assert.IsTrue(String.Compare(JsonObjectStringWithAllTypes, result.ToString(), CultureInfo.CurrentCulture,
                CompareOptions.IgnoreSymbols) == 0);
        }
    }
}