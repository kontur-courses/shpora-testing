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

        act.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 2, true)]
    [TestCase(1, 2, false)]
    public void ThrowArgumentException_WhenScaleExceedsPrecision(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>();
    }
    
    [TestCase(5, -1, true)]
    [TestCase(5, -1, false)]
    public void ThrowArgumentException_WhenScaleIsLessThatZero(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>();
    }

    [TestCase("0.0")]
    [TestCase("0")]
    [TestCase("1.0")]
    [TestCase("0.1")]
    [TestCase("0.01")]
    [TestCase("+1.0")]
    [TestCase("+1")]
    [TestCase("+1.00")]
    public void IsValidNumber_IsTrue_OnValidCases(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("a.a")]
    [TestCase("+a.a")]
    [TestCase("aa.a")]
    [TestCase("-aa.a")]
    [TestCase("a")]
    [TestCase("+a")]
    public void IsValidNumber_IsFalse_WhenInputIsNotValue(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("-0.00")]
    [TestCase("-0.0")]
    [TestCase("-1.0")]
    [TestCase("-0.1")]
    public void IsValidNumber_IsFalse_WhenOnlyPositiveTrueAndNegativeInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: true);

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("-0.00")]
    [TestCase("-0.0")]
    [TestCase("-1.0")]
    [TestCase("-0.1")]
    public void IsValidNumber_IsTrue_WhenOnlyPositiveFalseAndNegativeInput(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator(onlyPositive: false);

        validator.IsValidNumber(value).Should().BeTrue();
    }
    
    [TestCase("0000.01")]
    [TestCase("-000.01")]
    [TestCase("+000.01")]
    [TestCase("100000")]
    [TestCase("-10000")]
    [TestCase("+10000")]
    public void IsValidNumber_IsFalse_WhenValueExceedsPrecision(string value)
    {
        var validator = ValidatorRegistry.GetDefaultNumberValidator();

        validator.IsValidNumber(value).Should().BeFalse();
    }
    
    [TestCase("0.0001")]
    [TestCase("-0.0001")]
    [TestCase("+0.0001")]
    [TestCase("0.0000")]
    public void IsValidNumber_IsFalse_WhenFractionExceedsScale(string value)
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