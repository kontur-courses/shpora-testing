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

    [TestCase("0", 1, 0, true, ExpectedResult = true)]
    [TestCase("0.1", 2, 1, true, ExpectedResult = true)]
    [TestCase("0,1", 2, 1, true, ExpectedResult = true)]
    [TestCase("-1", 2, 0, false, ExpectedResult = true)]
    [TestCase("-1.1", 3, 1, false, ExpectedResult = true)]
    [TestCase("-1,1", 3, 1, false, ExpectedResult = true)]
    [TestCase("+1", 2, 0, true, ExpectedResult = true)]
    [TestCase("+1.1", 3, 1, true, ExpectedResult = true)]
    [TestCase("+1,1", 3, 1, true, ExpectedResult = true)]
    
    [TestCase("", 2, 1, true, ExpectedResult = false)]
    [TestCase(".0", 2, 1, true, ExpectedResult = false)]
    [TestCase("0.", 2, 1, true, ExpectedResult = false)]
    [TestCase("-1", 2, 0, true, ExpectedResult = false)]
    [TestCase("a.a", 2, 1, true, ExpectedResult = false)]
    [TestCase("1.1.1", 3, 2, true, ExpectedResult = false)]
    [TestCase("23,1", 2, 1, true, ExpectedResult = false)]
    [TestCase("23,1", 3, 0, true, ExpectedResult = false)]
    [TestCase("+1", 1, 0, true, ExpectedResult = false)]
    public bool IsValid(string number, int precision, int scale, bool onlyPositive)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number);
    }

    [TestCase(-1, 2, true, TestName = "negative precision")]
    [TestCase(1, -2, true, TestName = "negative scale")]
    [TestCase(1, 2, true, TestName = "scale > precision")]
    [TestCase(1, 1, true, TestName = "scale == precision")]
    [TestCase(0, 0, false, TestName = "precision == 0")]
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