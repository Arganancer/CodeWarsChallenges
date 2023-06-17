using System;

namespace Codewars
{
	/// <summary>
	/// 
	/// </summary>
	internal class Program
	{
		static void Main()
		{
			Console.Write("Type Y to enter \"ToRoman\" Mode. Else, from roman mode: ");
			bool toRoman = Console.ReadLine() == "Y";
			if (toRoman)
			{
				while (true)
				{
					string userInput = Console.ReadLine();
					if (!int.TryParse( userInput, out int value ))
					{
						return;
					}

					Console.WriteLine();
					Console.Write( $"{value} = " );
					Console.WriteLine( RomanNumerals.ToRoman( value ) );
				}
			}
			else
			{
				while (true)
				{
					string userInput = Console.ReadLine();

					Console.WriteLine();
					Console.Write( $"{userInput} = " );
					Console.WriteLine( RomanNumerals.FromRoman( userInput ) );
				}
			}
		}
	}
}
