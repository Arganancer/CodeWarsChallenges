using System.Linq;

public class Finder
{
	/// <summary>
	/// https://www.codewars.com/kata/5765870e190b1472ec0022a2/train/csharp
	/// </summary>
	public static bool PathFinder( string _maze )
	{
		char[][] world = _maze.Split( "\n" ).Select( _row => _row.ToCharArray() ).ToArray();

		return FindExit( 0, 0 );

		bool FindExit( int _x, int _y )
		{
			world[_x][_y] = ' ';
			return (_x == world[0].Length - 1 && _y == world.Length - 1) ||
			       (_x > 0 && world[_x - 1][_y] == '.' && FindExit( _x - 1, _y )) ||
			       (_x < world[0].Length - 1 && world[_x + 1][_y] == '.' && FindExit( _x + 1, _y )) ||
			       (_y > 0 && world[_x][_y - 1] == '.' && FindExit( _x, _y - 1 )) ||
			       (_y < world.Length - 1 && world[_x][_y + 1] == '.' && FindExit( _x, _y + 1 ));
		}
	}
}
