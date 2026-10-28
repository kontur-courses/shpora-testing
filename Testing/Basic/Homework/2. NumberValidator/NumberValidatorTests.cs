
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    //я выделил табами новые тесты (для вашего удобства, чтобы с прошлой версией не сранивать)
        [TestCase(0, 1, TestName = "precision={0}, scale={1}")]
        [TestCase(2, 3, TestName = "precision={0}, scale={1}")]
    [TestCase(-1, 0, TestName = "precision={0}, scale={1}")]
    [TestCase(2, -1, TestName = "precision={0}, scale={1}")]
    [TestCase(2, 2, TestName = "precision={0}, scale={1}")]
    public void NumberValidator_InvalidArguments_Throws(int precision, int scale)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive: false);

        act.Should().Throw<ArgumentException>();
    }

        [TestCase(17, false, "-123", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
        [TestCase(4, true, "+1.23", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
        [TestCase(4, true, "1234", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(17, true, "123", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(17, true, "0.0", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(17, true, "1,23", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(17, true, "+1.23", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(17, false, "-1.23", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    [TestCase(5, true, "123.45", TestName = "value=\"{2}\", precision={0}, onlyPositive={1}")]
    public void IsValidNumber_ValidValues_ReturnsTrue(
        int precision, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale: 2, onlyPositive);

        validator.IsValidNumber(value).Should().BeTrue();
    }

        [TestCase(17, null, TestName = "value=null, precision={0}")]
        [TestCase(17, "-123", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "-0.0", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(4, "+1234", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, " 123", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "123 ", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "1 23", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "1. 23", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "\t123", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "123\n", TestName = "value=\"{1}\", precision={0}")] //не проходит. Чисто теоретически, так и надо
        [TestCase(17, "123\r\n", TestName = "value=\"{1}\", precision={0}")]
        [TestCase(17, "123\r", TestName = "value=\"{1}\", precision={0}")]
    [TestCase(17, "", TestName = "value=\"{1}\", precision={0}")]
    [TestCase(17, "a.sd", TestName = "value=\"{1}\", precision={0}")]
    [TestCase(3, "1234", TestName = "value=\"{1}\", precision={0}")]
    [TestCase(17, "0.000", TestName = "value=\"{1}\", precision={0}")]
    [TestCase(17, "-1.23", TestName = "value=\"{1}\", precision={0}")]
    public void IsValidNumber_InvalidValues_ReturnsFalse(int precision, string value)
    {
        var validator = new NumberValidator(precision, scale: 2, onlyPositive: true);

        validator.IsValidNumber(value).Should().BeFalse();
    }
}