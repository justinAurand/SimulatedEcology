namespace SimulatedEcology
{
	using System;

	public class Coordinate : IEquatable<Coordinate>
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(Coordinate coordinate)
		{
			return coordinate.X == X && coordinate.Y == Y;
		}
	}
}