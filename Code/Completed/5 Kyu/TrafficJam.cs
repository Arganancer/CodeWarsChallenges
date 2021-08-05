using System.Collections.Generic;
using System.Linq;

public class TrafficJamKata
{
	public static string TrafficJam( string mainRoad, string[] sideStreets )
	{
		List<char> output = mainRoad.ToList();
		for (int i = sideStreets.Length - 1; i >= 0; --i)
		{
			for (int j = 0; j < sideStreets[i].Length; ++j)
			{
				int insertIndex = i + (j * 2) + 1;
				if (insertIndex > output.Count)
				{
					output.Add( sideStreets[i][^(j + 1)] );
				}
				else
				{
					output.Insert( insertIndex, sideStreets[i][^(j + 1)] );
				}
			}
		}

		return string.Join( "", output.Take( output.IndexOf( 'X' ) + 1 ) );
	}
}
