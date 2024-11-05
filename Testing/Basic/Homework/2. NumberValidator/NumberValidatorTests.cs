using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    #region Constructor

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsZero()
    {
        var act = () => new NumberValidator(0, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsNegative()
    {
        var act = () => new NumberValidator(-1, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void Constructor_ThrowArgumentException_ScaleIsNegative()
    {
        var act = () => new NumberValidator(1, -1);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsLessThenScale()
    {
        var act = () => new NumberValidator(1, 3);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void Constructor_ThrowArgumentException_PrecisionIsEqualToScale()
    {
        var act = () => new NumberValidator(2, 2);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
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

    [Test]
    public void IsValidNumber_ReturnFalse_ValueIsNullOrEmpty()
    {
        var validator = new NumberValidator(3, 2);

        validator.IsValidNumber(null).Should().BeFalse();
        validator.IsValidNumber("").Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_RegexNotMatched()
    {
        var validator = new NumberValidator(3, 2);

        validator.IsValidNumber("a.sd").Should().BeFalse();
        validator.IsValidNumber("3,3.3").Should().BeFalse();
        validator.IsValidNumber("3..3").Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnTrue_PrecisionIsEqualOrLessThenValueLength()
    {
        var validator = new NumberValidator(4);

        validator.IsValidNumber("1111").Should().BeTrue();
        validator.IsValidNumber("1").Should().BeTrue();
        validator.IsValidNumber("+111").Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_ValueLengthIsGreaterThenPrecision()
    {
        var validator = new NumberValidator(3);

        validator.IsValidNumber("1111").Should().BeFalse();
        validator.IsValidNumber("+111").Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_FractionalPartIsGreaterThenScale()
    {
        var validator = new NumberValidator(7, 2);

        validator.IsValidNumber("1.111").Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnTrue_ValueLengthIsEqualOrLessThenScale()
    {
        var validator = new NumberValidator(4, 3);

        validator.IsValidNumber("1.111").Should().BeTrue();
        validator.IsValidNumber("1.1").Should().BeTrue();
        validator.IsValidNumber("1").Should().BeTrue();
    }

    [Test]
    public void IsValidNumber_ReturnFalse_OnlyPositiveIsTrueWithNegativeValue()
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber("-1.11").Should().BeFalse();
    }

    [Test]
    public void IsValidNumber_ReturnTrue_OnlyPositiveIsTrueWithPositiveValue()
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber("1.11").Should().BeTrue();
        validator.IsValidNumber("+1.11").Should().BeTrue();
    }

    [Test] public void IsValidNumber_ReturnTrue_OnlyPositiveIsFalseWithNegativeValue()
    {
        var validator = new NumberValidator(4, 2, false);

        validator.IsValidNumber("-1.11").Should().BeTrue();
    }

    #endregion
}