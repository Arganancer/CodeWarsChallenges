using System;

namespace Codewars
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			Console.SetWindowSize( 110, 20 );
			Console.BufferHeight = 8000;

			Console.WriteLine( JomoPipi.StringFunc( "]QfvDJUpxe", 775607689 ) );

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine( "\n\n++ All tests executed." );
			Console.Read();
		}
	}
}
