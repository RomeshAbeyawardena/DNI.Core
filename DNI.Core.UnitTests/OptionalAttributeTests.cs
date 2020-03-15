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

        [TestCase(0, 1, 0)]
        [TestCase(1, null, 0)]
        [TestCase(null, null, 1)]
        [TestCase(null, 0, 1)]
        public void Validation_should_pass(int aValue, int bValue, int cValue)
        {
            var testClass = new TestClass_int { A = aValue, B = bValue, C = cValue };
            var validateContext = new ValidationContext(testClass);
            var validationResults = new List<ValidationResult>();
            Assert.True(Validator.TryValidateObject(testClass, validateContext, validationResults, true));
        }

        [TestCase(null, null, null)]
        [TestCase(0, 0, 0)]
        public void Validation_should_fail(int aValue, int bValue, int cValue)
        {
            var testClass = new TestClass_int { A = aValue, B = bValue, C = cValue };
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

        internal class TestClass_int
        {
            [Optional(nameof(B), nameof(C))]
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
        }
    }
}
