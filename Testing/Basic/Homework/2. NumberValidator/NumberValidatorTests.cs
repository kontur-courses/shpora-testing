
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Should_Throw_ArgumentException_If_Precision_Is_Negative() =>
        ShouldThrowExceptionWhenCreating(-1, 5);

    [Test]
    public void Should_Throw_ArgumentException_If_Scale_Is_Negative() =>
        ShouldThrowExceptionWhenCreating(5, -1);

    [Test]
    public void Should_Throw_ArgumentException_If_Scale_Greater_Precision() =>
        ShouldThrowExceptionWhenCreating(2, 5);

    [Test]
    public void Should_Throw_ArgumentException_If_Scale_Equal_Precision() =>
        ShouldThrowExceptionWhenCreating(2, 2);

    private void ShouldThrowExceptionWhenCreating(int precision, int scale)
    {
        var builder = new TestNumberValidatorBuilder().WithPrecisionAndScale(precision, scale);
        
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Test]
    public void Should_DoesNotThrow_Exception_If_Scale_And_Precision_Is_Correct()
    {
        var builder = new TestNumberValidatorBuilder();
        
        Assert.DoesNotThrow(() => builder.Build());
    }

    [Test]
    public void IsValidNumber_Should_Return_False_If_Value_Is_Null() =>
        IsValidNumberShouldReturnFalse(null!, 10, 5);
    
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("    ")]
    public void IsValidNumber_Should_Return_False_If_Value_Is_Empty(string value) => 
        IsValidNumberShouldReturnFalse(value, 10, 5);
    
    [TestCase("a.sd")]
    [TestCase("0.")]
    [TestCase(",0")]
    [TestCase("0*0")]
    [TestCase("!0,0")]
    public void IsValidNumber_Should_Return_False_If_Match_With_Value_Fail(string value) => 
        IsValidNumberShouldReturnFalse(value, 10, 5);
    
    [TestCase("1,234")]
    [TestCase("1111")]
    [TestCase("22,32")]
    [TestCase("+1,23")]
    [TestCase("+0,00")]
    [TestCase("-0,00")]
    public void IsValidNumber_Should_Return_False_If_Sum_Of_IntPart_And_FracPart_More_Than_Precision(string value) => 
        IsValidNumberShouldReturnFalse(value, 3, 2);

    [Test]
    public void IsValidNumber_Should_Return_False_If_FracPart_More_Than_Scale() =>
        IsValidNumberShouldReturnFalse("0,000", 10, 2);
    
    [TestCase("-0,0")]
    [TestCase("-0,000")]
    [TestCase("-0")]
    public void IsValidNumber_Should_Return_False_If_OnlyPositive_True_And_Value_Contains_Minus(string value) => 
        IsValidNumberShouldReturnFalse(value, 10, 5);

    private void IsValidNumberShouldReturnFalse(string value, int precision, int scale, bool onlyPositive = true)
    {
        var numberValidator = new TestNumberValidatorBuilder()
            .WithPrecisionAndScale(precision, scale)
            .WithOnlyPositive(onlyPositive)
            .Build();
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeFalse();
    }
    
    [TestCase("-0,0")]
    [TestCase("-0,000")]
    [TestCase("-0")]
    public void IsValidNumber_Should_Return_True_If_OnlyPositive_False_And_Value_Contains_Minus(string value)
    {
        var numberValidator = new TestNumberValidatorBuilder().WithOnlyPositive(false).Build();
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeTrue();
    }
    
    [TestCase("0.0")]
    [TestCase("0")]
    [TestCase("+1.23")]
    [TestCase("1.444")]
    [TestCase("+1.444")]
    public void IsValidNumber_Should_Return_True_If_Value_Correct(string value)
    {
        var numberValidator = new TestNumberValidatorBuilder().Build();
        
        var actual = numberValidator.IsValidNumber(value);
        
        actual.Should().BeTrue();
    }
}