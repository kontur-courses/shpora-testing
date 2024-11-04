using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true)]
    [TestCase(-1, 2, false)]
    [TestCase(0, 2, true)]
    [TestCase(0, 2, false)]
    public void ThrowArgumentException_WhenPrecisionIsLessThanOne(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>("precision must be a positive number");
    }

    [TestCase(1, 2, true)]
    [TestCase(1, 2, false)]
    public void ThrowArgumentException_WhenScaleExceedsPrecision(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>("precision must be a non-negative number less or equal than precision");
    }

    [TestCase(5, -1, true)]
    [TestCase(5, -1, false)]
    public void ThrowArgumentException_WhenScaleIsLessThatZero(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>("precision must be a non-negative number less or equal than precision");
    }

    [TestCase("0.00")]
    [TestCase("0.0")]
    [TestCase("0")]
    [TestCase("1.0")]
    [TestCase("1.00")]
    [TestCase("0.1")]
    [TestCase("0.10")]
    [TestCase("0.01")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveTrueAndSimpleInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: true);

        validator.IsValidNumber(value).Should().BeTrue();
    }
    
    [TestCase("0.00")]
    [TestCase("0.0")]
    [TestCase("0")]
    [TestCase("1.0")]
    [TestCase("1.00")]
    [TestCase("0.1")]
    [TestCase("0.10")]
    [TestCase("0.01")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveFalseAndSimpleInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: false);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("-0.00")]
    [TestCase("-0.0")]
    [TestCase("-0")]
    [TestCase("-1.0")]
    [TestCase("-1.00")]
    [TestCase("-0.1")]
    [TestCase("-0.10")]
    [TestCase("-0.01")]
    public void IsValidNumber_IsFalse_WhenOnlyPositiveTrueAndNegativeInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: true);

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("-0.00")]
    [TestCase("-0.0")]
    [TestCase("-0")]
    [TestCase("-1.0")]
    [TestCase("-1.00")]
    [TestCase("-0.1")]
    [TestCase("-0.10")]
    [TestCase("-0.01")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveFalseAndNegativeInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: false);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("+0.00")]
    [TestCase("+0.0")]
    [TestCase("+0")]
    [TestCase("+1.0")]
    [TestCase("+1.00")]
    [TestCase("+0.1")]
    [TestCase("+0.10")]
    [TestCase("+0.01")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveTrueAndPositiveInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: true);

        validator.IsValidNumber(value).Should().BeTrue();
    }
    
    [TestCase("+0.00")]
    [TestCase("+0.0")]
    [TestCase("+0")]
    [TestCase("+1.0")]
    [TestCase("+1.00")]
    [TestCase("+0.1")]
    [TestCase("+0.10")]
    [TestCase("+0.01")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveFalseAndPositiveInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: false);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("a")]
    [TestCase("aa")]
    [TestCase("a.a")]
    [TestCase("a.aa")]
    [TestCase("aa.aa")]
    [TestCase("aa.a")]
    public void IsValidNumber_IsFalse_WhenInputIsNotValue(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("+a")]
    [TestCase("+aa")]
    [TestCase("+a.a")]
    [TestCase("+a.aa")]
    [TestCase("+aa.aa")]
    [TestCase("+aa.a")]
    public void IsValidNumber_IsFalse_WhenInputIsNotValueWithPlus(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("-a")]
    [TestCase("-aa")]
    [TestCase("-a.a")]
    [TestCase("-a.aa")]
    [TestCase("-aa.aa")]
    [TestCase("-aa.a")]
    public void IsValidNumber_IsFalse_WhenInputIsNotValueWithMinus(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("0000.01")]
    [TestCase("100000")]
    public void IsValidNumber_IsFalse_WhenValueExceedsPrecision(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("+000.01")]
    [TestCase("+10000")]
    public void IsValidNumber_IsFalse_WhenPositiveValueExceedsPrecision(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("-000.01")]
    [TestCase("-10000")]
    public void IsValidNumber_IsFalse_WhenNegativeValueExceedsPrecision(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("0.0001")]
    [TestCase("0.0000")]
    public void IsValidNumber_IsFalse_WhenFractionExceedsScale(string value)
    {
        var validator = new NumberValidator(99, 3);

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("+0.0001")]
    [TestCase("+0.0000")]
    public void IsValidNumber_IsFalse_WhenPositiveFractionExceedsScale(string value)
    {
        var validator = new NumberValidator(99, 3);

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("-0.0001")]
    [TestCase("-0.0000")]
    public void IsValidNumber_IsFalse_WhenNegativeFractionExceedsScale(string value)
    {
        var validator = new NumberValidator(99, 3);

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_IsFalse_WhenInputIsEmpty()
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber("").Should().BeFalse();
    }
}