using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_NotThrow_WhenScaleIsZero()
    {
        Action act = () => new NumberValidator(1, 0, true);

        act
            .Should().NotThrow();
    }

    [Test]
    public void NumberValidator_Throw_WhenPrecisionIsNegative()
    {
        NumberValidator_Throw(-1, 2);
    }

    [Test]
    public void NumberValidator_Throw_WhenPrecisionIsZero()
    {
        NumberValidator_Throw(0, 2);
    }

    [Test]
    public void NumberValidator_Throw_WhenPrecisionEqualsScale()
    {
        NumberValidator_Throw(4, 4);
    }

    [Test]
    public void NumberValidator_Throw_WhenScaleIsNegative()
    {
        NumberValidator_Throw(1, -1);
    }

    [Test]
    public void NumberValidator_Throw_WhenPrecisionLessThanScale()
    {
        NumberValidator_Throw(1, 2);
    }

    private static void NumberValidator_Throw(int precision, int scale)
    {
        Action act = () => new NumberValidator(precision, scale);

        act
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenPrecisionAndScaleMoreThanValueIntAndFracParts()
    {
        IsValidNumber_ReturnsTrue("0.0", new (17, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsTrue_WhenSeparatorIsComma()
    {
        IsValidNumber_ReturnsTrue("0,0", new(3, 2, true));
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

    private static void IsValidNumber_ReturnsTrue(string value, NumberValidator validator)
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
    public void IsValidNumber_ReturnsFalse_WhenValueHaveFewSeparators()
    {
        IsValidNumber_ReturnsFalse("0.9845435,9080", new(17, 16, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueHaveFewSigns()
    {
        IsValidNumber_ReturnsFalse("+-0.466", new(17, 16, true));
    }

    [TestCase("^")]
    [TestCase(":")]
    [TestCase(";")]
    [TestCase("/")]
    public void IsValidNumber_ReturnsFalse_WhenSeparatorNotCommaOrDot(string separator)
    {
        IsValidNumber_ReturnsFalse($"0{separator}0", new(3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueSymbolsCountMoreThanPrecision()
    {
        IsValidNumber_ReturnsFalse("00.00", new (3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueIsEmptyString()
    {
        IsValidNumber_ReturnsFalse("", new(3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueHaveSeparatorWithoutFracPart()
    {
        IsValidNumber_ReturnsFalse("34290.", new(17, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueIsNull()
    {
        IsValidNumber_ReturnsFalse(null, new(3, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueNegative_WithOnlyPositiveNumberValidator()
    {
        IsValidNumber_ReturnsFalse("-0.00", new(4, 2, true));
    }

    [Test]
    public void IsValidNumber_ReturnsFalse_WhenValueStartsWithSeparator()
    {
        IsValidNumber_ReturnsFalse(".000", new(6, 4, true));
    }

    private static void IsValidNumber_ReturnsFalse(string value, NumberValidator validator)
    {
        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }
}