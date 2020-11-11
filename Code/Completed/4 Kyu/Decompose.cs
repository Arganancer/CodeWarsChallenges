using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://www.codewars.com/kata/54eb33e5bc1a25440d000891
/// </summary>
public class Decompose
{
	public string decompose(long _n)
	{
		long goal = 0;
		Stack<long> result = new Stack<long>();
		result.Push(_n);
		while (result.Count > 0)
		{
			long current = result.Pop();
			goal += current * current;
			for (long i = current - 1; i > 0; --i)
			{
				long squaredValue = i * i;
				if (goal - squaredValue >= 0)
				{
					goal -= squaredValue;
					result.Push(i);
					if (goal == 0)
					{
						List<long> list = result.ToList();
						list.Sort();
						return string.Join(' ', list);
					}
				}
			}
		}

		return null;
	}
}