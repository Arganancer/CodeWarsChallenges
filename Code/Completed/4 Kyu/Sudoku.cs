using System.Collections.Generic;
using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/529bf0e9bdf7657179000008/train/csharp
	/// </summary>
	public class Sudoku
	{
		public static bool ValidateSolution(int[][] board)
		{
			List<int>[] verticalLinesContent = Enumerable.Range(0, board.Length).Select(i => new List<int>()).ToArray();
			List<int>[][] groups = Enumerable.Range(0, 3).Select(i => Enumerable.Range(0, 3).Select(j => new List<int>()).ToArray()).ToArray();

			for (int y = 0; y < board.Length; y++)
			{
				List<int> horizontalLineContent = new List<int>();
				for (int x = 0; x < board[0].Length; x++)
				{
					int valueAtIndex = board[y][x];
					if (valueAtIndex == 0 ||
					    horizontalLineContent.Contains(valueAtIndex) || 
					    verticalLinesContent[x].Contains(valueAtIndex) || 
					    groups[y / 3][x / 3].Contains(valueAtIndex))
					{
						return false;
					}

					horizontalLineContent.Add(valueAtIndex);
					verticalLinesContent[x].Add(valueAtIndex);
					groups[y / 3][x / 3].Add(valueAtIndex);
				}
			}

			return true;
		}
	}
}
