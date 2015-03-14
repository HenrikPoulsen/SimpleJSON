using System;
using System.Globalization;
using NUnit.Framework;

namespace SimpleJSON.Test
{
    [TestFixture]
    public class TestToString
    {
        [Test]
        public void ToString_SimpleObject_NullSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": null
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_IntegerSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 1
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_DoubleSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 1.5
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_StringSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": ""String""
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_LongSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 12345678900000,
                ""value2"": 12345678900001
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_BoolSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": true,
                ""value2"": false
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_ObjectSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": {},
                ""value2"": {
                    ""array"": [],
                    ""integer"": 1,
                    ""float"": 1.5,
                    ""string "": ""string"",
                    ""boolean"": false
                }
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToString_SimpleObject_ArraySuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": [],
                ""value2"": [1, 1.5, ""string"", false, null, {}]
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_NullSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": null
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_IntegerSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 1
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_DoubleSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 1.5
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_StringSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": ""String""
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_LongSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": 12345678900000,
                ""value2"": 12345678900001
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_BoolSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": true,
                ""value2"": false
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringToStringFormatted_SimpleObject_ObjectSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": {},
                ""value2"": {
                    ""array"": [],
                    ""integer"": 1,
                    ""float"": 1.5,
                    ""string "": ""string"",
                    ""boolean"": false
                }
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }

        [Test]
        public void ToStringFormatted_SimpleObject_ArraySuccess()
        {
            // arrange
            const string expected = @"
            {
                ""value"": [],
                ""value2"": [1, 1.5, ""string"", false, null, {}]
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString("");

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }
    }
}