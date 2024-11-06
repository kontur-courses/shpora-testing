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
        IsValidNumber_ReturnsTrue("0.0", new (17, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenValueIsIntegerAndHasLessDigitsThanPrecision()
    {
        IsValidNumber_ReturnsTrue("0", new (17, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenLengthIntPartAndFracPartEqualToPrecision_IncludingNumberSign()
    {
        IsValidNumber_ReturnsTrue("+1.23", new (4, 2, true));
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
        IsValidNumber_ReturnsFalse("+1.23", new (3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenNumberLengthMoreThanPrecision_IncludingSignForNegativeNumber()
    {
        IsValidNumber_ReturnsFalse("-1.23", new (3, 2));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueFracPartMoreThanScale()
    {
        IsValidNumber_ReturnsFalse("0.000", new (17, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueContainsNonDigitSymbols()
    {
        IsValidNumber_ReturnsFalse("a.sd", new (3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueSymbolsCountMoreThanPrecision()
    {
        IsValidNumber_ReturnsFalse("00.00", new (3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueNegative_WithOnlyPositiveNumberValidator()
    {
        IsValidNumber_ReturnsFalse("-0.00", new (4, 2, true));
    }

    private void IsValidNumber_ReturnsFalse(string value, NumberValidator validator)
    {
        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }
}