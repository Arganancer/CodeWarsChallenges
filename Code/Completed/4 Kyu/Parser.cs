using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/525c7c5ab6aecef16e0001a5/csharp
/// </summary>
public class Parser
{
	public enum Number
	{
		zero = 0,
		one = 1,
		two = 2,
		three = 3,
		four = 4,
		five = 5,
		six = 6,
		seven = 7,
		eight = 8,
		nine = 9,

		// Could create a rule for sixteen - nineteen with the "teen" suffixe,
		// but honestly don't think it's worth the headache and performance cost.
		ten = 10,
		eleven = 11,
		twelve = 12,
		thirteen = 13,
		fourteen = 14,
		fifteen = 15,
		sixteen = 16,
		seventeen = 17,
		eighteen = 18,
		nineteen = 19,

		// Could also create a rule for all 2 digit numbers that are equivalent to
		// their unit counterparts but with a "ty" suffixe, but since this is not consistent, also not worth it.
		twenty = 20,
		thirty = 30,
		forty = 40,
		fifty = 50,
		sixty = 60,
		seventy = 70,
		eighty = 80,
		ninety = 90,

		hundred = 100,
		thousand = 1000,
		million = 1000000
	}

	public static int ParseInt( string s )
	{
		string[] sections = s.Split( '-', ' ' ).Where( _x => _x != "and" ).ToArray();
		int splitIndex = Array.IndexOf( sections, Enum.GetName( typeof( Number ), Number.thousand ) );
		return splitIndex > -1
			? ParseSection( sections.Take( splitIndex + 1 ) ) + ParseSection( sections.Skip( splitIndex + 1 ) )
			: ParseSection( sections );
	}

	private static int ParseSection( IEnumerable<string> _section )
	{
		int output = 0;
		foreach (string numberText in _section)
		{
			Number number = Enum.Parse<Number>( numberText );
			if (new[] { Number.hundred, Number.thousand, Number.million }.Contains( number ))
			{
				output *= (int)number;
			}
			else
			{
				output += (int)number;
			}
		}

		return output;
	}
}
