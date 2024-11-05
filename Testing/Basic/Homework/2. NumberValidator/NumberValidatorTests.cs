
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Test()
    {
        //Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
        //Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
        //Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, false));
        //Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));

        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
        ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
        ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
        ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
        ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    }

    [Test]
    public void NumberValidator_WhenPrecisionIsNegative_Fails()
    {
        Action act = () => new NumberValidator(-1, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenPrecisionIsZero_Fails()
    {
        Action act = () => new NumberValidator(0, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }

    [Test]
    public void NumberValidator_WhenScaleIsZero_NotFails()
    {
        Action act = () => new NumberValidator(1, 0, true);

        act
            .Should().NotThrow();
    }

    [Test]
    public void NumberValidator_WhenScaleIsNegative_Fails()
    {
        Action act = () => new NumberValidator(1, -1, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    [Test]
    public void NumberValidator_WhenPrecisionLessThanScale_Fails()
    {
        Action act = () => new NumberValidator(1, 2, false);

        act
            .Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

    
}