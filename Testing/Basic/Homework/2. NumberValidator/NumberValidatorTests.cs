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

    [TestCase(null, false, TestName = "IfValueIsNull")]
    [TestCase(" ", false, TestName = "IfValueIsEmpty")]
    [TestCase("2.a1", false, TestName = "IfValueContainsLetter")]
    [TestCase("0.", false, TestName = "IfValueContainsSeparatorWithoutFracPart")]
    [TestCase(",0", false, TestName = "IfValueWithoutIntPart")]
    [TestCase("0*0", false, TestName = "IfValueSeparatorIncorrect")]
    [TestCase("!0,0", false, TestName = "IfValueSignIncorrect")]
    [TestCase("1,234", false, TestName = "IfValueFracPartMoreThanScale")]
    [TestCase("11111", false, TestName = "IfValueIntPartMoreThanPrecision")]
    [TestCase("225,32", false, TestName = "IfValueWithFracPartMoreThanPrecision")]
    [TestCase("+14,23", false, TestName = "IfValueWithSignMoreThanPrecision")]
    [TestCase("-0", false, TestName = "IfOnlyPositiveTrueAndValueContainsMinus")]
    [TestCase("-1,4", true, false, TestName = "IfOnlyPositiveFalseAndValueContainsMinus")]
    [TestCase("1", true, TestName = "IfValueIntPart")]
    [TestCase("+1,44", true, TestName = "IfValueContainsFracPartAndSign")]
    [TestCase("15,44", true, TestName = "IfValueContainsFracPart")]
    public void IsValidNumber_ShouldReturnExpectedResult(string value,  bool expectedResult, bool onlyPositive = true)
    {
        var numberValidator = new NumberValidator(4, 2, onlyPositive);
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().Be(expectedResult);
    }
}