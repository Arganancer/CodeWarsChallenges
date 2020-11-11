namespace Codewars.Code.Utilities
{
	public static class MathUtils
	{
		public static double Factorial( int n, int r )
		{
			double top = 1;
			double bottom = 1;
			for ( int i = 1; i <= n; i++ )
			{
				top *= i;
			}

			for ( int i = 1; i <= n - r; i++ )
			{
				bottom *= i;
			}

			return top / bottom;
		}
	}
}
