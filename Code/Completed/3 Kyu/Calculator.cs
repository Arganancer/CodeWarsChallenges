using System;
using System.Collections.Generic;
using System.Numerics;
using static System.Math;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/5518a860a73e708c0a000027/train/csharp
	/// </summary>
	public class Calculator
	{
		public static int LastDigit(int[] array)
		{
			if (array.Length == 0)
			{
				return 1;
			}

			BigInteger n = array[^1];
			for (int i = 2; i <= array.Length; i++)
			{
				n = BigInteger.Pow(array[^i], (int)(n < 4 ? n : n % 4 + 4));
			}

			return (int)(n % 10);
		}
	}

	public static class CalculatorTests
	{
		public static void RunTests()
		{
			Logger.Log(1, Calculator.LastDigit(new int[0]), "new int[0]");
			Logger.Log(0, Calculator.LastDigit(new[] { 0 }), "new[] { 0 }");
			Logger.Log(8, Calculator.LastDigit(new[] { 8 }), "new[] { 8 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 489351 }), "new[] { 489351 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 5641, 876132 }), "new[] { 5641, 876132 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 0, 0 }), "new[] { 0, 0 }");
			Logger.Log(0, Calculator.LastDigit(new[] { 0, 0, 0 }), "new[] { 0, 0, 0 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 2 }), "new[] { 1, 2 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 3, 4, 5 }), "new[] { 3, 4, 5 }");
			Logger.Log(4, Calculator.LastDigit(new[] { 4, 3, 6 }), "new[] { 4, 3, 6 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 7, 6, 21 }), "new[] { 7, 6, 21 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 12, 30, 21 }), "new[] { 12, 30, 21 }");
			Logger.Log(4, Calculator.LastDigit(new[] { 2, 2, 2, 0 }), "new[] { 2, 2, 2, 0 }");
			Logger.Log(0, Calculator.LastDigit(new[] { 937640, 767456, 981242 }), "new[] { 937640, 767456, 981242 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 123232, 694022, 140249 }), "new[] { 123232, 694022, 140249 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 499942, 898102, 846073 }), "new[] { 499942, 898102, 846073 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }), "1, 2, 3, 4, 5, 6, 7, 8, 9");
			Logger.Log(1, Calculator.LastDigit(new[] { 23423, 2, 3, 4, 5, 6, 7, 8, 9 }), "23423, 2, 3, 4, 5, 6, 7, 8, 9"); // 23423**(2**(3**(4**(5**(6**(7**(8**9)))) mod 10
			Logger.Log(4, Calculator.LastDigit(new[] { 87324, 1237, 4569845, 12314 }), "new[] { 87324, 1237, 4569845, 12314 }");
			Logger.Log(4, Calculator.LastDigit(new[] { 14, 17, 12, 22 }), "new[] { 14, 17, 12, 22 }"); // 14**(17**(12**(22))) mod 10
			Logger.Log(7, Calculator.LastDigit(new[] { 67, 33, 954, 2 }), "new[] { 67, 33, 954, 2 }"); // 67**(33**(954**(2))) mod 10
			Logger.Log(8, Calculator.LastDigit(new[] { 68, 33, 954, 2 }), "new[] { 68, 33, 954, 2 }");
			Logger.Log(9, Calculator.LastDigit(new[] { 69, 33, 954, 2 }), "new[] { 69, 33, 954, 2 }");
			Logger.Log(0, Calculator.LastDigit(new[] { 70, 33, 954, 2 }), "new[] { 70, 33, 954, 2 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 71, 33, 954, 2 }), "new[] { 71, 33, 954, 2 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 1, 1 }), "new[] { 1, 1, 1 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 2, 2, 2 }), "new[] { 2, 2, 2  }");
			Logger.Log(7, Calculator.LastDigit(new[] { 3, 3, 3 }), "new[] { 3, 3, 3 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 4, 4, 4 }), "new[] { 4, 4, 4 }");
			Logger.Log(5, Calculator.LastDigit(new[] { 5, 5, 5 }), "new[] { 5, 5, 5 }");
			Logger.Log(6, Calculator.LastDigit(new[] { 6, 6, 6 }), "new[] { 6, 6, 6 }");
			Logger.Log(1, Calculator.LastDigit(new[] { 423, 6, 6 }), "new[] { 423, 6, 6 }");
			Logger.Log(4, Calculator.LastDigit(new[] { 64, 8, 0, 298, 123, 6542, 57, 1 }), "new[] { 64, 8, 0, 298, 123, 6542, 57, 1 }"); // 64**8**0**298**123**6542**57**1 mod 10
			Logger.Log(6, Calculator.LastDigit(new[] { 56, 23, 8, 19, 21, 65, 10, 20, 50 }), "new[] { 56, 23, 8, 19, 21, 65, 10, 20, 50 }"); // 56**23**8**19**21**65**10**20**50 mod 10
			Logger.Log(6, Calculator.LastDigit(new[] { 44, 68, 88, 3, 7, 4444, 9855, 0, 2 }), "new[] { 44, 68, 88, 3, 7, 4444, 9855, 0, 2 }"); // 44**68**88**3**7**4444**9855**0**2 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 250121, 954061, 885408, 119958, 609734, 243446, 270961 }), "250121, 954061, 885408, 119958, 609734, 243446, 270961"); // 250121**954061**885408**119958**609734**243446**270961 mod 10
			Logger.Log(2, Calculator.LastDigit(new[] { 2, 1, 1, 1, 1, 0, 2 }), "2, 1, 1, 1, 1, 0, 2"); // 2**1**1**1**1**0**2 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 0, 0, 0, 1, 0, 1 }), "1, 0, 0, 0, 1, 0, 1"); // 1**0**0**0**1**0**1 mod 10
			Logger.Log(7, Calculator.LastDigit(new[] { 957427, 518321, 393426, 272101, 400776, 323062, 958262, 978860, 845020, 474222, 586413, 562508, 30738, 431563 }), "957427, 518321, 393426, 272101, 400776, 323062, 958262, 978860, 845020, 474222, 586413, 562508, 30738, 431563"); // 957427**518321**393426**272101**400776**323062**958262**978860**845020**474222**586413**562508**30738**431563 mod 10
			Logger.Log(2, Calculator.LastDigit(new[] { 2, 0, 0, 2, 2, 0, 2, 1, 2, 2, 1, 1, 2, 0 }), "2, 0, 0, 2, 2, 0, 2, 1, 2, 2, 1, 1, 2, 0"); // 2**0**0**2**2**0**2**1**2**2**1**1**2**0 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 2, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0 }), "2, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0"); // 2**0**1**1**0**1**0**0**0**1**0**1**0**0 mod 10
			Logger.Log(6, Calculator.LastDigit(new[] { 251328, 61548, 729336, 1549 }), "251328, 61548, 729336, 1549"); // 251328**61548**729336**1549 mod 10
			Logger.Log(4, Calculator.LastDigit(new[] { 2, 2, 0, 0 }), "2, 2, 0, 0"); // 2**2**0**0 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 2, 0, 0, 0 }), "2, 0, 0, 0"); // 2**0**0**0 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0 }), "1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0"); // 1**0**1**0**1**1**1**0**0**1**1**0**0**1**1**0 mod 10
			Logger.Log(5, Calculator.LastDigit(new[] { 961695 }), "961695"); // 961695 mod 10
			Logger.Log(2, Calculator.LastDigit(new[] { 2 }), "2"); // 2 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 650601, 895330, 413673, 494118, 940539, 710451, 790800, 555399, 375925, 937235, 28231, 124740, 446664, 481863 }), "650601, 895330, 413673, 494118, 940539, 710451, 790800, 555399, 375925, 937235, 28231, 124740, 446664, 481863"); // 650601**895330**413673**494118**940539**710451**790800**555399**375925**937235**28231**124740**446664**481863 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 3, 0, 2, 2, 1, 1, 2, 0, 1, 2, 0, 0, 1, 2 }), "3, 0, 2, 2, 1, 1, 2, 0, 1, 2, 0, 0, 1, 2"); // 3**0**2**2**1**1**2**0**1**2**0**0**1**2 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1 }), "1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1"); // 1**0**0**1**1**1**0**0**0**0**1**1**0**1 mod 10
			Logger.Log(7, Calculator.LastDigit(new[] { 217097, 778837, 707697 }), "217097, 778837, 707697"); // 217097**778837**707697 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 1, 2 }), "1, 2"); // 1**2 mod 10
			Logger.Log(6, Calculator.LastDigit(new[] { 4, 4, 1 }), "4, 4, 1"); // 4**4**1 mod 10
			Logger.Log(7, Calculator.LastDigit(new[] { 3, 3, 1 }), "3, 3, 1"); // 3**3**1 mod 10
			Logger.Log(3, Calculator.LastDigit(new[] { 3, 3, 2 }), "3, 3, 2"); // 3**3**2 mod 10
			Logger.Log(3, Calculator.LastDigit(new[] { 3, 5, 3 }), "3, 5, 3"); // 3**5**3 mod 10
			Logger.Log(1, Calculator.LastDigit(new[] { 3, 4, 5 }), "3, 4, 5"); // 3**4**5 mod 10
			Logger.Log(4, Calculator.LastDigit(new[] { 4, 3, 6 }), "4, 3, 6"); // 4**3**6 mod 10
		}
	}
}
