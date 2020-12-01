using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class BlaineIsAPain
{
	private static Cell m_Start;
	private static Cell[,] m_World;
	private static int m_WorldWidth;
	private static int m_WorldHeight;

	public static int TrainCrash( string _track, string _aTrain, int _aTrainPos, string _bTrain, int _bTrainPos, int _limit )
	{
		//throw new Exception( $"BlaineIsAPain.TrainCrash(\"{_track.Replace( Environment.NewLine, "*" ).Replace( "\\", "\\\\" ).Replace( "*", "\\r\\n" )}\", \"{_aTrain}\", {_aTrainPos}, \"{_bTrain}\", {_bTrainPos}, {_limit});" );
		Console.CursorVisible = false;
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();
		Console.Write( _track );
		string[] lines = _track.Split( Environment.NewLine ).Select( _t => _t.TrimEnd() ).ToArray();
		m_WorldHeight = lines.Length;
		m_WorldWidth = lines.Max( _line => _line.Length );
		m_World = new Cell[m_WorldWidth, m_WorldHeight];
		m_Start = null;

		// Init world:
		for ( int y = 0; y < m_WorldHeight; y++ )
		{
			char[] currentLine = lines[y].ToCharArray();
			for ( int x = 0; x < m_WorldWidth; x++ )
			{
				Vector2 currentPos = new Vector2( x, y );
				if ( x >= currentLine.Length )
				{
					m_World[x, y] = new Cell( ' ', currentPos );
				}
				else
				{
					Cell cell = new Cell( currentLine[x], currentPos );
					m_World[x, y] = cell;
					if ( m_Start == null && cell.IsTrack )
					{
						m_Start = cell;
					}
				}
			}
		}

		// Init track:
		List<Train> trains = new List<Train>();
		Cell previousCell = null;
		Cell currentCell = m_Start;
		int currentTrackPos = 0;
		do
		{
			if ( currentTrackPos == _aTrainPos )
			{
				trains.Add( new Train( _aTrain, currentCell, previousCell ) );
			}

			if ( currentTrackPos == _bTrainPos )
			{
				trains.Add( new Train( _bTrain, currentCell, previousCell ) );
			}

			Cell nextCell = currentCell.Type switch
			{
				'-' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, 0 ), new Vector2( 1, 0 ) }, new[] { '-', '\\', '/', '+', 'S' } ) ),
				'|' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 0, -1 ), new Vector2( 0, 1 ) }, new[] { '|', '\\', '/', '+', 'S' } ) ),
				'/' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, 0 ), new Vector2( 1, 0 ) }, new[] { '-', '+' } ),
					( new[] { new Vector2( 0, -1 ), new Vector2( 0, 1 ) }, new[] { '|', '+' } ),
					( new[] { new Vector2( 1, -1 ), new Vector2( -1, 1 ) }, new[] { '/', 'X', 'S' } ) ),
				'\\' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 0, -1 ), new Vector2( 0, 1 ) }, new[] { '|', '+' } ),
					( new[] { new Vector2( -1, -1 ), new Vector2( 1, 1 ) }, new[] { '\\', 'X', 'S' } ),
					( new[] { new Vector2( -1, 0 ), new Vector2( 1, 0 ) }, new[] { '-', '+' } ) ),
				'+' => new Vector2( previousCell.Position.X - currentCell.Position.X, previousCell.Position.Y - currentCell.Position.Y ) switch
				{
					{ } offset when offset.Y != 0 => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 0, -1 ), new Vector2( 0, 1 ) }, new[] { '|', '\\', '/', '+' } ) ),
					{ } offset when offset.X != 0 => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, 0 ), new Vector2( 1, 0 ) }, new[] { '-', '\\', '/', '+' } ) ),
				},
				'X' => new Vector2( previousCell.Position.X - currentCell.Position.X, previousCell.Position.Y - currentCell.Position.Y ) switch
				{
					{ } offset when Math.Abs( offset.Y + offset.X ) == 0 => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 1, -1 ), new Vector2( -1, 1 ) }, new[] { '/', 'X' } ) ),
					{ } offset when Math.Abs( offset.Y + offset.X ) == 2 => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, -1 ), new Vector2( 1, 1 ) }, new[] { '\\', 'X' } ) )
				},
				'S' => previousCell.Type switch
				{
					'-' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, 0 ), new Vector2( 1, 0 ) }, new[] { '-' } ) ),
					'|' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 0, -1 ), new Vector2( 0, 1 ) }, new[] { '|' } ) ),
					'\\' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( -1, -1 ), new Vector2( 1, 1 ) }, new[] { '\\' } ) ),
					'/' => FindNextCell( previousCell, currentCell, ( new[] { new Vector2( 1, -1 ), new Vector2( -1, 1 ) }, new[] { '/' } ) )
				}
			};

			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.SetCursorPosition( currentCell.Position.X, currentCell.Position.Y );
			Console.Write( currentCell.Type );

			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.SetCursorPosition( nextCell.Position.X, nextCell.Position.Y );
			Console.Write( nextCell.Type );

			Thread.Sleep( 5 );

			if ( previousCell != null )
			{
				currentCell.AddNext( nextCell, previousCell );
				currentCell.AddPrevious( previousCell, nextCell );
			}

			previousCell = currentCell;
			currentCell = nextCell;
			currentTrackPos++;
		} while ( currentCell.GetNext( previousCell ) == null );

		foreach ( Train train in trains )
		{
			train.FinalizeTrain();
		}

		// Run Simulation:
		int ticksSinceStart = 0;
		while ( ticksSinceStart <= _limit )
		{
			if ( trains[0].Intersects( trains[1] ) )
			{
				return ticksSinceStart;
			}

			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition( 0, 0 );
			Console.Write( _track );

			++ticksSinceStart;
			foreach ( Train train in trains )
			{
				train.Update();

				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.SetCursorPosition( train.EnginePos.Position.X, train.EnginePos.Position.Y );
				Console.Write( char.ToUpper( train.m_Train[0] ) );

				foreach ( Cell cell in train.CarriagesPos )
				{
					Console.SetCursorPosition( cell.Position.X, cell.Position.Y );
					Console.Write( char.ToLower( train.m_Train[0] ) );
				}
			}

			Thread.Sleep( 15 );
		}

		return -1;
	}

	private static Cell FindNextCell( Cell _previousCell, Cell _currentCell, params (IEnumerable<Vector2> offsets, char[] types)[] _possibilities )
	{
		foreach ( ( IEnumerable<Vector2> offsets, char[] types ) in _possibilities )
		{
			foreach ( Vector2 offset in offsets )
			{
				Vector2 pos = new Vector2( offset.X + _currentCell.Position.X, offset.Y + _currentCell.Position.Y );
				if ( pos.X < 0 || pos.Y < 0 || pos.X >= m_WorldWidth || pos.Y >= m_WorldHeight )
				{
					continue;
				}

				Cell nextCell = m_World[pos.X, pos.Y];
				if ( _previousCell != nextCell && nextCell.IsTrack && types.Contains( nextCell.Type ) )
				{
					return nextCell;
				}
			}
		}

		return null;
	}

	internal class Train
	{
		private readonly bool m_IsExpress;
		public readonly string m_Train;
		private Cell m_LastCell;
		private int m_WaitTicksRemaining;
		public List<Cell> CarriagesPos { get; }
		public Cell EnginePos { get; private set; }


		public Train( string _train, Cell _enginePos, Cell _previousCell )
		{
			CarriagesPos = new List<Cell>();
			m_Train = _train;
			m_LastCell = _previousCell;
			EnginePos = _enginePos;
			m_IsExpress = char.ToLower( m_Train[0] ) == 'x';
		}

		public void FinalizeTrain()
		{
			Cell lastCell;
			if ( char.IsUpper( m_Train[0] ) )
			{
				lastCell = m_LastCell;
				m_LastCell = EnginePos.GetNext( m_LastCell );
			}
			else
			{
				lastCell = EnginePos.GetNext( m_LastCell );
			}

			Cell currentCell = EnginePos;
			for ( int i = 0; i < m_Train.Length - 1; i++ )
			{
				Cell nextCell = char.IsUpper( m_Train[0] ) ? currentCell.GetNext( lastCell ) : currentCell.GetPrevious( lastCell );

				CarriagesPos.Add( nextCell );
				lastCell = currentCell;
				currentCell = nextCell;
			}
		}

		public void Update()
		{
			if ( m_WaitTicksRemaining > 0 )
			{
				--m_WaitTicksRemaining;
			}
			else
			{
				for ( int i = CarriagesPos.Count - 1; i >= 0; --i )
				{
					if ( i == 0 )
					{
						CarriagesPos[i] = EnginePos;
					}
					else
					{
						CarriagesPos[i] = CarriagesPos[i - 1];
					}
				}

				Cell nextCell = char.IsUpper( m_Train[0] ) ? EnginePos.GetPrevious( m_LastCell ) : EnginePos.GetNext( m_LastCell );
				m_LastCell = EnginePos;
				EnginePos = nextCell;

				if ( !m_IsExpress && EnginePos.IsStation )
				{
					m_WaitTicksRemaining = CarriagesPos.Count;
				}
			}
		}

		public bool Intersects( Train _other )
		{
			Cell[] trainParts = CarriagesPos.Union( new[] { EnginePos } ).ToArray();
			Cell[] otherTrainParts = _other.CarriagesPos.Union( new[] { _other.EnginePos } ).ToArray();
			return otherTrainParts.Any( _otherCell => trainParts.Any( _thisCell => _thisCell.Equals( _otherCell ) ) ) ||
			       trainParts.Length <= CarriagesPos.Count ||
			       otherTrainParts.Length <= _other.CarriagesPos.Count;
		}
	}

	internal class Cell
	{
		private static readonly char[] TrackPieces = { '-', '|', '/', '\\', '+', 'X' };

		public char Type { get; }
		public Vector2 Position { get; }
		public bool IsTrack { get; }
		public bool IsStation { get; }

		private Dictionary<Cell, Cell> NextFromOrigin { get; }
		private Dictionary<Cell, Cell> PreviousFromOrigin { get; }

		public Cell( char _c, Vector2 _position )
		{
			NextFromOrigin = new Dictionary<Cell, Cell>();
			PreviousFromOrigin = new Dictionary<Cell, Cell>();

			Type = _c;
			Position = _position;
			IsTrack = TrackPieces.Contains( _c );
			IsStation = _c == 'S';

			if ( IsStation )
			{
				IsTrack = IsStation;
			}
		}

		public void AddNext( Cell _next, Cell _origin = null )
		{
			NextFromOrigin.Add( _origin, _next );
		}

		public void AddPrevious( Cell _previous, Cell _origin )
		{
			PreviousFromOrigin.Add( _origin, _previous );
		}

		public Cell GetNext( Cell _origin )
		{
			return _origin == null ? NextFromOrigin.Values.First() : NextFromOrigin.TryGetValue( _origin, out Cell next ) ? next : null;
		}

		public Cell GetPrevious( Cell _origin )
		{
			return _origin == null ? PreviousFromOrigin.Values.First() : PreviousFromOrigin.TryGetValue( _origin, out Cell previous ) ? previous : null;
		}
	}

	internal class Vector2
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2( int _x, int _y )
		{
			X = _x;
			Y = _y;
		}
	}
}