using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCaseSource(nameof(ValidatorTestCases))]
    public bool IsValid(NumberValidator numberValidator, string number)
    {
        return numberValidator.IsValidNumber(number);
    }
    
    public static IEnumerable ValidatorTestCases
    {
        get
        {
            yield return new TestCaseData(new NumberValidator(17, 2, true), "0.0")
                .SetName("precision=17, scale=2, onlyPositive=true, number=0.0")
                .Returns(true);
            
            yield return new TestCaseData(new NumberValidator(17, 2, true), "0")
                .SetName("precision=17, scale=2, onlyPositive=true, number=0")
                .Returns(true);
            
            yield return new TestCaseData(new NumberValidator(3, 2, true), "00.00")
                .SetName("precision=3, scale=2, onlyPositive=true, number=00.00")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(3, 2, true), "-0.00")
                .SetName("precision=3, scale=2, onlyPositive=true, number=-0.00")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(3, 2, true), "+0.00")
                .SetName("precision=3, scale=2, onlyPositive=true, number=+0.00")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(4, 2, true), "+1.23")
                .SetName("precision=4, scale=2, onlyPositive=true, number=+1.23")
                .Returns(true);
            
            yield return new TestCaseData(new NumberValidator(3, 2, true), "+1.23")
                .SetName("precision=3, scale=2, onlyPositive=true, number=+1.23")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(17, 2, true), "0.000")
                .SetName("precision=17, scale=2, onlyPositive=true, number=0.000")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(3, 2, true), "-1.23")
                .SetName("precision=3, scale=2, onlyPositive=true, number=-1.23")
                .Returns(false);;
           
            yield return new TestCaseData(new NumberValidator(3, 2, true), "a.sd")
                .SetName("precision=3, scale=2, onlyPositive=true, number=a.sd")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(4, 2), "-1.23")
                .SetName("precision=4, scale=2, onlyPositive=false, number=-1.23")
                .Returns(true);
            
            yield return new TestCaseData(new NumberValidator(3, 2), "-1.23")
                .SetName("precision=3, scale=2, onlyPositive=false, number=-1.23")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(4, 2), "a.sd")
                .SetName("precision=4, scale=2, onlyPositive=false, number=a.sd")
                .Returns(false);

            yield return new TestCaseData(new NumberValidator(3, 2), "-0.00")
                .SetName("precision=3, scale=2, onlyPositive=false, number=-0.00")
                .Returns(false);
            
            yield return new TestCaseData(new NumberValidator(4, 2), "-0.00")
                .SetName("precision=4, scale=2, onlyPositive=false, number=-0.00")
                .Returns(true);
        }
    }

    [TestCase(-1, 2, true, TestName = "negative precision")]
    [TestCase(1, -2, true, TestName = "negative scale")]
    [TestCase(1, 2, true, TestName = "scale >= precision")]
    public void NumberValidator_ThrowsException_AfterWrongCreation(int precision, int scale, bool onlyPositive)
    {
        var wrongCreation = () => new NumberValidator(precision, scale, onlyPositive);
        wrongCreation.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_DoesNotThrowException_AfterRightCreation()
    {
        var rightCreation = () => new NumberValidator(3, 2, true);
        rightCreation.Should().NotThrow();
    }
}