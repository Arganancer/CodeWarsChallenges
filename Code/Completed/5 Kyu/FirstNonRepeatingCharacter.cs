using System.Linq;

public class FirstNonRepeatingCharacter
{
	public static string FirstNonRepeatingLetter( string s )
	{
		string result = s.FirstOrDefault( a => s.Count( b => char.ToLower( a ) == char.ToLower( b ) ) == 1 ).ToString();
		return result == "\0" ? "" : result;
	}
}
