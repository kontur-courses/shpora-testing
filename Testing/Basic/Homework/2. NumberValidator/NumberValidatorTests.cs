using System.Collections;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase("0", 1, 0, true, ExpectedResult = true, 
        TestName = "number=0; precision=1; scale=0; onlyPositive=true" )]
    [TestCase("0.1", 2, 1, true, ExpectedResult = true,
        TestName = "number=0.1; precision=2; scale=1; onlyPositive=true" )]
    [TestCase("0,1", 2, 1, true, ExpectedResult = true,
        TestName = "number=0,1; precision=2; scale=1; onlyPositive=true" )]
    [TestCase("-1", 2, 0, false, ExpectedResult = true,
        TestName = "number=-1; precision=2; scale=0; onlyPositive=false" )]
    [TestCase("-1.1", 3, 1, false, ExpectedResult = true,
        TestName = "number=-1.1; precision=3; scale=1; onlyPositive=false")]
    [TestCase("-1,1", 3, 1, false, ExpectedResult = true,
        TestName = "number=-1,1; precision=3; scale=1; onlyPositive=false")]
    [TestCase("+1", 2, 0, true, ExpectedResult = true,
        TestName = "number+1; precision=2; scale=0; onlyPositive=true")]
    [TestCase("+1.1", 3, 1, true, ExpectedResult = true,
        TestName = "number+1.1; precision=3; scale=1; onlyPositive=true")]
    [TestCase("+1,1", 3, 1, true, ExpectedResult = true,
        TestName = "number+1,1; precision=3; scale=1; onlyPositive=true")]
    
    [TestCase("", 2, 1, true, ExpectedResult = false,
        TestName = "number=\"\"; precision=2; scale=1; onlyPositive=true")]
    [TestCase(".0", 2, 1, true, ExpectedResult = false,
        TestName = "number=.0; precision=2; scale=1; onlyPositive=true")]
    [TestCase("0.", 2, 1, true, ExpectedResult = false,
        TestName = "number=0.; precision=2; scale=1; onlyPositive=true")]
    [TestCase("-1", 2, 0, true, ExpectedResult = false,
        TestName = "number=-1; precision=2; scale=0; onlyPositive=true" )]
    [TestCase("a.a", 2, 1, true, ExpectedResult = false,
        TestName = "number=a.a; precision=2; scale=1; onlyPositive=true")]
    [TestCase("1.1.1", 3, 2, true, ExpectedResult = false,
        TestName = "number=1.1.1; precision=3; scale=2; onlyPositive=true")]
    [TestCase("23,1", 2, 1, true, ExpectedResult = false,
        TestName = "number=23,1; precision=2; scale=1; onlyPositive=true")]
    [TestCase("23,1", 3, 0, true, ExpectedResult = false,
        TestName = "number=23,1; precision=3; scale=0; onlyPositive=true")]
    public bool IsValid(string number, int precision, int scale, bool onlyPositive)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);
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