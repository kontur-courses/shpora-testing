
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Negative precision.")]
    [TestCase(1, -2, true,  TestName = "Negative scale.")]
    [TestCase(4, 5, true,  TestName = "Precision is less than scale.")]
    [TestCase(5, 5, true,  TestName = "Scale is equal to precision.")]
    [TestCase(0, 0, true,  TestName = "Precision and scale are equal to zero.")]
    public void Constructor_ShouldThrowArgumentException_ForInvalidArguments(int precision, int scale, bool allowSigned)
    {
        Action act = () => new NumberValidator(precision, scale, allowSigned);
        act.Should().Throw<ArgumentException>(); 
    }
    
    [TestCase(4, 3, true, TestName = "Precision is greater than scale.")]
    [TestCase(3, 0, true, TestName = "Scale is equal to zero.")]
    [TestCase(5, 0, false, TestName = "Scale is equal to zero with changed sign.")]
    public void Constructor_DoNotThrowArgumentException_ForValidArguments(int precision, int scale, bool allowSigned)
    {
        Action act = () => new NumberValidator(precision, scale, allowSigned);
        act.Should().NotThrow(); 
    }
    
    [TestCase(17, 3, true,"0", TestName = "Integer positive value with scale > 0.")]
    [TestCase(10, 3, true,"999999999.0", TestName = "Fractional positive value with scale > 0 and length = precision.")]
    [TestCase(17, 3, true,"0.0", TestName = "Fractional positive value with scale > 0.")]
    [TestCase(4, 3, true,"0.000", TestName = "Fractional positive value with fractional part length = scale.")]
    [TestCase(2, 0, false,"-0", TestName = "Fractional negative value with scale = 0 and precision = value length.")]
    [TestCase(3, 1, false,"-0", TestName = "Fractional negative value.")]
    [TestCase(17, 3, false,"-0.000", TestName = "Fractional negative value and fractional part length = scale.")]
    [TestCase(4, 2, true,"+0.00", TestName = "Fractional positive value with length with '+' = precision.")] 
    public void IsValidNumber_ShouldBeTrue_ForValidArguments(int precision, int scale, bool allowSigned, string value)
    {
        var numberValidator = new NumberValidator(precision, scale, allowSigned);
        numberValidator.IsValidNumber(value).Should().BeTrue();
    }
    
    [TestCase(17, 2, true,"-0", TestName = "Negative integer value with onlyPositive flag.")]
    [TestCase(3, 2, true,"00.00", TestName = "Value length != precision.")]
    [TestCase(3, 2, true,"-0.00", TestName = "Negative fractional value with onlyPositive flag and fractional part length = scale.")]
    [TestCase(17, 2, true,"0.000", TestName = "Scale is greater than fractional part length.")]
    [TestCase(3, 2, true,"+1.23", TestName = "Fractional positive value with length without '+' = precision.")]
    [TestCase(3, 2, true,"-1.23", TestName = "Fractional negative value with length without '-' = precision.")]
    [TestCase(3, 2, true,"a.sd", TestName = "Value is not number")]
    [TestCase(4, 2, false,null, TestName = "Value is null.")]
    [TestCase(4, 2, false,"", TestName = "Value is empty string.")]
    public void IsValidNumber_ShouldBeFalse_ForInvalidArguments(int precision, int scale, bool allowSigned, string value)
    {
        var numberValidator = new NumberValidator(precision, scale, allowSigned);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }
}