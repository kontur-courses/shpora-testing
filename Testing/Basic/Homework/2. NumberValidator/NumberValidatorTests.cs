
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PrecisionExceptionMessage = "precision must be a positive number";
    private const string ScaleExceptionMessage = "scale must be a non-negative number less or equal than precision";

    [TestCase(1, 2)]
    [TestCase(1, 1)]
    public void ConstructorShould_ThrowException_WhenPositivePrecisionLessOrEqualScale(int  precision, int scale)
    {
        var createValidator = () => new NumberValidator(precision, scale, true);

        createValidator
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ScaleExceptionMessage);
    }

    [Test]
    public void ConstructorShould_ThrowException_WhenNegativeScale()
    {
        var createValidator = () => new NumberValidator(1, -1, true);

        createValidator
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ScaleExceptionMessage);
    }

    [Test]
    [TestCase(1, 0, true)]
    [TestCase(5, 2, false)]
    [TestCase(4, 3, true)]
    public void ConstructorShould_NotThrowException_WhenValidParameters(int precision, int scale, bool onlyPositive)
    {
         var createValidator = () => new NumberValidator(precision, scale, onlyPositive);

         createValidator
             .Should()
             .NotThrow();
    }

    [TestCase(4, 2, true, "12.34")]
    [TestCase(4, 3, true, "+1.23")]
    [TestCase(5, 4, true, "1.2345")]
    public void IsValidNumberShould_BeTrue_WhenValidNumber(int precision, int scale, bool onlyPositive, string testNumber)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeTrue(because: $"values is ({precision}, {scale}, onlyPositive: {onlyPositive}, testNumber: {testNumber})");
    }

    [Test]
    public void IsValidNumberShould_BeFalse_WhenNegativeNumberWithOnlyPositiveTrue()
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber("-2.34")
            .Should()
            .BeFalse();
    }

    [Test]
    public void IsValidNumberShould_BeTrue_WhenNegativeNumberWithOnlyPositiveFalse()
    {
        var validator = new NumberValidator(4, 2, false);

        validator.IsValidNumber("-2.34")
            .Should()
            .BeTrue();
    }

    [TestCase(4, 2, true, "512.34")]
    [TestCase(4, 3, true, "12.345")]
    public void IsValidNumberShould_BeFalse_WhenNumberLongerThanPrecision(int precision, int scale, bool onlyPositive, string testNumber)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [Test]
    public void IsValidNumberShould_BeFalse_WhenNumberWithFracLongerThanScaleButValidPrecision()
    {
        var validator = new NumberValidator(5, 2, true);

        validator.IsValidNumber("12.345")
            .Should()
            .BeFalse();
    }

    [TestCase("aaa")]
    [TestCase("ab.cd")]
    [TestCase("./wqq")]
    public void IsValidNumberShould_BeFalse_WhenString(string testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [Test]
    public void IsValidNumberShould_BeTrue_WhenInt()
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber("12")
            .Should()
            .BeTrue();
    }

    [TestCase("")]
    [TestCase(null)]
    public void IsValidNumberShould_BeFalse_WhenNullOrEmpty(string testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [TestCase("12.3")]
    [TestCase("12,3")]
    public void IsValidNumberShould_BeTrue_WhenDifferentSeparator(string testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeTrue();
    }

    [TestCase("1.")]
    [TestCase("1,")]
    public void IsValidNumberShould_BeFalse_WhenNumberWithinInt(string testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [TestCase("1.")]
    [TestCase("1,")]
    public void Should_BeFalse_WhenNumberWithSeparatorButWithinFrac(string testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [TestCase("1.2.3")]
    [TestCase("1,222,3")]
    public void Should_BeFalse_WhenIncorrectNum(string  testNumber)
    {
        var validator = new NumberValidator(4, 2, true);

        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse();
    }

    [TestCase("    12")]
    [TestCase("12    ")]
    [TestCase("1   2")]
    public void Should_BeFalse_WhenNumWithWhitespaces(string  testNumber)
    {
        var validator = new NumberValidator(4, 2, true);
        
        validator.IsValidNumber(testNumber)
            .Should()
            .BeFalse(because: $"value is {testNumber}");
    }
}