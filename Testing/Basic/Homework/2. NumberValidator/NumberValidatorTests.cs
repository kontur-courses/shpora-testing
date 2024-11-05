using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private static NumberValidator GetCorrectValidator(bool onlyPositive = false) => new(2, 1, onlyPositive);

    [TestCase(1, TestName = "Correct parameters")]
    public void Constructor_WithCorrectParameters_NotThrows(int precision, int scale = 0)
    {
        var creation = () => new NumberValidator(precision, scale, true);

        creation.Should()
            .NotThrow<ArgumentException>();
    }

    [TestCase(-1, TestName = "negative precision")]
    [TestCase(0, TestName = "zero precision")]
    public void Constructor_WithIncorrectPrecision_Throws(int precision, int scale = 0)
    {
        var creation = () => new NumberValidator(precision, scale);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [TestCase(1, -1, TestName = "negative scale")]
    [TestCase(1, 2, TestName = "scale greater than precision")]
    [TestCase(1, 1, TestName = "scale equal to precision")]
    public void Constructor_WithIncorrectScale_Throws(int precision, int scale)
    {
        var creation = () => new NumberValidator(precision, scale);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("scale must be a non-negative number less than precision");
    }

    [TestCase(2, 0, true, "+1", TestName = "number with a plus")]
    public void IsValidNumber_CorrectPlus_ReturnsTrue(
        int precision,
        int scale,
        bool onlyPositive,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase(2, 0, false, "-1", TestName = "number with a minus")]
    public void IsValidNumber_CorrectMinus_ReturnsTrue(
        int precision,
        int scale,
        bool onlyPositive,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase(1, 0, true, "0", TestName = "one digit")]
    [TestCase(3, 0, true, "123", TestName = "three digits")]
    public void IsValidNumber_CorrectNumberOfDigits_ReturnsTrue(
        int precision,
        int scale,
        bool onlyPositive,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("0.1", TestName = "separator dot")]
    [TestCase("0,1", TestName = "separator comma")]
    public void IsValidNumber_CorrectSeparator_ReturnsTrue(string expectedResultValue)
    {
        GetCorrectValidator().IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("", TestName = "empty string")]
    [TestCase(null, TestName = "null")]
    public void IsValidNumber_IsNullOrEmpty_ReturnsFalse(string expectedResultValue)
    {
        GetCorrectValidator().IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase(".0", TestName = "intPart is missing")]
    [TestCase("0.", TestName = "fracPart part is missing")]
    public void IsValidNumber_MatchIsNotSuccess_ReturnsFalse(string expectedResultValue)
    {
        GetCorrectValidator().IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase("+11", TestName = "more numbers than precision (only intPart)")]
    [TestCase("-1.1", TestName = "more numbers than precision (intPart and fracPart)")]
    public void IsValidNumber_IncorrectPrecision_ReturnsFalse(string expectedResultValue)
    {
        GetCorrectValidator().IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase("0.11", TestName = "more digits in fracPart than scale")]
    public void IsValidNumber_IncorrectScale_ReturnsFalse(string expectedResultValue)
    {
        GetCorrectValidator().IsValidNumber(expectedResultValue).Should().BeFalse();
    }
}