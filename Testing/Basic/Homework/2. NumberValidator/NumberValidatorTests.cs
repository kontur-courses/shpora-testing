using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(1, TestName = "precision 1, scale 0")]
    public void NumberValidator_CorrectParameters_AfterCreatingCorrectValidator(int precision, int scale = 0)
    {
        var creation = () => new NumberValidator(precision, scale, true);

        creation.Should()
            .NotThrow<ArgumentException>();
    }

    [TestCase(-1, TestName = "negative precision")]
    [TestCase(0, TestName = "zero precision")]
    public void NumberValidator_IncorrectPrecision_AfterCreatingIncorrectValidator(int precision, int scale = 0)
    {
        var creation = () => new NumberValidator(precision, scale);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [TestCase(1, -1, TestName = "negative scale")]
    [TestCase(1, 2, TestName = "scale greater than precision")]
    [TestCase(1, 1, TestName = "scale equal to precision")]
    public void NumberValidator_IncorrectScale_AfterCreatingIncorrectValidator(int precision, int scale)
    {
        var creation = () => new NumberValidator(precision, scale);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("scale must be a non-negative number less than precision");
    }

    [TestCase(1, 0, true, "0", ExpectedResult = true, TestName = "precision 1, scale 0, only positive, result 0")]
    [TestCase(2, 1, true, "0.1", ExpectedResult = true, TestName = "precision 2, scale 1, only positive, result 0.1")]
    [TestCase(2, 1, true, "0,1", ExpectedResult = true, TestName = "precision 2, scale 1, only positive, result 0,1")]
    [TestCase(2, 0, false, "-1", ExpectedResult = true, TestName = "precision 2, scale 0, not only positive, result -1")]
    [TestCase(3, 1, false, "-1.1", ExpectedResult = true, TestName = "precision 3, scale 1, not only positive, result -1.1")]
    [TestCase(3, 1, false, "-1,1", ExpectedResult = true, TestName = "precision 3, scale 1, not only positive, result -1,1")]
    [TestCase(2, 0, true, "+1", ExpectedResult = true, TestName = "precision 2, scale 0, only positive, result +1")]
    [TestCase(3, 1, true, "+1.1", ExpectedResult = true, TestName = "precision 3, scale 1, only positive, result +1.1")]
    [TestCase(3, 1, true, "+1,1", ExpectedResult = true, TestName = "precision 3, scale 1, only positive, result +1,1")]
    
    [TestCase(2, 1, true, "", ExpectedResult = false, TestName = "precision 2, scale 1, only positive, result not empty string")]
    [TestCase(2, 1, true, null, ExpectedResult = false, TestName = "precision 2, scale 1, only positive, result not null")]
    [TestCase(2, 1, true, ".0", ExpectedResult = false, TestName = "precision 2, scale 1, only positive, result not .0")]
    [TestCase(2, 1, true, "0.", ExpectedResult = false, TestName = "precision 2, scale 1, only positive, result not 0.")]
    [TestCase(2, 0, true, "-1", ExpectedResult = false, TestName = "precision 2, scale 0, only positive, result not -1")]
    [TestCase(1, 0, true, "+1", ExpectedResult = false, TestName = "precision 1, scale 0, only positive, result not +1")]
    public bool IsValidNumber_VariousInput_AfterCreatingValidator(
        int precision,
        int scale,
        bool onlyPositive,
        string expectedResultValue)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(expectedResultValue);
    }
}