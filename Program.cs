using System;

namespace Codewars
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			Console.BufferHeight = 8000;
			Console.SetWindowSize( 100, 40 );

			Console.WriteLine( Finder.PathFinder( "......\n" +
			                                      "......\n" +
			                                      "......\n" +
			                                      "......\n" +
			                                      "......\n" +
			                                      "......" ) );

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine( "++ All tests executed." );
			Console.Read();
		}
	}
}
