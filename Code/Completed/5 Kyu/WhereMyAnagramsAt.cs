using System.Collections.Generic;
using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/523a86aa4230ebb5420001e1/csharp
	/// </summary>
	public class WhereMyAnagramsAt
	{
		public static List<string> Anagrams(string _word, List<string> _words)
		{
			string alphabetizedWord = string.Concat( _word.OrderBy( c => c ) );
			return _words.Where( word => string.Concat( word.OrderBy( c => c ) ) == alphabetizedWord ).ToList();
		}
	}
}
