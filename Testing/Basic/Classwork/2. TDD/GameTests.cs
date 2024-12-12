using FluentAssertions;
using NUnit.Framework;
using TDD.Task;

namespace TDD;

[TestFixture]
public class GameTests
{
    [Test]
    [Explicit]
    public void HaveZeroScore_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }
}