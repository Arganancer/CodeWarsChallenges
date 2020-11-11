using System;
using System.Linq;

public class BlaineIsAPain
{
	private static Vector2 m_StartPos;
	private static char[,] m_World;
	private static int m_WorldWidth;
	private static int m_WorldHeight;
	private static readonly char[] m_TrackPieces = { '-', '|', '/', '\\', '+', 'X' };

	public static int TrainCrash( string _track, string _aTrain, int _aTrainPos, string _bTrain, int _bTrainPos, int _limit )
	{
		string[] lines = _track.Split( Environment.NewLine ).Select( _t => _t.TrimEnd() ).ToArray();
		m_WorldHeight = lines.Length;
		m_WorldWidth = lines.Max( _line => _line.Length );
		m_World = new char[m_WorldWidth, m_WorldHeight];
		m_StartPos = new Vector2( -1, -1 );

		for ( int y = 0; y < m_WorldHeight; y++ )
		{
			char[] currentLine = lines[y].ToCharArray();
			for ( int x = 0; x < m_WorldWidth; x++ )
			{
				if ( x >= currentLine.Length )
				{
					m_World[x, y] = ' ';
				}
				else
				{
					if ( m_StartPos.X == -1 && m_TrackPieces.Contains( currentLine[x] ) )
					{
						m_StartPos = new Vector2( x, y );
					}

					m_World[x, y] = currentLine[x];
				}
			}
		}

		// Your code here!
		return 0;
	}

	internal class Vector2
	{
		public Vector2( int _x, int _y )
		{
			X = _x;
			Y = _y;
		}

		public int X { get; set; }
		public int Y { get; set; }
	}
}