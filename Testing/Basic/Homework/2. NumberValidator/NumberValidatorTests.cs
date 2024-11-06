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

    [TestCase(3, 2, true, "00.00", ExpectedResult = false)]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false)]
    [TestCase(3, 2, true, "a.sd", ExpectedResult = false)]
    [TestCase(17, 2, true, "+.00", ExpectedResult = false)]
    [TestCase(17, 2, true, "0.0a", ExpectedResult = false)]
    [TestCase(17, 2, true, "a0.0", ExpectedResult = false)]
    [TestCase(17, 2, true, "0.000", ExpectedResult = false)]
    [TestCase(17, 2, true, "", ExpectedResult = false)]
    [TestCase(17, 2, true, null, ExpectedResult = false)]
    [TestCase(17, 2, true, "0.0", ExpectedResult = true)]
    [TestCase(17, 2, null, "0.0", ExpectedResult = true)]
    [TestCase(17, 2, true, "0,0", ExpectedResult = true)]
    [TestCase(17, 2, true, "0", ExpectedResult = true)]
    [TestCase(4, 2, true, "+1.23", ExpectedResult = true)]
    [TestCase(4, 2, false, "-1.23", ExpectedResult = true)]
    public bool IsValid_ReturnResult_AfterExecuting(int precision, int scale, bool onlyPositive, string str)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(str);
    }
}