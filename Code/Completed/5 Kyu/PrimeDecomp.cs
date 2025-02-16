using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Completed._5_Kyu
{
	/// <summary>
	/// https://www.codewars.com/kata/54d512e62a5e54c96200019e/train/csharp
	/// </summary>
	class PrimeDecomp
    {
		public static String factors( int lst )
		{
			int currentPrime = 2;
			Dictionary<int, int> factors = new Dictionary<int, int>();
			while(lst > 1)
			{
				if(IsPrime(lst))
				{
					currentPrime = lst;
				}
				int divisionsCount = 0;
				while(lst % currentPrime == 0)
				{
					lst /= currentPrime;
					divisionsCount++;
				}
				if(divisionsCount > 0)
				{
					factors.Add( currentPrime, divisionsCount );
				}
				currentPrime = GetNextPrime( currentPrime );
			}

			return string.Join("", factors.Select( x => (x.Value == 1 ? $"({x.Key})" : $"({x.Key}**{x.Value})")));
		}

		private static int GetNextPrime( int _currentPrime)
		{
			int nextPrime = _currentPrime + 1;
			while(!IsPrime( nextPrime ))
			{
				++nextPrime;
			}
			return nextPrime;
		}

		private static bool IsPrime(int number)
		{
			for (int i = 2; i < number; i++)
			{
				if (number % i == 0)
				{
					return false;
				}
				if(number / i < 1.0f)
				{
					return true;
				}
			}
			return true;
		}
	}
}
