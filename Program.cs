using System;
using System.Diagnostics;

namespace Codewars
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.SetWindowSize(200, 60);

			foreach ( int val in Primes.Stream() )
			{
				if ( val > 10000 )
				{
					Console.WriteLine(val);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("++ All tests executed.");
			Console.Read();
		}
	}
}
