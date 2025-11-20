using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string PrecisionExceptionMessage = "precision must be a positive number";
    private const string ScaleExceptionMessage = "scale must be a non-negative number less than precision";
    
    [Test]
    [TestCase(-1)]
    [TestCase(0)]
    public void Should_ThrowException_WhenNotPositivePrecision(int precision)
    {
        
        var action = () => new NumberValidator(precision, 2, true);

        action.Should().Throw<ArgumentException>()
            .WithMessage(PrecisionExceptionMessage);
    }
    
    [Test]
    [TestCase(1, 2)]
    [TestCase(1, 1)]
    public void Should_ThrowException_WhenPositivePrecisionLessOrEqualScale(int  precision, int scale)
    {
        
        var action = () => new NumberValidator(precision, scale, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage(ScaleExceptionMessage);
    }
    
    [Test]
    public void Should_ThrowException_WhenNegativeScale()
    {
        var action = () => new NumberValidator(1, -1, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage(ScaleExceptionMessage);
    }

    [Test]
    [TestCase(1, 0, true)]
    [TestCase(5, 2, false)]
    [TestCase(4, 3, true)]
    public void ShouldNot_ThrowException_WhenValidParameters(int precision, int scale, bool onlyPositive)
    {
         var action = () => new NumberValidator(precision, scale, onlyPositive);
         
         action.Should().NotThrow();
    }
    
    [Test]
    [TestCase(4, 2, true, "12.34")]
    [TestCase(4, 3, true, "+2.34")]
    [TestCase(4, 3, true, "1.345")]
    public void Should_BeTrue_WhenValidNumber(int precision, int scale, bool onlyPositive, string testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
            
        var res = testNum.IsValidNumber("12.34");
        
        res.Should().BeTrue();
    }

    [Test]
    public void Should_BeFalse_WhenNegativeNumberWithOnlyPositiveTrue()
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber("-2.34");
        
        res.Should().BeFalse();
    }

    [Test]
    public void Should_BeTrue_WhenNegativeNumberWithOnlyPositiveFalse()
    {
        var testNum = new NumberValidator(4, 2, false);
            
        var res = testNum.IsValidNumber("-2.34");
        
        res.Should().BeTrue();
    }
    
    [Test]
    [TestCase(4, 2, true, "512.34")]
    [TestCase(4, 3, true, "12.345")]
    public void Should_BeFalse_WhenNumberLongerThanPrecision(int precision, int scale, bool onlyPositive, string testNumber)
    {
        var testNum = new NumberValidator(precision, scale, onlyPositive);
        
        var res = testNum.IsValidNumber(testNumber);
        
        res.Should().BeFalse();
    }

    [Test]
    public void Should_BeFalse_WhenNumberWithFracLongerThanScaleButValidPrecision()
    {
        var testNum = new NumberValidator(5, 2, true);
        
        var res = testNum.IsValidNumber("12.345");
        
        res.Should().BeFalse();
    }

    [Test]
    public void Should_BeFalse_WhenString()
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber("ab.cd");
        
        res.Should().BeFalse();
    }
    
    [Test]
    public void Should_BeTrue_WhenInt()
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber("52");
        
        res.Should().BeTrue();
    }
    
    [Test]
    [TestCase("")]
    [TestCase(null)]
    public void Should_BeFalse_WhenNullOrEmpty(string testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber(testNumber);
        
        res.Should().BeFalse();
    }
    
    [Test]
    [TestCase("12.43")]
    [TestCase("12,43")]
    public void Should_BeTrue_WhenDifferentSeparator(string testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber(testNumber);
        
        res.Should().BeTrue();
    }
    
    [Test]
    public void Should_BeFalse_WhenNumberWithinInt()
    {
        var testNum1 = new NumberValidator(4, 2, true).IsValidNumber(".5");
        var testNum2 = new NumberValidator(4, 2, true).IsValidNumber(",5");
        
        testNum1.Should().BeFalse();
        testNum2.Should().BeFalse();
    }
    
    [Test]
    [TestCase("1.")]
    [TestCase("1,")]
    public void Should_BeFalse_WhenNumberWithSeparatorButWithinFrac(string  testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber(testNumber);
        
        res.Should().BeFalse();
    }

    [Test]
    [TestCase("1.2.3")]
    [TestCase("1,,3")]
    public void Should_BeFalse_WhenIncorrectNum(string  testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
        
        var res = testNum.IsValidNumber(testNumber);
        
        res.Should().BeFalse();
    }
    
    [Test]
    [TestCase(" 12")]
    [TestCase("12 ")]
    [TestCase("1 2")]
    public void Should_BeFalse_WhenNumWithWhitespaces(string  testNumber)
    {
        var testNum = new NumberValidator(4, 2, true);
        var res = testNum.IsValidNumber(testNumber);
        res.Should().BeFalse();

    }
}