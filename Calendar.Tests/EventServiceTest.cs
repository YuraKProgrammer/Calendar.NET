using Calendar.DataModels;
using Calendar.Models;
using Calendar.WebService.Services;
using Kalantyr.Auth.Client;
using Moq;
using NUnit.Framework;

namespace Calendar.Tests
{
    public class EventServiceTest
    {
        private readonly Mock<IAppAuthClient> _appAuthClient = new Mock<IAppAuthClient>();
        private readonly Mock<IEventStorage> _eventStorage = new Mock<IEventStorage>();
        private readonly Mock<IEventValidator> _eventValidator = new Mock<IEventValidator>();

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("   ")]
        public async Task GetCount_Token_Test(string token)
        {
            var service = new EventService(_appAuthClient.Object, _eventStorage.Object, _eventValidator.Object);
            var result = await service.GetCountAsync(new DateTime(), new DateTime(), token, CancellationToken.None);
            Assert.AreEqual(Errors.TokenNotFound.Code,result.Error.Code);
        }

        [TestCase("12345",true)]
        [TestCase("54321",false)]
        public async Task GetCount_UserId_Test(string token, bool success)
        {
            _appAuthClient
                .Setup(ac => ac.GetUserIdAsync("12345", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Kalantyr.Web.ResultDto<uint> {Result=123});
            _appAuthClient
                .Setup(ac => ac.GetUserIdAsync("54321", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Kalantyr.Web.ResultDto<uint> { Error=new Kalantyr.Web.Error {Code="E"}});
            var service = new EventService(_appAuthClient.Object, _eventStorage.Object, _eventValidator.Object);
            var result = await service.GetCountAsync(new DateTime(), new DateTime(), token, CancellationToken.None);
            if (success == true)
            {
                Assert.IsNull(result.Error);
            }
            else
            {
                Assert.AreEqual("E", result.Error.Code);
            }
        }
    }
}