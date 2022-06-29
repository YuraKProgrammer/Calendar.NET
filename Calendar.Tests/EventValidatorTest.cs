using Calendar.Models;
using Calendar.WebService.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Tests
{
    internal class EventValidatorTest
    {
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("@fejfi")]
        [TestCase("$orgkr")]
        public void Validate_Test_Name(string name)
        {
            var ev = new Event { Name = name, Date = DateTime.Now };
            IEventValidator validator = new EventValidator();
            var result = validator.Validate(ev);
            Assert.AreEqual(Errors.IncorrectData.Code, result.Error.Code);
        }
        [Test]
        public void Validate_Test_Date()
        {
            var ev = new Event { Name = "fkrgr", Date = DateTime.MaxValue };
            IEventValidator validator = new EventValidator();
            var result = validator.Validate(ev);
            Assert.AreEqual(Errors.IncorrectData.Code, result.Error.Code);
        }
    }
}
