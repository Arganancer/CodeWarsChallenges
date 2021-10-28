using System.Collections.Generic;
using System.Linq;

public partial class Kata
{
	public static long NextSmaller( long n )
	{
		List<char> numbers = n.ToString().ToCharArray().ToList();
		char first;
		for (int i = numbers.Count - 1; i > 0; --i)
		{
			if (numbers.Skip( i ).Count( _x => _x < numbers[i] ) > 0)
			{
				first = numbers.Skip( i + 1 ).Where( _x => _x < numbers[i] ).Max();
				numbers.RemoveAt( numbers.LastIndexOf( first ) );
				return long.Parse( $"{string.Join( "", numbers.Take( i ) )}{first}{string.Join( "", numbers.Skip( i ).OrderByDescending( _x => _x ) )}" );
			}
		}

		first = numbers.Where( _x => _x < numbers[0] ).DefaultIfEmpty( '0' ).Max();
		if (first == '0')
		{
			return -1;
		}

		numbers.Remove( first );
		return long.Parse( $"{first}{string.Join( "", numbers.OrderByDescending( _x => _x ) )}" );
	}
}
