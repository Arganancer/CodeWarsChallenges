using System.Collections.Generic;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/5672682212c8ecf83e000050/train/csharp
	/// </summary>
	public class DoubleLinear
	{
		public static int DblLinear(int n)
		{
			List<int> u = new List<int> {1};
			int x = 0;
			int y = 0;

			for ( int i = 0; i < n; i++ )
			{
				int nextY = 2 * u[x] + 1;
				int nextZ = 3 * u[y] + 1;
				if ( nextY <= nextZ )
				{
					u.Add( nextY );
					++x;
					if ( nextY == nextZ)
					{
						++y;
					}
				}
				else
				{
					u.Add(nextZ);
					++y;
				}
			}

			return u[n];
		}
	}
}
