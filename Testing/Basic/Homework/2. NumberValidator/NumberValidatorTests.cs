
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

    [TestCase("2", 1, 0, true, TestName = "2 should be valid number")]
    [TestCase("52", 2, 1, true, TestName = "52 should be valid number")]
    [TestCase("-300", 4, 0, false, TestName = "-300 should be valid number")]
    [TestCase("+300", 4, 0, true, TestName = "+300 should be valid number")]
    [TestCase("-99", 3, 2, false, TestName = "-99 should be valid number")]
    [TestCase("9999999", 7, 1, false, TestName = "9999999 should be valid number")]
    [TestCase("2147483649", 10, 1, false, TestName = "Int overflow should be valid number")]
    [TestCase("-1234567890", 11, 8, false, TestName = "-1234567890 should be valid number")]
    [TestCase("0101010", 20, 12, false, TestName = "0101010 should be valid number")]
    [TestCase("01110.01010", 10, 5, true, TestName = "01110.01010 should be valid number")]
    [TestCase("01110,01010", 10, 5, true, TestName = "01110,01010 should be valid number")]
    [TestCase("+01110,01010", 11, 5, true, TestName = "+01110,01010 should be valid number")]
    [TestCase("-1234567890.1234567890", 21, 10, false, TestName = "-1234567890.1234567890 should be valid number")]
    [TestCase("+0.0", 3, 1, false, TestName = "+0.0 should be valid number")]
    [TestCase("0.000", 4, 3, true, TestName = "0.000 should be valid number")]
    [TestCase("300.52", 5, 2, true, TestName = "300.52 should be valid number")]
    [TestCase("+3.52", 15, 10, false, TestName = "+3.52 should be valid number")]
    [TestCase("+300,52", 17, 10, false, TestName = "+300,52 should be valid number")]
    [TestCase("1,23", 17, 10, false, TestName = "1,23 should be valid number")]
    [TestCase("+100", 4, 1, true, TestName = "+100 should be valid number")]
    [TestCase("+44", 3, 0, true, TestName = "+44 should be valid number")]
    [TestCase("100\n", 4, 2, true, TestName = "100\n should be valid number")]
    public void IsValidNumber_ShouldBeTrue_WithValidParams(string value, int precision, int scale, bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeTrue();

    [TestCase("", 17, 10, false, TestName = "empty string should be invalid number")]
    [TestCase(null, 17, 10, false, TestName = "null should be invalid number")]
    [TestCase("II", 17, 10, false, TestName = "II should be invalid number")]
    [TestCase("III", 17, 10, false, TestName = "III should be invalid number")]
    [TestCase("VI", 17, 10, false, TestName = "VI should be invalid number")]
    [TestCase("X", 17, 10, false, TestName = "X should be invalid number")]
    [TestCase("ఎనమద", 4, 0 , true, TestName = "ఎనమద should be invalid number")]
    [TestCase("తమమద", 4, 0 , true, TestName = "తమమద should be invalid number")]
    [TestCase("0xFF", 4, 0, true, TestName = "0xFF should be invalid number")]
    [TestCase("0x1A", 4, 0, true, TestName = "0x1A should be invalid number")]
    [TestCase("0o75", 4, 0, true, TestName = "0o75 should be invalid number")]
    [TestCase("0o44", 4, 0, true, TestName = "0o44 should be invalid number")]
    [TestCase("0b1010", 6, 0, true, TestName = "0b1010 should be invalid number")]
    [TestCase("0b01110.01010", 10, 5, true, TestName = "0b01110.1010 should be invalid number")]
    [TestCase("seven.eleven", 17, 10, false, TestName = "seven eleven should be invalid number")]
    [TestCase("five/two", 17, 10, false, TestName = "five/two should be invalid number")]
    [TestCase("11.11.11", 17, 10, false, TestName = "11.11.11 should be invalid number")]
    [TestCase("+127.000.001", 17, 10, false, TestName = "127.000.001 should be invalid number")]
    [TestCase("11.22,33", 17, 10, false, TestName = "11.22.33 should be invalid number")]
    [TestCase("55,22,33", 17, 10, false, TestName = "55.22.33 should be invalid number")]
    [TestCase("+1\\2", 17, 10, false, TestName = "+1\\2 should be invalid number")]
    [TestCase("X,Y", 3, 2, true, TestName = "X,Y should be invalid number")]
    [TestCase("1\n.5", 3, 2, true, TestName = "1\n.5 should be invalid number")]
    [TestCase("\n2.4", 3, 2, true, TestName = "\n2.4 should be invalid number")]
    [TestCase("+-15", 3, 2, false, TestName = "+-15 should be invalid number")]
    [TestCase("--10", 4, 2, false, TestName = "--10 should be invalid number")]
    [TestCase("++1", 3, 0, true, TestName = "++1 should be invalid number")]
    [TestCase("300_000", 17, 10, false, TestName = "300_000 should be invalid number")]
    [TestCase("52 000", 17, 10, false, TestName = "52 000 should be invalid number")]
    [TestCase("1/2", 17, 10, false, TestName = "1/2 should be invalid number")]
    [TestCase("12 . 22", 17, 10, false, TestName = "12 . 22 should be invalid number")]
    [TestCase("12 , 22", 17, 10, false, TestName = "12 , 22 should be invalid number")]
    [TestCase("a.sd", 17, 10, false, TestName = "a.sd should be invalid number")]
    [TestCase("-100", 4, 0, true, TestName = "-100 should be invalid number because onlyPositive is true")]
    [TestCase("+1,23", 3, 2, true, TestName = "+1.23 should be invalid number because precision < 4")]
    [TestCase("1.123", 4, 2, true, TestName = "1.123 should be invalid number because scale < 3")]
    [TestCase("0.00", 5, 1, true, TestName = "0.000 should be invalid number because scale < 2")]
    [TestCase("52", 1, 0, true, TestName = "52 should be invalid number because precision < 2")]
    [TestCase("-300", 3, 0, false, TestName = "-300 should be invalid number because precision < 4")]
    [TestCase("300", 2, 0, false, TestName = "300 should be invalid number because precision < 3")] 
    [TestCase("+100", 3, 0, false, TestName = "+100 should be invalid number because precision < 4")] 
    [TestCase("-1.23", 3, 2, false, TestName = "-1.23 should be invalid number because precision < 4")]
    [TestCase("-1234567890.1234567890", 20, 10, false, TestName = "-1234567890.1234567890 should be invalid number because precision < 21")]
    public void IsValidNumber_ShouldBeFalse_WithInvalidParams(string value, int precision, int scale, 
        bool onlyPositive) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeFalse();
}