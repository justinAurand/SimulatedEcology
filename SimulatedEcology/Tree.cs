namespace SimulatedEcology
{
	public class Tree
	{
		private int ageInMonths;

		public Tree(bool forestInitialization)
		{
			ageInMonths = forestInitialization ? 12 : 0;
		}

		public TreeType Type
		{
			get
			{
				if (ageInMonths >= 120)
					return TreeType.Elder;
				return ageInMonths >= 12 ? TreeType.Tree : TreeType.Sapling;
			}
		}

		public void Mature()
		{
			ageInMonths++;
		}
	}
}
