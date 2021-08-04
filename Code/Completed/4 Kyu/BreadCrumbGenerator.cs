using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/563fbac924106b8bf7000046/train/csharp
/// </summary>
public class BreadCrumbGenerator
{
	private static readonly string[] IgnoredWords = { "the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a" };

	public static string GenerateBC( string url, string separator )
	{
		if (url.Contains( "://" ))
		{
			url = url.Substring( url.IndexOf( "://" ) + 3 );
		}
		
		List<string> splitUrl = url.Split( '/' ).Select( StripDecorations ).Where( x => !string.IsNullOrWhiteSpace( x ) ).ToList();

		if (splitUrl.Last().StartsWith( "index" ))
		{
			splitUrl.RemoveAt( splitUrl.Count - 1 );
		}

		if (splitUrl.Count == 1)
		{
			return ConvertToSpan( "HOME" );
		}

		splitUrl[0] = ConvertToA( "HOME", "/" );

		List<string> path = new List<string>();
		for (int i = 1; i < splitUrl.Count - 1; i++)
		{
			path.Add( splitUrl[i] );
			splitUrl[i] = ConvertToA( splitUrl[i], $"/{string.Join( '/', path )}/" );
		}

		splitUrl[^1] = ConvertToSpan( splitUrl[^1] );

		return string.Join( separator, splitUrl );
	}

	private static string ConvertToSpan( string input )
	{
		return ConvertToElement( input, "span", "class=\"active\"" );
	}

	private static string ConvertToA( string input, string href )
	{
		return ConvertToElement( input, "a", $"href=\"{href}\"" );
	}

	private static string ConvertToElement( string input, string elementType, string property )
	{
		return $"<{elementType} {property}>{ShortenLongInput( input ).ToUpper()}</{elementType}>";
	}

	private static string ShortenLongInput( string input )
	{
		return input.Length > 30
			? new string( input.Split( '-' ).Where( x => !IgnoredWords.Contains( x ) ).Select( x => x[0] ).ToArray() )
			: input.Replace( '-', ' ' );
	}

	private static string StripDecorations( string input )
	{
		int index = input.IndexOfAny( new[] { '#', '.', '?' } );
		return input.Substring( 0, index >= 0 ? index : input.Length );
	}
}
