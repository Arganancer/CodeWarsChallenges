using System.Collections.Generic;
using System.Text;
using Codewars;

public class TheObservedPin
{
	private static readonly Dictionary<char, char[]> Possibilities = new Dictionary<char, char[]>
	{
		{'0', new []{'0', '8'}},
		{'1', new []{'1', '2', '4'}},
		{'2', new []{'2', '1', '3', '5'}},
		{'3', new []{'3', '2', '6'}},
		{'4', new []{'4', '1', '5', '7'}},
		{'5', new []{'5', '2', '4', '6', '8'}},
		{'6', new []{'6', '3', '5', '9'}},
		{'7', new []{'7', '4', '8'}},
		{'8', new []{'8', '5', '7', '9', '0'}},
		{'9', new []{'9', '6', '8'}}
	};

	public static List<string> GetPINs(string observed)
	{
		List<string> combinations = new List<string> { observed };
		CreateCombinations(observed, 0);
		return combinations;

		void CreateCombinations(string combination, int index)
		{
			if (index >= combination.Length) return;

			StringBuilder builder = new StringBuilder(combination);
			foreach (char adjacentNumber in Possibilities[combination[index]])
			{
				string newCombination = builder.ReplaceAt(index, adjacentNumber);
				if (newCombination != combination) combinations.Add(newCombination);
				CreateCombinations(newCombination, index + 1);
			}
		}
	}
}

public static class StringBuilderExtensions
{
	public static string ReplaceAt(this StringBuilder stringBuilder, int index, char newChar)
	{
		stringBuilder[index] = newChar;
		return stringBuilder.ToString();
	}
}

public static class TheObservedPinTests
{
	public static void RunTests()
	{
		Logger.Log(new List<string> { "5", "7", "8", "9", "0" }, TheObservedPin.GetPINs("8"), false, "8");
		Logger.Log(new List<string> { "11", "22", "44", "12", "21", "14", "41", "24", "42" }, TheObservedPin.GetPINs("11"), false, "11");
		Logger.Log(new List<string> { "339", "366", "399", "658", "636", "258", "268", "669", "668", "266", "369", "398", "256", "296", "259", "368", "638", "396", "238", "356", "659", "639", "666", "359", "336", "299", "338", "696", "269", "358", "656", "698", "699", "298", "236", "239" }, TheObservedPin.GetPINs("369"), false, "369");
	}
}