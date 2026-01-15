
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test, TestCaseSource(nameof(GenerateValidatorTestsWithExceptions))]
    public void Tests_NumberValidator_WithException(int precision, int scale, bool onlyPositive)
    {
        var validatorAction = () => new NumberValidator(precision, scale, onlyPositive);
        validatorAction.Should().Throw<ArgumentException>();
    }

    public static IEnumerable<TestCaseData> GenerateValidatorTestsWithExceptions
    {
        get
        {
            var tests = new[]
            {
                new { precision = -1, scale = 2, onlyPositive = true},
                new { precision = -1, scale = 2, onlyPositive = false},
                new { precision = 1, scale = -1, onlyPositive = false},
                new { precision = 1, scale = 10, onlyPositive = false},
            };
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.precision, test.scale, test.onlyPositive)
                    .SetName($"Exception should be thrown with {test.precision} precision, {test.scale} scale, {test.onlyPositive} onlyPositive");
            }
        }
    }

    [Test, TestCaseSource(nameof(GenerateValidatorTestsWithoutExceptions))]
    public void Tests_NumberValidator_WithoutException(int precision, int scale, bool onlyPositive)
    {
        var validatorAction = () => new NumberValidator(precision, scale, onlyPositive);
        validatorAction.Should().NotThrow<ArgumentException>();
    }

    public static IEnumerable<TestCaseData> GenerateValidatorTestsWithoutExceptions
    {
        get
        {
            var tests = new[]
            {
                new { precision = 1, scale = 0, onlyPositive = true},
            };
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.precision, test.scale, test.onlyPositive)
                    .SetName($"Exception should be thrown with {test.precision} precision, {test.scale} scale, {test.onlyPositive} onlyPositive");
            }
        }
    }

    [Test, TestCaseSource(nameof(GenerateValidatorTestIsValidNumberMethod))]
    public bool ValidatorTest_IsValidNumber_Method(int precision, int scale, bool onlyPositive, string validatingNumber)
    {
        var numberValidator = new NumberValidator(precision, scale, onlyPositive);
        return numberValidator.IsValidNumber(validatingNumber);
    }

    public static IEnumerable<TestCaseData> GenerateValidatorTestIsValidNumberMethod
    {
        get
        {
            var tests = new[]
            {
                new { precision = 17, scale = 2, onlyPositive = true, validatingNumber = "0.0", expectingResult = true},
                new { precision = 17, scale = 2, onlyPositive = true, validatingNumber = "0", expectingResult = true},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "00.00", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "-0.00", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "+0.00", expectingResult = false},
                new { precision = 4, scale = 2, onlyPositive = true, validatingNumber = "1.23", expectingResult = true},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "+1.23", expectingResult = false},
                new { precision = 17, scale = 2, onlyPositive = true, validatingNumber = "0.000", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "-1.23", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "a.sd", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "", expectingResult = false},
                new { precision = 3, scale = 2, onlyPositive = true, validatingNumber = "0,0", expectingResult = true},
                new { precision = 17, scale = 2, onlyPositive = false, validatingNumber = "+-1.23", expectingResult = false},
                new { precision = 17, scale = 2, onlyPositive = false, validatingNumber = "-1.23", expectingResult = true},
                new { precision = 17, scale = 2, onlyPositive = true, validatingNumber = ".23", expectingResult = false},
                new { precision = 17, scale = 2, onlyPositive = true, validatingNumber = "23.", expectingResult = false},
            };
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.precision, test.scale, test.onlyPositive, test.validatingNumber)
                    .Returns(test.expectingResult)
                    .SetName($"Test with precision: {test.precision}, scale: {test.scale}, onlyPositive: {test.onlyPositive}," +
                             $"validatingNumber: {test.validatingNumber} should return {test.expectingResult}, but returned {!test.expectingResult}");
            }
        }
    }
}