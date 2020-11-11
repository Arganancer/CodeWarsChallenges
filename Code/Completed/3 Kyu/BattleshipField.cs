using System.Collections.Generic;
using System.Linq;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/52bb6539a4cf1b12d90005b7/train/csharp
	/// </summary>
	public class BattleshipField
	{
		public static bool ValidateBattlefield(int[,] field)
		{
			bool[,] validatedPositions = new bool[field.GetLength(0), field.GetLength(1)];

			Dictionary<int, int> ships = new Dictionary<int, int> {{4, 1}, {3, 2}, {2, 3}, {1, 4}};

			for (int x = 0; x < field.GetLength(0); x++)
			{
				for (int y = 0; y < field.GetLength(1); y++)
				{
					int shipSize = 0;
					if (validatedPositions[x, y] || field[x, y] != 1) continue;

					// Validate quantity of ships with size.
					if (!ShipAtPositionIsValid(x, y, ref shipSize) || shipSize > 4 || --ships[shipSize] < 0)
					{
						return false;
					}
				}
			}

			return ships.Values.All(shipCount => shipCount == 0);

			bool ShipAtPositionIsValid(int x, int y, ref int shipSize)
			{
				validatedPositions[x, y] = true;
				++shipSize;

				for (int xModifier = -1; xModifier <= 1; xModifier++)
				{
					for (int yModifier = -1; yModifier <= 1; yModifier++)
					{
						int modifiedX = x + xModifier;
						int modifiedY = y + yModifier;
						if (modifiedX < 0 || modifiedX >= field.GetLength(0) ||
							modifiedY < 0 || modifiedY >= field.GetLength(1)) continue;

						if (field[modifiedX, modifiedY] == 1 &&
							(xModifier != 0 && yModifier != 0 || 
						    !validatedPositions[modifiedX, modifiedY] && 
						    !ShipAtPositionIsValid(modifiedX, modifiedY, ref shipSize)))
						{
							return false;
						}
					}
				}

				return true;
			}
		}
	}
}
