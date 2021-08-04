using System;
using System.Linq;

namespace myjinxin
{
	/// <summary>
	/// https://www.codewars.com/kata/58ad0e0a154165a1c80000ea/csharp
	/// </summary>
	public class Kata
	{
		public int PacMan( int N, int[] PM, int[][] enemies )
		{
			int leftX = enemies.Select( _enemy => _enemy[0] ).Where( _x => _x < PM[0] ).Select( _x => _x + 1 ).DefaultIfEmpty( 0 ).Max();
			int rightX = enemies.Select( _enemy => _enemy[0] ).Where( _x => _x > PM[0] ).DefaultIfEmpty( N ).Min();
			int topY = enemies.Select( _enemy => _enemy[1] ).Where( _y => _y < PM[1] ).Select( _y => _y + 1 ).DefaultIfEmpty( 0 ).Max();
			int bottomY = enemies.Select( _enemy => _enemy[1] ).Where( _y => _y > PM[1] ).DefaultIfEmpty( N ).Min();

			int subtract = 1;

			if (enemies.Any( _x => _x[0] == PM[0] ))
			{
				if (enemies.Any( _x => _x[1] == PM[1] ))
				{
					return 0;
				}

				subtract = 0;
				bottomY -= 1;
			}
			else if (enemies.Any( _x => _x[1] == PM[1] ))
			{
				subtract = 0;
				rightX -= 1;
			}

			Console.WriteLine( $"N: {N}" +
			                   $"\nPM: {PM[0]},{PM[1]}" +
			                   $"\nEnemies:\n{string.Join( "\n  ", enemies.Select( x => $"{x[0]},{x[1]}" ) )}" +
			                   $"\nLeftX: {leftX}" +
			                   $"\nRightX: {rightX}" +
			                   $"\nTopY: {topY}" +
			                   $"\nBottomY {bottomY}" +
			                   $"\nSubtract {subtract}\n{"".PadRight( 15, '-' )}" );

			return Math.Max( 0, Math.Abs( (leftX - rightX) * (topY - bottomY) ) - subtract );
		}
	}
}
