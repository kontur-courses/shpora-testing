using NUnit.Framework;
using FluentAssertions;
using System;

namespace HomeExercise.Tasks.NumberValidator;

public class NumberValidatorTests
{
    private const string ConstructorTestNamePrefix = "Constructor_Should";
    private const string IsValidNumberTestNamePrefix = "IsValidNumber_ShouldReturn";
    
    [TestCase(-1, 2, true, TestName = $"{ConstructorTestNamePrefix}Throw_WhenPrecisionIsNegative")]
    [TestCase(1, -2, true, TestName = $"{ConstructorTestNamePrefix}Throw_WhenScaleIsNegative")]
    [TestCase(1, 2, true, TestName = $"{ConstructorTestNamePrefix}Throw_WhenScaleExceedsPrecision")]
    [TestCase(1, 1, true, TestName = $"{ConstructorTestNamePrefix}Throw_WhenScaleEqualsPrecision")]
    [TestCase(0, 0, false, TestName = $"{ConstructorTestNamePrefix}Throw_WhenPrecisionIsZero")]
    public void Constructor_ShouldThrowArgumentException_WhenInvalidArguments(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }
    
    [TestCase(1, 0, true, TestName = $"{ConstructorTestNamePrefix}NotThrow_WhenValidArguments")]
    public void Constructor_ShouldNotThrow_WhenValidArguments(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().NotThrow();
    }
    
    [TestCase("0", 1, 0, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenSingleDigit")]
    [TestCase("0.1", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenValidNumberWithDecimalPoint")]
    [TestCase("0,1", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenValidNumberWithComma")]
    [TestCase("-1", 2, 0, false, TestName = $"{IsValidNumberTestNamePrefix}True_WhenNegativeIntegerAllowed")]
    [TestCase("-1.1", 3, 1, false, TestName = $"{IsValidNumberTestNamePrefix}True_WhenNegativeDecimalAllowed")]
    [TestCase("-1,1", 3, 1, false, TestName = $"{IsValidNumberTestNamePrefix}True_WhenNegativeDecimalWithCommaAllowed")]
    [TestCase("+1", 2, 0, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenPositiveIntegerWithSign")]
    [TestCase("+1.1", 3, 1, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenPositiveDecimalWithSign")]
    [TestCase("+1,1", 3, 1, true, TestName = $"{IsValidNumberTestNamePrefix}True_WhenPositiveDecimalWithComma")]
    public void IsValidNumber_ShouldReturnTrue(string value, int precision, int scale, bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(value);
        result.Should().Be(true);
    }

    [TestCase("", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenEmptyString")]
    [TestCase(".0", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenMissingIntegerPart")]
    [TestCase("0.", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenMissingFractionalPart")]
    [TestCase("-1", 2, 0, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenNegativeAndOnlyPositive")]
    [TestCase("a.a", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenNonNumericString")]
    [TestCase("1.1.1", 3, 2, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenMultipleDecimalPoints")]
    [TestCase("23,1", 2, 1, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenExceedsPrecisionWithComma")]
    [TestCase("23,1", 3, 0, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenExceedsScaleWithComma")]
    [TestCase("+1", 1, 0, true, TestName = $"{IsValidNumberTestNamePrefix}False_WhenSignExceedsPrecision")]
    public void IsValidNumber_ShouldReturnFalse(string value, int precision, int scale, bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(value);
        result.Should().Be(false);
    }
}