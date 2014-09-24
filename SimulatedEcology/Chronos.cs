namespace SimulatedEcology
{
	using System;

    public static class Chronos
    {
		public static void Main()
		{
			var forest = new Forest();
			Console.Write(forest.ToString());
			Console.WriteLine();

			int monthsProcessed = 0;
			while(monthsProcessed < 4800 && forest.HasTrees)
			{
				forest.Mature();
				monthsProcessed++;

				Console.Write(forest.ToString());
				Console.WriteLine();

				if (monthsProcessed % 12 == 0)
					Console.ReadKey();
			}

			Console.ReadKey();
		}
	}
}