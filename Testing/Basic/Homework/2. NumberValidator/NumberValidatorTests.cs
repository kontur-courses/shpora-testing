using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_NotThrow_WhenScaleIsZero()
    {
        Action act = () => new NumberValidator(1, 0, true);

        act
            .Should().NotThrow();
    }

    [TestCase(-1, 2, TestName = "WhenPrecisionIsNegative")]
    [TestCase(0, 2, TestName = "WhenPrecisionIsZero")]
    [TestCase(4, 4, TestName = "WhenPrecisionEqualsScale")]
    [TestCase(10, -2, TestName = "WhenScaleIsNegative")]
    [TestCase(1, 2, TestName = "WhenPrecisionLessThanScale")]
    public void NumberValidator_Throw(int precision, int scale)
    {
        Action act = () => new NumberValidator(precision, scale);

        act
            .Should().Throw<ArgumentException>();
    }
    [TestCase("a.sd")]
    [TestCase("sad")]
    [TestCase("897,ава")]
    [TestCase("987.3948*")]
    [TestCase("?.00")]
    [TestCase("а.сд")]
    [TestCase("VI,II")]
    [TestCase("230!450.335")]
    [TestCase("⚠️📲64.56❤️")]
    [TestCase("我們.我們")]
    [TestCase("مان.ود")]
    [TestCase("^879%56(")]
    public void WhenValueContainsNonDigitSymbols(string value)
    {
        IsValidNumber_ReturnsFalse(value, 12, 7);
    }


    [TestCase("0.9845435,9080")]
    [TestCase("+0,,908")]
    [TestCase("-0.00.0")]
    [TestCase("0,.000")]
    [TestCase("+0,00.0")]
    [TestCase("0.9845435,9080")]
    [TestCase("+0,,908")]
    [TestCase("-0.00.0")]
    [TestCase("0,.000")]
    [TestCase("+0,00.0")]
    public void WhenValueHaveFewSeparators(string value)
    {
        IsValidNumber_ReturnsFalse(value, 17, 16);
    }

    [TestCase("+-0.466")]
    [TestCase("-+0.000")]
    [TestCase("---0.000")]
    [TestCase("0.000+-")]
    [TestCase("++0.000")]
    [TestCase("+")]
    [TestCase("+.0-00")]
    public void WhenValueHaveFewSigns(string value)
    {
        IsValidNumber_ReturnsFalse(value, 17, 16);
    }

    [TestCase("^")]
    [TestCase(":")]
    [TestCase(";")]
    [TestCase("/")]
    [TestCase("!")]
    [TestCase("=")]
    [TestCase("-")]
    [TestCase("+")]
    public void WhenSeparatorNotCommaOrDot(string separator)
    {
        IsValidNumber_ReturnsFalse($"0{separator}0", 3, 2, true);
    }

    [TestCase("0.0", 17, 2, true, TestName = "WhenPrecisionAndScaleMoreThanValueIntAndFracParts")]
    [TestCase("0,0", 3, 2, true, TestName = "WhenSeparatorIsComma")]
    [TestCase("0", 17, 2, true, TestName = " WhenValueIsIntegerAndHasLessDigitsThanPrecision")]
    [TestCase("12345", 5, 2, true, TestName = "WhenValueIsIntegerAndDigitCountEqualsPrecision")]
    [TestCase("+1.23", 4, 2, true, TestName = "WhenLengthIntPartAndFracPartEqualToPrecision_IncludingNumberSign")]
    [TestCase("-1.23", 5, 2, TestName = "WhenValueIsNegativeAndValidatorNotOnlyPositive")]
    public void IsValidNumber_ReturnsTrue(string value, int precision, int scale, bool onlyPositive=false)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeTrue();
    }

    [TestCase("+1.23", 3, 2, true, TestName = "WhenNumberLengthMoreThanPrecision_IncludingSignForPositiveNumber")]
    [TestCase("-1.23", 3, 2, true, TestName = "WhenNumberLengthMoreThanPrecision_IncludingSignForNegativeNumber")]
    [TestCase("0.000", 17, 2, true, TestName = "WhenValueFracPartMoreThanScale")]
    [TestCase("00.00", 3, 2, true, TestName = "WhenValueSymbolsCountMoreThanPrecision")]
    [TestCase("", 3, 2, true, TestName = "WhenValueIsEmptyString")]
    [TestCase("34290.", 17, 2, true, TestName = "WhenValueHaveSeparatorWithoutFracPart")]
    [TestCase(null, 3, 2, TestName = "WhenValueIsNull")]
    [TestCase("-0.00", 4, 2, true, TestName = "WhenValueNegative_WithOnlyPositiveNumberValidator")]
    [TestCase(".000", 6, 4, true, TestName = "WhenValueStartsWithSeparator")]
    public void IsValidNumber_ReturnsFalse(string value, int precision, int scale, bool onlyPositive = false)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        var isValid = validator.IsValidNumber(value);

        isValid
            .Should().BeFalse();
    }
}