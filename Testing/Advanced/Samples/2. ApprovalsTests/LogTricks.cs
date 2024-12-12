using Advanced.Classwork.ThingCache;
using ApprovalTests;
using ApprovalTests.Reporters;
using FakeItEasy;
using log4net.Appender;
using log4net.Config;
using NUnit.Framework;

namespace Advanced.Samples.ApprovalsTests;

[TestFixture]
[Explicit]
internal class LogTricks
{
    [Test]
    [UseReporter(typeof(DiffReporter))]
    public void Log()
    {
        // перехватываем записи логгера в память
        var memoryAppender = new MemoryAppender();
        BasicConfigurator.Configure(memoryAppender);

        var id = "42";
        Thing result = null;

        var service = A.Fake<IThingService>();
        A.CallTo(() => service.TryRead(id, out result)).Returns(false);

        var cache = new ThingCache(service);

        cache.Get(id);

        var logs = memoryAppender
            .GetEvents()
            .Select(x => x.RenderedMessage);


        Approvals.Verify(string.Join("\n", logs));
    }
}
