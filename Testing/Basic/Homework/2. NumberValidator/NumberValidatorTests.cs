
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_IncorrectPrecision_Exception()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
    }

    [Test]
    public void NumberValidator_CorrectInitialization_NoExceptions()
    {
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    }

    [Test]
    public void NumberValidator_IncorrectScale_Exception()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(1, -1, true));
        Assert.Throws<ArgumentException>(() => new NumberValidator(1, 2, true));
        Assert.Throws<ArgumentException>(() => new NumberValidator(1, 1, true));
    }

    [Test]
    public void IsValidNumber_InvalidCharacters_False()
    {
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    }

    [TestCase("00.00", 3, 2)]
    [TestCase("+1.23", 3, 2)]
    [TestCase("0.000", 17, 2)]
    public void IsValidNumber_NotTheAppropriateLength_False(string value, int precision, int scale = 0, bool onlyPositive = true)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value));
    }

    [TestCase("+1.23", 4, 2)]
    [TestCase("0", 17, 2)]
    [TestCase("0.0", 17, 2)]
    public void IsValidNumber_CorrectValue_True(string value, int precision, int scale = 0, bool onlyPositive = true)
    {
        ClassicAssert.IsTrue(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value));
    }

    [TestCase("-0.00", 3, 2)]
    [TestCase("+0.00", 3, 2, false)]
    public void IsValidNumber_TheWrongSign_False(string value, int precision, int scale = 0, bool onlyPositive = true)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value));
    }

    [TestCase("", 1)]
    [TestCase(null, 1)]
    public void IsValidNumber_NullOrEmpty_False(string value, int precision, int scale = 0, bool onlyPositive = true)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value));
    }

    [Test]
    public void IsValidNumber_InappropriateAccuracy_False()
    {
        ClassicAssert.IsFalse(new NumberValidator(4, 2, true).IsValidNumber("0.000"));
    }
}