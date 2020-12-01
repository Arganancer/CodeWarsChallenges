using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/51c8e37cee245da6b40000bd/train/csharp
/// </summary>
public class StripCommentsSolution
{
	public static string StripComments( string text, string[] commentSymbols )
	{
		return string.Join( "\n", text.Split( "\n" ).Select( t =>
		{
			int i = t.IndexOfAny( commentSymbols.SelectMany( s => s.ToCharArray() ).ToArray() );
			return (i >= 0 ? t.Substring( 0, i ) : t).TrimEnd();
		} ) );
	}
}
