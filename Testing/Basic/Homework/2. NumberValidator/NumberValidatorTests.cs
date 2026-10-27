using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    // [Test]
    // public void Test()
    // {
    //     Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
    //     Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    //     Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, false));
    //     Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    //
    //     ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //     ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
    //     ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
    //     ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
    //     ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
    //     ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
    //     ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    // }

    // Original cases
    [TestCase(17, 2, true, "0.0", true, Description = "Correct positive fractional number")]
    [TestCase(17, 2, true, "0", true, Description = "Correct positive number")]
    [TestCase(17, 2, true, "0.000", false, Description = "Positive fractional number with fractional part excess")]
    [TestCase(3, 2, true, "00.00", false, Description = "Precision excess in positive fractional number")]
    [TestCase(3, 2, true, "-1.23", false, Description = "Precision excess in negative fractional number due to minus sign")]
    [TestCase(3, 2, true, "+1.23", false, Description = "Precision excess in positive fractional number due to plus sign")]
    [TestCase(3, 2, true, "a.sd", false, Description = "Incorrect number format: letters expected")]
    [TestCase(4, 2, true, "+1.23", true, Description = "Correct positive fractional number")]
    // Own cases
    [TestCase(4, 2, false, "-1.23", true, Description = "Negative number is allowed when onlyPositive is false")]
    [TestCase(4, 2, true, "-1.23", false, Description = "Negative number is forbidden when onlyPositive is true")]
    [TestCase(4, 0, true, "-123", false, Description = "Negative integer is forbidden when onlyPositive is true")]
    [TestCase(4, 2, false, "12,34", true, Description = "Correct positive fractional number with comma as separator")]
    [TestCase(3, 1, false, "1.23", false, Description = "Fractional part exceeds scale")]
    [TestCase(3, 0, false, "1.0", false, Description = "Fractional number is invalid when scale is zero")]
    [TestCase(4, 2, false, null, false, Description = "Null input")]
    [TestCase(4, 2, false, "", false, Description = "Empty input")]
    [TestCase(4, 2, false, " ", false, Description = "Whitespace input")]
    [TestCase(4, 2, false, "1.", false, Description = "Missing fractional part")]
    [TestCase(4, 2, false, "+", false, Description = "Sign without digits")]
    public void NumberValidator_IsValidNumber_ReturnsExpectedResult(int precision, int scale, bool onlyPositive, 
        string inputValue, bool isValid)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        numberValidator.IsValidNumber(inputValue).Should().Be(isValid);
    }
    
    // Original cases
    [TestCase(-1, 2, true, Description = "Negative precision with onlyPositive enabled")]
    [TestCase(-1, 2, false, Description = "Negative precision with onlyPositive disabled")]
    // Own cases
    [TestCase(0, 0, false, Description = "Zero precision")]
    [TestCase(1, -1, false, Description = "Negative scale")]
    [TestCase(1, 1, false, Description = "Scale equals precision")]
    [TestCase(1, 2, false, Description = "Scale exceeds precision")]
    public void NumberValidator_Constructor_ThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var numberValidatorInitialization = () => new NumberValidator(precision, scale, onlyPositive);
        numberValidatorInitialization.Should().Throw<ArgumentException>();
    }

    // Original case
    [TestCase(1, 0, true, Description = "Minimal correct precision")]
    // Own cases
    [TestCase(1, 0, false, Description = "Minimal precision with onlyPositive disabled")]
    [TestCase(2, 1, true, Description = "Correct precision and scale")]
    public void NumberValidator_Constructor_DoesNotThrow(int precision, int scale, bool onlyPositive)
    {
        var numberValidatorInitialization = () => new NumberValidator(precision, scale, onlyPositive);
        numberValidatorInitialization.Should().NotThrow();
    }
}