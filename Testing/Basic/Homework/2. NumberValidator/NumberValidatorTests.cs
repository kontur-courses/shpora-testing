using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    [TestCase(2, 5)]
    [TestCase(2, 2)]
    public void Should_Throw_Exception_With_Incorrect_Parameters(int precision, int scale)
    {
        var act = () => new NumberValidator(precision, scale);
        
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Should_DoesNotThrow_Exception_If_Scale_And_Precision_Is_Correct()
    {
        var act = () => new NumberValidator(10, 5);
        
        act.Should().NotThrow<ArgumentException>();
    }

    [TestCase(null)]
    [TestCase(" ")]
    [TestCase("a.sd")]
    [TestCase("0.")]
    [TestCase(",0")]
    [TestCase("0*0")]
    [TestCase("!0,0")]
    [TestCase("1,234")]
    [TestCase("11111")]
    [TestCase("22,32")]
    [TestCase("+14,23")]
    [TestCase("-0")]
    public void IsValidNumber_Should_Return_False(string value)
    {
        var numberValidator = new NumberValidator(4, 2, true);
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeFalse();
    }
    
    [TestCase("-1,4", false)]
    [TestCase("1")]
    [TestCase("+1,444")]
    [TestCase("1,444")]
    public void IsValidNumber_Should_Return_True(string value, bool onlyPositive = true)
    {
        var numberValidator = new NumberValidator(10, 5, onlyPositive);
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeTrue();
    }
}