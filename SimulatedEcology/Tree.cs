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
				if (ageInMonths >= 12)
					return TreeType.Tree;
				return TreeType.Sapling;
			}
		}

		public void Mature()
		{
			ageInMonths++;
		}
	}
}