using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// https://www.codewars.com/kata/5519a584a73e70fa570005f5/train/csharp
/// </summary>
public class Primes
{
	/// <summary>
	/// Using Sieve of Eratosthenes:
	/// https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
	/// </summary>
	public static IEnumerable<int> Stream()
	{
		int end = 20000000;
		BitArray composite = new BitArray( end );
		int sqrt = (int)Math.Sqrt(end);
		for (int p = 2; p < sqrt; ++p)
		{
			if (composite[p]) continue;

			yield return p;

			for (int i = p * p; i < end; i += p)
			{
				composite[i] = true;
			}
		}

		for (int p = sqrt + 1; p < end; ++p)
		{
			if (!composite[p]) yield return p;
		}
	}
}