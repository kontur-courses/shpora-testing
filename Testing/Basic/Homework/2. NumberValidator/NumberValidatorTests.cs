using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 5, TestName = "IfPrecisionIsNegative")]
    [TestCase(5, -1, TestName = "IfScaleIsNegative")]
    [TestCase(2, 5, TestName = "IfScaleGreaterPrecision")]
    [TestCase(2, 2, TestName = "IfScaleEqualPrecision")]
    public void ShouldThrowArgumentException(int precision, int scale)
    {
        var numberValidatorConstructor = () => new NumberValidator(precision, scale);
        
        numberValidatorConstructor.Should().Throw<ArgumentException>();
    }

    [TestCase(null, ExpectedResult = false, TestName = "IfValueIsNull")]
    [TestCase(" ", ExpectedResult = false, TestName = "IfValueIsEmpty")]
    [TestCase("2.a1", ExpectedResult = false, TestName = "IfValueContainsLetter")]
    [TestCase("0.", ExpectedResult = false, TestName = "IfValueContainsSeparatorWithoutFracPart")]
    [TestCase(",0", ExpectedResult = false, TestName = "IfValueWithoutIntPart")]
    [TestCase("0*0", ExpectedResult = false, TestName = "IfValueSeparatorIncorrect")]
    [TestCase("!0,0", ExpectedResult = false, TestName = "IfValueSignIncorrect")]
    [TestCase("1,234", ExpectedResult = false, TestName = "IfValueFracPartMoreThanScale")]
    [TestCase("11111", ExpectedResult = false, TestName = "IfValueIntPartMoreThanPrecision")]
    [TestCase("225,32", ExpectedResult = false, TestName = "IfValueWithFracPartMoreThanPrecision")]
    [TestCase("+14,23", ExpectedResult = false, TestName = "IfValueWithSignMoreThanPrecision")]
    [TestCase("-0", ExpectedResult = false, TestName = "IfOnlyPositiveTrueAndValueContainsMinus")]
    [TestCase("-1,4", false, ExpectedResult = true, TestName = "IfOnlyPositiveFalseAndValueContainsMinus")]
    [TestCase("1", ExpectedResult = true, TestName = "IfValueIntPart")]
    [TestCase("+1,44", ExpectedResult = true, TestName = "IfValueContainsFracPartAndSign")]
    [TestCase("15,44", ExpectedResult = true, TestName = "IfValueContainsFracPart")]
    public bool IsValidNumber_ShouldReturnExpectedResult(string value, bool onlyPositive = true)
    {
        var numberValidator = new NumberValidator(4, 2, onlyPositive);
        
        return numberValidator.IsValidNumber(value);
    }
}