namespace SimulatedEcology
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Forest
	{
		private readonly EventLog log = new EventLog();
		private readonly Random random = new Random();
		private int ageInMonths;
		private List<Plot> plots;
		private int size;

		public Forest()
		{
			GetSize();
			GetPlots();
			SpawnTrees();
			SpawnLumberjacks();
			SpawnBears();
		}

		public bool HasTrees { get { return plots.Where(plot => plot.Tree != null).ToList().Count > 0; } }

		public override string ToString()
		{
			var output = new StringBuilder();

			for (int i = 0; i < plots.Count(); i++ )
			{
				Plot plot = plots[i];
				if (plot.Tree != null && plot.Mammal == Mammal.None)
					switch (plot.Tree.Type)
					{
						case TreeType.Sapling:
							output.Append("S");
							break;
						case TreeType.Tree:
							output.Append("T");
							break;
						case TreeType.Elder:
							output.Append("E");
							break;
					}
				else if (plot.Tree == null && plot.Mammal == Mammal.None)
					output.Append(" ");
				else if (plot.Mammal == Mammal.Lumberjack)
					if (plot.Tree == null)
						output.Append("L");
					else
						output.Append("x");
				else
					if (plot.Tree == null)
						output.Append("B");
					else
						output.Append("X");

				if ((i + 1) % size == 0)
					output.Append(Environment.NewLine);
			}

			return output.ToString();
		}

		public void Mature()
		{
			Events.EvolveTrees(plots, random, log);
			Events.MoveLumberjacks(plots, random, log);
			Events.MoveBears(plots, random, log);
			ageInMonths++;

			if (ageInMonths % 12 == 0)
			{
				Events.ManageLumberjacks(plots, random, log);
				Events.ManageBears(plots, random, log);
			}

			log.WriteToFile(ageInMonths);
			log.Clear(ageInMonths);
		}

		#region Private
		private void GetPlots()
		{
			plots = Enumerable.Range(0, size)
				.SelectMany(y => Enumerable.Range(0, size)
					.Select(x => new Plot(x, y)))
				.ToList();
		}
		private void GetSize()
		{
			Console.WriteLine("Forest size:");
			string line = Console.ReadLine();

			int forestSize = 0;
			if (!Int32.TryParse(line, out forestSize) || forestSize < 1)
			{
				Console.WriteLine("You failed to enter a positive number. PEACE!");
				Console.ReadKey();
				Environment.Exit(0);
			}

			size = forestSize;
		}
		private void SpawnBears()
		{
			int bearCount = size * size / 50;
			Events.ProduceBears(plots, random, log, bearCount, true);
		}
		private void SpawnLumberjacks()
		{
			int lumberjackCount = size * size / 10;
			Events.HireLumberjacks(plots, random, log, lumberjackCount, true);
		}
		private void SpawnTrees()
		{
			int treeCount = size * size / 2;
			List<Plot> availablePlots = plots.Where(plot => plot.Mammal != Mammal.Lumberjack).ToList();

			for (int i = 0; i < treeCount; i++)
			{
				int randomIndex = random.Next(availablePlots.Count);
				availablePlots[randomIndex].Tree = new Tree(true);
				availablePlots.RemoveAt(randomIndex);
			}
		}
		#endregion
	}
}