using NUnit.Framework;
using FluentAssertions;
using System;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Constructor_ShouldThrow_WhenPrecisionIsNegative")]
    [TestCase(1, -2, true, TestName = "Constructor_ShouldThrow_WhenScaleIsNegative")]
    [TestCase(1, 2, true, TestName = "Constructor_ShouldThrow_WhenScaleExceedsPrecision")]
    [TestCase(1, 1, true, TestName = "Constructor_ShouldThrow_WhenScaleEqualsPrecision")]
    [TestCase(0, 0, false, TestName = "Constructor_ShouldThrow_WhenPrecisionIsZero")]
    public void Constructor_ShouldThrowArgumentException_WhenInvalidArguments(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true, TestName = "Constructor_ShouldNotThrow_WhenValidArguments")]
    public void Constructor_ShouldNotThrow_WhenValidArguments(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().NotThrow();
    }

    [TestCase("0", true, 1, 0, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenSingleDigit")]
    [TestCase("0.1", true, 2, 1, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenValidNumberWithDecimalPoint")]
    [TestCase("0,1", true, 2, 1, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenValidNumberWithComma")]
    [TestCase("-1", true, 2, 0, false, TestName = "IsValidNumber_ShouldReturnTrue_WhenNegativeIntegerAllowed")]
    [TestCase("-1.1", true, 3, 1, false, TestName = "IsValidNumber_ShouldReturnTrue_WhenNegativeDecimalAllowed")]
    [TestCase("-1,1", true, 3, 1, false, TestName = "IsValidNumber_ShouldReturnTrue_WhenNegativeDecimalWithCommaAllowed")]
    [TestCase("+1", true, 2, 0, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenPositiveIntegerWithSign")]
    [TestCase("+1.1", true, 3, 1, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenPositiveDecimalWithSign")]
    [TestCase("+1,1", true, 3, 1, true, TestName = "IsValidNumber_ShouldReturnTrue_WhenPositiveDecimalWithComma")]
    [TestCase("", false, 2, 1, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenEmptyString")]
    [TestCase(".0", false, 2, 1, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenMissingIntegerPart")]
    [TestCase("0.", false, 2, 1, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenMissingFractionalPart")]
    [TestCase("-1", false, 2, 0, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenNegativeAndOnlyPositive")]
    [TestCase("a.a", false, 2, 1, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenNonNumericString")]
    [TestCase("1.1.1", false, 3, 2, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenMultipleDecimalPoints")]
    [TestCase("23,1", false, 2, 1, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenExceedsPrecisionWithComma")]
    [TestCase("23,1", false, 3, 0, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenExceedsScaleWithComma")] 
    [TestCase("+1", false, 1, 0, true, TestName = "IsValidNumber_ShouldReturnFalse_WhenSignExceedsPrecision")]
    public void IsValidNumber_ShouldReturnExpectedResult(string value, bool expectedResult, int precision, int scale, bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(value);
        result.Should().Be(expectedResult);
    }
}