using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorShould
{
    [TestCase(-1, 0)]
    [TestCase(0, 0)]
    [TestCase(1, -2)]
    [TestCase(1, 2)]
    [TestCase(1, 1)]
    public void ThrowArgumentException_AfterCreatingWith(int precision, int scale)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale);

        createNumberValidator.Should().Throw<ArgumentException>();
    }

    [TestCase(3, 2, true, "00.00")]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(3, 2, true, "a.sd")]
    [TestCase(17, 2, true, "+.00")]
    [TestCase(17, 2, true, "0.0a")]
    [TestCase(17, 2, true, "a0.0")]
    [TestCase(17, 2, true, "0.000")]
    [TestCase(17, 2, true, "")]
    [TestCase(17, 2, true, null)]
    public void IsValidNumber_ReturnFalse_AfterExecutingWith(int precision, int scale, bool onlyPositive, string str)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        validator.IsValidNumber(str).Should().BeFalse();
    }

    [TestCase(17, 2, true, "0.0")]
    [TestCase(17, 2, null, "0.0")]
    [TestCase(17, 2, true, "0,0")]
    [TestCase(17, 2, true, "0")]
    [TestCase(4, 2, true, "+1.23")]
    [TestCase(4, 2, false, "-1.23")]
    public void IsValidNumber_ReturnTrue_AfterExecutingWith(int precision, int scale, bool onlyPositive, string str)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
            
        validator.IsValidNumber(str).Should().BeTrue();
    }
}