using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 5, TestName = "_IfPrecisionIsNegative")]
    [TestCase(5, -1, TestName = "_IfScaleIsNegative")]
    [TestCase(2, 5, TestName = "_IfScaleGreaterPrecision")]
    [TestCase(2, 2, TestName = "_IfScaleEqualPrecision")]
    public void ShouldThrowArgumentException(int precision, int scale)
    {
        var numberValidatorConstructor = () => new NumberValidator(precision, scale);
        
        numberValidatorConstructor.Should().Throw<ArgumentException>();
    }

    [TestCase(null, TestName = "IsValidNumber_Should_Return_False_If_Value_Is_Null")]
    [TestCase(" ", TestName = "IsValidNumber_Should_Return_False_If_Value_Is_Empty")]
    [TestCase("2.a1", TestName = "IsValidNumber_Should_Return_False_If_Value_Contains_Letter")]
    [TestCase("0.", TestName = "IsValidNumber_Should_Return_False_If_Value_Contains_Separator_Without_FracPart")]
    [TestCase(",0", TestName = "IsValidNumber_Should_Return_False_If_Value_Without_IntPart")]
    [TestCase("0*0", TestName = "IsValidNumber_Should_Return_False_If_Value_Separator_Incorrect")]
    [TestCase("!0,0", TestName = "IsValidNumber_Should_Return_False_If_Value_Sign_Incorrect")]
    [TestCase("1,234", TestName = "IsValidNumber_Should_Return_False_If_Value_FracPart_More_Than_Scale")]
    [TestCase("11111", TestName = "IsValidNumber_Should_Return_False_If_Value_IntPart_More_Than_Precision")]
    [TestCase("225,32", TestName = "IsValidNumber_Should_Return_False_If_Value_With_FracPart_More_Than_Precision")]
    [TestCase("+14,23", TestName = "IsValidNumber_Should_Return_False_If_Value_With_Sign_More_Than_Precision")]
    [TestCase("-0", TestName = "IsValidNumber_Should_Return_False_If_OnlyPositive_True_And_Value_Contains_Minus")]
    public void IsValidNumber_Should_Return_False(string value)
    {
        var numberValidator = new NumberValidator(4, 2, true);
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeFalse();
    }
    
    [TestCase("-1,4", false, TestName = "IsValidNumber_Should_Return_True_If_OnlyPositive_False_And_Value_Contains_Minus")]
    [TestCase("1", TestName = "IsValidNumber_Should_Return_True_If_Value_IntPart")]
    [TestCase("+1,444", TestName = "IsValidNumber_Should_Return_True_If_Value_Contains_FracPart_And_Sign")]
    [TestCase("1,444", TestName = "IsValidNumber_Should_Return_True_If_Value_Contains_FracPart")]
    public void IsValidNumber_Should_Return_True(string value, bool onlyPositive = true)
    {
        var numberValidator = new NumberValidator(10, 5, onlyPositive);
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeTrue();
    }
}