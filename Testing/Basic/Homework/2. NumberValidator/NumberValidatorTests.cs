using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", TestName = "IsValidNumber_ShouldBeTrue_WhenZeroDecimalWithinPrecisionAndScale")]
    [TestCase(17, 2, true, "0", TestName = "IsValidNumber_ShouldBeTrue_WhenZeroWithoutFraction")]
    [TestCase(4, 2, true, "+1.23", TestName = "IsValidNumber_ShouldBeTrue_WhenPositiveNumberWithinPrecisionAndScale")]
    [TestCase(5, 2, false, "-1.23", TestName = "IsValidNumber_ShouldBeTrue_WhenNegativeNumberAllowedAndWithinPrecision")]
    [TestCase(3, 1, false, "9.9", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionExactlyMatchesNumberLength")]
    [TestCase(5, 0, false, "-1234", TestName = "IsValidNumber_ShouldBeTrue_WhenIntegerWithinPrecisionWithoutFraction")]
    [TestCase(5, 2, false, "999.99", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionAndScaleAtUpperBound")]
    [TestCase(20, 5, false, "9223372036854775808", TestName = "IsValidNumber_ShouldBeTrue_WhenNumberExceedsLongMaxValue")]
    [TestCase(50, 10, false, "12345678901234567890.1234567890", TestName = "IsValidNumber_ShouldBeTrue_WhenVeryLongNumberWithinPrecision")]
    [TestCase(5, 2, false, "1,23", TestName = "IsValidNumber_ShouldBeTrue_WhenCommaUsedAsDecimalSeparator")]
    [TestCase(3, 1, true, "+0,5", TestName = "IsValidNumber_ShouldBeTrue_WhenPositiveSignAndCommaUsed")]
    [TestCase(4, 2, false, "-0,12", TestName = "IsValidNumber_ShouldBeTrue_WhenNegativeWithCommaAndWithinPrecision")]
    [TestCase(4, 2, false, "1,00", TestName = "IsValidNumber_ShouldBeTrue_WhenFractionContainsZerosAndComma")]
    [TestCase(3, 1, false, "0,0", TestName = "IsValidNumber_ShouldBeTrue_WhenZeroWithCommaSeparator")]
    [TestCase(6, 2, false, "123,45", TestName = "IsValidNumber_ShouldBeTrue_WhenCommaFractionWithinPrecisionAndScale")]
    [TestCase(1, 0, false, "5", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionIsOneAndSingleDigit")]
    [TestCase(1, 0, false, "9", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionIsOneAndMaxDigit")]
    [TestCase(4, 2, false, "123\n", TestName = "IsValidNumber_ShouldBeTrue_When\\nAtEnd")]
    [TestCase(10, 2, false, "１２３", TestName = "IsValidNumber_ShouldBeTrue_WhenJapaneseDigitsUsed")]
    [TestCase(10, 2, false, "１２３.４５", TestName = "IsValidNumber_ShouldBeTrue_WhenJapaneseDigitsWithDotUsed")]
    [TestCase(10, 2, false, "١٢٣", TestName = "IsValidNumber_ShouldBeTrue_WhenUrduDigitsUsed")]
    [TestCase(10, 2, false, "१२३", TestName = "IsValidNumber_ShouldBeTrue_WhenDevanagariDigitsUsed")]
    [TestCase(10, 2, false, "１２3", TestName = "IsValidNumber_ShouldBeTrue_WhenJapaneseAndArabicDigitsMixed")]
    [TestCase(10, 2, false, "1٢3", TestName = "IsValidNumber_ShouldBeTrue_WhenArabicAndUrduDigitsMixed")]
    public void IsValidNumber_ShouldBeTrue_WhenParametersValid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(true);
    }
    
    [TestCase(3, 2, true, "00.00", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededByLeadingZeros")]
    [TestCase(3, 2, true, "-0.00", TestName = "IsValidNumber_ShouldBeFalse_WhenNegativeAndOnlyPositiveAllowed")]
    [TestCase(3, 2, true, "+0.00", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededBySign")]
    [TestCase(3, 2, true, "+1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberExceedsPrecision")]
    [TestCase(17, 2, true, "0.000", TestName = "IsValidNumber_ShouldBeFalse_WhenFractionalPartExceedsScale")]
    [TestCase(3, 2, true, "-1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenNegativeAndOnlyPositiveAllowed")]
    [TestCase(10, 2, false, null, TestName = "IsValidNumber_ShouldBeFalse_WhenValueIsNull")]
    [TestCase(10, 2, false, "", TestName = "IsValidNumber_ShouldBeFalse_WhenValueIsEmptyString")]
    [TestCase(10, 2, false, "abc", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsLetters")]
    [TestCase(10, 2, false, "1..2", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsTwoDots")]
    [TestCase(10, 2, false, "1.", TestName = "IsValidNumber_ShouldBeFalse_WhenEndsWithDotWithoutFraction")]
    [TestCase(10, 2, false, "1.2.3", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsMultipleFractionSeparators")]
    [TestCase(10, 2, false, "a.sd", TestName = "IsValidNumber_ShouldBeFalse_WhenInvalidCharactersUsed")]
    [TestCase(5, 2, false, "123456", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededByIntegerPart")]
    [TestCase(3, 0, false, "1234", TestName = "IsValidNumber_ShouldBeFalse_WhenIntegerPartLongerThanPrecision")]
    [TestCase(5, 2, false, "000.000", TestName = "IsValidNumber_ShouldBeFalse_WhenTotalDigitsExceedPrecision")]
    [TestCase(10, 2, false, ".1", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberStartsWithDot")]
    [TestCase(10, 2, false, ",1", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberStartsWithComma")]
    [TestCase(10, 2, false, "1,", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberEndsWithComma")]
    [TestCase(10, 2, false, "1.2a", TestName = "IsValidNumber_ShouldBeFalse_WhenFractionContainsLetter")]
    [TestCase(10, 2, false, " 1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberStartsWithSpace")]
    [TestCase(10, 2, false, "1. 23", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberContainsSpaceInside")]
    [TestCase(10, 2, false, "1_23", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberContainsUnderline")]
    [TestCase(10, 2, false, "1.2345", TestName = "IsValidNumber_ShouldBeFalse_WhenFractionTooLong")]
    [TestCase(10, 2, false, "+-1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenDoubleSignUsed")]
    [TestCase(10, 2, false, "--1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenDoubleMinusSignUsed")]
    [TestCase(10, 2, false, "++1", TestName = "IsValidNumber_ShouldBeFalse_WhenDoublePlusSignUsed")]
    [TestCase(10, 2, false, "1..", TestName = "IsValidNumber_ShouldBeFalse_WhenTwoDotsAtEnd")]
    [TestCase(10, 2, false, ".,1", TestName = "IsValidNumber_ShouldBeFalse_WhenStartsWithDotComma")]
    [TestCase(10, 2, false, "+", TestName = "IsValidNumber_ShouldBeFalse_WhenOnlySignUsed")]
    [TestCase(10, 2, false, "-", TestName = "IsValidNumber_ShouldBeFalse_WhenOnlyMinusUsed")]
    [TestCase(1, 0, false, "10", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionIsOneButTwoDigits")]
    [TestCase(5, 0, false, "1.2", TestName = "IsValidNumber_ShouldBeFalse_WhenScaleZeroButFractionUsed")]
    [TestCase(10, 2, false, "ابج", TestName = "IsValidNumber_ShouldBeFalse_WhenAbjadDigitsUsed")]
    [TestCase(10, 2, false, "１２３．４５", TestName = "IsValidNumber_ShouldBeTrue_WhenJapaneseDigitsAndJapaneseDotUsed")]
    [TestCase(10, 2, true, "123\n.1", TestName = "IsValidNumber_ShouldBeFalse_When\\nBeforeDot")]
    [TestCase(10, 2, true, "123.\n1", TestName = "IsValidNumber_ShouldBeFalse_When\\nAfterDot")]
    [TestCase(10, 2, true, "\n123.1", TestName = "IsValidNumber_ShouldBeFalse_When\\nBeforeNumber")]
    [TestCase(10, 2, false, "12345678901234567890.1234567890", TestName = "IsValidNumber_ShouldBeFalse_WhenVeryLongNumberExceedsPrecision")]
    [TestCase(15, 2, false, "12345678901234567890.1234567890", TestName = "IsValidNumber_ShouldBeFalse_WhenIntegerPartExceedsPrecisionAndScaleValid")]
    [TestCase(10, 2, false, "四", TestName = "IsValidNumber_ShouldBeFalse_WhenSingleKanjiDigitUsed")]
    [TestCase(10, 2, false, "四五六", TestName = "IsValidNumber_ShouldBeFalse_WhenKanjiDigitsUsed")]
    [TestCase(10, 2, false, "一二三", TestName = "IsValidNumber_ShouldBeFalse_WhenChineseDigitsUsed")]
    [TestCase(10, 2, false, "123六", TestName = "IsValidNumber_ShouldBeFalse_WhenArabicAndKanjiDigitsMixed")]
    [TestCase(10, 2, false, "1一3", TestName = "IsValidNumber_ShouldBeFalse_WhenLatinAndKanjiDigitsMixed")]
    public void IsValidNumber_ShouldBeFalse_WhenParametersInvalid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(false);
    }
    
    [TestCase(-1, 2, true, TestName = "Constructor_ShouldThrow_WhenPrecisionNegative")]
    [TestCase(1, -1, false, TestName = "Constructor_ShouldThrow_WhenScaleNegative")]
    [TestCase(1, 1, false, TestName = "Constructor_ShouldThrow_WhenScaleEqualsPrecision")]
    [TestCase(0, 2, false, TestName = "Constructor_ShouldThrow_WhenPrecisionZero")]
    [TestCase(2, 5, false, TestName = "Constructor_ShouldThrow_WhenScaleGreaterThanPrecision")]
    [TestCase(0, -1, false, TestName = "Constructor_ShouldThrow_WhenPrecisionZeroAndScaleNegative")]
    [TestCase(-5, -5, false, TestName = "Constructor_ShouldThrow_WhenBothPrecisionAndScaleNegative")]
    public void Constructor_ShouldThrowException_WhenPrecisionOrScaleInvalid(int precision, int scale, bool onlyPositive)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().Throw<ArgumentException>();
    }
    
    [TestCase(1, 0, true, TestName = "Constructor_ShouldNotThrow_WhenMinimalValidPrecisionAndScale")]
    [TestCase(5, 2, false, TestName = "Constructor_ShouldNotThrow_WhenCommonValidValues")]
    [TestCase(10, 0, true, TestName = "Constructor_ShouldNotThrow_WhenScaleIsZero")]
    [TestCase(10, 9, false, TestName = "Constructor_ShouldNotThrow_WhenScaleJustBelowPrecision")]
    public void Constructor_ShouldNotThrow_WhenParametersValid(int precision, int scale, bool onlyPositive)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().NotThrow();
    }
}