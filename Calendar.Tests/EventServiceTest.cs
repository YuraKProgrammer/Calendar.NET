using Calendar.WebService.Services;
using NUnit.Framework;

namespace Calendar.Tests
{
    public class EventServiceTest
    {
        [Test]
        public void GetCountTest()
        {
            var service = new EventService();
            var result = service.GetCount(new DateTime(), new DateTime(), null);
            Assert.IsNotNull(result.Error);
        }
    }
}