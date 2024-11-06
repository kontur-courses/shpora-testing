using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeExercise.Tasks.NumberValidator
{
	using ValidateTestCaseArgs = (int Precision, int Scale, bool OnlyPositive, string Value);

	using ExceptionTestCaseArgs = (int Precision, int Scale);

	// для каждого теста может быть много тест кейсов, поэтому я вынес их в отдельные коллекции
	public partial class NumberValidatorTests
	{

		public static IEnumerable<ValidateTestCaseArgs> ValidTestCases
		{
			get
			{
				yield return (17, 1, true, "0.0");

				yield return (17, 1, true, "0,0");

				yield return (17, 1, true, "+1,0");

				yield return (17, 2, true, "0");

				yield return (4, 1, true, "1234");

				yield return (4, 1, true, "+123");

				yield return (4, 1, true, "+12.3");

				yield return (7, 3, false, "+1.00");
			}
		}

		public static IEnumerable<ValidateTestCaseArgs> NotValidTestCases
		{
			get
			{
				yield return (3, 2, true, "00.00");

				yield return (3, 2, true, "-0.0");

				yield return (3, 2, true, "+0.00");

				yield return (3, 2, true, "a.sd");

				yield return (17, 2, true, "0.000");

				yield return (3, 2, true, "");

				yield return (3, 2, true, null);

				yield return (4, 2, false, "10*00");

				yield return (4, 1, false, "1.0.0");
			}
		}

		public static IEnumerable<ExceptionTestCaseArgs> ThrowArgumentExceptionTestCases
		{
			get
			{
				yield return (-1, 2);

				yield return (1, -2);

				yield return (1, 2);

				yield return (1, 1);
			}
		}

		public static IEnumerable<ExceptionTestCaseArgs> NotThrowArgumentExceptionTestCases
		{
			get
			{
				yield return (1, 0);

				yield return (2, 1);
			}
		}
	}
}
