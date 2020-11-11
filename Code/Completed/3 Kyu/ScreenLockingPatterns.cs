using System;
using System.Numerics;

namespace Codewars
{
	public static class ScreenLockingPatterns
	{
		public static int CountPatternsFrom(char firstDot, int length)
		{
			if (length <= 0 || length > 9)
			{
				return 0;
			}

			int count = 0;

			int dotLinearIndex = char.ToUpper(firstDot) - 65;
			TracePath(new bool[3, 3], new Vector2I(dotLinearIndex % 3, dotLinearIndex / 3), 1);

			return count;

			void TracePath(bool[,] swipedDots, Vector2I pos, int currentLength)
			{
				if (currentLength == length)
				{
					++count;
					return;
				}
				swipedDots[pos.X, pos.Y] = true;
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						if (swipedDots[x, y])
						{
							continue;
						}

						Vector2I destination = new Vector2I(x, y);
						if (CanSwipeToDot(pos, destination, swipedDots))
						{
							TracePath((bool[,])swipedDots.Clone(), destination, currentLength + 1);
						}
					}
				}
			}
		}

		public static bool CanSwipeToDot(Vector2I origin, Vector2I destination, bool[,] swipedDots)
		{
			int xDifference = destination.X - origin.X;
			int yDifference = destination.Y - origin.Y;

			int xAbsoluteDifference = Math.Abs(xDifference);
			int yAbsoluteDifference = Math.Abs(yDifference);

			if (xAbsoluteDifference <= 1)
			{
				if (yAbsoluteDifference <= 1 || xAbsoluteDifference == 1)
				{
					return true;
				}
			}
			else if (yAbsoluteDifference == 1)
			{
				return true;
			}

			Vector2I intermediatePos = new Vector2I(origin.X + xDifference / 2, origin.Y + yDifference / 2);
			return swipedDots[intermediatePos.X, intermediatePos.Y];
		}
	}

	public readonly struct Vector2I
	{
		public int X { get; }
		public int Y { get; }

		public Vector2I(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Vector2I operator +(Vector2I a, Vector2I b)
		{
			return new Vector2I(a.X + b.X, a.Y + b.Y);
		}
	}

	public static class ScreenLockingPatternsTests
	{
		public static void RunTests()
		{
			Logger.Log(0, ScreenLockingPatterns.CountPatternsFrom('A', 0), "'A', 0");
			Logger.Log(0, ScreenLockingPatterns.CountPatternsFrom('A', 10), "'A', 10");
			Logger.Log(1, ScreenLockingPatterns.CountPatternsFrom('B', 1), "'B', 1");
			Logger.Log(5, ScreenLockingPatterns.CountPatternsFrom('C', 2), "'C', 2");
			Logger.Log(37, ScreenLockingPatterns.CountPatternsFrom('D', 3), "'D', 3");
			Logger.Log(256, ScreenLockingPatterns.CountPatternsFrom('E', 4), "'E', 4");
			Logger.Log(23280, ScreenLockingPatterns.CountPatternsFrom('E', 8), "'E', 8");
		}
	}
}
