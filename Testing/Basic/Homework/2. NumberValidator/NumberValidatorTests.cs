
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections;

namespace HomeExercise.Tasks.NumberValidator;



[TestFixture]
public partial class NumberValidatorTests
{

	[TestCase(17, 1, true, "0.0", TestName = "Number with point separator")]
	[TestCase(17, 1, true, "0,0", TestName = "Number with comma separator")]
	[TestCase(17, 2, true, "0", TestName = "Number without fractionl part")]
	[TestCase(4, 1, true, "1234", TestName = "Number length equals to precision")]
	[TestCase(17, 1, true, "+1,0", TestName = "Positive number with comma")]
	[TestCase(4, 1, true, "+123", TestName = "Positive number length with equals to precision")]
	[TestCase(4, 1, true, "+12.3", TestName = "Positive number length with fractionl part equals to precision")]
	[TestCase(5, 3, true, "+1.000", TestName = "Positive number length with fractional part equals to precision")]
	[TestCase(17, 1, false, "-1,0", TestName = "Negative number with comma")]
	[TestCase(4, 1, false, "-123", TestName = "Negative number length with plus equals to precision")]
	[TestCase(4, 1, false, "-12.3", TestName = "Negative number length with frctionl part equals to precision")]
	[TestCase(5, 3, false, "-1.000", TestName = "Negative number length with fractional part and plus equals to precision")]

	public void IsValidNumber_Shoud_BeTrue_WhenArgsCorrect(int precision, int scale, bool onlyPositive, string value)
	{
		var validator = new NumberValidator(precision, scale, onlyPositive);

		var result = validator.IsValidNumber(value);

		result.Should().BeTrue();
	}


	[TestCase(3, 2, true, "00.00", TestName = "Number length with fractional part more than precision")]
	[TestCase(3, 2, true, "-0.0", TestName = "Number length with fractional part and sign more than precision")]
	[TestCase(3, 2, true, "+0.00", TestName = "Number length with sign more than precision and fractional prt equls to scale")]
	[TestCase(3, 2, true, "a.sd", TestName = "Number consists letters")]
	[TestCase(17, 2, true, "0.000", TestName = "Length number's frationl part more thn scale")]
	[TestCase(3, 2, true, "", TestName = "Number is empty string")]
	[TestCase(3, 2, true, null, TestName = "Number is null")]
	[TestCase(4, 2, false, "10*00", TestName = "Number with invalid separator")]
	[TestCase(4, 1, false, "1.0.0", TestName = "Number with more than one separator")]
	public void IsValidNumber_Shoud_BeFalse_WhenArgsBad(int precision, int scale, bool onlyPositive, string value)
	{
		var validator = new NumberValidator(precision, scale, onlyPositive);

		var result = validator.IsValidNumber(value);

		result.Should().BeFalse();
	}


	[TestCase(-1, 2, TestName = "Negative precision")]
	[TestCase(1, -2, TestName = "Negative scale")]
	[TestCase(1, 2, TestName = "Precision less than scale")]
	[TestCase(1, 1, TestName = "Precision equals with scale")]
	public void Constructor_Should_Throw_ArgumentException(int precision, int scale)
	{
		var action = () => new NumberValidator(precision, scale);
		action.Should().Throw<ArgumentException>();
	}


	[TestCase(1, 0, TestName = "Zero scale")]
	[TestCase(2, 1, TestName = "Precision more than scale")]
	public void Constructor_Should_NotThrow_AnyException(int precision, int scale)
	{
		var action = () => new NumberValidator(precision, scale);
		action.Should().NotThrow();
	}
}