
using FluentAssertions;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public partial class NumberValidatorTests
{
	[TestCaseSource(nameof(ValidTestCases))]
	public void Should_Be_Valid(int precision, int scale, bool onlyPositive, string value)
	{
		var validator = new NumberValidator(precision, scale, onlyPositive);

		var result = validator.IsValidNumber(value);

		result.Should().BeTrue();
	}

	[TestCaseSource(nameof(NotValidTestCases))]
	public void Should_Be_NotValid(int precision, int scale, bool onlyPositive, string value)
	{
		var validator = new NumberValidator(precision, scale, onlyPositive);

		var result = validator.IsValidNumber(value);

		result.Should().BeFalse();
	}

	[TestCaseSource(nameof(ThrowArgumentExceptionTestCases))]
	public void Should_ThrowArgumentException(int precision, int scale, bool onlyPositive)
	{
		var action = () => new NumberValidator(precision, scale, onlyPositive);
		action.Should().Throw<ArgumentException>();
	}

	[TestCaseSource(nameof(NotThrowArgumentExceptionTestCases))]
	public void Should_NotThrowArgumentException(int precision, int scale, bool onlyPositive)
	{
		var action = () => new NumberValidator(precision, scale, onlyPositive);
		action.Should().NotThrow();
	}
}