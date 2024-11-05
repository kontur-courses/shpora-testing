
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_WhenPrecisionIsNegative_Fails()
    {
        Action act = () => new NumberValidator(-1, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenPrecisionIsZero_Fails()
    {
        Action act = () => new NumberValidator(0, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenScaleIsZero_NotFails()
    {
        Action act = () => new NumberValidator(1, 0, true);

        act
            .Should().NotThrow();
    }

    [Test]
    public void NumberValidator_WhenScaleIsNegative_Fails()
    {
        Action act = () => new NumberValidator(1, -1, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void NumberValidator_WhenPrecisionLessThanScale_Fails()
    {
        Action act = () => new NumberValidator(1, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenPrecisionAndScaleMoreThanValueIntAndFracParts()
    {
        var value = "0.0";
        var numbValidator = new NumberValidator(17, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenValueIsIntegerAndHasLessDigitsThanPrecision()
    {
        var value = "0";
        var numbValidator = new NumberValidator(17, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenLengthIntPartAndFracPartEqualToPrecision_IncludingNumberSign()
    {
        var value = "+1.23";
        var numbValidator = new NumberValidator(4, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberLengthMoreThanPrecision_IncludingSignForPositiveNumber()
    {
        var value = "+1.23";
        var numbValidator = new NumberValidator(3, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberLengthMoreThanPrecision_IncludingSignForNegativeNumber()
    {
        var value = "-1.23";
        var numbValidator = new NumberValidator(3, 2, false);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueFracPartMoreThanScale()
    {
        var value = "0.000";
        var numbValidator = new NumberValidator(17, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueContainsNonDigitSymbols()
    {
        var value = "a.sd";
        var numbValidator = new NumberValidator(3, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueSymbolsCountMoreThanPrecision()
    {
        var value = "00.00";
        var numbValidator = new NumberValidator(3, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueNegative_WithOnlyPositiveNumberValidator()
    {
        var value = "-0.00";
        var numbValidator = new NumberValidator(4, 2, true);

        var isValid = numbValidator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }
}