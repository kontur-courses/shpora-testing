using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
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

    [TestCase("+11", TestName = "less numbers than precision (intPart with plus)")]
    [TestCase("-1.1", TestName = "less numbers than precision (intPart and fracPart with minus)")]
    [TestCase("11", TestName = "less numbers than precision (only intPart)")]
    [TestCase("1.1", TestName = "less numbers than precision (intPart and fracPart)")]
    [TestCase("123", TestName = "numbers equals precision (only intPart)")]
    [TestCase("1.23", TestName = "numbers equals precision (intPart and fracPart)")]
    public void IsValidNumber_WithCorrectPrecision_ReturnsTrue(string expectedResultValue)
    {
        new NumberValidator(3, 2).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("+1", TestName = "number with plus")]
    [TestCase("1", TestName = "number without sign")]
    public void IsValidNumber_WithOnlyPositive_ReturnsTrue(string expectedResultValue)
    {
        new NumberValidator(2, 1, true).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("-1", TestName = "number with minus")]
    [TestCase("+1", TestName = "number with plus")]
    [TestCase("1", TestName = "number without sign")]
    public void IsValidNumber_WithNotOnlyPositive_ReturnsTrue(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase(2, 0, true, "+1", TestName = "number with a plus")]
    [TestCase(3, 1, true, "+1.2", TestName = "number with a plus and separator dot")]
    [TestCase(3, 1, true, "+1,2", TestName = "number with a plus and separator comma")]
    public void IsValidNumber_WithCorrectPlus_ReturnsTrue(
        int precision,
        int scale,
        bool onlyPositive,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase(2, 0, "-1", TestName = "number with a minus")]
    [TestCase(3, 1, "-1.2", TestName = "number with a minus and separator dot")]
    [TestCase(3, 1, "-1,2", TestName = "number with a minus and separator comma")]
    public void IsValidNumber_WithCorrectMinus_ReturnsTrue(
        int precision,
        int scale,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase(1, 0, "0", TestName = "one digit")]
    [TestCase(3, 0, "123", TestName = "three digits")]
    [TestCase(3, 1, "12.3", TestName = "three digits with one number after the dot")]
    [TestCase(3, 2, "1.23", TestName = "three digits with two number after the dot")]
    public void IsValidNumber_WithCorrectNumberOfDigits_ReturnsTrue(
        int precision,
        int scale,
        string expectedResultValue)
    {
        new NumberValidator(precision, scale).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("0.1", TestName = "separator dot")]
    [TestCase("0,1", TestName = "separator comma")]
    public void IsValidNumber_WithCorrectSeparator_ReturnsTrue(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("0.1", TestName = "separator dot")]
    [TestCase("0,1", TestName = "separator comma")]
    [TestCase("1", TestName = "number")]
    [TestCase("+1", TestName = "number with plus")]
    [TestCase("-1", TestName = "number with minus")]
    public void IsValidNumber_WithStringCorrectFormat_ReturnsTrue(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeTrue();
    }

    [TestCase("", TestName = "empty string")]
    [TestCase(null, TestName = "null")]
    public void IsValidNumber_WithNullOrEmpty_ReturnsFalse(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase(".0", TestName = "intPart is missing")]
    [TestCase("0.", TestName = "fracPart part is missing")]
    [TestCase("0.0.0", TestName = "more than one separator")]
    [TestCase(" 1 ", TestName = "spaces on the sides")]
    [TestCase("1 . 1", TestName = "spaces on the sides of the separator")]
    [TestCase("1 1", TestName = "spaces between numbers")]
    [TestCase("1'1", TestName = "foreign separator (apostrophe)")]
    [TestCase("1`1", TestName = "foreign separator (backtick)")]
    [TestCase("1~1", TestName = "foreign separator (tilde)")]
    [TestCase("1/1", TestName = "foreign separator (slash)")]
    [TestCase("a", TestName = "not a number")]
    [TestCase("a.a", TestName = "not a number with separator dot")]
    [TestCase("a,a", TestName = "not a number with separator comma")]
    public void IsValidNumber_WithStringIncorrectFormat_ReturnsFalse(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase("+11", TestName = "more numbers than precision (intPart with plus)")]
    [TestCase("-1.1", TestName = "more numbers than precision (intPart and fracPart with minus)")]
    [TestCase("123", TestName = "more numbers than precision (only intPart)")]
    [TestCase("1.23", TestName = "more numbers than precision (intPart and fracPart)")]
    public void IsValidNumber_WithIncorrectPrecision_ReturnsFalse(string expectedResultValue)
    {
        new NumberValidator(2).IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase("0.12", TestName = "more digits in fracPart than scale")]
    [TestCase("+0.12", TestName = "more digits in fracPart than scale with plus")]
    [TestCase("-0.12", TestName = "more digits in fracPart than scale with minus")]
    public void IsValidNumber_WithIncorrectScale_ReturnsFalse(string expectedResultValue)
    {
        new NumberValidator(2, 1).IsValidNumber(expectedResultValue).Should().BeFalse();
    }

    [TestCase("-1", TestName = "number with minus")]
    [TestCase("-1.1", TestName = "number with minus and separator dot")]
    [TestCase("-1,1", TestName = "number with minus and separator comma")]
    public void IsValidNumber_WithOnlyPositive_ReturnsFalse(string expectedResultValue)
    {
        new NumberValidator(2, 1, true).IsValidNumber(expectedResultValue).Should().BeFalse();
    }
}