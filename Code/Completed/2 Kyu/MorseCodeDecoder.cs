using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 2 Kyu Part 3:
/// https://www.codewars.com/kata/decode-the-morse-code-for-real
/// </summary>
public partial class MorseCodeDecoder
{
	public static string decodeBitsAdvanced(string bits)
	{
		if (string.IsNullOrWhiteSpace(bits))
		{
			return "";
		}
		bits = bits.Trim('0');
		int shortRate = int.MaxValue;
		int mediumRate = 0;
		int longRate = 0;

		List<int> rates = new List<int>();

		int currentSequenceCount = 1;
		char currentBit = '1';
		for (int i = 1; i < bits.Length; i++)
		{
			bool incrementedCount = bits[i] == currentBit;
			if (incrementedCount)
			{
				++currentSequenceCount;
			}

			if (!incrementedCount || i == bits.Length - 1)
			{
				if (shortRate > currentSequenceCount)
				{
					shortRate = currentSequenceCount;
				}

				if (currentBit == '1' && mediumRate < currentSequenceCount)
				{
					mediumRate = currentSequenceCount;
				}

				if (currentBit == '0' && longRate < currentSequenceCount)
				{
					longRate = currentSequenceCount;
				}

				rates.Add( currentSequenceCount );

				currentBit = bits[i];
				currentSequenceCount = 1;
			}
		}

		if (shortRate >= mediumRate)
		{
			if ( longRate / shortRate < 5 )
			{
				mediumRate = longRate;
			}
			else
			{
				mediumRate = shortRate + 1;
			}
		}
		else if (longRate - mediumRate > 1 )
		{
			int[] values = rates.Distinct().Where(x => x >= mediumRate).OrderBy(x => x).ToArray();
			int biggestGap = 0;
			int biggestGapIndex = 0;
			for (int i = 0; i < values.Length - 1; i++)
			{
				int gap = values[i + 1] - values[i];
				if (gap > biggestGap)
				{
					biggestGap = gap;
					biggestGapIndex = i;
				}
			}

			if (biggestGap == 1)
			{
				mediumRate += (int)Math.Floor((longRate - mediumRate) / 2f);
			}
			else
			{
				mediumRate = values[biggestGapIndex];
			}
		}

		if (mediumRate - shortRate > 1 )
		{
			int[] values = rates.Distinct().Where( x => x <= mediumRate).OrderBy(x => x).ToArray();
			int biggestGap = 0;
			int biggestGapIndex = 0;
			for ( int i = 0; i < values.Length - 1; i++ )
			{
				int gap = values[i + 1] - values[i];
				if ( gap > biggestGap )
				{
					biggestGap = gap;
					biggestGapIndex = i;
				}
			}

			if ( biggestGap == 1 )
			{
				shortRate += (int)Math.Floor((mediumRate - shortRate) / 2.1f);
			}
			else
			{
				shortRate = values[biggestGapIndex];
			}
		}

		currentSequenceCount = 1;
		currentBit = '1';
		StringBuilder morseCode = new StringBuilder();

		for (int i = 1; i <= bits.Length; i++)
		{
			bool incrementedCount = false;
			if (i < bits.Length)
			{
				incrementedCount = bits[i] == currentBit;
			}
			if (incrementedCount)
			{
				++currentSequenceCount;
			}

			if (!incrementedCount)
			{
				if (currentSequenceCount <= shortRate)
				{
					if (currentBit == '1')
					{
						morseCode.Append(".");
					}
				}
				else if (currentSequenceCount <= mediumRate)
				{
					if (currentBit == '1')
					{
						morseCode.Append("-");
					}
					else
					{
						morseCode.Append(" ");
					}
				}
				else
				{
					morseCode.Append("   ");
				}

				if (i < bits.Length)
				{
					currentBit = bits[i];
					currentSequenceCount = 1;
				}
			}
		}

		try
		{
			decodeMorse( morseCode.ToString() );
		}
		catch ( Exception e )
		{
			throw new Exception(
				$"\nInput:   {bits}\n" +
				$"Morse:   {morseCode}\n" +
				$"Decoded: {e}\n" +
				$"Short Range: {shortRate}\n" +
				$"Medium Range: {mediumRate}\n" +
				$"Values: {string.Join( "\n", rates.Distinct().OrderBy( x => x ).Select( x => $"{x}: {rates.Count(y => y == x)}" ))}");
		}

		return morseCode.ToString();
	}

	public static string decodeMorse(string morseCode)
	{
		if (string.IsNullOrWhiteSpace(morseCode))
		{
			return "";
		}
		return string.Join(' ', morseCode.Trim().Split("   ").Select(x => string.Join("", x.Split(' ').Select(c => Preloaded.MORSE_CODE[c]))));
	}
}

/// <summary>
/// 4 kyu Part 2:
/// https://www.codewars.com/kata/54b72c16cd7f5154e9000457
/// </summary>
public partial class MorseCodeDecoder
{
	public static string DecodeBits(string bits)
	{
		bits = bits.Trim('0');
		int rate = int.MaxValue;

		int currentSequenceCount = 1;
		char currentBit = '1';
		for (int i = 1; i < bits.Length; i++)
		{
			if (bits[i] == currentBit)
			{
				++currentSequenceCount;
			}
			else
			{
				if (rate > currentSequenceCount)
				{
					rate = currentSequenceCount;
				}

				currentBit = bits[i];
				currentSequenceCount = 1;
			}
		}

		if (rate < currentSequenceCount)
		{
			rate = currentSequenceCount;
		}

		return bits.Replace("".PadLeft(rate * 7, '0'), "   ")
			.Replace("".PadLeft(rate * 3, '0'), " ")
			.Replace("".PadLeft(rate * 3, '1'), "-")
			.Replace("".PadLeft(rate, '1'), ".")
			.Replace("0", "");
	}

	public static string DecodeMorse(string morseCode)
	{
		return string.Join(' ', morseCode.Trim().Split("   ").Select(x => string.Join("", x.Split(' ').Select(MorseCode.Get))));
	}
}

/// <summary>
/// 6 kyu Part 1:
/// https://www.codewars.com/kata/54b724efac3d5402db00065e/train/csharp
/// </summary>
public partial class MorseCodeDecoder
{
	public static string Decode(string morseCode)
	{
		return string.Join(' ', morseCode.Trim().Split("   ").Select(x => string.Join("", x.Split(' ').Select(MorseCode.Get))));
	}
}

public static class Preloaded
{
	public static Dictionary<string, string> MORSE_CODE = new Dictionary<string, string>();
}

public static class MorseCode
{
	public static string Get(string _morse)
	{
		return null;
	}
}