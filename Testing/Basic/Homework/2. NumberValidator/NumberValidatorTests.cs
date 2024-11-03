using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-2, 2, true)]
    [TestCase(-2, 2, false)]
    public void Constructor_ShouldThrowArgumentException_WhenPrecisionIsNegative(
        int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 2, true)]
    [TestCase(1, 2, false)]
    public void Constructor_ShouldThrowArgumentException_WhenScaleGreaterThanPrecision(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(2, -1, true)]
    [TestCase(2, -1, false)]
    public void Constructor_ShouldThrowArgumentException_WhenScaleIsLessThatZero(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(2, 2, true)]
    [TestCase(2, 2, false)]
    public void NumberValidator_ShouldThrowArgumentException_WhenScaleEqualPrecision(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true)]
    [TestCase(1, 0, false)]
    public void Constructor_ShouldNotThrow_WithValidParameters(
        int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().NotThrow();
    }

    [TestCase("0.0", 15, 2, true, true, TestName = "Simple decimal")]
    [TestCase("0", 15, 2, true, true, TestName = "Integer only")]
    [TestCase("00.00", 3, 2, true, false, TestName = "Leading zeros - invalid")]
    [TestCase("-0.00", 3, 2, true, false, TestName = "Negative zero - invalid")]
    [TestCase("+0.00", 3, 2, true, false, TestName = "Positive zero with sign - invalid")]
    [TestCase("+1.23", 4, 2, true, true, TestName = "Positive number with sign - valid length")]
    [TestCase("+1.23", 3, 2, true, false, TestName = "Positive number with sign - invalid length")]
    [TestCase("0.000", 15, 2, true, false, TestName = "Too many fractional digits")]
    [TestCase("-1.23", 3, 2, false, false, TestName = "Negative number - invalid length")]
    [TestCase("-1.23", 4, 2, true, false, TestName = "Negative number - not allowed")]
    [TestCase("a.sd", 3, 2, true, false, TestName = "Non-numeric input")]
    [TestCase("", 2, 1, true, false, TestName = "Empty input")]
    public void IsValidNumber_ShouldValidateCorrectly(
        string number,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {

        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(number);
        result.Should().Be(expectedResult);
    }

    [TestCase(".123", 4, 2, true, false, TestName = "Missing leading zero")]
    [TestCase("1.", 4, 2, true, false, TestName = "Missing trailing digits")]
    [TestCase("999.99", 4, 2, true, false, TestName = "Exceeding total digits")]
    [TestCase("1,23", 4, 2, true, true, TestName = "Another decimal separator")]
    public void IsValidNumber_AdditionalCases(
        string number,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(number);
        result.Should().Be(expectedResult);
    }
}
