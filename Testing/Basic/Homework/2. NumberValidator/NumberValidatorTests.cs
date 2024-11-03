
using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_ThrowsArgumentException_WhenPrecisionIsNegative()
    {
        const string msg = "precision must be a positive number";
        Action act = () => new NumberValidator(-1, 2); 
        act.Should().Throw<ArgumentException>().WithMessage(msg);
    }

    [Test]
    public void NumberValidator_ThrowsArgumentException_WhenScaleLessThanZero()
    {
        const string msg = "precision must be a non-negative number less or equal than precision";
        Action act = () => new NumberValidator(1, -2);
        act.Should().Throw<ArgumentException>().WithMessage(msg);
    }

    [Test]
    public void NumberValidator_ThrowsArgumentException_WhenScaleGreaterOrEqualThanPrecision()
    {
        const string msg = "precision must be a non-negative number less or equal than precision";
        Action actWhenGreater = () => new NumberValidator(17, 18);
        Action actWhenEqual = () => new NumberValidator(17, 17); 
        actWhenGreater.Should().Throw<ArgumentException>().WithMessage(msg);
        actWhenEqual.Should().Throw<ArgumentException>().WithMessage(msg);
    }

    [Test]
    public void NumberValidator_DoesNotThrow_WhenPrecisionIsPositive()
    {
        Action act = () => new NumberValidator(1);
        act.Should().NotThrow();
    }

    [Test]
    public void NumberValidator_DoesNotThrow_WhenScaleLessThanPrecision()
    {
        Action act = () => new NumberValidator(2, 1);
        act.Should().NotThrow();
    }

    [TestCase("2", 1, true)]
    [TestCase("52", 2, true)]
    [TestCase("-300", 4, false)]
    [TestCase("300", 3, false)]
    [TestCase("-99", 3, false)]
    [TestCase("9999999", 7, true)]
    [TestCase("-999999999", 10, false)] 
    public void IsValidNumber_ShouldBeTrue_WhenValueIsInteger(string value, int precision, bool onlyPositive) => 
        TestShouldBeTrue(value, precision, 0, onlyPositive);
    
    [TestCase("52", 1, true)]
    [TestCase("-300", 3, false)]
    [TestCase("300", 2, false)]
    [TestCase("-1234567890", 10, false)]
    public void IsValidNumber_ShouldBeFalse_WhenValueIsInteger(string value, int precision, bool onlyPositive) =>
        TestShouldBeFalse(value, precision, 0, onlyPositive);
   
    [TestCase("a.sd")]
    [TestCase("seven.eleven")]
    [TestCase("     ")]
    [TestCase("five/two")]
    [TestCase("")]
    [TestCase(null)]
    public void IsValidNumber_ShouldBeFalse_WhenValueIsNonNumber(string value) => TestShouldBeFalse(value);
    
    [TestCase("0.000", 4,3, true)]
    [TestCase("-1234567890.1234567890", 21, 10, false)]
    [TestCase("0.0", 2, 1,false)]
    [TestCase("300.52", 5, 2, true)]
    public void IsValidNumber_ShouldBeTrue_WhenValueIsFloatNumber(string value, int precision, int scale, 
        bool onlyPositive) => TestShouldBeTrue(value, precision, scale, onlyPositive);
    
    [TestCase("-1.23", 3, 2, false)]
    [TestCase("-1234567890.1234567890", 20, 10, false)]
    [TestCase("100", 2, 1, true)]
    public void IsValidNumber_ShouldBeFalse_WhenValueIsFloatNumber(string value, int precision, int scale, 
        bool onlyPositive) => TestShouldBeFalse(value, precision, scale, onlyPositive);
    
    [TestCase("1.1")]
    [TestCase("300,52")]
    public void IsValidNumber_ShouldBeTrue_WhenFormatOfValueIsOk(string value) => TestShouldBeTrue(value);
    
    [TestCase("X")]
    [TestCase("X,Y")]
    [TestCase("1\n.5")]
    [TestCase("+-15")]
    [TestCase("300_000")]
    [TestCase("52 000")]
    [TestCase("1/2")]
    [TestCase("12 . 22")]
    public void IsValidNumber_ShouldBeFalse_WhenFormatOfValueIsNotOk(string value) => TestShouldBeFalse(value);
    
    [TestCase("+1.23")]
    [TestCase("+300.52")]
    [TestCase("-999.99")]
    [TestCase("-101")]
    public void IsValidNumber_ShouldBeTrue_WhenValueHasSign(string value) => TestShouldBeTrue(value);
    
    private static void TestShouldBeTrue(string value, int precision = 17, int scale = 10, bool onlyPositive = false) => 
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeTrue();

    private static void TestShouldBeFalse(string value, int precision = 17, int scale = 10, bool onlyPositive = false) =>
        new NumberValidator(precision, scale, onlyPositive)
            .IsValidNumber(value)
            .Should().BeFalse();
}