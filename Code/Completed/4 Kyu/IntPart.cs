using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/55cf3b567fc0e02b0b00000b
/// </summary>
public class IntPart
{
	public static string Part( long n )
	{
		long range = 0;

		SortedSet<long> values = new SortedSet<long> { 1 };

		FindAllPartitions( new List<long>(), 0, n );

		double average = values.Average( x => x );
		double median = values.Count % 2 == 0 ? ( values.ElementAt( values.Count / 2 ) + values.ElementAt( values.Count / 2 - 1 ) ) * 0.5d : values.ElementAt( values.Count / 2 );

		return $"Range: {range} Average: {average:F2} Median: {median:F2}";

		void FindAllPartitions( List<long> _summands, long _currentSum, long _maxSummandValue )
		{
			for ( long i = 1; i <= _maxSummandValue; ++i )
			{
				List<long> newSummands = new List<long>( _summands ) { i };
				long nextSum = _currentSum + i;
				if ( nextSum == n )
				{
					long product = newSummands.Aggregate( 1L, ( _l, _l1 ) => _l * _l1 );
					if ( product - 1 > range )
					{
						range = product - 1;
					}

					if ( !values.Contains( product ) )
					{
						values.Add( product );
					}

					return;
				}

				FindAllPartitions( newSummands, nextSum, i );
			}
		}
	}
}