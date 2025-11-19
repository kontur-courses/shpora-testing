
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true)]
    [TestCase(0, 2, true)]
    [TestCase(1, -1, true)]
    [TestCase(1, 3, true)]
    [TestCase(1, 1, true)]
    public void Constructor_ThrowArgumentException_WhenParametersInvalid(int precision, int scale, bool onlyPositive)
    {
        var numberValidatorConstructor = () => new NumberValidator(precision, scale, onlyPositive);
        numberValidatorConstructor.Should().Throw<ArgumentException>();
    }
    
    [TestCase(1, 0, true)]
    [TestCase(2, 1, true)]
    public void Constructor_NotThrowArgumentException_WhenParametersValid(int precision, int scale, bool onlyPositive)
    {
        var numberValidatorConstructor = () => new NumberValidator(precision, scale, onlyPositive);
        numberValidatorConstructor.Should().NotThrow<ArgumentException>();
    }
    
    [TestCase(17, 2, true, "0.0", true)]
    [TestCase(17, 2, true, "0.000", false)]
    [TestCase(17, 2, true, "0", true)]
    [TestCase(3, 2, true, "00.00", false)]
    [TestCase(3, 2, true, "+1.23", false)]
    [TestCase(3, 2, true, "-1.23", false)]
    [TestCase(3, 2, true, "123", true)]
    [TestCase(3, 2, true, "123.", false)]
    [TestCase(4, 2, true, "+1.23", true)]
    [TestCase(4, 2, true, "a.sd", false)]
    [TestCase(4, 2, true, "-1.23", false)]
    [TestCase(4, 2, true, "00,00", true)]
    [TestCase(4, 2, true, "1.2.3", false)]
    [TestCase(4, 2, true, ".0", false)]
    [TestCase(4, 2, true, "", false)]
    [TestCase(4, 2, true, "    ", false)]
    [TestCase(4, 2, false, "-1.23", true)]
    [TestCase(4, 2, false, null, false)]
    public void IsValidNumber_ReturnExpectedResult(int precision, int scale, bool onlyPositive, string input, bool expected)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(input).Should().Be(expected);
    }
}