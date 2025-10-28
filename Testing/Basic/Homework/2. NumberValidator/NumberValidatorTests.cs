using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, TestName = "Negative precision")]
    [TestCase(0, 2, TestName = "Zero precision")]
    [TestCase(1, -1, TestName = "Negative scale")]
    [TestCase(1, 1, TestName = "Equal precision and scale")]
    [TestCase(1, 3, TestName = "Scale greater than precision")]
    public void NumberValidator_ShouldThrowException_InvalidArguments(int precision, int scale)
    {
        var validator = () => new NumberValidator(precision, scale);
        validator.Should().Throw<ArgumentException>();
    }

    [TestCase(1, 0, TestName = "Positive precision and scale")]
    public void NumberValidator_ShouldNotThrowException_ValidArguments(int precision, int scale)
    {
        var validator = () => new NumberValidator(precision, scale);
        validator.Should().NotThrow<ArgumentException>();
    }

    [TestCase(17, 2, "0.0", TestName = "Zero with decimal")]
    [TestCase(17, 2, "0", TestName = "Zero without decimal")]
    public void IsValidNumber_ShouldBeTrue_CorrectZeros(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale).IsValidNumber(number).Should().BeTrue();
    }

    [TestCase(3, 2, "00.00", TestName = "Number is longer than precision")]
    [TestCase(3, 2, "-0.00", TestName = "Number is longer than precision with minus")]
    [TestCase(3, 2, "+0.00", TestName = "Number is longer than precision with plus")]
    public void IsValidNumber_ShouldBeFalse_NumberBiggerThanPrecision(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale).IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(3, 2, "0.000", TestName = "Number is longer than scale")]
    [TestCase(3, 0, "0.0", TestName = "Scale is zero")]
    public void IsValidNumber_ShouldBeFalse_PartAfterTheDecimalBiggerThanScale(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale).IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(3, 2, "a.sd", TestName = "Only letters")]
    [TestCase(1, 0, "", TestName = "Empty string")]
    [TestCase(1, 0, "-", TestName = "Only minus")]
    public void IsValidNumber_ShouldBeFalse_NumberIsNotANumber(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale).IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(4, 2, "+1.23", TestName = "Positive number")]
    [TestCase(3, 2, "1.23", TestName = "Positive number without sign")]
    public void IsValidNumber_ShouldBeTrue_PositiveNumbers(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(number).Should().BeTrue();
    }

    [TestCase(4, 2, "-1.23", TestName = "Negative number")]
    public void IsValidNumber_ShouldBeFalse_NegativeNumbers(int precision, int scale, string number)
    {
        new NumberValidator(precision, scale, true).IsValidNumber(number).Should().BeFalse();
    }
}