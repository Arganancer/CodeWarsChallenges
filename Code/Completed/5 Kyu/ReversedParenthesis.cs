using System;

/// <summary>
/// https://www.codewars.com/kata/5a3f2925b6cfd78fb0000040
/// </summary>
public class Solution
{
	public static int solve( string s )
	{
		if (s.Length % 2 == 1)
		{
			return -1;
		}

		int reversals = 0;
		int balance = 0;
		foreach (char paren in s)
		{
			if (paren == '(')
			{
				balance++;
			}
			else
			{
				balance--;
				if (balance < 0)
				{
					++reversals;
					balance += 2;
				}
			}
		}

		return reversals + balance / 2;
	}
}
