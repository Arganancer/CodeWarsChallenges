using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/5270d0d18625160ada0000e4
	/// </summary>
	public class GreedIsGood
	{
		public static int Score(int[] dice)
		{
			int score = 0;
			for ( int i = 1; i <= 6; i++ )
			{
				int diceCount = dice.Count( d => d == i );
				if ( diceCount >= 3 )
				{
					score += i == 1 ? 1000 : i * 100;
				}

				score += (diceCount % 3) * (i == 1 ? 100 : i == 5 ? 50 : 0);
			}

			return score;
		}
	}
}
