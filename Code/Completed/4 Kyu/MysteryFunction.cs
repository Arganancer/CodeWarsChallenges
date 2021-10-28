using System;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/56b2abae51646a143400001d/train/csharp
/// </summary>
public class MysteryFunction
{
	public static long Mystery( long n )
	{
		return n ^ (n >> 1);
	}

	public static long MysteryInv( long n )
	{
		long mask = n >> 1;
		while (mask != 0)
		{
			n ^= mask;
			mask >>= 1;
		}

		return n;
	}

	public static string NameOfMystery()
	{
		return "Gray Code";
	}
}
