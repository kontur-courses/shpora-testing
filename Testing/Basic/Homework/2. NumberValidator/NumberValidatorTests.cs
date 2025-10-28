using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    
    [Test]

    public void NumberValidator_ThrowExc_AfterAdditionNotPositivePrecision()
    {
        var action1 = () => new NumberValidator(-1, 2, true);
        var action2 = () => new NumberValidator(0, 2, true);
        
        action1.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
        action2.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }
    
    [Test]

    public void NumberValidator_ThrowExc_AfterAdditionPositivePrecisionLessThanScale()
    {
        var action = () => new NumberValidator(1, 2, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }
    [Test]
    
    public void NumberValidator_ThrowExc_AfterAdditionPositivePrecisionEqualScale()
    {
        var action = () => new NumberValidator(1, 2, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }
    [Test]
    
    public void NumberValidator_ThrowExc_AfterAdditionNegativeScale()
    {
        var action = () => new NumberValidator(1, -1, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }
    [Test]
    
    public void NumberValidator_NotThrow_WhenValidParameters()
    {
        
         var action1 = () => new NumberValidator(1, 0, true);
         var action2 = () => new NumberValidator(5, 2, false);
         var action3 = () => new NumberValidator(4, 3, true);
         
         action1.Should().NotThrow();
         action2.Should().NotThrow();
         action3.Should().NotThrow();
    }
    [Test]

    public void IsValidNumber_IsValid_AfterAdditionValidNumber()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("12.34");
        var testNum2 = new NumberValidator(4, 3, true).IsValidNumber("+2.34");
        var testNum3 = new NumberValidator(4, 3, true).IsValidNumber("1.345");
        
        testNum1.Should().BeTrue();
        testNum2.Should().BeTrue();
        testNum3.Should().BeTrue();
    }

    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNegativeNumberWhenOnlyPositiveTrue()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("-2.34");
        testNum1.Should().BeFalse();
    }

    [Test]

    public void IsValidNumber_IsValid_AfterAdditionNegativeNumberWhenOnlyPositiveFalse()
    {
        var testNum1 = new NumberValidator(4, 2, false).IsValidNumber("-2.34");
        testNum1.Should().BeTrue();
    }
    
    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNumberLongerThanPrecision()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("512.34");
        var testNum2 = new NumberValidator(4, 3, true).IsValidNumber("12.345");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }

    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNumberWithFracLongerThanScaleButValidPrecision()
    {
        var testNum1 = new NumberValidator(5, 2, true).IsValidNumber("12.345");

        testNum1.Should().BeFalse();
    }

    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionString()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("ab.cd");
        
        testNum1.Should().BeFalse();
    }
    
    [Test]

    public void IsValidNumber_IsValid_AfterAdditionInt()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("52");
        
        testNum1.Should().BeTrue();
    }
    
    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNullOrEmpty()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber(null);
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }
    
    [Test]

    public void IsValidNumber_IsValid_AfterAdditionDifferentSeparator()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("12.43");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber("12,43");
        
        testNum1.Should().BeTrue();
        testNum2.Should().BeTrue();
    }
    
    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNumberWithinInt()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber(".5");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber(",5");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }
    [Test]
    
    public void IsValidNumber_IsNotValid_AfterAdditionNumberWithSeparatorButWithinFrac()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("1.");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber("1,");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }

    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionIncorrectNum()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber("1.2.3");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber("1..2");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }
    
    [Test]

    public void IsValidNumber_IsNotValid_AfterAdditionNumWithWhitespaces()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber(" 12");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber("12 ");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }
}