using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, TestName = "InvalidPrecision")]
    [TestCase(1, -1, TestName = "InvalidScale")]
    [TestCase(2, 3, TestName = "ScaleIsBiggerThanPrecision")]
    public void ShouldThrowArgumentException_When(int precision, int scale)
    {
        var func = () => new NumberValidator(precision, scale);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ShouldNotThrow_WhenValid()
    {
        var func = () => new NumberValidator(1, 0, true);
        func.Should().NotThrow();
    }

    [TestCase("", false, true, TestName = "EmptyInput")]
    [TestCase(null, false, true, TestName = "NullInput")]
    [TestCase("12345", true, true, TestName = "ValidIntegerAndOnlyPositive")]
    [TestCase("123.45", true, true, TestName = "ValidDecimalAndOnlyPositive")]
    [TestCase("-123.45", false, true, TestName = "NegativeDecimalAndOnlyPositive")]
    [TestCase("001.23", true, true, TestName = "LeadingZerosAndValidDecimal")]
    [TestCase("0", true, true, TestName = "Zero")]
    [TestCase("0.0", true, true, TestName = "ZeroDecimal")]
    [TestCase("+123.45", false, true, TestName = "PositiveSign")]
    [TestCase("123456", false, true, TestName = "LongerThanPrecision")]
    [TestCase("123.456", false, true, TestName = "TooManyDecimals")]
    [TestCase("0000.00", false, true, TestName = "LeadingZerosLongerThanPrecision")]
    [TestCase("0.000", false, true, TestName = "TooManyZeroesDecimal")]
    [TestCase("ab.c", false, true, TestName = "InvalidCharacters")]
    [TestCase("123.45.67", false, true, TestName = "MultipleDots")]
    [TestCase("123,67", true, true, TestName = "CommaAsSeparator")]
    [TestCase("123 45", false, true, TestName = "SpaceInNumber")]
    [TestCase("++1", false, true, TestName = "DoublePositiveSign")]
    [TestCase("1.", false, true, TestName = "DecimalWithoutDigitAfterDot")]
    [TestCase("12345", true, false, TestName = "ValidIntegerAndAllowNegative")]
    [TestCase("123.45", true, false, TestName = "ValidDecimalAndAllowNegative")]
    [TestCase("123.45a", false, false, TestName = "InvalidCharactersAndAllowNegative")]
    [TestCase("12.3.4", false, false, TestName = "MultipleDotsAndAllowNegative")]
    [TestCase("-0", true, false, TestName = "NegativeZero")]
    [TestCase("-12.45", true, false, TestName = "NegativeDecimal")]
    [TestCase("-123.45", false, false, TestName = "NegativeDecimalLongerThanPrecision")]
    [TestCase("--1", false, false, TestName = "DoubleNegativeSign")]
    [TestCase("-1.", false, false, TestName = "NegativeDecimalWithoutDigitAfterDot")]
    public void ShouldReturnExpectedValidationResult_When(string input, bool expected, bool onlyPositive)
    {
        var validator = new NumberValidator(5, 2, onlyPositive);
        var result = validator.IsValidNumber(input);
        result.Should().Be(expected, $"Input '{input}' did not return expected result '{expected}'.");
    }
}