using System;

namespace Codewars.SnailSolution
{
	public static class SnailSolution
	{
		/// <summary>
		/// https://www.codewars.com/kata/521c2db8ddc89b9b7a0000c1/train/csharp
		/// </summary>
		public static int[] Snail(int[][] array)
		{
			int[] output = new int[array.Length * array[0].Length];

			int currentSteps = array.GetLength(0);
			int nextStepCountSwitch = 1;

			Direction currentDirection = Direction.Right;
			Vector2I pos = new Vector2I(0, 0);

			int remainingSteps = currentSteps;
			for (int i = 0; i < output.Length; i++)
			{
				// Assign Value
				output[i] = array[pos.Y][pos.X];

				// Update direction
				if (--remainingSteps == 0)
				{
					if (--nextStepCountSwitch == 0)
					{
						nextStepCountSwitch = 2;
						--currentSteps;
					}

					remainingSteps = currentSteps;

					currentDirection = currentDirection.GetNextDirection();
				}

				// Update Position
				pos += currentDirection.ToVector2I();
			}

			return output;
		}
	}

	public struct Vector2I
	{
		public Vector2I(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X;
		public int Y;

		public static Vector2I operator +(Vector2I a, Vector2I b)
		{
			return new Vector2I(a.X + b.X, a.Y + b.Y);
		}
	}

	public enum Direction
	{
		Right,
		Down,
		Left,
		Up
	}

	public static class DirectionExtensions
	{
		public static Direction GetNextDirection(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					return Direction.Down;
				case Direction.Down:
					return Direction.Left;
				case Direction.Left:
					return Direction.Up;
				case Direction.Up:
					return Direction.Right;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		public static Vector2I ToVector2I(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					return new Vector2I(1, 0);
				case Direction.Down:
					return new Vector2I(0, 1);
				case Direction.Left:
					return new Vector2I(-1, 0);
				case Direction.Up:
					return new Vector2I(0, -1);
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}
	}
}
