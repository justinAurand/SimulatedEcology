namespace SimulatedEcology
{
	using System;

	public class Plot
	{
		public Coordinate Coordinate { get; private set; }
		public Mammal Mammal { get; set; }
		public Tree Tree { get; set; }

		public Plot(int x, int y)
		{
			Coordinate = new Coordinate(x, y);
		}
	}
}
