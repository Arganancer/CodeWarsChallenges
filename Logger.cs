using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Codewars
{
	public static class Logger
	{
		private static int _testNumber = 0;
		public static void Log<T>(T expected, T actual, string test = null)
		{
			ConsoleColor color = ConsoleColor.Green;
			if (expected is null)
			{
				if (!ReferenceEquals(actual, null))
				{
					color = ConsoleColor.Red;
				}
			}
			else if (!expected.Equals(actual))
			{
				color = ConsoleColor.Red;
			}

			Console.ForegroundColor = color == ConsoleColor.Green ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
			Console.WriteLine($"{$"{++_testNumber}{"Input:".PadLeft((int) (12 - Math.Floor(Math.Log10(_testNumber) + 1)))}",-14}{test ?? ""}");

			Console.ForegroundColor = color;
			Console.WriteLine($"{"Expected:".PadLeft(12),-14}{expected?.ToString() ?? "null"}\n" +
			                  $"{"Actual:".PadLeft(12),-14}{actual?.ToString() ?? "null"}\n");
		}

		public static void TimedLog<TInput, TOutput>( TInput _input, Func<TInput, TOutput> _runTest, int _idealMs )
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			TOutput result = _runTest( _input );
			stopwatch.Stop();

			Console.ForegroundColor = stopwatch.ElapsedMilliseconds < _idealMs ? ConsoleColor.Green : ConsoleColor.Red;
			Console.WriteLine($"Input:  {_input}\n" +
			                  $"Output: {result}\n" +
			                  $"Time:   {stopwatch.ElapsedMilliseconds}ms\n");
		}

		public static void Log<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool compareOrder = true, string test = null)
		{
			ConsoleColor color = ConsoleColor.Green;
			if (expected is null)
			{
				if (!ReferenceEquals(actual, null))
				{
					color = ConsoleColor.Red;
				}
			}
			else if (!expected.Equals(actual))
			{
				expected = expected.ToList();
				actual = actual.ToList();
				if (!compareOrder)
				{
					if (expected.Count() != actual.Count())
					{
						color = ConsoleColor.Red;
					}
					else if (expected.Any(item => !actual.Contains(item)))
					{
						color = ConsoleColor.Red;
					}
				}
				else
				{
					color = ConsoleColor.Red;
				}
			}

			Console.ForegroundColor = color == ConsoleColor.Green ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
			Console.WriteLine($"{++_testNumber}) {test ?? ""}");

			Console.ForegroundColor = color;
			Console.WriteLine($"    Expected: {string.Join(", ", expected.OrderBy(t => t))}\n" +
			                  $"    Actual:   {string.Join(", ", actual.OrderBy(t => t))}\n");
		}
	}
}
