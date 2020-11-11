using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///     https://www.codewars.com/kata/58905bfa1decb981da00009e
/// </summary>
public class Dinglemouse
{
	private static Floor[] Floors;
	private static int PassengerCount;
	private static Lift lift;
	public static int[] TheLift(int[][] queues, int capacity)
	{
		PassengerCount = 0;
		Floors = new Floor[queues.GetLength(0)];
		for (int i = 0; i < queues.Length; i++)
		{
			Floors[i] = new Floor(i, queues[i]);
		}

		lift = new Lift(capacity);
		lift.ChangeFloor(Floors[0], Direction.Up);

		while (PassengerCount > 0)
		{
			Floor nextFloor = FindNextFloor();
			if (nextFloor == null)
			{
				lift.InvertDirection(true);
				nextFloor = FindNextFloor();
			}

			if (!nextFloor.HasPassengersGoingDirection(lift.CurrentDirection) &&
				 lift.CurrentPassengers.Count == 0 &&
				 FindNextFloor(nextFloor) == null)
			{
				lift.InvertDirection(true);
			}

			lift.ChangeFloor(nextFloor, lift.CurrentDirection);
		}

		if (lift.FloorHistory.Last() != 0)
		{
			lift.FloorHistory.Add(0);
		}

		return lift.FloorHistory.ToArray();
	}

	private static Floor FindNextFloor(Floor _floor = null)
	{
		int nextInvertedFloor = -1;
		int nextFloorIndex = lift.CurrentPassengers.Count > 0
			? lift.NextPassengerStop
			: lift.CurrentDirection == Direction.Up ? Floors.Length - 1 : 0;
		int startIndex = (_floor?.FloorIndex ?? lift.CurrentFloor.FloorIndex) + (lift.CurrentDirection == Direction.Up ? 1 : -1);
		for (int i = startIndex;
			lift.CurrentDirection == Direction.Up ? i <= nextFloorIndex : i >= nextFloorIndex;
			i += lift.CurrentDirection == Direction.Up ? 1 : -1)
		{
			if (Floors[i].HasQueuedPassengers)
			{
				if (Floors[i].HasPassengersGoingDirection(lift.CurrentDirection))
				{
					return Floors[i];
				}

				nextInvertedFloor = i;
			}
		}

		if (lift.CurrentPassengers.Count == 0 && nextInvertedFloor != -1)
		{
			lift.InvertDirection(false);
			return Floors[nextInvertedFloor];
		}

		if (Floors[nextFloorIndex].HasQueuedPassengers
			 || lift.CurrentPassengers.Count > 0
			 && nextFloorIndex == lift.NextPassengerStop)
		{
			if (lift.CurrentFloor.FloorIndex == nextFloorIndex)
			{
				lift.InvertDirection(true);
			}
			return Floors[nextFloorIndex];
		}

		return null;
	}

	internal class Lift
	{
		public List<int> FloorHistory;
		public List<Passenger> CurrentPassengers { get; private set; }
		public Direction CurrentDirection { get; private set; }
		public Floor CurrentFloor { get; private set; }
		public int Capacity { get; }
		public int RemainingSpace => Capacity - CurrentPassengers.Count;
		public int NextPassengerStop => CurrentPassengers[0].DesiredFloor;

		public Lift(int _capacity)
		{
			FloorHistory = new List<int>();
			CurrentPassengers = new List<Passenger>();
			Capacity = _capacity;
		}

		public void InvertDirection(bool _boardPassengers)
		{
			if (CurrentDirection == Direction.Up)
			{
				CurrentDirection = Direction.Down;
			}
			else
			{
				CurrentDirection = Direction.Up;
			}

			if (_boardPassengers)
			{
				ChangeFloor(CurrentFloor, CurrentDirection);
			}
		}

		public void AddPassengers(IEnumerable<Passenger> _passengers)
		{
			CurrentPassengers.AddRange(_passengers);
			if (CurrentDirection == Direction.Up)
			{
				CurrentPassengers = CurrentPassengers.OrderBy(p => p.DesiredFloor).ToList();
			}
			else
			{
				CurrentPassengers = CurrentPassengers.OrderByDescending(p => p.DesiredFloor).ToList();
			}
		}

		public void ChangeFloor(Floor _nextFloor, Direction _nextDirection)
		{
			CurrentFloor = _nextFloor;
			CurrentDirection = _nextDirection;
			if (FloorHistory.Count == 0 || FloorHistory.Last() != _nextFloor.FloorIndex)
			{
				FloorHistory.Add(CurrentFloor.FloorIndex);
			}
			while (CurrentPassengers.Count > 0 && CurrentPassengers[0].DesiredFloor == CurrentFloor.FloorIndex)
			{
				--PassengerCount;
				CurrentPassengers.RemoveAt(0);
			}

			if (CurrentFloor.HasQueuedPassengers)
			{
				AddPassengers(CurrentFloor.GetPassengers(CurrentDirection, RemainingSpace));
			}
		}
	}

	internal class Floor
	{
		public int FloorIndex { get; }
		public List<Passenger> PassengersGoingUp { get; }
		public List<Passenger> PassengersGoingDown { get; }

		public bool HasQueuedPassengers { get; private set; }

		public Floor(int _floorIndex, int[] _passengers)
		{
			FloorIndex = _floorIndex;
			PassengersGoingUp = new List<Passenger>();
			PassengersGoingDown = new List<Passenger>();
			HasQueuedPassengers = _passengers.Length > 0;
			foreach (int passengerDestination in _passengers)
			{
				if (passengerDestination != _floorIndex)
				{
					++PassengerCount;
					bool IsGoingUp = passengerDestination > _floorIndex;
					if (IsGoingUp)
					{
						PassengersGoingUp.Add(new Passenger(passengerDestination, Direction.Up));
					}
					else
					{
						PassengersGoingDown.Add(new Passenger(passengerDestination, Direction.Down));
					}
				}
			}
		}

		public bool HasPassengersGoingDirection(Direction _direction)
		{
			return (_direction == Direction.Up ? PassengersGoingUp : PassengersGoingDown).Count > 0;
		}

		public List<Passenger> GetPassengers(Direction _direction, int _max)
		{
			List<Passenger> OnboardingPassengers = new List<Passenger>();
			if (_max == 0)
			{
				return OnboardingPassengers;
			}
			List<Passenger> passengers = _direction == Direction.Up ? PassengersGoingUp : PassengersGoingDown;
			if (passengers.Count == 0)
			{
				return OnboardingPassengers;
			}

			for (int i = 0; i < Math.Min(_max, passengers.Count); i++)
			{
				OnboardingPassengers.Add(passengers[i]);
			}

			foreach (Passenger passenger in OnboardingPassengers)
			{
				passengers.Remove(passenger);
			}

			HasQueuedPassengers = PassengersGoingUp.Count > 0 || PassengersGoingDown.Count > 0;
			return OnboardingPassengers;
		}
	}

	internal class Passenger
	{
		public int DesiredFloor { get; }
		public Direction Direction { get; }

		public Passenger(int _destination, Direction _direction)
		{
			DesiredFloor = _destination;
			Direction = _direction;
		}
	}

	internal enum Direction
	{
		Up,
		Down,
	}
}