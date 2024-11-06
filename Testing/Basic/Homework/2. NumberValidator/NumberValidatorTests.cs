using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 0, TestName = "NegativePrecision")]
    [TestCase(0, 0, TestName = "ZeroPrecision")]
    [TestCase(1, -1, TestName = "NegativeScale")]
    [TestCase(1, 2, TestName = "ScaleGreraterThenPrecision")]
    public void ValidateConstructor_ThrowsException_WithUncorrectData(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase(2, 1, TestName = "CorrectData")]
    [TestCase(1, 0, TestName = "CorrectDataWithZeroScope")]
    public void ValidateConstructor_DontThrowsException_WithCorrectData(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale);
        action.Should().NotThrow();
    }

    [TestCase("0.0", true, true, TestName = "SimpleCorrectValueWithPoint")]
    [TestCase("0,0", true, true, TestName = "SimpleCorrectValueWithComma")]
    [TestCase("0", true, true, TestName = "SimpleDataWithoutValue")]
    [TestCase("0.0", true, true, TestName = "SimpleCorrectValue")]
    [TestCase("000.00", true, false, TestName = "TooMuchNumbers")]
    [TestCase("0.000", true, false, TestName = "BigFracPart")]
    [TestCase("-0.0", true, false, TestName = "NegativeValueForPositiveValidator")]
    [TestCase("-0.0", false, true, TestName = "NegativeValueForNegativeValidator")]
    [TestCase("+0.000", true, false, TestName = "PlusMustIncludeInPrecision")]
    [TestCase("a.asd", true, false, TestName = "UncorrectValue")]
    [TestCase("", true, false, TestName = "EmptyValue")]
    [TestCase(null, true, false, TestName = "NullValue")]
    public void Validator_IsValidate(string value, bool onlyPositive, bool expectedResult)
    {
        var validatorResult = new NumberValidator(4, 2, onlyPositive).IsValidNumber(value);
        validatorResult.Should().Be(expectedResult, $"Проверка числа { value } для валидатора (4, 2, {onlyPositive})");
    }
}