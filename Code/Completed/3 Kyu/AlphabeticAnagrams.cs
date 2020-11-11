using System.Collections.Generic;
using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/53e57dada0cb0400ba000688/train/csharp
	/// </summary>
	public class AlphabeticAnagrams
	{
		public static long ListPosition(string value)
		{
			double position = 1;

			while (value.Length > 1)
			{
				string alphabeticalArrangement = string.Concat(value.OrderBy(c => c));
				char currentLetter = value.First();
				long permutations = value.CalculatePossiblePermutations();
				int index = alphabeticalArrangement.IndexOf(currentLetter);
				position += permutations * (index / (double)value.Length);

				value = value.Substring(1);
			}

			return (long) position;
		}
	}

	public static class LongExtensions
	{
		public static long Factorial(this long value)
		{
			long result = 1;
			for (long i = result; i <= value; i++)
			{
				result *= i;
			}

			return result;
		}
	}

	public static class StringExtensions
	{
		public static long CalculatePossiblePermutations(this string value)
		{
			Dictionary<char, long> repeatingLetters = new Dictionary<char, long>();
			foreach (char letter in value.Distinct())
			{
				repeatingLetters[letter] = value.Count(x => x == letter);
			}

			long permutations = ((long)value.Length).Factorial();
			foreach (KeyValuePair<char, long> repeatingLetter in repeatingLetters.Where(x => x.Value > 1))
			{
				permutations /= repeatingLetter.Value.Factorial();
			}

			return permutations;
		}
	}
}
