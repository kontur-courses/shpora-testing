
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0")]
    [TestCase(17, 2, true, "0")]
    [TestCase(4, 2, true, "+1.23")]
    [TestCase(4, 2, false, "-1.23")]
    public void IsValidNumber_True_When_CorrectValues(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(3, 2, true, "00.00")]
    [TestCase(3, 2, true, "-0.00")]
    [TestCase(3, 2, true, "+0.00")]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(17, 2, true, "0.000")]
    [TestCase(3, 2, true, "-1.23")]
    [TestCase(3, 2, true, "a.sd")]
    [TestCase(3, 2, true, "")]
    [TestCase(3, 2, true, null)]
    public void IsValidNumber_False_When_InCorrectValues(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeFalse();
    }


    [TestCase(-1, 2, true)]
    [TestCase(1, 2, false)]
    [TestCase(5, -2, false)]
    public void NumberValidator_ThrowException_When_InCorrectValue(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision,  scale, onlyPositive);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, true)]
    [TestCase(3, 2, true)]
    [TestCase(10, 9, false)]
    [TestCase(3, 2, false)]
    public void NumberValidator_NotThrowException_When_CorrectValue(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }
}