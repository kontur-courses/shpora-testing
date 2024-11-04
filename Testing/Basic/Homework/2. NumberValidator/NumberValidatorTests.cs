using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Test()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
        Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));

        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
        ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    }
}