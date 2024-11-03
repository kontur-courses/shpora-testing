using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1,2,true,ExpectedResult = true,TestName = "Negative precision")]
    [TestCase(1,0,true,ExpectedResult = false,TestName = "Not catch Exception")]
    [TestCase(2,5,true,ExpectedResult = true,TestName = "Precision less then scale")]
    [TestCase(2,-3,true,ExpectedResult = true,TestName = "Negative scale")]
    public bool NumberValidator_Exceptions_Test(int precision, int scale, bool onlyPositive)
    {
        try
        {
            new NumberValidator(precision, scale, onlyPositive);
        }
        catch (ArgumentException)
        {
            return true;
        }
        
        return false;
    }

    [Test, TestCaseSource(nameof(NumberValidatorTestCases))]
    public bool IsValidNumber(NumberValidator numberValidator, String number)
    {
        return numberValidator.IsValidNumber(number);
    }

    public static IEnumerable<TestCaseData> NumberValidatorTestCases
    {
        get
        {
            yield return new TestCaseData(new NumberValidator(8, 2, true), "0.0").Returns(true);
            yield return new TestCaseData(new NumberValidator(3, 0, true), "0").Returns(true);
            yield return new TestCaseData(new NumberValidator(4, 2, true), "-1.23").Returns(false);
            yield return new TestCaseData(new NumberValidator(4, 2, false), "-1.24").Returns(true);
            yield return new TestCaseData(new NumberValidator(5, 2, true), "1.253").Returns(false);
            yield return new TestCaseData(new NumberValidator(3, 2, false), "a.ds").Returns(false); 
            yield return new TestCaseData(new NumberValidator(4, 2, true), "").Returns(false);
            yield return new TestCaseData(new NumberValidator(5, 2, true), "+-1.25").Returns(false);
            yield return new TestCaseData(new NumberValidator(5, 2, true), null).Returns(false);
            yield return new TestCaseData(new NumberValidator(3, 0, true), "1234").Returns(false);
        }
    }
}