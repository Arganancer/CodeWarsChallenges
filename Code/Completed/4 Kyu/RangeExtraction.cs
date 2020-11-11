using System;
using System.Collections.Generic;

/// <summary>
/// https://www.codewars.com/kata/51ba717bb08c1cd60f00002f
/// </summary>
class RangeExtraction
{
	public static string Extract(int[] args)
	{
		List<string> result = new List<string>();
		int rangeStart = args[0];
		int rangeCount = 1;

		for (int i = 1; i < args.Length; i++)
		{
			bool incrementRange = Math.Abs(args[i] - args[i - 1]) == 1;
			if (incrementRange)
			{
				++rangeCount;
			}

			if (!incrementRange || i == args.Length - 1)
			{
				if (rangeCount >= 3)
				{
					result.Add($"{rangeStart}-{rangeStart + rangeCount - 1}");
				}
				else
				{
					for (int j = rangeStart; j < rangeStart + rangeCount; j++)
					{
						result.Add(j.ToString());
					}
				}

				rangeCount = 1;
				rangeStart = args[i];

				if (!incrementRange && i == args.Length - 1)
				{
					result.Add(args[i].ToString());
				}
			}
		}

		return string.Join(',', result);
	}
}
