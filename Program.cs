using System;
using System.Collections.Generic;

namespace Codewars
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			Console.BufferHeight = 8000;
			Console.SetWindowSize( 100, 40 );

			Type myType = typeof( object );
			Dictionary<string, Type> properties = new Dictionary<string, Type> { { "SomeInt", typeof( int ) }, { "SomeString", typeof( string ) }, { "SomeObject", typeof( object ) } };
			CreateClassAtRuntime.DefineClass( "SomeClass", properties, ref myType );

			dynamic myInstance = Activator.CreateInstance( myType );
			myInstance.SomeObject = myInstance;
			myInstance.SomeString = "Hey there";
			myInstance.SomeInt = 3;
			Console.WriteLine( $"{myInstance.SomeObject}: {myInstance.SomeString}, {myInstance.SomeInt}" );

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine( "++ All tests executed." );
			Console.Read();
		}
	}
}
