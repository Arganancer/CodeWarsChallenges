using System;
using System.Collections.Generic;
using System.Linq;

public partial class Kata
{
	public static long NextBiggerNumber(long n)
	{
		long[] digits = GetDigits(n);

		List<Digit> passedValues = new List<Digit>();

		int smallestDigitIndex = digits.Length - 1;

		for (int i = digits.Length - 1; i > 0; --i)
		{
			passedValues.Add(new Digit(i, digits[i]));
			if (passedValues.Any(digit => digit.value > digits[i - 1]))
			{
				passedValues = new List<Digit>(passedValues.OrderBy(digit => digit.value));
				Digit nextBiggest = passedValues.First(digit => digit.value > digits[i - 1]);
        
				digits[nextBiggest.index] = digits[i - 1];
				digits[i - 1] = nextBiggest.value;

				Array.Sort(digits, i, digits.Length - i);
				return Reassemble(digits);
			}
		}

		return -1;
	}

	public struct Digit : IComparable
	{
		public long value;
		public int index;

		public Digit(int index, long value)
		{
			this.index = index;
			this.value = value;
		}

		public int CompareTo(object obj)
		{
			Digit other = (Digit) obj;
			return other.value.CompareTo(value);
		}
	}

	public static long Reassemble(long[] digits)
	{
		long multiplier = 1;
		long result = 0;
		for (int i = digits.Length - 1; i >= 0; --i)
		{
			result += digits[i] * multiplier;
			multiplier *= 10;
		}

		return result;
	}

	public static long[] GetDigits(long n)
	{
		List<long> digits = new List<long>();

		while (n > 0)
		{
			long remainder = n % 10;
			digits.Add(remainder);
			n = (n - remainder) / 10;
		}

		digits.Reverse();

		return digits.ToArray();
	}
}
