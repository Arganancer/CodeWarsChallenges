using System;

/// <summary>
/// https://www.codewars.com/kata/59568be9cc15b57637000054/train/csharp
/// </summary>
public static class Immortal
{
	/// set true to enable debug
	public static bool Debug = false;

	public static long ElderAge(long n, long m, long l, long t)
	{
		if (m == 0 || n == 0)
		{
			return 0;
		}

		long lm = 1;
		while (lm < Math.Max(n, m))
		{
			lm <<= 1;
		}

		long ln = 1;
		while (ln < Math.Min(n, m))
		{
			ln <<= 1;
		}

		if (l > ln)
		{
			return 0;
		}

		if (lm == ln)
		{
			return Sum(1, ln - l - 1) * (m + n - ln) + ElderAge(ln - n, lm - m, 1, t) % t;
		}

		lm = (long)Math.Floor((double)ln / 2);
		long tmp = Sum(1, ln - l - 1) * m - (ln - n) * Sum(Math.Max(0, lm - l), ln - l - 1);
		if (l <= lm)
		{
			tmp += (lm - l) * (lm - m) * (ln - n) + ElderAge(lm - m, ln - n, 0, t);
		}
		else
		{
			tmp += ElderAge(lm - m, ln - n, l - lm, t);
		}

		return tmp % t;
	}

	private static long Sum(long l, long r)
	{
		return (l + r) * (long)Math.Floor((double)(r - l + 1) / 2);
	}
}
