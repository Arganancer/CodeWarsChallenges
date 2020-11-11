using System.Collections.Generic;

namespace Codewars
{
	public class Josephus
	{
		public static List<object> JosephusPermutation(List<object> items, int k)
		{
			int count = items.Count;
			int currentIndex = 0;
			List<object> output = new List<object>();
			while (count-- > 0)
			{
				currentIndex = (currentIndex + k - 1) % items.Count;
				output.Add(items[currentIndex]);
				items.RemoveAt(currentIndex);
			}

			return output;
		}
	}
}
