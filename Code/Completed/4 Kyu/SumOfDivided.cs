using System.Collections.Generic;
using System.Linq;

public class SumOfDivided
{
	public static string sumOfDivided( int[] _list )
	{
		IEnumerable<(int, IEnumerable<int>)> primes = _list.Select( x => ( x, GetPrimeFactors( x ) ) ).ToArray();
		return string.Join( "", primes.SelectMany( x => x.Item2 ).Distinct().OrderBy( x => x )
			.Select( x => $"({x} {primes.Sum( y => y.Item2.Contains( x ) ? y.Item1 : 0 )})" ) );
	}

	private static IEnumerable<int> GetPrimeFactors( int _value )
	{
		_value *= _value < 0 ? -1 : 1;
		for ( int i = 2; _value > 1; i++ )
		{
			if ( _value % i == 0 )
			{
				while ( _value % i == 0 )
				{
					_value /= i;
				}

				yield return i;
			}
		}
	}
}