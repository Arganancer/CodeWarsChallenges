using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// https://www.codewars.com/kata/51b66044bce5799a7f000003/train/csharp
/// 
/// Write two functions that convert a roman numeral to and from an integer value.
/// Multiple roman numeral values will be tested for each function.
/// Modern Roman numerals are written by expressing each digit separately starting
/// with the left most digit and skipping any digit with a value of zero.
/// In Roman numerals 1990 is rendered: 1000 = M, 900 = CM, 90 = XC; resulting in MCMXC.
/// 2008 is written as 2000=MM, 8=VIII; or MMVIII.
/// 1666 uses each Roman symbol in descending order: MDCLXVI.
/// 
/// ReSharper disable twice InvalidXmlDocComment
/// range : 1 <= n < 4000
/// 
/// In this kata 4 should be represented as IV, NOT as IIII (the "watchmaker's four").
/// </summary>
public class RomanNumerals
{
	private static readonly Dictionary<char, int> Numerals = new Dictionary<char, int>()
	{
		{ 'M', 1000 },
		{ 'D', 500 },
		{ 'C', 100 },
		{ 'L', 50 },
		{ 'X', 10 },
		{ 'V', 5 },
		{ 'I', 1 },
	};

	public static string ToRoman( int n )
	{
		StringBuilder result = new StringBuilder();
		for (int i = 0; i < Numerals.Count; i++)
		{
			(char currentNumeral, int currentValue) = Numerals.ElementAt( i );
			UpdateResult( currentValue, currentNumeral.ToString() );

			if (i < Numerals.Count - 1)
			{
				(char nextNumeral, int nextValue) = Numerals.ElementAt( i + 1 );
				if (currentValue - nextValue == nextValue)
				{
					(nextNumeral, nextValue) = Numerals.ElementAt( i + 2 );
				}
				
				UpdateResult( currentValue - nextValue, $"{nextNumeral}{currentNumeral}" );
			}
		}

		return result.ToString();

		void UpdateResult( int symbolValue, string symbol )
		{
			int numeralCount = n / symbolValue;
			if (numeralCount > 0)
			{
				result.Insert( result.Length, symbol, numeralCount );
			}

			n -= numeralCount * symbolValue;
		}
	}

	public static int FromRoman( string romanNumeral )
	{
		int value = 0;
		for (int i = 0; i < romanNumeral.Length; i++)
		{
			char currentNumeral = romanNumeral[i];
			int currentValue = Numerals[currentNumeral];
			int nextValue = i < romanNumeral.Length - 1 ? Numerals[romanNumeral[i + 1]] : 0;
			if (nextValue > currentValue)
			{
				i += 1;
				value += nextValue - currentValue;
			}
			else
			{
				value += currentValue;
			}
		}

		return value;
	}
}
