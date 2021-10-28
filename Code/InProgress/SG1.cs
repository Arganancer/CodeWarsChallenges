//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class SG1
//{
//	private static (char, int steps)[][] world;
//	private static Queue<(int, int)> queue;

//	/// <summary>
//	/// https://www.codewars.com/kata/59669eba1b229e32a300001a/train/csharp
//	/// Pathfinding task.
//	/// </summary>
//	/// <param name="existingWires"></param>
//	/// <returns></returns>
//	public static string WireDHD( string existingWires )
//	{
//		world = existingWires.Split( "\n" ).Select( _row => _row.Select( _x => (_x, int.MaxValue) ).ToArray() ).ToArray();
//		world[0][0].steps = 0;

//		queue = new Queue<(int, int)>();
//		queue.Enqueue( (0, 0) );

//		while (queue.Any())
//		{
//			(int x, int y) = queue.Dequeue();
//			if (x == world[0].Length - 1 && y == world.Length - 1)
//			{
//				return world[x][y].steps;
//			}

//			if (x > 0)
//			{
//				UpdateNode( (x - 1, y), world[x][y].steps + 1 );
//			}

//			if (y > 0)
//			{
//				UpdateNode( (x, y - 1), world[x][y].steps + 1 );
//			}

//			if (x < world[0].Length - 1)
//			{
//				UpdateNode( (x + 1, y), world[x][y].steps + 1 );
//			}

//			if (y < world.Length - 1)
//			{
//				UpdateNode( (x, y + 1), world[x][y].steps + 1 );
//			}
//		}

//		return "Oh for crying out loud...";
//	}

//	private static void UpdateNode( (int x, int y) _pos, int _newSteps )
//	{
//		(int x, int y) = _pos;
//		if (world[x][y].Item1 == '.' && world[x][y].steps > _newSteps)
//		{
//			queue.Enqueue( (x, y) );
//			world[x][y].steps = _newSteps;
//		}
//	}

//	private struct Vector2
//	{
//		public int X { get; set; }
//		public int Y { get; set; }
//	}

//	private Stack<PathNode> FindPath( Vector2 _startNode, Vector2 _endNode )
//	{
//		_openSet.Add( new PathNode( World.NodeMap.GetClosestNode( _startNode ) ) );
//		Vector3i endNodePos = World.NodeMap.GetClosestNode( _endNode ).Position;

//		while (_openSet.Any())
//		{
//			PathNode currentNode = _openSet.Pop();

//			if (currentNode.Position == endNodePos)
//			{
//				return ReconstructPath( currentNode );
//			}

//			_closedSet.Add( currentNode );

//			Node currentOrigin = World.NodeMap.GetClosestNode( currentNode.Position );

//			foreach (Vector3i neighbor in currentOrigin.Neighbors)
//			{
//				if (_closedSet.Contains( neighbor ))
//				{
//					continue;
//				}

//				Single gValue = currentNode.G + neighbor.Distance( currentNode.Position );

//				if (_openSet.Update( neighbor, gValue ))
//				{
//					continue;
//				}

//				PathNode currentNeighbor = new PathNode( neighbor )
//				{
//					G = gValue,
//					H = GetH( neighbor, endNodePos )
//				};

//				currentNeighbor.F = gValue + currentNeighbor.H;
//				currentNeighbor.Parent = currentNode;

//				_openSet.Add( currentNeighbor );
//			}
//		}

//		return null;
//	}
//}
