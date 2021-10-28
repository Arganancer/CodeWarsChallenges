using System;
using System.Collections.Generic;

namespace Codewars
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			Console.SetWindowSize( 110, 20 );
			Console.BufferHeight = 8000;

			//Console.WriteLine( new BefungeInterpreter().Interpret( ">987v>.v\nv456<  :\n>321 ^ _@" ) );
			//Console.WriteLine( new BefungeInterpreter().Interpret( "08>:1-:v v *_$.@ \n  ^    _$>\\:^  ^    _$>\\:^" ) );
			Console.WriteLine( new BefungeInterpreter().Interpret( "2>:3g\" \"-!v\\  g30          <" +
			                                                       "\n |!`\"&\":+1_:.:03p>03g+:\"&\"`|" +
			                                                       "\n @               ^  p3\\\" \":<" +
			                                                       "\n2 2345678901234567890123456789012345678" ) );

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine( "\n\n++ All tests executed." );
			Console.Read();
		}
	}
}
