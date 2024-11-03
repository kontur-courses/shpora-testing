using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase("0.0", 17, 2, true, ExpectedResult = true)]
    [TestCase("0", 17, 2, true, ExpectedResult = true)]
    [TestCase("00.00", 3, 2, true, ExpectedResult = false)]
    [TestCase("+0.00", 3, 2, true, ExpectedResult = false)]
    [TestCase("+1.23", 4, 2, true, ExpectedResult = true)]
    [TestCase("0.000", 17, 2, true, ExpectedResult = false)]
    [TestCase("a.sd", 3, 2, true, ExpectedResult = false)]
    [TestCase("000.00", 17, 2, true, ExpectedResult = true)]
    [TestCase("-1.23", 4, 2, true, ExpectedResult = false)]
    [TestCase("-1.23", 4, 2, false, ExpectedResult = true)]
    [TestCase("   ", 4, 2, true, ExpectedResult = false)]
    [TestCase(null, 4, 2, true, ExpectedResult = false)]
    [TestCase("0,0", 17, 2, true, ExpectedResult = true)]
    [TestCase("0.", 17, 2, true, ExpectedResult = false)]
    [TestCase(".0", 17, 2, true, ExpectedResult = false)]
    [TestCase("0..0", 17, 2, true, ExpectedResult = false)]
    [TestCase("+-3", 17, 2, true, ExpectedResult = false)]
    public bool IsValidNumber(string number, int precision, int scale = 0, bool onlyPositive = false)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);
    }
    
    [TestCase(0, 1, true, TestName = "precision == 0")]
    [TestCase(-1, 1, true, TestName = "precision < 0")]
    [TestCase(1, -1, true, TestName = "scale < 0")]
    [TestCase(2, 2, true, TestName = "scale == precision")]
    [TestCase(1, 2, true, TestName = "scale > precision")]
    public void NumberValidator_ShouldThrowExc_AfterInvalidArgs(int precision, int scale = 0, bool onlyPositive = false)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCase(3, 2, true)]
    [TestCase(3)]
    public void NumberValidator_ShouldNotThrowExc_AfterValidArgs(int precision, int scale = 0, bool onlyPositive = false)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should().NotThrow();
    }
}