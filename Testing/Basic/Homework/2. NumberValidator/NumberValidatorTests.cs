using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(2, 3, true)]
    [TestCase(2, -1, false)]
    [TestCase(0, 2, false)]
    public void Constructor_IncorrectArguments_ShouldThrowArgumentException(int precision, int scale,
        bool onlyPositive)
    {
        Action action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true)]
    public void Constructor_CorrectArguments_ShouldNotThrowArgumentException(int precision, int scale,
        bool onlyPositive)
    {
        Action action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }

    [TestCase(3, 2, true, "a.sd")]
    [TestCase(3, 2, true, "0+1.0.1")]
    public void IsValidNumber_WrongInputType_ShouldReturnFalse(int precision, int scale, bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [TestCase(3, 2, true, "00.00")]
    public void IsValidNumber_WrongPrecision_ShouldReturnFalse(int precision, int scale, bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [TestCase(17, 1, true, "00.00")]
    public void IsValidNumber_WrongScale_ShouldReturnFalse(int precision, int scale, bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [TestCase(4, 3, true, " ")]
    [TestCase(4, 3, true, null)]
    public void IsValidNumber_NullOrWhitespaces_ShouldReturnFalse(int precision, int scale, bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [TestCase(1, 0, false, "-0")]
    public void IsValidNumber_NegativeNumberWhenNotAllowed_ShouldReturnFalse(int precision, int scale,
        bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeFalse();
    }

    [TestCase(17, 2, true, "0")]
    public void IsValidNumber_CorrectInput_ShouldReturnTrue(int precision, int scale, bool onlyPositive,
        string input)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(input).Should().BeTrue();
    }
}