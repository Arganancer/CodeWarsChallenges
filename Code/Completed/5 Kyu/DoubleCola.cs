using System;
using System.Linq;

public class Line
{
	public static string WhoIsNext( string[] names, long n )
	{
		if (n < names.Length)
		{
			return names[n - 1];
		}

		long drinkCounts = 0;
		long peopleCount = names.Length;
		while (drinkCounts + peopleCount < n)
		{
			drinkCounts += peopleCount;
			peopleCount *= 2;
		}

		long index = (long)(((n - drinkCounts) / (float)(peopleCount)) * names.Length);

		Console.WriteLine( $"drinkCounts: {drinkCounts}\nPeopleCount: {peopleCount}\nN: {n}\nNamesLength: {names.Length}\n" );
		Console.WriteLine( $"(({n} - {drinkCounts}) / ({peopleCount})) * {names.Length} = {index}" );
		Console.WriteLine( $"({n - drinkCounts} / {peopleCount}) * {names.Length} = {index}" );
		Console.WriteLine( $"({(n - drinkCounts) / (float)peopleCount}) * {names.Length} = {index}" );

		int personIndex = 0;
		Console.WriteLine( string.Join( "\n", names.Select( x => $"{personIndex++}: {x}" ) ) );
		return names[index];
	}
}
