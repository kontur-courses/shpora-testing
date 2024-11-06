using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "Negative precision throws exception")]
    [TestCase(0, 2, false, TestName = "Precision equals 0 throws exception")]
    [TestCase(1, -2, true, TestName = "Negative scale throws exception")]
    [TestCase(1, 2, false, TestName = "Scale greater than precision throws exception")]
    [TestCase(1, 1, true, TestName = "Scale equals precision throws exception")]
    public void NumberValidation_ConstructorThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);

        action.Should().Throw<ArgumentException>();
    }

    [Test(Description = "Creating an instance of a class with the correct parameters should not throw an exception")]
    public void NumberValidation_ConstructorDoesNotHaveArgumentException()
    {
        var action = () => new NumberValidator(1, 0, true);

        action.Should().NotThrow();
    }

    [TestCaseSource(nameof(TestCase1))]
    [TestCaseSource(nameof(TestCase2))]
    public bool IsValidNumber_WorksCorrectly(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }

    public static IEnumerable TestCase1
    {
        get
        {
            yield return new TestCaseData(1, 0, true, "0").SetName("Validation of zero with the correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(2, 1, true, "0.0").SetName("Validation of zero with a zero fractional part separated by a dot with the correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(2, 1, true, "0.1").SetName("Validation of zero with a non-zero fractional part separated by a dot with correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(2, 1, true, "0,1").SetName("Validation of zero with a non-zero decimal part separated by a comma with correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(2, 0, true, "+1").SetName("Validation of a positive integer with an explicit plus sign with correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(3, 1, true, "+1.1").SetName("Validation of a positive number with a non-zero fractional part separated by a dot and an explicit plus sign with the correct parameters must be completed successfully").Returns(true);
            yield return new TestCaseData(3, 1, true, "+1,1").SetName("Validation of a positive number with a non-zero fractional part separated by a comma and an explicit plus sign with the correct parameters must be completed successfully").Returns(true);
            yield return new TestCaseData(2, 0, false, "-1").SetName("Validation of a negative integer with an explicit minus sign with correct parameters should be completed successfully").Returns(true);
            yield return new TestCaseData(3, 1, false, "-1.1").SetName("Validation of a negative number with a non-zero fractional part separated by a dot and an explicit minus sign with the correct parameters must be completed successfully").Returns(true);
            yield return new TestCaseData(3, 1, false, "-1,1").SetName("Validation of a negative number with a non-zero fractional part separated by a comma and an explicit minus sign with the correct parameters must be completed successfully").Returns(true);
            yield return new TestCaseData(7, 2, true, "671,23").SetName("Validation a real number with a non-zero fractional part separated by a comma with the correct parameters must be completed successfully").Returns(true);
        }
    }

    public static IEnumerable TestCase2
    {
        get
        {
            yield return new TestCaseData(2, 0, true, "").SetName("Empty string is not allowed").Returns(false);
            yield return new TestCaseData(2, 1, true, ".0").SetName("Integer part of a real number must be").Returns(false);
            yield return new TestCaseData(2, 0, true, "0.").SetName("Fractional part of a real number must be if a separator character is used").Returns(false);
            yield return new TestCaseData(2, 0, true, "-1").SetName("If a minus is explicitly specified, onlyPositive must be false").Returns(false);
            yield return new TestCaseData(10, 2, true, "abcde.f").SetName("Use only digits").Returns(false);
            yield return new TestCaseData(10, 9, true, "127.0.0.1").SetName("Only real numbers format").Returns(false);
            yield return new TestCaseData(3, 2, true, "4321.5").SetName("If precision is less than number of digits in the number, return false").Returns(false);
            yield return new TestCaseData(3, 0, true, "+145").SetName("If a plus is explicitly specified, it must be taken into account when calculating precision").Returns(false);
            yield return new TestCaseData(3, 0, false, "-124").SetName("If a minus is explicitly specified, it must be taken into account when calculating precision").Returns(false);
        }
    }
}