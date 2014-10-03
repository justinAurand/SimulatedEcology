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

		public int LumberHarvestedThisYear { get { return annualLog.LumberHarvested; } }
		public int MawlingsThisYear { get { return annualLog.LumberjacksMawled; } }

		#region Public
		public void Clear(int ageInMonths)
		{
			monthlyLog = new Log();
			if (ageInMonths % 12 == 0)
				annualLog = new Log();
		}
		public void ElderTreeHarvested()
		{
			monthlyLog.LumberHarvested += 2;
			annualLog.LumberHarvested += 2;
		}
		public void BearProduced()
		{
			monthlyLog.BearsProduced++;
			annualLog.BearsProduced++;
		}
		public void BearTrapped()
		{
			monthlyLog.BearsTrapped++;
			annualLog.BearsTrapped++;
		}
		public void LumberjackFired()
		{
			monthlyLog.LumberjacksFired++;
			annualLog.LumberjacksFired++;
		}
		public void LumberjackHired()
		{
			monthlyLog.LumberjacksHired++;
			annualLog.LumberjacksHired++;
		}
		public void LumberjackMawled()
		{
			monthlyLog.LumberjacksMawled++;
			annualLog.LumberjacksMawled++;
		}
		public void SaplingMatured()
		{
			monthlyLog.SaplingsMatured++;
			annualLog.SaplingsMatured++;
		}
		public void SaplingSpawned()
		{
			monthlyLog.SaplingsSpawned++;
			annualLog.SaplingsSpawned++;
		}
		public void TreeHarvested()
		{
			monthlyLog.LumberHarvested++;
			annualLog.LumberHarvested++;
		}
		public void TreeMatured()
		{
			monthlyLog.TreesMatured++;
			annualLog.TreesMatured++;
		}
		public void WriteToFile(int ageInMonths)
		{
			var output = new StringBuilder();
			output.Append(String.Format("Month [{0}]:{1}", ageInMonths, Environment.NewLine));
			output.Append(monthlyLog);

			if (ageInMonths % 12 == 0)
			{
				output.Append(String.Format("Year [{0}]:{1}", ageInMonths / 12, Environment.NewLine));
				output.Append(annualLog);
			}

			File.AppendAllText(filePath, output.ToString());
		}
		#endregion

		private class Log
		{
			internal int BearsTrapped { get; set; }
			internal int BearsProduced { get; set; }
			internal int LumberHarvested { get; set; }
			internal int LumberjacksFired { get; set; }
			internal int LumberjacksHired { get; set; }
			internal int LumberjacksMawled { get; set; }
			internal int SaplingsMatured { get; set; }
			internal int SaplingsSpawned { get; set; }
			internal int TreesMatured { get; set; }

			public override string ToString()
			{
				if (SaplingsSpawned <= 0 && SaplingsMatured <= 0 && TreesMatured <= 0 && LumberHarvested <= 0 && LumberjacksMawled <= 0
					&& LumberjacksHired <= 0 && LumberjacksFired <= 0 && BearsProduced <= 0 && BearsTrapped <= 0)
					return "No events to report." + Environment.NewLine;

				var output = new StringBuilder();
				if (SaplingsSpawned > 0)
					output.Append(String.Format(EventDescriptions.SapplingsSpawned, SaplingsSpawned) + Environment.NewLine);
				if (SaplingsMatured > 0)
					output.Append(String.Format(EventDescriptions.SapplingsMatured, SaplingsMatured) + Environment.NewLine);
				if (TreesMatured > 0)
					output.Append(String.Format(EventDescriptions.TreesMatured, TreesMatured) + Environment.NewLine);
				if (LumberHarvested > 0)
					output.Append(String.Format(EventDescriptions.LumberHarvested, LumberHarvested) + Environment.NewLine);
				if (LumberjacksMawled > 0)
					output.Append(String.Format(EventDescriptions.LumberjacksMawled, LumberjacksMawled) + Environment.NewLine);
				if (LumberjacksHired > 0)
					output.Append(String.Format(EventDescriptions.LumberjackHired, LumberjacksHired) + Environment.NewLine);
				if (LumberjacksFired > 0)
					output.Append(String.Format(EventDescriptions.LumberjackFired, LumberjacksFired) + Environment.NewLine);
				if (BearsProduced > 0)
					output.Append(String.Format(EventDescriptions.BearsProduced, BearsProduced) + Environment.NewLine);
				if (BearsTrapped > 0)
					output.Append(String.Format(EventDescriptions.BearsTrapped, BearsTrapped) + Environment.NewLine);
				return output.ToString();
			}
		}
	}
}
