
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Constructor_ShouldNotThrow_WhenParametersAreValid()
    {
        var action = () => new NumberValidator(2,1, true);
        action.Should().NotThrow();
    }
    
    [TestCase(-1, 2, TestName = "Precision is negative")]
    [TestCase(2, -2, TestName = "Scale is negative")]
    [TestCase(2, 2, TestName = "Scale equals precision")]
    [TestCase(2, 3, TestName = "Scale is greater than precision")]
    [TestCase(0, 0, TestName = "Precision is zero")]
    public void Constructor_ShouldThrowArgumentException_WhenParametersAreInvalid(int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale, true);
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase(4, 2, true, "0.1", TestName = "Number without sign")]
    [TestCase(4, 2, true, "+23.1", TestName = "Total length equals precision")]
    [TestCase(4, 2, true, "0.12", TestName = "FracPart equals scale")]
    [TestCase(4, 0, true, "12", TestName = "Scale is zero")]
    [TestCase(4, 2, true, "+0.12", TestName = "Number with plus sign")]
    [TestCase(4, 2, false, "-0.12", TestName = "Number with minus sign")]
    [TestCase(4, 2, true, "+0,12", TestName = "Number with ,")]
    [TestCase(4, 2, false, "-0,12", TestName = "Negative value when onlyPositive=false")]
    [TestCase(4, 2, false, "-0.00", TestName = "Value with sign length equals precision")]
    [TestCase(3, 2, false, "0", TestName = "Zero")]
    public void IsValidNumber_ShouldReturnTrue_WhenValueIsValid(int precision, int scale, bool onlyPositive, string value) 
    { 
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var actual = validator.IsValidNumber(value); 

        actual.Should().BeTrue(); 
    }    
    
    [TestCase(4, 2, false, "112.03", TestName = "Number length greater than precision")]
    [TestCase(4, 2, false, "2.033", TestName = "FracPart is greater than scale")]
    [TestCase(4, 0, false, "112.3", TestName = "Scale is zero")]
    [TestCase(3, 2, false, "+1.23", TestName = "The sign is also taken into account")]
    [TestCase(6, 2, false, "+1!23", TestName = "! instead (.,)")]
    [TestCase(6, 2, false, "+1/23", TestName = "/ instead (.,)")]
    [TestCase(6, 2, false, "!1.23", TestName = "Invalid sign")]
    [TestCase(3, 2, false, "a.sd", TestName = "Only letter")]
    [TestCase(6, 2, false, "a1.1d", TestName = "Letter with numbers")]
    [TestCase(3, 2, false, "1.2.3", TestName = "Two decimal separators")]
    [TestCase(3, 2, false, "--1", TestName = "Two minus")]
    [TestCase(3, 2, false, "++1", TestName = "Two plus")]
    [TestCase(3, 2, false, "+-1", TestName = "Mixed signs")]
    [TestCase(4, 3, false, "1+1", TestName = "The sign inside the number")]
    [TestCase(3, 2, false, "", TestName = "Empty string")]
    [TestCase(3, 2, false, "   ", TestName = "Value contains only whitespace")]
    [TestCase(10, 2, true, "1 2", TestName = "Spaces inside")]
    [TestCase(10, 2, true, " 1 ", TestName = "Spaces around")]
    [TestCase(3, 2, false, "1.", TestName = ". Without fracPart")]
    [TestCase(3, 2, false, "1,", TestName = ", Without fracPart")]
    [TestCase(3, 2, false, "00.00", TestName = "Zeros count too")]
    [TestCase(3, 2, false, null, TestName = "value is null")]
    [TestCase(4, 2, true, "-123", TestName = "Negative value when onlyPositive=true")]
    [TestCase(10, 2, true, ".", TestName = "Not numbers")]
    [TestCase(10, 2, true, ".12", TestName = "Not integer part")]
    [TestCase(10, 2, true, "1\n", TestName = "\\n at the end")]
    [TestCase(10, 2, true, "1\n11.2", TestName = "\\n at the middle")]
    [TestCase(10, 2, true, "1\t", TestName = "\\t at the end")]
    public void IsValidNumber_ShouldReturnFalse_WhenValueIsInvalid(int precision, int scale, bool onlyPositive, string value)
    { 
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var actual = validator.IsValidNumber(value); 

        actual.Should().BeFalse(); 
    } 
}
