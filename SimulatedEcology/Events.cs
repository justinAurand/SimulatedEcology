namespace SimulatedEcology
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Events
	{
		public static void EvolveTrees(List<Plot> plots, Random random, EventLog log)
		{
			List<Plot> treePlots = plots.Where(plot => plot.Tree != null).ToList();
			foreach (Plot treePlot in treePlots)
			{
				switch (treePlot.Tree.Type)
				{
					case TreeType.Sapling:
						continue;
					case TreeType.Tree:
						if (random.Next(10) != 0)
							continue;
						break;
					case TreeType.Elder:
						if (random.Next(5) != 0)
							continue;
						break;
				}

				List<Plot> availablePlots = plots.Where(plot => plot.Tree == null && plot.Mammal == Mammal.None && Adjacent(plot, treePlot)).ToList();
				if (!availablePlots.Any())
					continue;
				int randomIndex = random.Next(availablePlots.Count);
				availablePlots[randomIndex].Tree = new Tree(false);
				log.SaplingSpawned();
			}

			foreach (Plot plot in treePlots)
			{
				TreeType oldTreeType = plot.Tree.Type;
				plot.Tree.Mature();
				TreeType newTreeType = plot.Tree.Type;

				if (oldTreeType == newTreeType)
					continue;
				if (newTreeType == TreeType.Tree)
					log.SaplingMatured();
				if (newTreeType == TreeType.Elder)
					log.TreeMatured();
			}
		}
		public static void HireLumberjacks(List<Plot> plots, Random random, EventLog log, int hires, bool forestInitialization)
		{
			List<Plot> availablePlots = plots.Where(plot => plot.Mammal == Mammal.None && plot.Tree == null).ToList();
			if (!availablePlots.Any())
				return;

			for (int i = 0; i < hires; i++)
			{
				int randomIndex = random.Next(availablePlots.Count);
				availablePlots[randomIndex].Mammal = Mammal.Lumberjack;
				availablePlots.RemoveAt(randomIndex);

				if (!forestInitialization)
					log.LumberjackHired();
				if (availablePlots.Count == 0)
					return;
			}
		}
		public static void ManageBears(List<Plot> plots, Random random, EventLog log)
		{
			if (log.MawlingsThisYear > 0)
			{
				List<Plot> bearPlots = plots.Where(plot => plot.Mammal == Mammal.Bear).ToList();
				int randomIndex = random.Next(bearPlots.Count);
				bearPlots[randomIndex].Mammal = Mammal.None;
				log.BearTrapped();
				return;
			}

			ProduceBears(plots, random, log, 1, false);
		}
		public static void ManageLumberjacks(List<Plot> plots, Random random, EventLog log)
		{
			List<Plot> lumberjackPlots = plots.Where(plot => plot.Mammal == Mammal.Lumberjack).ToList();
			int lumberjackCount = lumberjackPlots.Count;
			if (log.LumberHarvestedThisYear < lumberjackCount)
			{
				int randomIndex = random.Next(lumberjackCount);
				lumberjackPlots[randomIndex].Mammal = Mammal.None;
				log.LumberjackFired();
				return;
			}

			int hires = lumberjackCount == 0 ? 1 : log.LumberHarvestedThisYear / lumberjackCount;
			HireLumberjacks(plots, random, log, hires, false);
		}
		public static void MoveBears(List<Plot> plots, Random random, EventLog log)
		{
			List<Plot> bearPlots = plots.Where(plot => plot.Mammal == Mammal.Bear).ToList();
			foreach (Plot bearPlot in bearPlots)
			{
				Plot currentPlot = bearPlot;

				int moves = 5;
				for (int i = 0; i < moves; i++)
				{
					List<Plot> availablePlots = plots.Where(plot => Adjacent(plot, currentPlot)).ToList();
					int randomIndex = random.Next(availablePlots.Count);
					if (availablePlots[randomIndex].Mammal == Mammal.Bear)
					{
						availablePlots.RemoveAt(randomIndex);
						randomIndex = random.Next(availablePlots.Count);
					}
					if (availablePlots[randomIndex].Mammal == Mammal.Bear)
						break;

					Plot nextPlot = availablePlots[randomIndex];
					if (nextPlot.Mammal == Mammal.Lumberjack)
					{
						MoveMammal(Mammal.Bear, ref currentPlot, ref nextPlot);
						HandleMawling(plots, random, log);
						break;
					}

					MoveMammal(Mammal.Bear, ref currentPlot, ref nextPlot);
				}
			}
		}
		public static void MoveLumberjacks(List<Plot> plots, Random random, EventLog log)
		{
			List<Plot> lumberjackPlots = plots.Where(plot => plot.Mammal == Mammal.Lumberjack).ToList();
			foreach (Plot lumberjackPlot in lumberjackPlots)
			{
				Plot currentPlot = lumberjackPlot;

				int moves = 3;
				for (int i = 0; i < moves; i++)
				{
					List<Plot> availablePlots = plots.Where(plot => Adjacent(plot, currentPlot)).ToList();
					int randomIndex = random.Next(availablePlots.Count);
					if (availablePlots[randomIndex].Mammal == Mammal.Lumberjack)
					{
						availablePlots.RemoveAt(randomIndex);
						randomIndex = random.Next(availablePlots.Count);
					}
					if (availablePlots[randomIndex].Mammal == Mammal.Lumberjack)
						break;

					Plot nextPlot = availablePlots[randomIndex];
					if (nextPlot.Mammal == Mammal.Bear)
					{
						currentPlot.Mammal = Mammal.None;
						HandleMawling(plots, random, log);
						break;
					}

					MoveMammal(Mammal.Lumberjack, ref currentPlot, ref nextPlot);
					if (currentPlot.Tree == null)
						continue;
					switch (currentPlot.Tree.Type)
					{
						case TreeType.Tree:
							currentPlot.Tree = null;
							log.TreeHarvested();
							break;
						case TreeType.Elder:
							currentPlot.Tree = null;
							log.ElderTreeHarvested();
							break;
					}
				}
			}
		}
		public static void ProduceBears(List<Plot> plots, Random random, EventLog log, int productions, bool forestInitialization)
		{
			List<Plot> availablePlots = plots.Where(plot => plot.Mammal == Mammal.None).ToList();
			if (!availablePlots.Any())
				return;

			for (int i = 0; i < productions; i++)
			{
				int randomIndex = random.Next(availablePlots.Count);
				availablePlots[randomIndex].Mammal = Mammal.Bear;
				availablePlots.RemoveAt(randomIndex);

				if (!forestInitialization)
					log.BearProduced();
				if (availablePlots.Count == 0)
					return;
			}
		}

		private static Func<Plot, Plot, bool> Adjacent = (plot, elementPlot) =>
			plot.Coordinate.X >= elementPlot.Coordinate.X - 1 && plot.Coordinate.X <= elementPlot.Coordinate.X + 1 &&
				plot.Coordinate.Y >= elementPlot.Coordinate.Y - 1 && plot.Coordinate.Y <= elementPlot.Coordinate.Y + 1 &&
				!plot.Coordinate.Equals(elementPlot.Coordinate);
		private static void HandleMawling(List<Plot> plots, Random random, EventLog log)
		{
			log.LumberjackMawled();
			if (plots.Where(plot => plot.Mammal == Mammal.Lumberjack).ToList().Count == 0)
				HireLumberjacks(plots, random, log, 1, false);
		}
		private static void MoveMammal(Mammal mammal, ref Plot currentPlot, ref Plot nextPlot)
		{
			currentPlot.Mammal = Mammal.None;
			nextPlot.Mammal = mammal;
			currentPlot = nextPlot;
		}
	}
}