using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 4 Kyu: https://www.codewars.com/kata/5671d975d81d6c1c87000022/csharp
/// 2 Kyu: https://www.codewars.com/kata/5679d5a3f2272011d700000d/train/csharp
/// </summary>
public class Skyscrapers
{
	private static Skyscraper[,] skyscrapers;
	private static int[] clues;

	public static int[][] SolvePuzzle( int[] _clues )
	{
		skyscrapers = new Skyscraper[6, 6];
		clues = _clues;
		for (int x = 0; x < 6; x++)
		{
			for (int y = 0; y < 6; y++)
			{
				skyscrapers[x, y] = new Skyscraper( x, y );
			}
		}

		AnalyzeClues();

		int[][] output = new int[6][];
		for (int y = 0; y < 6; y++)
		{
			output[y] = new int[6];
			for (int x = 0; x < 6; x++)
			{
				//Console.Write( $"{skyscrapers[x, y].Height}({String.Join( ',', skyscrapers[x, y].PossibleValues )}),{"".PadLeft( 12 - (skyscrapers[x, y].PossibleValues.Count * 2) )} " );
				output[y][x] = skyscrapers[x, y].Height;
			}

			//Console.WriteLine();
		}

		//Console.WriteLine();

		return output;
	}

	private static void AnalyzeClues()
	{
		for (int i = 0; i < clues.Length; i++)
		{
			int clue = clues[i];
			if (clue == 0)
			{
				continue;
			}

			int distanceFromExterior = 0;
			if (i < 6)
			{
				int x = i % 6;
				for (int y = 0; y < 6; y++)
				{
					ApplyClue( x, y );
				}
			}
			else if (i < 12)
			{
				int y = i % 6;
				for (int x = 6 - 1; x > 0; x--)
				{
					ApplyClue( x, y );
				}
			}
			else if (i < 18)
			{
				int x = 5 - (i % 6);
				for (int y = 6 - 1; y > 0; y--)
				{
					ApplyClue( x, y );
				}
			}
			else
			{
				int y = 5 - (i % 6);
				for (int x = 0; x < 6; x++)
				{
					ApplyClue( x, y );
				}
			}

			void ApplyClue( int _x, int _y )
			{
				if (skyscrapers[_x, _y].MakeLessThanOrEqualTo( (7 - clue) + distanceFromExterior )
				    || skyscrapers[_x, _y].MakeGreaterThanOrEqualTo( distanceFromExterior == 0 && clue == 1 ? 6 : 1 ))
				{
					RemoveValueFromConflicting( _x, _y );
				}

				distanceFromExterior++;
			}
		}
	}

	private static void RemoveValueFromConflicting( int _x, int _y )
	{
		int value = skyscrapers[_x, _y].Height;
		for (int x = 0; x < 6; x++)
		{
			if (x == _x)
			{
				continue;
			}

			if (skyscrapers[x, _y].RemoveValues( value ))
			{
				RemoveValueFromConflicting( x, _y );
			}
		}

		for (int y = 0; y < 6; y++)
		{
			if (y == _y)
			{
				continue;
			}

			if (skyscrapers[_x, y].RemoveValues( value ))
			{
				RemoveValueFromConflicting( _x, y );
			}
		}

		CheckRowsAndColumns( skyscrapers[_x, _y] );
	}

	private static void CheckRowsAndColumns( Skyscraper _skyscraper )
	{
		MakeGroupHeightsDistinct( from Skyscraper skyscraper in skyscrapers
		                          where skyscraper.Y == _skyscraper.Y
		                          select skyscraper );

		MakeGroupHeightsDistinct( from Skyscraper skyscraper in skyscrapers
		                          where skyscraper.X == _skyscraper.X
		                          select skyscraper );

		ValidateClues( _skyscraper.X, _skyscraper.Y );
	}

	private static void MakeGroupHeightsDistinct( IEnumerable<Skyscraper> _skyscrapers )
	{
		(Skyscraper, bool)[] uniqueHeights = new (Skyscraper, bool)[6];
		foreach (Skyscraper skyscraper in _skyscrapers)
		{
			foreach (int value in skyscraper.PossibleValues)
			{
				if (uniqueHeights[value - 1].Item1 == null)
				{
					uniqueHeights[value - 1].Item1 = skyscraper;
					uniqueHeights[value - 1].Item2 = true;
				}
				else
				{
					uniqueHeights[value - 1].Item2 = false;
				}
			}
		}

		for (int i = 0; i < uniqueHeights.Length; i++)
		{
			if (uniqueHeights[i].Item1 != null && uniqueHeights[i].Item2)
			{
				if (uniqueHeights[i].Item1.LimitToValues( i + 1 ))
				{
					RemoveValueFromConflicting( uniqueHeights[i].Item1.X, uniqueHeights[i].Item1.Y );
				}
			}
		}
	}

	private static void ValidateClues( int _x, int _y )
	{
		int xTopClue = clues[_x];
		int xBottomClue = clues[17 - _x];
		int yRightClue = clues[6 + _y];
		int yLeftClue = clues[23 - _y];

		if (xTopClue != 0)
		{
			ValidateClue( (from Skyscraper skyscraper in skyscrapers
			               orderby skyscraper.Y
			               where skyscraper.X == _x
			               select skyscraper).ToArray(), xTopClue );
		}

		if (xBottomClue != 0)
		{
			ValidateClue( (from Skyscraper skyscraper in skyscrapers
			               orderby skyscraper.Y descending
			               where skyscraper.X == _x
			               select skyscraper).ToArray(), xBottomClue );
		}

		if (yRightClue != 0)
		{
			ValidateClue( (from Skyscraper skyscraper in skyscrapers
			               orderby skyscraper.X descending
			               where skyscraper.Y == _y
			               select skyscraper).ToArray(), yRightClue );
		}

		if (yLeftClue != 0)
		{
			ValidateClue( (from Skyscraper skyscraper in skyscrapers
			               orderby skyscraper.X
			               where skyscraper.Y == _y
			               select skyscraper).ToArray(), yLeftClue );
		}
	}

	private static void ValidateClue( Skyscraper[] _orderedSkyscrapers, int _clue )
	{
		List<int[]> validConfigurations = new List<int[]>();

		GenerateConfigurations( new int[6], 0 );

		for (int i = 0; i < 6; i++)
		{
			if (_orderedSkyscrapers[i].LimitToValues( validConfigurations.Select( _validConfiguration => _validConfiguration[i] ).Distinct().ToArray() ))
			{
				RemoveValueFromConflicting( _orderedSkyscrapers[i].X, _orderedSkyscrapers[i].Y );
			}
		}

		void GenerateConfigurations( int[] _heights, int _index )
		{
			foreach (int value in _orderedSkyscrapers[_index].PossibleValues)
			{
				if (_heights.Contains( value ))
				{
					continue;
				}

				int[] heights = new int[6];
				Array.Copy( _heights, heights, _heights.Length );
				heights[_index] = value;
				if (_index < 5)
				{
					GenerateConfigurations( heights, _index + 1 );
				}
				else
				{
					int tallestSkyscraper = heights[0];
					int visibleCount = 1;
					for (int i = 1; i < 6; i++)
					{
						if (heights[i] > tallestSkyscraper)
						{
							tallestSkyscraper = heights[i];
							++visibleCount;
							if (tallestSkyscraper == 6)
							{
								break;
							}
						}
					}

					if (visibleCount == _clue)
					{
						validConfigurations.Add( heights );
					}
				}
			}
		}
	}

	public class Skyscraper
	{
		public readonly List<int> PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6 };

		public int Height { get; set; }
		public int X { get; }
		public int Y { get; }

		public Skyscraper( int _x, int _y )
		{
			X = _x;
			Y = _y;
		}

		public bool MakeLessThanOrEqualTo( int _value )
		{
			bool valuesRemoved = PossibleValues.RemoveAll( _x => _x > _value ) > 0;
			if (valuesRemoved)
			{
				if (PossibleValues.Count == 1)
				{
					Height = PossibleValues.First();
					return true;
				}

				CheckRowsAndColumns( this );
			}

			return false;
		}

		public bool MakeGreaterThanOrEqualTo( int _value )
		{
			bool valuesRemoved = PossibleValues.RemoveAll( _x => _x < _value ) > 0;
			if (valuesRemoved)
			{
				if (PossibleValues.Count == 1)
				{
					Height = PossibleValues.First();
					return true;
				}

				CheckRowsAndColumns( this );
			}

			return false;
		}

		public bool RemoveValues( params int[] _values )
		{
			bool valuesRemoved = false;
			foreach (int value in _values)
			{
				valuesRemoved = valuesRemoved || PossibleValues.Remove( value );
			}

			if (valuesRemoved)
			{
				if (PossibleValues.Count == 1)
				{
					Height = PossibleValues.First();
					return true;
				}

				CheckRowsAndColumns( this );
			}

			return false;
		}

		public bool LimitToValues( params int[] _values )
		{
			if (PossibleValues.Count > 1)
			{
				if (PossibleValues.RemoveAll( _x => !_values.Contains( _x ) ) > 0)
				{
					if (PossibleValues.Count == 1)
					{
						Height = PossibleValues.First();
						return true;
					}

					CheckRowsAndColumns( this );
				}
			}

			return false;
		}
	}
}
