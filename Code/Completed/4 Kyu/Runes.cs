using System.Linq;

public class Runes
{
	public static int solveExpression( string expression )
	{
		int missingDigit = -1;

		int opIndex = expression.IndexOfAny( new[] { '*', '-', '+' }, 1 );
		int equalityIndex = expression.IndexOf( '=' );
		char op = expression[opIndex];
		string[] equation =
		{
			expression.Substring( 0, opIndex ),
			expression.Substring( opIndex + 1, equalityIndex - opIndex - 1 ),
			expression.Substring( equalityIndex + 1 )
		};

		int startIndex = equation.Any( m => m.First( c => c != '-' ) == '?' && m.Replace( "-", "" ).Length > 1 ) ? 1 : 0;

		for ( int i = startIndex; i < 10; i++ )
		{
			if ( expression.Contains( i.ToString() ) ) continue;
			if ( ApplyOperator( Convert( equation[0], i ), Convert( equation[1], i ), op ) == Convert( equation[2], i ) ) return i;
		}

		return missingDigit;
	}

	private static long Convert( string _s, int _i )
	{
		return long.Parse( _s.Replace( "?", _i.ToString() ) );
	}

	private static long ApplyOperator( long _n1, long _n2, char _op )
	{
		return _op switch
		{
			'*' => _n1 * _n2,
			'-' => _n1 - _n2,
			'+' => _n1 + _n2,
		};
	}
}