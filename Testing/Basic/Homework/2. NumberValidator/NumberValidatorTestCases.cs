using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeExercise.Tasks.NumberValidator
{
	// для каждого теста может быть много тест кейсов, поэтому я вынес их в отдельные коллекции
	public partial class NumberValidatorTests
	{
		public static IEnumerable ValidTestCases
		{
			get
			{
				yield return new TestCaseData(17, 1, true, "0.0");

				yield return new TestCaseData(17, 2, true, "0");

				yield return new TestCaseData(4, 2, true, "+1.23");

				yield return new TestCaseData(7, 3, false, "+1.00");
			}
		}

		public static IEnumerable NotValidTestCases
		{
			get
			{
				yield return new TestCaseData(3, 2, true, "00.00");

				yield return new TestCaseData(3, 2, true, "-0.0");

				yield return new TestCaseData(3, 2, true, "+0.00");

				yield return new TestCaseData(3, 2, true, "+1.23");

				yield return new TestCaseData(3, 2, true, "-1.23");

				yield return new TestCaseData(3, 2, true, "a.sd");

				yield return new TestCaseData(17, 2, true, "0.000");

				yield return new TestCaseData(3, 2, true, "");

				yield return new TestCaseData(3, 2, true, null);

				yield return new TestCaseData(4, 2, false, "10*00");

				yield return new TestCaseData(4, 1, false, "1.0.0");
			}
		}

		public static IEnumerable ThrowArgumentExceptionTestCases
		{
			get
			{
				yield return new TestCaseData(-1, 2, true);

				yield return new TestCaseData(1, -2, false);

				yield return new TestCaseData(1, 2, true);

				yield return new TestCaseData(1, 1, false);
			}
		}

		public static IEnumerable NotThrowArgumentExceptionTestCases
		{
			get
			{
				yield return new TestCaseData(1, 0, true);

				yield return new TestCaseData(2, 1, true);
			}
		}
	}
}
