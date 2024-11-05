using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_WhenScaleIsZero_NotFails()
    {
        Action act = () => new NumberValidator(1, 0, true);

        act
            .Should().NotThrow();
    }

    [Test]
    public void NumberValidator_WhenPrecisionIsNegative_Fails()
    {
        Action act = () => new NumberValidator(-1, 2);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenPrecisionIsZero_Fails()
    {
        Action act = () => new NumberValidator(0, 2);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenScaleIsNegative_Fails()
    {
        Action act = () => new NumberValidator(1, -1);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void NumberValidator_WhenPrecisionLessThanScale_Fails()
    {
        Action act = () => new NumberValidator(1, 2);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenPrecisionAndScaleMoreThanValueIntAndFracParts()
    {
        var value = "0.0";
        var validator = new NumberValidator(17, 2, true);

        IsValidNumber_ReturnsTrue(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenValueIsIntegerAndHasLessDigitsThanPrecision()
    {
        var value = "0";
        var validator = new NumberValidator(17, 2, true);

        IsValidNumber_ReturnsTrue(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenLengthIntPartAndFracPartEqualToPrecision_IncludingNumberSign()
    {
        var value = "+1.23";
        var validator = new NumberValidator(4, 2, true);

        IsValidNumber_ReturnsTrue(value, validator);
    }

    private void IsValidNumber_ReturnsTrue(string value, NumberValidator validator)
    {
        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberLengthMoreThanPrecision_IncludingSignForPositiveNumber()
    {
        var value = "+1.23";
        var validator = new NumberValidator(3, 2, true);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberLengthMoreThanPrecision_IncludingSignForNegativeNumber()
    {
        var value = "-1.23";
        var validator = new NumberValidator(3, 2);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueFracPartMoreThanScale()
    {
        var value = "0.000";
        var validator = new NumberValidator(17, 2, true);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueContainsNonDigitSymbols()
    {
        var value = "a.sd";
        var validator = new NumberValidator(3, 2, true);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueSymbolsCountMoreThanPrecision()
    {
        var value = "00.00";
        var validator = new NumberValidator(3, 2, true);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueNegative_WithOnlyPositiveNumberValidator()
    {
        var value = "-0.00";
        var validator = new NumberValidator(4, 2, true);

        IsValidNumber_ReturnsFalse(value, validator);
    }

    private void IsValidNumber_ReturnsFalse(string value, NumberValidator validator)
    {
        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }
}