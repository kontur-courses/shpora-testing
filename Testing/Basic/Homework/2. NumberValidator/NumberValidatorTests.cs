
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static System.Formats.Asn1.AsnWriter;
using Xunit.Sdk;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    #region Constructor Tests

    [TestCase(-1, 2, true)]
    [TestCase(-3, 2, false)]
    [TestCase(0, 2, true)]
    public void ArgumentException_When_WrongPrecision(int precision, int scale, bool onlyPositive)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
    }

    [Test]
    public void DoNotThrowArgumentException_When_ScaleIsZero()
    {
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    }

    [TestCase(17, -1, true)]
    [TestCase(17, 17, true)]
    [TestCase(17, 18, true)]
    public void ArgumentException_When_NotValidScale(int precision, int scale, bool onlyPositive)
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
    }

    [TestCase(6, 3, true)]
    [TestCase(6, 3, false)]
    [TestCase(10, 9, false)]
    public void DoNotThrowArgumentException_When_ValidArguments(int precision, int scale, bool onlyPositive)
    {
        Assert.DoesNotThrow(() => new NumberValidator(precision, scale, onlyPositive));
    }

    #endregion

    #region IsValidNumber Tests

    [TestCase(17, 6, true, "0")]
    [TestCase(17, 2, false, "9")]
    [TestCase(17, 0, true, "3")]
    public void IsValidNumber_WhenNumberWithoutFractionalPart_ReturnTrue(int precision, int scale,
        bool onlyPositive, string testString)
    {
        ClassicAssert.IsTrue(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(17, 1, true, "0.0")]
    [TestCase(17, 3, true, "0,000")]
    [TestCase(10, 6, true, "2,1")]
    public void IsValidNumber_WithFractionalPart_ReturnTrue(int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsTrue(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(17, 1, true, null)]
    [TestCase(17, 3, true, "")]
    public void IsValidNumber_WhenNullOrEmpty_ReturnFalse(int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(17, 6, true, "#1.0")]
    [TestCase(17, 6, true, "a.sd")]
    [TestCase(17, 6, true, "1.1.0")]
    [TestCase(17, 6, true, "1,1.0")]
    [TestCase(17, 6, true, "2.1sd")]
    public void IsValidNumber_WhenWrongString_ReturnFalse(int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(6, 4, true, "1234567")]
    [TestCase(6, 4, true, "1234.567")]
    [TestCase(3, 2, true, "00.00")]
    [TestCase(6, 4, true, "+1234.56")]
    public void IsValidNumber_OverflowPrecision_ReturnFalse
        (int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(6, 3, true, "3.1416")]
    [TestCase(6, 0, true, "3.1")]
    public void IsValidNumber_FractionExceedsScale_ReturnFalse
        (int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(6, 3, true, "-1.23")]
    [TestCase(6, 0, true, "-5")]
    public void IsValidNumber_WhenOnlyPositiveTrue_ReturnFalse
        (int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsFalse(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    [TestCase(4, 2, true, "+1.23")]
    [TestCase(6, 0, false, "-5")]
    [TestCase(6, 4, true, "12.3456")]
    public void IsValidNumber_WithValidInput_ReturnsTrue
        (int precision, int scale, bool onlyPositive, string testString)
    {
        ClassicAssert.IsTrue(new NumberValidator(precision, scale, onlyPositive).IsValidNumber(testString));
    }

    #endregion
}