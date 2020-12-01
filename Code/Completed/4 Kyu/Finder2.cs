using System.Collections.Generic;
using System.Linq;

public class Finder2
{
	/// <summary>
	/// https://www.codewars.com/kata/57658bfa28ed87ecfa00058a/train/csharp
	/// </summary>
	public static int PathFinder( string _maze )
	{
		(char, int steps)[][] world = _maze.Split( "\n" ).Select( _row => _row.Select( _x => (_x, int.MaxValue) ).ToArray() ).ToArray();
		world[0][0].steps = 0;

		Queue<(int, int)> queue = new Queue<(int, int)>();
		queue.Enqueue( (0, 0) );

		while (queue.Any())
		{
			(int x, int y) = queue.Dequeue();
			if (x == world[0].Length - 1 && y == world.Length - 1) return world[x][y].steps;

			if (x > 0) UpdateNode( (x - 1, y), world[x][y].steps + 1 );
			if (y > 0) UpdateNode( (x, y - 1), world[x][y].steps + 1 );
			if (x < world[0].Length - 1) UpdateNode( (x + 1, y), world[x][y].steps + 1 );
			if (y < world.Length - 1) UpdateNode( (x, y + 1), world[x][y].steps + 1 );
		}

		return -1;

		void UpdateNode( (int x, int y) _pos, int _newSteps )
		{
			(int x, int y) = _pos;
			if (world[x][y].Item1 == '.' && world[x][y].steps > _newSteps)
			{
				queue.Enqueue( (x, y) );
				world[x][y].steps = _newSteps;
			}
		}
	}
}
