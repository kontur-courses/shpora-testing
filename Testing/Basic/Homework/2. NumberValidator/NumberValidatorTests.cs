using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PRECISION_ERROR_MESSAGE = "precision must be a positive number";
    private const string SCALE_ERROR_MESSAGE = "precision must be a non-negative number less or equal than precision";
    
    # region NumberValidator constructor tests
    [TestCase(-1, 2, PRECISION_ERROR_MESSAGE, Description = "precision <= 0")]
    [TestCase(10, -1, SCALE_ERROR_MESSAGE, Description = "scale < 0")]
    [TestCase(1, 100, SCALE_ERROR_MESSAGE, Description = "scale >= precision")]
    public void NumberValidator_ConstructorArgumentException(int precision, int scale, string message)
    {
        var ctor = () => new NumberValidator(precision, scale);
        ctor.Should()
            .Throw<ArgumentException>()
            .WithMessage(message);
    }
    
    [TestCase(5, 2, Description = "Regular case")]
    [TestCase(1, 0, Description = "Boundaries case")]
    public void NumberValidator_ConstructBoundariesCases(int precision, int scale)
    {
        var ctor = () => new NumberValidator(precision, scale);
        ctor.Should().NotThrow();
    }
    # endregion NumberValidator constructor tests
    
    # region NumberValidator IsValidNumber method tests
    [TestCase(17, 2, true, "0", ExpectedResult = true)]
    [TestCase(17, 2, true, "0.0", ExpectedResult = true)]
    [TestCase(3, 2, true, "-0.00", ExpectedResult = false)]
    [TestCase(3, 2, true, "+0.00", ExpectedResult = false)]
    public bool IsValidNumber_ZeroHasNoSign(int precision, int scale, bool onlyPositive, string testNumber)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testNumber);
    }
    
    [TestCase(4, 2, true, "+1.23", ExpectedResult = true)]
    [TestCase(5, 2, true, "+01.23", ExpectedResult = true)]
    [TestCase(3, 2, true, "00.00", ExpectedResult = false)]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false)]
    public bool IsValidNumber_NumberFitsInPrecision(int precision, int scale, bool onlyPositive, string testNumber)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testNumber);
    }

    [TestCase(4, 2, false, "-1.23", ExpectedResult = true)]
    [TestCase(4, 2, true, "-1.23", ExpectedResult = false)]
    public bool IsValidNumber_OnlyPositiveRestriction(int precision, int scale, bool onlyPositive, string testNumber)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testNumber);
    }
    
    [TestCase(3, 2, true, "a.sd", ExpectedResult = false)]
    [TestCase(3, 2, true, "1.sd", ExpectedResult = false)]
    [TestCase(3, 2, true, "a.11", ExpectedResult = false)]
    public bool IsValidNumber_NonNumericValues(int precision, int scale, bool onlyPositive, string testNumber)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testNumber);
    }

    [TestCase(6, 5, true, "1.234", ExpectedResult = true)]
    [TestCase(4, 2, true, "1.123", ExpectedResult = false)]
    [TestCase(17, 2, true, "0.000", ExpectedResult = false)]
    public bool IsValidNumber_NumberFitsInScale(int precision, int scale, bool onlyPositive, string testNumber)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testNumber);
    }
    # endregion NumberValidator IsValidNumber method tests
}