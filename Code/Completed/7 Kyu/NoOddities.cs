using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/51fd6bc82bc150b28e0000ce/solutions/csharp
/// </summary>
public class NoOddities
{
	public static int[] NoOdds( int[] values )
	{
		return values.Where( x => x % 2 == 0 ).ToArray();
	}
}
