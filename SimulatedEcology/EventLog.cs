namespace SimulatedEcology
{
	using System;
	using System.IO;
	using System.Text;

	public class EventLog
	{
		private const string filePath = @"C:\Users\Justin\Desktop\EventLog.txt";
		private Log monthlyLog = new Log();
		private Log annualLog = new Log();

		public EventLog()
		{
			File.WriteAllText(filePath, string.Empty);
		}

		public int LumberHarvestedThisYear { get { return annualLog.lumberHarvested; } }
		public int MawlingsThisYear { get { return annualLog.lumberjacksMawled; } }

		public void Clear(int ageInMonths)
		{
			monthlyLog = new Log();
			if (ageInMonths % 12 == 0)
				annualLog = new Log();
		}
		public void ElderTreeHarvested()
		{
			monthlyLog.lumberHarvested += 2;
			annualLog.lumberHarvested += 2;
		}
		public void BearProduced()
		{
			monthlyLog.bearsProduced++;
			annualLog.bearsProduced++;
		}
		public void BearTrapped()
		{
			monthlyLog.bearsTrapped++;
			annualLog.bearsTrapped++;
		}
		public void LumberjackFired()
		{
			monthlyLog.lumberjacksFired++;
			annualLog.lumberjacksFired++;
		}
		public void LumberjackHired()
		{
			monthlyLog.lumberjacksHired++;
			annualLog.lumberjacksHired++;
		}
		public void LumberjackMawled()
		{
			monthlyLog.lumberjacksMawled++;
			annualLog.lumberjacksMawled++;
		}
		public void SaplingMatured()
		{
			monthlyLog.saplingsMatured++;
			annualLog.saplingsMatured++;
		}
		public void SaplingSpawned()
		{
			monthlyLog.saplingsSpawned++;
			annualLog.saplingsSpawned++;
		}
		public void TreeHarvested()
		{
			monthlyLog.lumberHarvested++;
			annualLog.lumberHarvested++;
		}
		public void TreeMatured()
		{
			monthlyLog.treesMatured++;
			annualLog.treesMatured++;
		}
		public void WriteToFile(int ageInMonths)
		{
			var output = new StringBuilder();
			output.Append(String.Format("Month [{0}]:{1}", ageInMonths, Environment.NewLine));
			output.Append(monthlyLog.ToString());

			if (ageInMonths % 12 == 0)
			{
				output.Append(String.Format("Year [{0}]:{1}", ageInMonths / 12, Environment.NewLine));
				output.Append(annualLog.ToString());
			}

			File.AppendAllText(filePath, output.ToString());
		}

		private class Log
		{
			internal int bearsTrapped { get; set; }
			internal int bearsProduced { get; set; }
			internal int lumberHarvested { get; set; }
			internal int lumberjacksFired { get; set; }
			internal int lumberjacksHired { get; set; }
			internal int lumberjacksMawled { get; set; }
			internal int saplingsMatured { get; set; }
			internal int saplingsSpawned { get; set; }
			internal int treesMatured { get; set; }

			public override string ToString()
			{
				if (saplingsSpawned > 0 || saplingsMatured > 0 || treesMatured > 0 || lumberHarvested > 0 || lumberjacksMawled > 0 ||
					lumberjacksHired > 0 || lumberjacksFired > 0 || bearsProduced > 0 || bearsTrapped > 0)
				{
					var output = new StringBuilder();
					if (saplingsSpawned > 0)
						output.Append(String.Format(EventDescriptions.SapplingsSpawned, saplingsSpawned) + Environment.NewLine);
					if (saplingsMatured > 0)
						output.Append(String.Format(EventDescriptions.SapplingsMatured, saplingsMatured) + Environment.NewLine);
					if (treesMatured > 0)
						output.Append(String.Format(EventDescriptions.TreesMatured, treesMatured) + Environment.NewLine);
					if (lumberHarvested > 0)
						output.Append(String.Format(EventDescriptions.LumberHarvested, lumberHarvested) + Environment.NewLine);
					if (lumberjacksMawled > 0)
						output.Append(String.Format(EventDescriptions.LumberjacksMawled, lumberjacksMawled) + Environment.NewLine);
					if (lumberjacksHired > 0)
						output.Append(String.Format(EventDescriptions.LumberjackHired, lumberjacksHired) + Environment.NewLine);
					if (lumberjacksFired > 0)
						output.Append(String.Format(EventDescriptions.LumberjackFired, lumberjacksFired) + Environment.NewLine);
					if (bearsProduced > 0)
						output.Append(String.Format(EventDescriptions.BearsProduced, bearsProduced) + Environment.NewLine);
					if (bearsTrapped > 0)
						output.Append(String.Format(EventDescriptions.BearsTrapped, bearsTrapped) + Environment.NewLine);
					return output.ToString();
				}

				return "No events to report." + Environment.NewLine;
			}
		}
	}
}