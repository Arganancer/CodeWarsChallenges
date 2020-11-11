using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// https://www.codewars.com/kata/5629db57620258aa9d000014/train/csharp
/// </summary>
public class Mixing
{
	public static string Mix(string s1, string s2)
	{
		List<string> result = new List<string>();

		foreach ( char current in (s1 + s2).Where( char.IsLower ).Distinct() )
		{
			int s1Count = s1.Count( c => c == current );
			int s2Count = s2.Count( c => c == current );
			int maxCount = Math.Max(s1Count, s2Count);
			if ( maxCount > 1 )
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append( s1Count >= s2Count ? s1Count == s2Count ? "=:" : "1:" : "2:" );
				stringBuilder.Append( "".PadLeft( maxCount, current ) );
				result.Add(stringBuilder.ToString() );
			}
		}

		return string.Join( '/', result.OrderByDescending( x => x.Length ).ThenBy( x => x, StringComparer.Ordinal ) );
	}
}