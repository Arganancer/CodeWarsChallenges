using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/5ae64f28d2ee274164000118/train/csharp
/// </summary>
public class JomoPipi
{
	public static string StringFunc( string s, long x )
	{
		List<char[]> iterations = new List<char[]> { s.ToCharArray() };
		//Console.WriteLine( $"Input: {s} Iterations: {x}" );
		for (int i = 0; i < x; i++)
		{
			char[] current = Reversal( iterations[i] );

			if (iterations[0].SequenceEqual( current ))
			{
				//WriteList( iterations );
				return new string( iterations[(int)(x % (i + 1))] );
			}

			iterations.Add( current );
		}

		return new string( iterations[^1] );
	}

	private static char[] Reversal( char[] charArray )
	{
		char[] output = new char[charArray.Length];
		int offset = 0;
		for (int i = 0; i < charArray.Length; ++i, ++offset)
		{
			output[i] = charArray[^(offset + 1)];
			if (++i < charArray.Length)
			{
				output[i] = charArray[offset];
			}
		}

		return output;
	}

	private static void WriteList( List<char[]> list )
	{
		for (int i = 0; i < list.Count; i++)
		{
			Console.WriteLine( $"{i} - {new string( list[i] )}" );
		}
	}
}
