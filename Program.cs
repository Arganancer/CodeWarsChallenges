using System;
using Code.Completed._5_Kyu;

namespace Codewars
{
	/// <summary>
	/// 
	/// </summary>
	internal class Program
	{
		static void Main()
		{
			const int input = 379721;
			Console.Write( $"Input: {input}" );
			Console.WriteLine($"Result: {PrimeDecomp.factors( input )}");
		}
	}
}
