using DNI.Core.Shared.Attributes;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.UnitTests
{
    public class OptionalAttributeTests
    {
        [TestCase("A", "B", "C")]
        [TestCase("A", null, "")]
        [TestCase("", "B", null)]
        [TestCase(null, "", "C")]
        public void Validation_should_pass(string aValue, string bValue, string cValue)
        {
            var testClass = new TestClass { A = aValue, B = bValue, C = cValue };
            var validateContext = new ValidationContext(testClass);
            var validationResults = new List<ValidationResult>();
            Assert.True(Validator.TryValidateObject(testClass, validateContext, validationResults, true));
        }

        [TestCase(null, null, null)]
        [TestCase("", "", "")]
        public void Validation_should_fail(string aValue, string bValue, string cValue)
        {
            var testClass = new TestClass { A = aValue, B = bValue, C = cValue };
            var validateContext = new ValidationContext(testClass);
            var validationResults = new List<ValidationResult>();
            Assert.False(Validator.TryValidateObject(testClass, validateContext, validationResults, true));
        }

        internal class TestClass
        {
            [Optional(nameof(B), nameof(C))]
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
        }
    }
}
