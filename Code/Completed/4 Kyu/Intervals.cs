using System;
using System.Collections.Generic;
using System.Linq;

public class Intervals
{
	public static int SumIntervals( (int, int)[] intervals )
	{
		List<(int a, int b)> collapsedIntervals = intervals.ToList();
		for (int i = 1; i < collapsedIntervals.Count; i++)
		{
			(int a, int b) currentInterval = collapsedIntervals[i];
			for (int j = i - 1; j >= 0; j--)
			{
				(int a, int b) previousInterval = collapsedIntervals[j];
				if (previousInterval.InRange( currentInterval.a ) || previousInterval.InRange( currentInterval.b ) ||
				    currentInterval.InRange( previousInterval.a ) || currentInterval.InRange( previousInterval.b ))
				{
					collapsedIntervals[i] = (currentInterval.a <= previousInterval.a ? currentInterval.a : previousInterval.a,
						currentInterval.b >= previousInterval.b ? currentInterval.b : previousInterval.b);
					collapsedIntervals.RemoveAt( j );
					i = 0;
					break;
				}
			}
		}

		Console.WriteLine( string.Join( ", ", intervals.Select( x => $"({x.Item1}, {x.Item2})" ) ) );
		Console.WriteLine( string.Join( ", ", collapsedIntervals.Select( x => $"({x.Item1}, {x.Item2})" ) ) );
		return collapsedIntervals.Sum( interval => interval.b - interval.a );
	}
}

public static class TupleExtensions
{
	public static bool InRange( this (int a, int b) _this, int _value )
	{
		return _value >= _this.a && _value <= _this.b;
	}
}
