using Calendar.Models;
using Calendar.WebService.Services;
using NUnit.Framework;

namespace Calendar.Tests
{
    public class EventServiceTest
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("   ")]
        public async Task GetCount_Token_Test(string token)
        {
            var service = new EventService();
            var result = await service.GetCountAsync(new DateTime(), new DateTime(), token, CancellationToken.None);
            Assert.AreEqual(Errors.TokenNotFound.Code,result.Error.Code);
        }
    }
}