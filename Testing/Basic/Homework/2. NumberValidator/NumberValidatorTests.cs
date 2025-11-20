using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    #region Constructor

    private const string PRECISION_VALIDATION_EXCEPTION_MESSAGE = "precision must be a positive number";
    private const string SCALE_VALIDATION_EXCEPTION_MESSAGE = "precision must be a non-negative number less or equal than precision";

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsZero()
    {
        var act = () => new NumberValidator(0, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(PRECISION_VALIDATION_EXCEPTION_MESSAGE);
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsNegative()
    {
        var act = () => new NumberValidator(-1, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(PRECISION_VALIDATION_EXCEPTION_MESSAGE);
    }

    [Test]
    public void Constructor_ThrowArgumentException_ScaleIsNegative()
    {
        var act = () => new NumberValidator(1, -1);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(SCALE_VALIDATION_EXCEPTION_MESSAGE);
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsLessThenScale()
    {
        var act = () => new NumberValidator(1, 3);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(SCALE_VALIDATION_EXCEPTION_MESSAGE);
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsEqualToScale()
    {
        var act = () => new NumberValidator(2, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage(SCALE_VALIDATION_EXCEPTION_MESSAGE);
    }

    [Test]
    public void Constructor_DoesNoThrow_CorrectParams()
    {
        var act = () => new NumberValidator(1, 0);

        act.Should().NotThrow<ArgumentException>();
        act.Invoke().Should().NotBeNull();
    }

    #endregion

    #region IsValidNumber

    [TestCase(null)]
    [TestCase("")]
    public void IsValidNumber_ReturnFalse_ValueIsNullOrEmpty(string value)
    {
        var validator = new NumberValidator(3, 2);

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("a.sd")]
    [TestCase("3,3.3")]
    [TestCase("3..3")]
    public void IsValidNumber_ReturnFalse_RegexNotMatched(string value)
    {
        var validator = new NumberValidator(3, 2);

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("1111")]
    [TestCase("1")]
    [TestCase("+111")]
    public void IsValidNumber_ReturnTrue_PrecisionIsEqualOrLessThenValueLength(string value)
    {
        var validator = new NumberValidator(4);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("1111")]
    [TestCase("+111")]
    public void IsValidNumber_ReturnFalse_ValueLengthIsGreaterThenPrecision(string value)
    {
        var validator = new NumberValidator(3);

        validator.IsValidNumber(value).Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_FractionalPartIsGreaterThenScale()
    {
        var validator = new NumberValidator(7, 2);

        validator.IsValidNumber("1.111").Should().BeFalse();
    }

    [TestCase("1.111")]
    [TestCase("1.1")]
    [TestCase("1")]
    public void IsValidNumber_ReturnTrue_ValueLengthIsEqualOrLessThenScale(string value)
    {
        var validator = new NumberValidator(4, 3);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_OnlyPositiveIsTrueWithNegativeValue()
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber("-1.11").Should().BeFalse();
    }

    [TestCase("1.11")]
    [TestCase("+1.11")]
    public void IsValidNumber_ReturnTrue_OnlyPositiveIsTrueWithPositiveValue(string value)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(value).Should().BeTrue();
    }

    [Test] public void IsValidNumber_ReturnTrue_OnlyPositiveIsFalseWithNegativeValue()
    {
        var validator = new NumberValidator(4, 2, false);

        validator.IsValidNumber("-1.11").Should().BeTrue();
    }

    #endregion
}