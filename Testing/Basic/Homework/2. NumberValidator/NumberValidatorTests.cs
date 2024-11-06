
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;

using ValidateTestCaseArgs = (int Precision, int Scale, bool OnlyPositive, string Value);

using ExceptionTestCaseArgs = (int Precision, int Scale);

[TestFixture]
public partial class NumberValidatorTests
{
	[TestCaseSource(nameof(ValidTestCases))]
	public void IsValidNumber_Shoud_BeTrue_WhenArgsCorrect(ValidateTestCaseArgs args)
	{
		var validator = new NumberValidator(args.Precision, args.Scale, args.OnlyPositive);

		var result = validator.IsValidNumber(args.Value);

		result.Should().BeTrue();
	}

	[TestCaseSource(nameof(NotValidTestCases))]
	public void IsValidNumber_Shoud_BeFalse_WhenArgsBad(ValidateTestCaseArgs args)
	{
		var validator = new NumberValidator(args.Precision, args.Scale, args.OnlyPositive);

		var result = validator.IsValidNumber(args.Value);

		result.Should().BeFalse();
	}

	[TestCaseSource(nameof(ThrowArgumentExceptionTestCases))]
	public void Should_Throw_ArgumentException(ExceptionTestCaseArgs args)
	{
		var action = () => new NumberValidator(args.Precision, args.Scale);
		action.Should().Throw<ArgumentException>();
	}

	[TestCaseSource(nameof(NotThrowArgumentExceptionTestCases))]
	public void Should_NotThrow_AnyException(ExceptionTestCaseArgs args)
	{
		var action = () => new NumberValidator(args.Precision, args.Scale);
		action.Should().NotThrow();
	}
}