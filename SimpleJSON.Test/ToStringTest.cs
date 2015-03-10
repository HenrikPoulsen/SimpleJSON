using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleJSON.Test
{
    [TestClass]
    public class ToStringTest
    {
        [TestMethod]
        public void ToString_SimpleObject_NullSuccess()
        {
            // arrange
            const string expected = @"
            {
                ""null"": null
            }";
            var node = JSON.Parse(expected);

            // act
            var actual = node.ToString();

            // assert
            Assert.IsTrue(String.Compare(expected, actual, CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols) ==
                          0);
        }
    }
}