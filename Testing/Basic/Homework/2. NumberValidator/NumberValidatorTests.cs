using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PRECISION_ERROR_MESSAGE = "precision must be a positive number";
    private const string SCALE_ERROR_MESSAGE = "precision must be a non-negative number less or equal than precision";
    
    # region NumberValidator constructor tests
    [TestCase(-1, 2, PRECISION_ERROR_MESSAGE, TestName = "precision <= 0")]
    [TestCase(10, -1, SCALE_ERROR_MESSAGE, TestName = "scale < 0")]
    [TestCase(1, 100, SCALE_ERROR_MESSAGE, TestName = "scale >= precision")]
    public void NumberValidator_ConstructorArgumentException(int precision, int scale, string message)
    {
        var ctor = () => new NumberValidator(precision, scale);
        ctor.Should()
            .Throw<ArgumentException>()
            .WithMessage(message);
    }
    
    [TestCase(5, 2, TestName = "Regular case")]
    [TestCase(1, 0, TestName = "Boundaries case")]
    public void NumberValidator_ConstructBoundariesCases(int precision, int scale)
    {
        var ctor = () => new NumberValidator(precision, scale);
        ctor.Should().NotThrow();
    }
    # endregion NumberValidator constructor tests
    
    # region NumberValidator IsValidNumber method tests
    [TestCase(17, 2, true, "0")]
    [TestCase(17, 2, true, "0.0")]
    [TestCase(4, 2, true, "+1.23")]
    [TestCase(4, 2, false, "-1.23")]
    public void IsValidNumber_AssertTrueResult(int precision, int scale, bool onlyPositive, string testingNumber)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(testingNumber).Should().BeTrue();
    }
    
    [TestCase(3, 2, true, "a.sd")]
    [TestCase(3, 2, true, "00.00")]
    [TestCase(3, 2, true, "-0.00")]
    [TestCase(3, 2, true, "+0.00")]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(3, 2, true, "-1.23")]
    [TestCase(17, 2, true, "0.000")]
    public void IsValidNumber_AssertFalseResult(int precision, int scale, bool onlyPositive, string testingNumber)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(testingNumber).Should().BeFalse();
    }
    # endregion NumberValidator IsValidNumber method tests
}