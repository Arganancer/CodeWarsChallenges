using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/55c04b4cc56a697bb0000048/csharp
	/// </summary>
	public class Scramblies
	{
		public static bool Scramble(string str1, string str2)
		{
			return str2.Distinct().All( c => str1.Count( c1 => c1 == c ) >= str2.Count( c1 => c1 == c ) );
		}
	}
}
