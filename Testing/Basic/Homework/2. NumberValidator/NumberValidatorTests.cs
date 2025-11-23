using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, TestName = "Precision should not be <= 0")]
    [TestCase(1, -1, TestName = "Scale should not be < 0")]
    [TestCase(1, 1, TestName = "Scale should not be >= precision")]
    public void NumberValidator_ThrowsArgumentException_WhenIncorrectConditions(int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase(1, 0, TestName = "Precision should be > 0. Scale should be >= 0 and < precision")]
    public void NumberValidator_NotThrowsException_WhenCorrectConditions(int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale);
        
        action.Should().NotThrow();
    }
    
    [TestCase(17, 2, true, "0.0", TestName = "Fractional number with dot")]
    [TestCase(17, 2, true, "0,0", TestName = "Fractional number with comma")]
    [TestCase(3, 2, false, "-1.1", TestName = "Negative fractional number with dot")]
    [TestCase(3, 2, false, "-1,1", TestName = "Negative fractional number with comma")]
    [TestCase(17, 2, true, "0", TestName = "Integer")]
    [TestCase(3, 2, false, "-00", TestName = "Negative integer")]
    [TestCase(3, 2, false, "-0.0", TestName = "Negative fractional number")]
    [TestCase(4, 2, true, "+12", TestName = "Positive signed integer")]
    [TestCase(4, 2, true, "+1.23", TestName = "Positive signed fractional number")]
    public void IsValidNumber_ReturnsTrue_WhenCorrectConditions(int precision, int scale, bool onlyPositive, string number)
    {
        var result = new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);

        result.Should().Be(true);
    }
    
    [TestCase(3, 2, true, "  \n", TestName = "WhiteSpace")]
    [TestCase(3, 2, true, null, TestName = "Null")]
    [TestCase(3, 2, true, "", TestName = "Empty string")]
    [TestCase(3, 2, true, "0.", TestName = "Fractional part missing")]
    [TestCase(3, 2, true, ".0", TestName = "Integer part is missing")]
    [TestCase(3, 2, true, "0000", TestName = "Integer part exceeds precision")]
    [TestCase(3, 2, true, "+000", TestName = "Integer part exceeds precision. Sign has weight")]
    [TestCase(3, 2, true, "-00", TestName = "Negative signed integer, but only positive numbers are allowed")]
    [TestCase(3, 2, true, "00.00", TestName = "Number of digits exceeds precision")]
    [TestCase(3, 2, true, "+0.00", TestName = "Number of digits and sign exceeds precision")]
    [TestCase(3, 2, true, "-0.0", TestName = "Negative signed fractional number, but only positive numbers are allowed")]
    [TestCase(17, 2, true, "0.000", TestName = "Fractional part exceeds scale")]
    [TestCase(3, 2, true, "a.sd", TestName = "Letters")]
    [TestCase(20, 2, true, "++123", TestName = "Two signs")]
    [TestCase(20, 2, true, "+12..3", TestName = "Two dots")]
    [TestCase(20, 2, true, "1320.a2", TestName = "Numbers and letters")]
    public void IsValidNumber_ReturnsFalse_WhenIncorrectConditions(int precision, int scale, bool onlyPositive, string number)
    {
        var result = new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);

        result.Should().Be(false);
    }
}