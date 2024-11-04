using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void Test()
    {
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

    [Test]
    public void NumberValidator_CorrectParameters_AfterCreating()
    {
        var creation = () => new NumberValidator(1, 0, true);

        creation.Should().NotThrow<ArgumentException>();
    }
    
    [Test]
    public void NumberValidator_IncorrectPrecision_AfterCreating()
    {
        var creation = () => new NumberValidator(-1, 2);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }
    
    [Test]
    public void NumberValidator_IncorrectScale_AfterCreating()
    {
        var creation = () => new NumberValidator(1, -1);

        creation.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
        
        var secondCreation = () => new NumberValidator(2, 1);

        secondCreation.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }

}