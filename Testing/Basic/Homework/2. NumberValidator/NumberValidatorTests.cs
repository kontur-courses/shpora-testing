using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string InvalidPrecisionExceptionMessage = "precision must be a positive number";
    private const string InvalidScaleExceptionMessage = "precision must be a non-negative number less or equal than precision";

    [TestCase("0.0", 17, 2, true, ExpectedResult = true)]
    [TestCase("0", 17, 2, true, ExpectedResult = true)]
    [TestCase("000.00", 17, 2, true, ExpectedResult = true)]
    [TestCase("0,0", 17, 2, true, ExpectedResult = true)]
    public bool IsValidNumber_AfterCorrectNumberPattern(string number, int precision, int scale = 0,
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    } 
    
    [TestCase("00.00", 3, 2, true, ExpectedResult = false)]
    [TestCase("+0.00", 3, 2, true, ExpectedResult = false)]
    public bool IsValidNumber_ShouldBeIncorrect_AfterOverflowPrecision(string number, int precision, int scale = 0, 
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    }

    [TestCase("0.000", 17, 2, true, ExpectedResult = false)]
    [TestCase("0.0", 17, ExpectedResult = false)]
    public bool IsValidNumber_ShouldBeIncorrect_AfterOverflowScale(string number, int precision, int scale = 0, 
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    }

    [TestCase("-1.23", 4, 2, true, ExpectedResult = false)]
    [TestCase("-1.23", 4, 2, false, ExpectedResult = true)]
    [TestCase("+1.23", 4, 2, true, ExpectedResult = true)]
    public bool IsValidNumber_AfterDifferentValuesOfOnlyPositiveArg(string number, int precision, int scale = 0,
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    }

    [TestCase("   ", 4, 2, true, ExpectedResult = false)]
    [TestCase(null, 4, 2, true, ExpectedResult = false)]
    public bool IsValidNumber_ShouldBeIncorrect_AfterBlankNumber(string number, int precision, int scale = 0, 
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    }

    [TestCase("0.", 17, 2, true, ExpectedResult = false)]
    [TestCase(".0", 17, 2, true, ExpectedResult = false)]
    [TestCase("0..0", 17, 2, true, ExpectedResult = false)]
    [TestCase("+-3", 17, 2, true, ExpectedResult = false)]
    [TestCase("a.sd", 3, 2, true, ExpectedResult = false)]
    [TestCase("&0.0", 17, 2, true, ExpectedResult = false)]
    public bool IsValidNumber_ShouldBeIncorrect_AfterIncorrectRegex(string number, int precision, int scale = 0,
        bool onlyPositive = false)
    {
        return _validateNumber(number, precision, scale, onlyPositive);
    }

    [TestCase(InvalidPrecisionExceptionMessage, 0, 1, true, TestName = "precision == 0")]
    [TestCase(InvalidPrecisionExceptionMessage, -1, 1, true, TestName = "precision < 0")]
    [TestCase(InvalidScaleExceptionMessage, 1, -1, true, TestName = "scale < 0")]
    [TestCase(InvalidScaleExceptionMessage, 2, 2, true, TestName = "scale == precision")]
    [TestCase(InvalidScaleExceptionMessage, 1, 2, true, TestName = "scale > precision")]
    public void NumberValidator_ShouldThrowExc_AfterInvalidArgs(string expectedMessage, int precision, 
        int scale = 0, bool onlyPositive = false)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().Throw<ArgumentException>().WithMessage(expectedMessage);
    }
    
    [TestCase(3, 2, true)]
    [TestCase(3)]
    public void NumberValidator_ShouldNotThrowExc_AfterValidArgs(int precision, int scale = 0, bool onlyPositive = false)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }
    
    private static bool _validateNumber(string number, int precision, int scale = 0, bool onlyPositive = false) => 
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);
}