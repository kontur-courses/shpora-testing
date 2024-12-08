using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace Advanced.Samples.Mocks;


[TestFixture]
public class Mocks
{
    [Test]
    public void Fail_OnNotConfiguredCalls_InStrictMode()
    {
        var service = A.Fake<IService>(o => o.Strict());
        Assert.Throws<ExpectationException>(
            () => service.Get());

    }
    [Test]
    public void ReturnsDefault_AfterSequenceEnds()
    {
        var service = A.Fake<IService>();
        A.CallTo(() => service.Get())
            .ReturnsNextFromSequence("1", "2");
        service.Get().Should().Be("1");
        service.Get().Should().Be("2");
        service.Get().Should().Be("");
    }

    [Test]
    public void Creates_ObjectWithParameterlessConstructor()
    {
        var func = A.Fake<Func<ComplexDto>>();
        var complexDto = func();
        complexDto.Should().NotBeNull();
        complexDto.dto.Should().BeNull();
        complexDto.s.Should().BeNull();
    }

    [Test]
    public void ReturnsOnce_HasStackBehaviour()
    {
        var service = A.Fake<IService>();
        A.CallTo(() => service.Get()).Returns("1").Once();
        A.CallTo(() => service.Get()).Returns("2").Once();
        service.Get().Should().Be("2");
        A.CallTo(() => service.Get()).Returns("3").Once();
        service.Get().Should().Be("3");
        service.Get().Should().Be("1");
        service.Get().Should().Be("");
    }

    [Test]
    public void MustNotHaveHappened()
    {
        var service = A.Fake<IService>();


        A.CallTo(() => service.Get())
            .MustNotHaveHappened();
    }

    [Test]
    public void OutParameters()
    {
        var service = A.Fake<IService>();
        var id = "id";
        string result = "42";

        A.CallTo(() => service.TryGet(id, out result))
            .Returns(true);

        service.TryGet(id, out var actualResult).Should().BeTrue();
        actualResult.Should().Be(result);

    }
}
