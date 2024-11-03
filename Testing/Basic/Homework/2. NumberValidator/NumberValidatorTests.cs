
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_DoesNotThrowOnCreation_WithCorrectArguments()
    {
        using (new AssertionScope())
        {
            new Action(() => new NumberValidator(1, 0, true)).Should().NotThrow();
            new Action(() => new NumberValidator(1, 0, false)).Should().NotThrow();
            new Action(() => new NumberValidator(12, 5, true)).Should().NotThrow();
            new Action(() => new NumberValidator(12, 5, false)).Should().NotThrow();
        }
    }

    [Test]
    public void NumberValidator_ThrowsOnCreation_WithScale_BeingMoreOrEqualTo_Precision()
    {
        using (new AssertionScope())
        {
            new Action(() => new NumberValidator(1, 2, false)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(1, 2, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, 9, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, 9, false)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, 7, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, 7, false)).Should().Throw<ArgumentException>();
        }
    }

    [Test]
    public void NumberValidator_ThrowsOnCreation_WithPrecision_BeingLessOrEqualTo_Zero()
    {
        using (new AssertionScope())
        {
            new Action(() => new NumberValidator(-1, 2, false)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(-1, 2, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(0, 0, false)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(0, 0, true)).Should().Throw<ArgumentException>();
        }
    }

    [Test]
    public void NumberValidator_ThrowsOnCreation_WithScale_BeingLessThan_Zero()
    {
        using (new AssertionScope())
        {
            new Action(() => new NumberValidator(1, -2, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(1, -2, false)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, -1, true)).Should().Throw<ArgumentException>();
            new Action(() => new NumberValidator(7, -1, false)).Should().Throw<ArgumentException>();
        }
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_ForOnlyPositiveCorrectNumbers()
    {
        var numberValidator = new NumberValidator(4, 2, true);
        var validNumbers = new[]
        {
            "0.0", "0,0", "0", "1.23", "+0.00", "01.10", "+0,00", "1,0"
        };
        
        var numberValidatorWithScaleOfZero = new NumberValidator(2, 0, true);
        var validNumbersWithScaleOfZero = new[]
        {
            "1", "+0", "01", "00"
        };


        using (new AssertionScope())
        {
            validNumbers.Should().OnlyContain(x => numberValidator.IsValidNumber(x));
            validNumbersWithScaleOfZero.Should().OnlyContain(x => numberValidatorWithScaleOfZero.IsValidNumber(x));
        }
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_ForIncorrectNumbers_And_ForNegativeNumbers()
    {
        var numberValidator = new NumberValidator(3, 2, true);
        var invalidNumbers = new[]
        {
            "000.0", "-0.00", "+0,00", "+1.23", "-1,23", "a.sd",
            "0.000", "0.", ",00", "", null, "    ", " ", "-", "+",
            "   1.5", "0,2   ", "0  . 11"
        };
        
        var numberValidatorWithScaleOfZero = new NumberValidator(3, 0, true);
        var invalidNumbersWithScaleOfZero = new List<string>
        {
            "-1", "+000", "0.0", "3.14", "-0.1"
        };
        invalidNumbersWithScaleOfZero.AddRange(invalidNumbers!);

        
        using (new AssertionScope()) {
            invalidNumbers.Should().OnlyContain(x => !numberValidator.IsValidNumber(x));
            invalidNumbersWithScaleOfZero.Should().OnlyContain(x => !numberValidatorWithScaleOfZero.IsValidNumber(x));
        }
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_ForAnyCorrectNumber()
    {
        var numberValidator = new NumberValidator(4, 2, false);
        var validNumbers = new[]
        {
            "0.0", "0", "-0", "-1.23", "+0.00", "01.10", "-1,10", "+10.0"
        };
        
        var numberValidatorWithScaleOfZero = new NumberValidator(2, 0, false);
        var validNumbersWithScaleOfZero = new[]
        {
            "1", "+0", "01", "00", "-0", "+2", "-2"
        };


        using (new AssertionScope()) {
            validNumbers.Should().OnlyContain(x => numberValidator.IsValidNumber(x));
            validNumbersWithScaleOfZero.Should().OnlyContain(x => numberValidatorWithScaleOfZero.IsValidNumber(x));
        }
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_ForAnyIncorrectNumber()
    {
        var numberValidator = new NumberValidator(3, 2, false);
        var invalidNumbers = new[]
        {
            "000.0", "-0.00", "+0,00", "+1.23", "-1,23", "a.sd",
            "0.000", "0.", ",00", "", null, "    ", " ", "-,0",
            "+0,", "   1.5", "0,2   ", "0  . 11"
        };
        
        var numberValidatorWithScaleOfZero = new NumberValidator(3, 0, false);
        var invalidNumbersWithScaleOfZero = new List<string>
        {
            "-1.0", "+000", "0.0", "3.14", "-0.1", "1111"
        };
        invalidNumbersWithScaleOfZero.AddRange(invalidNumbers!);
        
        
        using (new AssertionScope()) {
            invalidNumbers.Should().OnlyContain(x => !numberValidator.IsValidNumber(x));
            invalidNumbersWithScaleOfZero.Should().OnlyContain(x => !numberValidatorWithScaleOfZero.IsValidNumber(x));
        }
    }
}