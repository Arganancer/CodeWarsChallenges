using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/52c4dd683bfd3b434c000292/csharp
/// </summary>
public static class CarMileageNumbers
{
	public static int IsInteresting( int number, List<int> awesomePhrases )
	{
		for (int i = 0; i < 3; i++)
		{
			if (number + i < 100)
			{
				continue;
			}

			string numberAsString = (number + i).ToString();

			if (IsSequential( numberAsString )
			    || IsPalindrome( numberAsString )
			    || AllSameNumber( numberAsString )
			    || AllZeros( numberAsString )
			    || awesomePhrases.Contains( number + i ))
			{
				return i == 0 ? 2 : 1;
			}
		}

		return 0;
	}

	private static bool AllSameNumber( string number )
	{
		return number.All( x => number[0] == x );
	}

	private static bool AllZeros( string number )
	{
		for (int i = 1; i < number.Length; i++)
		{
			if (number[i] != '0')
			{
				return false;
			}
		}

		return true;
	}

	private static bool IsSequential( string number )
	{
		return "1234567890".Contains( number ) || "9876543210".Contains( number );
	}

	private static bool IsPalindrome( string number )
	{
		for (int i = 0; i < number.Length / 2; i++)
		{
			if (number[i] != number[^(i + 1)])
			{
				return false;
			}
		}

		return true;
	}
}
