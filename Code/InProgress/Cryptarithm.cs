using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
///     https://www.codewars.com/kata/5b5fe164b88263ad3d00250b
///		NOT COMPLETED. Couldn't get the performance down.
/// </summary>
public class Cryptarithm
{
	private static Dictionary<char, Letter> MappedLetters;
	private static Letter[] Letters;
	private static List<Equation> Equations;
	private static char[][] Terms;
	private static List<Term> LeftTerms;
	private static Term RightTerm;

	public static string Alphametics(string _input)
	{
		MappedLetters = new Dictionary<char, Letter>();
		foreach (char c in _input)
		{
			if (char.IsLetter(c) && !MappedLetters.ContainsKey(c))
			{
				MappedLetters.Add(c, new Letter(c));
			}
		}

		Letters = MappedLetters.Values.ToArray();
		Terms = _input.Split(' ').Where(subString => subString.All(char.IsLetter)).Select(subString => subString.ToCharArray()).ToArray();

		// Map out Letters to Terms to increase performance when evaluating Permutations.
		LeftTerms = new List<Term>();
		RightTerm = null;
		for (int i = 0; i < Terms.GetLength(0); i++)
		{
			if (i == Terms.GetLength(0) - 1)
			{
				RightTerm = new Term(Terms[i].Select(c => MappedLetters[c]).ToArray());
			}
			else
			{
				LeftTerms.Add(new Term(Terms[i].Select(c => MappedLetters[c]).ToArray()));
			}
		}

		CreateEquations();

		// Trim possibilities by deduction:
		NarrowSearch();

		// Brute Force the rest:
		return BruteForce();
	}

	private static void CreateEquations()
	{
		Equations = new List<Equation>();
		Equation lastEquation = null;
		int resultLength = Terms[Terms.Length - 1].Length;
		for (int i = resultLength - 1; i >= 0; i--)
		{
			List<Letter> left = new List<Letter>();
			for (int j = 0; j < Terms.Length - 1; j++)
			{
				int termIndex = i - (resultLength - Terms[j].Length);
				if (termIndex >= 0)
				{
					Letter letter = MappedLetters[Terms[j][termIndex]];
					left.Add(letter);

					if (termIndex == 0)
					{
						if (letter.Remove(new[] { 0 }, out _))
						{
							RemoveValueFromAllOtherLetters(letter);
						}
					}
				}
			}

			Letter right = MappedLetters[Terms[Terms.Length - 1][i]];
			if (i == 0)
			{
				if (right.Remove(new[] { 0 }, out _))
				{
					RemoveValueFromAllOtherLetters(right);
				}
			}

			int maximumCarryOver = 0;
			if (lastEquation != null)
			{
				maximumCarryOver = lastEquation.CurrentMaximumSummedValue / 10;
			}

			Equation currentEquation = new Equation(left, right, maximumCarryOver);
			Equations.Add(currentEquation);

			if (left.Count == 0)
			{
				if (right.LimitToRange(currentEquation.CarryOverPossibleValues, out _))
				{
					RemoveValueFromAllOtherLetters(right);
				}
			}

			if (lastEquation != null && lastEquation.Left.Count > currentEquation.Left.Count)
			{
				if (currentEquation.Left.Count == 1)
				{
					if (currentEquation.Left[0] == currentEquation.Right)
					{
						currentEquation.CarryOverPossibleValues.RemoveAll(carryOver => carryOver != 0);
						lastEquation.MaximumSummedValue = 9;
					}
					else
					{
						currentEquation.CarryOverPossibleValues.Remove(0);
						lastEquation.MinimumSummedValue = 10;
					}
				}
			}

			lastEquation = currentEquation;
		}
	}

	private static void NarrowSearch()
	{
		for (int i = 0; i < Equations.Count; i++)
		{
			bool restartNarrowSearch = false;
			Equation currentEquation = Equations[i];

			if (!currentEquation.Right.SingleValueObtained && currentEquation.Left.All(summand => summand == currentEquation.Right))
			{
				List<int> rangeLimit = new List<int>();
				int summandsCount = currentEquation.Left.Count;
				if (currentEquation.CarryOverPossibleValues.Last() == 0)
				{
					for (int j = 0; j <= 9; j++)
					{
						if (j * summandsCount % 10 == j)
						{
							rangeLimit.Add(j);
						}
					}
				}
				else if (summandsCount == 3)
				{
					currentEquation.CarryOverPossibleValues.Remove(1);
					if (currentEquation.CarryOverPossibleValues[0] == 0)
					{
						rangeLimit.AddRange(new[] { 0, 5 });
					}
					if (currentEquation.CarryOverPossibleValues.Max() == 2)
					{
						rangeLimit.AddRange(new[] { 4, 9 });
					}
				}

				if (rangeLimit.Count > 0 && currentEquation.Right.LimitToRange(rangeLimit.ToArray(), out restartNarrowSearch))
				{
					RemoveValueFromAllOtherLetters(currentEquation.Right);
				}
			}

			if (!currentEquation.Right.SingleValueObtained && currentEquation.SummandLettersCount.All(keyVal => keyVal.Value % 2 == 0))
			{
				if (currentEquation.CarryOverPossibleValues.All(carryOver => carryOver % 2 == 0))
				{
					if (currentEquation.Right.LimitToEven(out restartNarrowSearch))
					{
						RemoveValueFromAllOtherLetters(currentEquation.Right);
					}
				}
				else if (currentEquation.CarryOverPossibleValues.All(carryOver => carryOver % 2 == 1))
				{
					if (currentEquation.Right.LimitToOdd(out restartNarrowSearch))
					{
						RemoveValueFromAllOtherLetters(currentEquation.Right);
					}
				}
			}

			if (currentEquation.MaximumSummedValue < currentEquation.CurrentMaximumSummedValue)
			{
				if (currentEquation.Left.Count > 1 && currentEquation.Left.Distinct().Count() == 1)
				{
					int leftCount = currentEquation.Left.Count;
					Letter letter = currentEquation.Left[0];
					int limitToLessThanValue = (int)Math.Floor((float)currentEquation.MaximumSummedValue / leftCount) + 1;

					if (!letter.SingleValueObtained && letter.LimitToLessThan(limitToLessThanValue, out restartNarrowSearch))
					{
						RemoveValueFromAllOtherLetters(letter);
					}
				}
				else
				{
					int maxSum = currentEquation.CurrentMaximumSummedValue;
					int difference = maxSum - currentEquation.MaximumSummedValue;

					if (difference < 9)
					{
						foreach (Letter letter in currentEquation.Left.Where(letter => letter.PossibleValues.Last() == 9).Distinct())
						{
							if (!letter.SingleValueObtained && letter.LimitToLessThan(difference + 1, out restartNarrowSearch))
							{
								RemoveValueFromAllOtherLetters(letter);
							}
						}
					}
				}
			}

			if (currentEquation.MinimumSummedValue > 0)
			{
				if (currentEquation.Left.Count > 1 && currentEquation.Left.Distinct().Count() == 1)
				{
					int leftCount = currentEquation.Left.Count;
					Letter letter = currentEquation.Left[0];
					int limitToGreaterThanValue = (int)Math.Ceiling((float)currentEquation.MinimumSummedValue / leftCount) - 1;

					if (!letter.SingleValueObtained && letter.LimitToGreaterThan(limitToGreaterThanValue, out restartNarrowSearch))
					{
						RemoveValueFromAllOtherLetters(letter);
					}
				}
				else
				{
					int maxSum = currentEquation.CurrentMaximumSummedValue;
					int difference = maxSum - currentEquation.MinimumSummedValue;

					if (difference < 9)
					{
						foreach (Letter letter in currentEquation.Left.Where(letter => letter.PossibleValues.Last() == 9).Distinct())
						{
							if (!letter.SingleValueObtained && letter.LimitToGreaterThan(9 - difference - 1, out restartNarrowSearch))
							{
								RemoveValueFromAllOtherLetters(letter);
							}
						}
					}
				}
			}

			if (currentEquation.Left.Count(summand => summand == currentEquation.Right) == 1 && currentEquation.Left.Count == 2)
			{
				Letter summand = currentEquation.Left.First(s => s != currentEquation.Right);
				int[] limitToRange;
				int maxCarryOverValue = currentEquation.CarryOverPossibleValues.Last();
				if (maxCarryOverValue == 0)
				{
					limitToRange = new[] { 0 };
				}
				else if (currentEquation.CarryOverPossibleValues.Count == 1)
				{
					limitToRange = new[] { 10 - currentEquation.CarryOverPossibleValues[0] };
				}
				else
				{
					List<int> range = new List<int>();

					if (currentEquation.CarryOverPossibleValues.Contains(0))
					{
						range.Add(0);
					}

					for (int j = 10 - maxCarryOverValue; j <= 9; j++)
					{
						range.Add(j);
					}

					limitToRange = range.ToArray();
				}

				if (!summand.SingleValueObtained && summand.LimitToRange(limitToRange, out restartNarrowSearch))
				{
					RemoveValueFromAllOtherLetters(summand);
				}
			}

			if (currentEquation.Left.Count == 1 && currentEquation.Right != currentEquation.Left[0])
			{
				if (Array.IndexOf(Letters, currentEquation.Right) < Array.IndexOf(Letters, currentEquation.Left[0]))
				{
					currentEquation.CarryOverPossibleValues.Remove(0);
					currentEquation.Left[0].AddConstraint(currentEquation.Right, letter =>
				   {
					   int min = letter.CurrentValue - currentEquation.CarryOverPossibleValues.Last();
					   int max = letter.CurrentValue;
					   List<int> constraints = new List<int>();

					   if (min < 0)
					   {
						   if (i == Equations.Count - 1)
						   {
							   if (currentEquation.Right.LimitToGreaterThan(-min, out restartNarrowSearch))
							   {
								   RemoveValueFromAllOtherLetters(currentEquation.Right);
							   }
						   }
						   else
						   {
							   for (int j = 9 + min; j <= 9; j++)
							   {
								   constraints.Add(j);
							   }
						   }
					   }

					   for (int j = Math.Max(0, min); j <= max; j++)
					   {
						   constraints.Add(j);
					   }

					   return constraints;
				   });
				}
				else
				{
					currentEquation.Right.AddConstraint(currentEquation.Left[0], letter =>
				   {
					   int min = letter.CurrentValue;
					   int max = letter.CurrentValue + currentEquation.CarryOverPossibleValues.Last();
					   List<int> constraints = new List<int>();

					   if (max > 9)
					   {
						   for (int j = 0; j < max - 9; j++)
						   {
							   constraints.Add(j);
						   }
					   }

					   for (int j = min; j <= Math.Min(9, max); j++)
					   {
						   constraints.Add(j);
					   }

					   return constraints;
				   });
				}
			}

			if (currentEquation.Left.Count > 0 && currentEquation.Left.Sum(summand => summand.PossibleValues.Count - 1) + currentEquation.CarryOverPossibleValues.Count < 9)
			{
				int min = currentEquation.Left.Sum(summand => summand.PossibleValues[0]) + currentEquation.CarryOverPossibleValues[0];
				currentEquation.MinimumSummedValue = min;
				if (min >= 10)
				{
					for (int j = 0; j < min / 10; j++)
					{
						Equations[i + 1].CarryOverPossibleValues.Remove(j);
					}
				}

				if (!currentEquation.Right.SingleValueObtained)
				{
					List<int> possibleResults = new List<int>();
					Letter[] distinctLettersInEquation = currentEquation.Left.Distinct().ToArray();
					ExpandAllPossibleSums(0, 0);

					void ExpandAllPossibleSums(int _index, int _sum)
					{
						Letter currentLetter = distinctLettersInEquation[_index];
						foreach (int possibleLetterValue in currentLetter.PossibleValues)
						{
							if (_index == distinctLettersInEquation.Length - 1)
							{
								foreach (int possibleCarryOverValue in currentEquation.CarryOverPossibleValues)
								{
									int currentSum = _sum + possibleLetterValue + possibleCarryOverValue;
									possibleResults.Add(currentSum % 10);
								}
							}
							else
							{
								ExpandAllPossibleSums(_index + 1, _sum + possibleLetterValue);
							}
						}
					}

					if (currentEquation.Right.LimitToRange(possibleResults.Distinct(), out restartNarrowSearch))
					{
						RemoveValueFromAllOtherLetters(currentEquation.Right);
					}
				}
			}

			if (i > 0)
			{
				Equation lastEquation = Equations[i - 1];
				while (currentEquation.CarryOverPossibleValues.Last() > lastEquation.MaximumSummedValue / 10)
				{
					currentEquation.CarryOverPossibleValues.RemoveAt(currentEquation.CarryOverPossibleValues.Count - 1);
					restartNarrowSearch = true;
				}

				if (lastEquation.MaximumSummedValue / 10 > currentEquation.CarryOverPossibleValues.Last())
				{
					lastEquation.MaximumSummedValue = currentEquation.CarryOverPossibleValues.Last() * 10 + 9;
					restartNarrowSearch = true;
				}

				while (currentEquation.CarryOverPossibleValues[0] < lastEquation.MinimumSummedValue / 10)
				{
					currentEquation.CarryOverPossibleValues.RemoveAt(0);
					restartNarrowSearch = true;
				}

				if (lastEquation.MinimumSummedValue / 10 < currentEquation.CarryOverPossibleValues[0])
				{
					lastEquation.MinimumSummedValue = currentEquation.CarryOverPossibleValues[0] * 10;
					restartNarrowSearch = true;
				}

			}

			if (restartNarrowSearch)
			{
				i = -1;
			}
		}
	}

	//private static string SmartBruteForce()
	//{
	//	List<Dictionary<Letter, int>> lastGenerationTopChoices = new List<Dictionary<Letter, int>>();
	//	SortedDictionary<int, Dictionary<Letter, int>> currentGeneration = new SortedDictionary<int, Dictionary<Letter, int>>();
	//	Random rand = new Random();
	//	while (true)
	//	{
	//		if (lastGenerationTopChoices.Count == 0)
	//		{
	//			for (int i = 0; i < 4; i++)
	//			{
	//				Dictionary<Letter, int> newRandomStrain = new Dictionary<Letter, int>();
	//				foreach (Letter letter in Letters)
	//				{
	//					newRandomStrain.Add(letter, rand.Next(letter.PossibleValues.Count - 1));
	//				}
	//				ApplyStrain(newRandomStrain);
	//				EvaluateStrain(out _, out newRandomStrain);
	//				lastGenerationTopChoices.Add(newRandomStrain);
	//			}
	//		}
	//		else
	//		{
	//			lastGenerationTopChoices = currentGeneration.Take(3).Select(keyVal => keyVal.Value).ToList();
	//			(int bestKey, Dictionary<Letter, int> bestStrain) = currentGeneration.Take(1).First();
	//			currentGeneration.Clear();
	//			currentGeneration.Add(bestKey, bestStrain);

	//			// Add random strain.
	//			Dictionary<Letter, int> newRandomStrain = new Dictionary<Letter, int>();
	//			foreach (Letter letter in Letters)
	//			{
	//				newRandomStrain.Add(letter, rand.Next(letter.PossibleValues.Count - 1));
	//			}
	//			ApplyStrain(newRandomStrain);
	//			EvaluateStrain(out _, out newRandomStrain);
	//			lastGenerationTopChoices.Add(newRandomStrain);
	//		}

	//		foreach (Dictionary<Letter, int> lastGenStrain in lastGenerationTopChoices)
	//		{
	//			Dictionary<double, int> choiceHistory = new Dictionary<double, int>();
	//			for (int i = 0; i < 5; i++)
	//			{
	//				double choiceKey = -1;
	//				do
	//				{
	//					ApplyStrain(lastGenStrain);
	//					List<Letter> lettersToMutate = new List<Letter>();
	//					int lettersToMutateCount = rand.Next(1, Math.Min(4, Letters.Length));
	//					while (lettersToMutate.Count < lettersToMutateCount)
	//					{
	//						Letter letterToMutate = Letters[rand.Next(Letters.Length - 1)];
	//						if (!letterToMutate.SingleValueObtained && !lettersToMutate.Contains(letterToMutate))
	//						{
	//							lettersToMutate.Add(letterToMutate);
	//						}
	//					}

	//					foreach (Letter letter in lettersToMutate)
	//					{
	//						int newIndex = letter.CurrentValueIndex;
	//						while (newIndex == letter.CurrentValueIndex)
	//						{
	//							newIndex = rand.Next(letter.PossibleValues.Count - 1);
	//						}

	//						letter.CurrentValueIndex = newIndex;
	//					}

	//					double multiplier = 1;
	//					choiceKey = Letters.Sum(letter =>
	//				   {
	//					   double value = letter.CurrentValue * multiplier;
	//					   multiplier *= 10;
	//					   return value;
	//				   });

	//				} while (choiceHistory.ContainsKey( choiceKey ));
	//				choiceHistory.Add(choiceKey, 0);

	//				if (EvaluateStrain(out int fitness, out Dictionary<Letter, int> tidiedStrain))
	//				{
	//					if (fitness == 0)
	//					{
	//						StringBuilder stringBuilder = new StringBuilder();
	//						int leftTermCount = LeftTerms.Count;
	//						for (int j = 0; j < leftTermCount; j++)
	//						{
	//							LeftTerms[j].AppendConversion(stringBuilder);
	//							if (j < leftTermCount - 1)
	//							{
	//								stringBuilder.Append(" + ");
	//							}
	//						}

	//						stringBuilder.Append(" = ");
	//						RightTerm.AppendConversion(stringBuilder);

	//						return stringBuilder.ToString();
	//					}

	//					while (currentGeneration.ContainsKey(fitness))
	//					{
	//						fitness += 1;
	//					}

	//					currentGeneration.Add(fitness, tidiedStrain);
	//				}
	//			}
	//		}
	//	}

	//	void ApplyStrain(Dictionary<Letter, int> _strain)
	//	{
	//		foreach (KeyValuePair<Letter, int> letter in _strain)
	//		{
	//			letter.Key.CurrentValueIndex = letter.Value;
	//		}
	//	}

	//	bool EvaluateStrain(out int _fitness, out Dictionary<Letter, int> _tidiedStrain)
	//	{
	//		_fitness = -1;
	//		_tidiedStrain = new Dictionary<Letter, int>();

	//		for (int i = 0; i < Letters.Length; i++)
	//		{
	//			Letter currentLetter = Letters[i];
	//			bool letterHasReset = false;
	//			for (int j = 0; j < i; j++)
	//			{
	//				if (Letters[j].CurrentValue == currentLetter.CurrentValue)
	//				{
	//					if (!currentLetter.Next())
	//					{
	//						if (letterHasReset)
	//						{
	//							return false;
	//						}

	//						letterHasReset = true;
	//						currentLetter.Reset();
	//					}

	//					j = -1;
	//				}
	//			}

	//			_tidiedStrain.Add(currentLetter, currentLetter.CurrentValueIndex);
	//		}

	//		_fitness = Math.Abs(RightTerm.GetValue() - LeftTerms.Sum(summand => summand.GetValue()));
	//		return true;
	//	}
	//}

	private static string BruteForce()
	{
		string result = "";
		RecursivePermutations(0);
		return result;

		bool RecursivePermutations(int _index)
		{
			Letter currentLetter = Letters[_index];
			currentLetter.Reset();
			do
			{
				for (int i = 0; i < _index; i++)
				{
					if (Letters[i].CurrentValue == currentLetter.CurrentValue)
					{
						if (!currentLetter.Next())
						{
							return false;
						}

						i = -1;
					}
				}

				// Only validate solution while updating last letter.
				if (_index == Letters.Length - 1 && IsSolutionValid())
				{
					StringBuilder stringBuilder = new StringBuilder();
					int leftTermCount = LeftTerms.Count;
					for (int i = 0; i < leftTermCount; i++)
					{
						LeftTerms[i].AppendConversion(stringBuilder);
						if (i < leftTermCount - 1)
						{
							stringBuilder.Append(" + ");
						}
					}

					stringBuilder.Append(" = ");
					RightTerm.AppendConversion(stringBuilder);

					result = stringBuilder.ToString();

					return true;
				}

				if (_index < Letters.Length - 1 && RecursivePermutations(_index + 1))
				{
					return true;
				}
			} while (currentLetter.Next());

			return false;
		}
	}

	private static bool IsSolutionValid()
	{
		return RightTerm.GetValue() == LeftTerms.Sum(summand => summand.GetValue());
	}

	private static void RemoveValueFromAllOtherLetters(Letter _letter)
	{
		List<Letter> additionalLettersToFilter = new List<Letter>();

		foreach (Letter letter in MappedLetters.Values)
		{
			if (letter == _letter)
			{
				continue;
			}

			if (!letter.SingleValueObtained && letter.Remove(new[] { _letter.PossibleValues.First() }, out _))
			{
				additionalLettersToFilter.Add(letter);
			}
		}

		foreach (Letter letter in additionalLettersToFilter)
		{
			RemoveValueFromAllOtherLetters(letter);
		}
	}

	internal class Term
	{
		public Letter[] OrderedLetters { get; }

		public Term(Letter[] _orderedLetters)
		{
			OrderedLetters = _orderedLetters;
		}

		public int GetValue()
		{
			int value = OrderedLetters[OrderedLetters.Length - 1].CurrentValue;
			int multiplier = 10;
			for (int i = OrderedLetters.Length - 2; i >= 0; i--)
			{
				value += OrderedLetters[i].CurrentValue * multiplier;
				multiplier *= 10;
			}

			return value;
		}

		public void AppendConversion(StringBuilder _stringBuilder)
		{
			foreach (Letter letter in OrderedLetters)
			{
				_stringBuilder.Append(letter.CurrentValue);
			}
		}
	}

	internal class Letter
	{
		public event EventHandler<Letter> LetterUpdated;

		public bool HasConstraints { get; private set; }

		public int CurrentValueIndex
		{
			get => m_CurrentValueIndex;
			set
			{
				m_CurrentValueIndex = value;
				CurrentValue = PossibleValues[m_CurrentValueIndex];

				OnLetterUpdated(this);
			}
		}

		public int CurrentValue { get; private set; }

		public List<int> PossibleValues { get; } = new List<int>();

		public bool SingleValueObtained => PossibleValues.Count == 1;

		public char Char { get; }

		private int m_CurrentValueIndex;

		public Letter(char _char)
		{
			Constraints = new Dictionary<Letter, List<int>>();
			Char = _char;
			for (int i = 0; i < 10; i++)
			{
				PossibleValues.Add(i);
			}
		}

		public bool Reset()
		{
			CurrentValueIndex = 0;
			if (!WithinConstraints())
			{
				return Next();
			}
			return true;
		}

		public bool Next()
		{
			if (CurrentValueIndex == PossibleValues.Count - 1)
			{
				return false;
			}

			if (!HasConstraints)
			{
				CurrentValueIndex++;
				return true;
			}

			do
			{
				if (CurrentValueIndex == PossibleValues.Count - 1)
				{
					return false;
				}

				CurrentValueIndex++;
			} while (!WithinConstraints());
			return true;
		}

		public void AddConstraint(Letter _letter, Func<Letter, List<int>> _onLetterUpdated)
		{
			HasConstraints = true;
			_letter.LetterUpdated += (obj, letter) =>
			{
				Constraints[letter] = _onLetterUpdated(letter);
			};
		}

		public bool LimitToOdd(out bool _possibleValuesRangeChanged)
		{
			_possibleValuesRangeChanged = false;
			for (int i = 0; i < 10; i += 2)
			{
				if (PossibleValues.Remove(i))
				{
					_possibleValuesRangeChanged = true;
				}
			}

			return PossibleValues.Count == 1;
		}

		public bool LimitToEven(out bool _possibleValuesRangeChanged)
		{
			_possibleValuesRangeChanged = false;
			for (int i = 1; i < 10; i += 2)
			{
				if (PossibleValues.Remove(i))
				{
					_possibleValuesRangeChanged = true;
				}
			}

			return PossibleValues.Count == 1;
		}

		public bool LimitToRange(IEnumerable<int> _range, out bool _possibleValuesRangeChanged)
		{
			int previousCount = PossibleValues.Count;
			PossibleValues.RemoveAll(i => !_range.Contains(i));
			_possibleValuesRangeChanged = previousCount != PossibleValues.Count;
			return PossibleValues.Count == 1;
		}

		public bool Remove(int[] _values, out bool _possibleValuesRangeChanged)
		{
			_possibleValuesRangeChanged = false;
			foreach (int value in _values)
			{
				if (PossibleValues.Remove(value))
				{
					_possibleValuesRangeChanged = true;
				}
			}

			return PossibleValues.Count == 1;
		}

		public bool LimitToLessThan(int _value, out bool _possibleValuesRangeChanged)
		{
			int previousCount = PossibleValues.Count;
			PossibleValues.RemoveAll(i => i >= _value);
			_possibleValuesRangeChanged = previousCount != PossibleValues.Count;

			return PossibleValues.Count == 1;
		}

		public bool LimitToGreaterThan(int _value, out bool _possibleValuesRangeChanged)
		{
			int previousCount = PossibleValues.Count;
			PossibleValues.RemoveAll(i => i <= _value);
			_possibleValuesRangeChanged = previousCount != PossibleValues.Count;

			return PossibleValues.Count == 1;
		}

		private bool WithinConstraints()
		{
			foreach (List<int> constraints in Constraints.Values)
			{
				if (!constraints.Contains(CurrentValue))
				{
					return false;
				}
			}

			return true;
		}

		private void OnLetterUpdated(Letter e)
		{
			LetterUpdated?.Invoke(this, e);
		}

		public Dictionary<Letter, List<int>> Constraints;
	}

	internal class Equation
	{

		public List<Letter> Left { get; }

		public Letter Right { get; }

		public List<int> CarryOverPossibleValues { get; }

		public int MinimumSummedValue { get; set; }

		public int MaximumSummedValue { get; set; }

		public int CurrentMaximumSummedValue => Left.Sum(summand => summand.PossibleValues.Last()) + CarryOverPossibleValues.Last();

		public Dictionary<char, int> SummandLettersCount { get; }

		public Equation(List<Letter> _left, Letter _right, int _maximumCarryOver)
		{
			Left = _left;
			Right = _right;

			CarryOverPossibleValues = new List<int>();
			for (int i = 0; i <= _maximumCarryOver; i++)
			{
				CarryOverPossibleValues.Add(i);
			}

			SummandLettersCount = new Dictionary<char, int>();
			foreach (Letter letter in Left)
			{
				if (!SummandLettersCount.ContainsKey(letter.Char))
				{
					SummandLettersCount.Add(letter.Char, 0);
				}

				SummandLettersCount[letter.Char]++;
			}

			MaximumSummedValue = CurrentMaximumSummedValue;
		}
	}
}

// Main Tests:
//static void Main(string[] args)
//{
//	Console.SetWindowSize(200, 60);

//	Logger.TimedLog("ELEVEN + NINE + FIVE + FIVE = THIRTY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("ZS + JIS = JZZ", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("VV + HBU = VHV", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("JNEXGNGA + GYNDANI + AIADDHX + YYYIH + HDEJGJX + AJGJGYE + GEGYJXEG = YXNNAHGG", Cryptarithm.Alphametics, 500);
//	//Console.Read();
//	Logger.TimedLog("FOURTEEN + TEN + TEN + SEVEN = FORTYONE", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("AACO + AACT + AACO + AACC + AACC + AAOC = CBAXO", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("DO + YOU + FEEL = LUCKY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("OPPPPPPP + OOPPPPPP + OOOPPPPP + OOOOPPPP + OOOOOPPP + OOOOOOPP + OOOOOOOP = ERTYUIOP", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("OZYCO + EDC + KYDYDZK + JKCWWW = OYZIWEY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("BILL + JIM = DUDES", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("SEND + MORE = MONEY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("ZEROES + ONES = BINARY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("COUPLE + COUPLE = QUARTET", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("NINETEEN + THIRTEEN + THREE + TWO + TWO + ONE + ONE + ONE = FORTYTWO", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("CIRCLE + CIRCLE + CIRCLE = SPHERE", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("CLARA + DIANE = LADIES", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("RAIN + WIND = STORM", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("FIRE + WATER = STEAM", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("BAT + FOOD = GUANO", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("SAND + SUN + SEX + SEA = IBIZA", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("SATURN + URANUS + NEPTUNE + PLUTO = PLANETS", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("WCY + CQWQYC + QPYXQX + CKKXWD = KKKYPCJ", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("TTAZPP + NPZ + UJTC + TZNTUUC = CAUAAAN", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("KVKWC + HBV + WCBQQF + QCBQE = BQVBBC", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("AEYODG + DSJO + PPOAO + XEDS = PAEEAD", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("SGLA + WTTWHOT + OLW + GJFTTJ + FGJTO = FHFGTLJ", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("FGGH + THTAA + ATDSD + TTGFKKG + FFKSKI = TASTNNA", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("VD + RVIIM + VMYV + DCFMLCYM + DLVBRBRI = BCMRFVRLY", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("GGIPFJ + HPSS + HAA + HFKSG + HHIATH = SPGGPI", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("GRSJRJ + KSQRKS + RIGTTQ + DRKSTK + QQDQWRKW = QJKSDWQT", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("SVH + SDB + PFHH + SHFDF + FVSBFBJ = FPPPKGH", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("YOPTI + QGQAQL + PQPY + OLIY + LTIAGG = AAPRGII", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("IJHLWH + VVBWLW + RIIWLI + RHJR + BRRWBLH = HWHHRLH", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("EZBAW + ABZDAF + BJAF + EWDVZVEJ + DVWWXEAF + JAFEJWW = VVBVBEDBB", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("CYPU + EPUJZE + EJZJO + UHPUJZ + OBJC + BBJYZZPH = BCYOCHBP", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("GBTFB + STJTDJS + STKGU + JFFGSJ + TKETGSGB + GEBFDUE = FTDFKGDD", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("WFFI + GECIWIE + FTJEE + GTHEFE + EEGTMCCM + CWCHCHGJ = MFTGMHTET", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("RFHFMLM + LLLXME + FLIDDS + HXERX + ISLMXMH + LIHFLMEH + DEHDILFH = XSRHHHEEH", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("LCMM + SSLDLM + LZJMCZHZ + DLMZCM + QMCZSQNC + QQHCMNQ + ZZNZZCN = DCQDDMJS", Cryptarithm.Alphametics, 500);
//	Logger.TimedLog("ECAZ + GFHCEDA + ZCCZG + GZZPOFP + AFEDGZ + GHHEOPF + POCCHFO = EZZEAEGF", Cryptarithm.Alphametics, 500);

//	Console.ForegroundColor = ConsoleColor.White;
//	Console.WriteLine("++ All tests executed.");
//	Console.Read();
//}