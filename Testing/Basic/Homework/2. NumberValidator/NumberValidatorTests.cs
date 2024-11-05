using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "negative precision throws exception")]
    [TestCase(0, 2, false, TestName = "precision equals 0 throws exception")]
    [TestCase(1, -2, true, TestName = "negative scale throws exception")]
    [TestCase(1, 2, false, TestName = "scale greater than precision throws exception")]
    [TestCase(1, 1, true, TestName = "scale equals precision throws exception")]
    public void NumberValidation_ConstructorThrowsArgumentException(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale, onlyPositive);
        action.Should()
            .Throw<ArgumentException>();
    }

    [Test(Description = "default case")]
    public void NumberValidation_ConstructorDoesNotHaveArgumentException()
    {
        var action = () => new NumberValidator(1, 0, true);
        action.Should().NotThrow();
    }

    [TestCaseSource(typeof(TrueCasesData), nameof(TrueCasesData.TestCases))]
    public bool IsValidNumber_ReturnTrue(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }

    [TestCaseSource(typeof(FalseCasesData), nameof(FalseCasesData.TestCases))]
    public bool IsValidNumber_ReturnFalse(int precision, int scale, bool onlyPositive, string value)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(value);
    }

    public class TrueCasesData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(1, 0, true, "0").SetName("precision for 0 greater or equal 1").Returns(true);
                yield return new TestCaseData(2, 1, true, "0.0").SetName("precision for 0.0 >= 2 and scale >= 1").Returns(true);
                yield return new TestCaseData(2, 1, true, "0.1").SetName("precision for 0.X >= 2 and scale >= 1").Returns(true);
                yield return new TestCaseData(2, 1, true, "0,1").SetName("comma does not change precision and scale").Returns(true);
                yield return new TestCaseData(2, 0, true, "+1").SetName("plus adds 1 to minimum precision").Returns(true);
                yield return new TestCaseData(3, 1, true, "+1.1").SetName("plus adds 1 to minimum precision works with real numbers").Returns(true);
                yield return new TestCaseData(3, 1, true, "+1,1").SetName("plus adds 1 to minimum precision works with real numbers and comma").Returns(true);
                yield return new TestCaseData(2, 0, false, "-1").SetName("minus adds 1 to minimum precision").Returns(true);
                yield return new TestCaseData(3, 1, false, "-1.1").SetName("minus adds 1 to minimum precision works with real numbers").Returns(true);
                yield return new TestCaseData(3, 1, false, "-1,1").SetName("minus adds 1 to minimum precision works with real numbers and comma").Returns(true);
                yield return new TestCaseData(7, 2, true, "671,23").SetName("common case for real number with comma").Returns(true);
            }
        }
    }

    public class FalseCasesData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(2, 0, true, "").SetName("empty string is incorrect").Returns(false);
                yield return new TestCaseData(2, 1, true, ".0").SetName("before . always places at least one digit").Returns(false);
                yield return new TestCaseData(2, 0, true, "0.").SetName("after . always places at least one digit").Returns(false);
                yield return new TestCaseData(2, 0, true, "-1").SetName("- not accounted in precision").Returns(false);
                yield return new TestCaseData(10, 2, true, "abcde.f").SetName("use only digits").Returns(false);
                yield return new TestCaseData(10, 9, true, "127.0.0.1").SetName("only real numbers format").Returns(false);
                yield return new TestCaseData(3, 2, true, "4321.5").SetName("precision must be >= sum of all digits").Returns(false);
                yield return new TestCaseData(3, 0, true, "+145").SetName("+ not accounted in precision").Returns(false);
                yield return new TestCaseData(3, 0, false, "-124").SetName("- not accounted in precision for number > 9").Returns(false);
            }
        }
    }
}