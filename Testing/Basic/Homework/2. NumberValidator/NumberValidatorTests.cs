using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(3, 2, true, "00.00", false)]
    [TestCase(3, 2, true, "-0.00", false)]
    [TestCase(3, 2, true, "+0.00", false)]
    [TestCase(3, 2, true, "+1.23", false)]
    [TestCase(17, 2, true, "0.000", false)]
    [TestCase(3, 2, true, "-1.23", false)]
    [TestCase(3, 2, true, "a.sd", false)]
    [TestCase(17, 2, true, "0.0", true)]
    [TestCase(17, 2, true, "0", true)]
    [TestCase(4, 2, true, "+1.23", true)]
    public void IsValidNumber_ReturnsExpectedResult(
        int precision, int scale, bool onlyPositive, string number, bool expected)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var result = validator.IsValidNumber(number);

        result.Should().Be(expected);
    }

    [TestCase(1, 0, true)]
    public void NumberValidator_WithValidArguments_DoesNotThrow(
        int precision, int scale, bool onlyPositive)
    {
        var validatorCreation = () => new NumberValidator(precision, scale, onlyPositive);

        validatorCreation.Should().NotThrow();
    }

    [TestCase(-1, 2, true)]
    public void NumberValidator_WithInvalidArguments_ThrowsArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        var validatorCreation = () => new NumberValidator(precision, scale, onlyPositive);

        validatorCreation.Should().Throw<ArgumentException>();
    }
}