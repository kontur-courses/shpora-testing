
using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PrecisionErrorMessage = "precision must be a positive number";
    private const string ScaleErrorMessage = "precision must be a non-negative number less or equal than precision";

    [TestCase(-1, 0, PrecisionErrorMessage, TestName = "precision < 0")]
    [TestCase(-10, 1, PrecisionErrorMessage, TestName = "precision < 0")]
    [TestCase(0, 0, PrecisionErrorMessage, TestName = "precision == 0")]
    [TestCase(0, 2, PrecisionErrorMessage, TestName = "precision == 0")]
    [TestCase(5, -1, ScaleErrorMessage, TestName = "scale < 0")]
    [TestCase(5, -6, ScaleErrorMessage, TestName = "scale < 0")]
    [TestCase(1, 2, ScaleErrorMessage, TestName = "scale > precision")]
    [TestCase(1, 10, ScaleErrorMessage, TestName = "scale > precision")]
    [TestCase(1, 1, ScaleErrorMessage, TestName = "scale == precision")]
    [TestCase(2, 2, ScaleErrorMessage, TestName = "scale == precision")]
    public void NumberValidator_ThrowsArgumentException_WithInvalidParams(int precision, int scale, string msg)
    {
        Action act = () => new NumberValidator(precision, scale);
        act.Should().Throw<ArgumentException>().WithMessage(msg);
    }

    [TestCase("2", 1, 0, true, TestName = "When value is one digit integer")]
    [TestCase("52", 2, 1, true, TestName = "When value is two digit integer")]
    [TestCase("-300", 4, 0, false, TestName = "When value is integer with negative sign")]
    [TestCase("+300", 4, 0, true, TestName = "When value is integer with positive sign")]
    [TestCase("-99", 3, 2, false, TestName = "When value is integer with negative sign and non zero scale")]
    [TestCase("9999999", 7, 1, false, TestName = "When value is integer and has 7 digits and non zero scale")]
    [TestCase("2147483649", 10, 1, false, TestName = "When value is Int overflow")]
    [TestCase("-1234567890", 11, 8, false, TestName = "When value is integer with negative sign 10 digits and non zero scale")]
    [TestCase("0101010", 20, 12, false, TestName = "When value with leading zero")]
    [TestCase("01110.01010", 10, 5, true, TestName = "When value is fraction with leading zero in integer part and leading zero in fraction part and separator is dot")]
    [TestCase("01110,01010", 10, 5, true, TestName = "When value is fraction with leading zero in integer part and leading zero in fraction part and separator is comma")]
    [TestCase("+01110,01010", 11, 5, true, TestName = "When value is fraction with leading zero in integer part and leading zero in fraction part and separator is comma with positive sign")]
    [TestCase("-1234567890.1234567890", 21, 10, false, TestName = "When value is fraction with negative sign")]
    [TestCase("+0.0", 3, 1, false, TestName = "When value is zero with zero fraction with sign and separator is dot")]
    [TestCase("0.000", 4, 3, true, TestName = "When value is zero with zero fraction and separator is dot")]
    [TestCase("300.52", 5, 2, true, TestName = "When value is fraction and separator is dot")]
    [TestCase("+3.52", 15, 10, false, TestName = "When value is fraction with sign and separator is dot")]
    [TestCase("+300,52", 17, 10, false, TestName = "When value  is fraction and has sign and separator is comma")]
    [TestCase("1,23", 17, 10, false, TestName = "When value is fraction and separator is comma")]
    [TestCase("+100", 4, 1, true, TestName = "When value is integer with sign")]
    [TestCase("100\n", 4, 2, true, TestName = "When \n at the end of a value")]
    public void IsValidNumber_ShouldBeTrue_WithValidParams(string value, int precision, int scale, bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeTrue();

    [TestCase("", 17, 10, false, TestName = "When value is empty string")]
    [TestCase(null, 17, 10, false, TestName = "When value is null")]
    [TestCase("II", 17, 10, false, TestName = "When value is II in roman numeric system")]
    [TestCase("III", 17, 10, false, TestName = "When value is III in roman numeric system")]
    [TestCase("VI", 17, 10, false, TestName = "When value is IV in roman numeric system")]
    [TestCase("X", 17, 10, false, TestName = "When value is X in roman numeric system")]
    [TestCase("ఎనమద", 4, 0 , true, TestName = "When value is ఎనమద in telugu numeric system")]
    [TestCase("తమమద", 4, 0 , true, TestName = "When value is తమమద in telugu numeric system")]
    [TestCase("0xFF", 4, 0, true, TestName = "When is FF and is hexadecimal number")]
    [TestCase("0x1A", 4, 0, true, TestName = "When is 1A and is hexadecimal number")]
    [TestCase("0o75", 4, 0, true, TestName = "When value is 75 and is octal number")]
    [TestCase("0o44", 4, 0, true, TestName = "When value is 44 and is octal number")]
    [TestCase("0b1010", 6, 0, true, TestName = "When value is 1010 and is binary number")]
    [TestCase("0b01110.01010", 10, 5, true, TestName = "When value is binary number with fraction")]
    [TestCase("seven.eleven", 17, 10, false, TestName = "When value is two english words separated by dot")]
    [TestCase("five/two", 17, 10, false, TestName = "When value is two english words separated by dot")]
    [TestCase("11.11.11", 17, 10, false, TestName = "When value has three dots")]
    [TestCase("+127.000.001", 17, 10, false, TestName = "When value has three dots and sign")]
    [TestCase("11.22,33", 17, 10, false, TestName = "When value has one dot and one comma")]
    [TestCase("55,22,33", 17, 10, false, TestName = "When value has two commas")]
    [TestCase("+1\\2", 17, 10, false, TestName = "When value has sign and \\ separator")]
    [TestCase("X,Y", 3, 2, true, TestName = "When value is two capital letters with comma separator")]
    [TestCase("a.sd", 17, 10, false, TestName = "When value have letters with dot separator")]
    [TestCase("1\n.5", 3, 2, true, TestName = "When \n in the middle of a value")]
    [TestCase("\n2.4", 3, 2, true, TestName = "When \n is in front of a value")]
    [TestCase("+-15", 3, 2, false, TestName = "When value has two different signs")]
    [TestCase("--10", 4, 2, false, TestName = "When value has two negative signs")]
    [TestCase("++1", 3, 0, true, TestName = "When value has two positive signs")]
    [TestCase("300_000", 17, 10, false, TestName = "When value has underscore separator")]
    [TestCase("52 000", 17, 10, false, TestName = "When value has space separator")]
    [TestCase("1/2", 17, 10, false, TestName = "When value has / separator")]
    [TestCase("12 . 22", 17, 10, false, TestName = "When value has `space dot space` separator")]
    [TestCase("12 , 22", 17, 10, false, TestName = "When value has `space comma space` separator")]
    [TestCase("-100", 4, 0, true, TestName = "When value is negative and onlyPositive is true")]
    [TestCase("+1,23", 3, 2, true, TestName = "When value is 3 digit fraction with sign and precision is 3 (Not 4)")]
    [TestCase("1.123", 4, 2, true, TestName = "When value is fraction with 3 digit in fraction part and scale is 2 (Not 3)")]
    [TestCase("0.00", 5, 1, true, TestName = "When value is fraction with 2 digit in fraction part and scale is 1 (Not 2)")]
    [TestCase("52", 1, 0, true, TestName = "When value is 2 digit integer and precision is 1 (Not 2)")]
    [TestCase("-300", 3, 0, false, TestName = "When value is 3 digit integer with negative sign and precision is 3 (Not 4)")]
    [TestCase("300", 2, 0, false, TestName = "When value is 3 digit integer and precision is 2 (Not 3)")] 
    [TestCase("+100", 3, 0, false, TestName = "When value is 3 digit integer with positive sign and precision is 3 (Not 4)")] 
    [TestCase("-1.23", 3, 2, false, TestName = "When value is 3 digit fraction with negative sign and precision is 3 (Not 4)")]
    [TestCase("-1234567890.1234567890", 20, 10, false, TestName = "When value is 20 digit fraction with negative sign and precision is 20 (Not 21)")]
    public void IsValidNumber_ShouldBeFalse_WithInvalidParams(string value, int precision, int scale, 
        bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeFalse();
}