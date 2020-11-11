using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/5254ca2719453dcc0b00027d/solutions/csharp
/// </summary>
class Permutations
{
	public static List<string> SinglePermutations(string _input)
	{
		List<string> permutations = new List<string>();
		List<char> characters = _input.ToCharArray().ToList();
		GetPermutations( characters, "" );

		void GetPermutations( List<char> _characters, string _currentPermutation )
		{
			for ( int i = 0; i < _characters.Count; i++ )
			{
				char c = _characters[i];

				if (_characters.Count == 1)
				{
					permutations.Add(_currentPermutation + c);
					return;
				}
				List<char> remainingChars = new List<char>( _characters );
				remainingChars.Remove( c );
				GetPermutations( remainingChars, _currentPermutation + c);
			}
		}

		return permutations.Distinct().ToList();
	}
}