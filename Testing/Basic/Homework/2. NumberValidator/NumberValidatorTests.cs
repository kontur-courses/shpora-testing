using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenPrecisionIsNegative()
    {
        Action action = () => new NumberValidator(-1, 2, true);

        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenScaleIsNegative()
    {
        Action act = () => new NumberValidator(3, -1, true);

        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenScaleExceedsThePrecision()
    {
        Action act = () => new NumberValidator(1, 4, true);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ShouldNotThrowExceprion_WhenInputValuesAreValid()
    {
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    }

    [TestCase(4, 3, true, "", false)]
    [TestCase(4, 3, true, null, false)]
    [TestCase(3, 2, true, "a.sd", false)]
    [TestCase(3, 2, true, "00.00", false)]
    [TestCase(17, 2, true, "0.000", false)]
    [TestCase(7, 3, true, "-1.23", false)]
    [TestCase(17, 2, true, "0.0", true)]
    [TestCase(17, 2, true, "0", true)]
    [TestCase(4, 2, true, "+1.23", true)]
    public void IsValidNumberTest(int precision, int scale, bool onlyPositive, string value, bool expectedResult)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);

        var isValidNumber = numberValidator.IsValidNumber(value);

        isValidNumber.Should().Be(expectedResult);
    }
}