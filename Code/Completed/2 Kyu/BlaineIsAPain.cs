using System;
using System.Linq;

public class BlaineIsAPain
{
	private static char[][] world;
	private static (int, int) startPos;
	public static int TrainCrash(string track, string aTrain, int aTrainPos, string bTrain, int bTrainPos, int limit)
	{
		world = track.Split( Environment.NewLine ).Select( t => t.ToCharArray() ).ToArray();
		for ( int x = 0; x < world.GetLength( 0 ); x++ )
		{
			
		}
	    // Your code here!
	    return 0;
    }

}