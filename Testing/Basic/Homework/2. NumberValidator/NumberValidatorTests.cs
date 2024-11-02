
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
	{
		[Test]
		[TestCaseSource(nameof(GetCaseValidators))]
		public bool CheckValidator(NumberValidator validator, string number)
		{
			return validator.IsValidNumber(number);
		}

		[Test]
		[TestCase(0, 2, true, TestName = "precision <= 0")]
		[TestCase(3, -1, false, TestName = "scale < 0")]
		[TestCase(3, 5, false, TestName = "scale > precision")]
		public void CheckArgumentException(int precision, int scale, bool onlyPositive)
		{
			Action action = () => new NumberValidator(precision, scale, onlyPositive);
			action.Should().Throw<ArgumentException>();
		}

		[Test]
		[TestCase(1, 0, true)]
		public void CheckDoesNotThrow(int precision, int scale, bool onlyPositive)
		{
			Action action = () => new NumberValidator(precision, scale, onlyPositive);
			action.Should().NotThrow();
		}

		private static IEnumerable<TestCaseData> GetCaseValidators()
		{
			yield return new TestCaseData(new NumberValidator(17, 2, true), "0").Returns(true);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "00.00").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "-0.00").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "+0.00").Returns(false);
			yield return new TestCaseData(new NumberValidator(4, 2, true), "+1.23").Returns(true);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "+1.23").Returns(false);
			yield return new TestCaseData(new NumberValidator(17, 2, true), "0.000").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "-1.23").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 2, true), "a.sd").Returns(false);
			// Новые тесты
			yield return new TestCaseData(new NumberValidator(4, 2), "-1.23").Returns(true);
			yield return new TestCaseData(new NumberValidator(1), "-").Returns(false);
			yield return new TestCaseData(new NumberValidator(2, 0, true), "").Returns(false);
			yield return new TestCaseData(new NumberValidator(4, 1), "+-13.0").Returns(false);
			yield return new TestCaseData(new NumberValidator(9, 5, true), "+133.00001").Returns(true);
			yield return new TestCaseData(new NumberValidator(3, 1, true), "+1,0").Returns(true);
			yield return new TestCaseData(new NumberValidator(3, 1, true), "+,1").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 1, true), ",1").Returns(false);
			yield return new TestCaseData(new NumberValidator(3, 1, true), ".").Returns(false);
		}
	}
