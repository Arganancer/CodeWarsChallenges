using System;
using System.Linq;
using System.Numerics;

public class BitCount
{
	/// <summary>
	/// https://www.codewars.com/kata/596d34df24a04ee1e3000a25
	/// Really difficult.
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <returns></returns>
	public static BigInteger CountOnes( long left, long right )
	{
		string leftBin = Convert.ToString( left, 2 );
		string rightBin = Convert.ToString( right, 2 );

		Console.WriteLine( $"Left Bin: {leftBin}\nRight Bin: {rightBin}" );
		BigInteger leftSum = CountOnesUpToBinaryString( leftBin );
		BigInteger rightSum = CountOnesUpToBinaryString( rightBin );

		Console.WriteLine( $"Left Sum: {leftSum}\nRight Sum: {rightSum}" );
		Console.WriteLine( $"Result = {rightSum - leftSum}" );

		return rightSum - leftSum;
	}


	private static BigInteger CountOnesUpToBinaryString( string binaryString )
	{
		BigInteger onesInBinaryLength = CountOnesInBinaryLength( binaryString.Length - 1 );
		Console.Write( $"  Adding: {binaryString} - OnesInBinaryLength: {onesInBinaryLength} - " );
		if (binaryString.Length > 1 && binaryString[1] == '1')
		{
			onesInBinaryLength *= 2;
		}

		if (binaryString[0] == '1')
		{
			onesInBinaryLength += 1;
		}

		BigInteger output = onesInBinaryLength;

		Console.WriteLine( $"Value: {output}" );

		binaryString = binaryString.Substring( 1, binaryString.Length - 1 );
		if (binaryString.Length > 0)
		{
			output += CountOnesUpToBinaryString( binaryString );
		}

		return output;
	}

	private static BigInteger CountOnesUpToBinaryString2( string binaryString )
	{
		int binaryStringLength = binaryString.Length;
		BigInteger output = 0;
		for (int i = 1; i <= binaryStringLength; i++)
		{
			output += CountOnesInBinaryLength( i );
		}

		for (int i = 1; i < binaryStringLength; i++)
		{
		}

		if (binaryString.Length > 1 && binaryString[1] == '1')
		{
			output *= 2;
		}

		if (binaryString[0] == '1')
		{
			output += 1;
		}

		return output;
	}

	/// <summary>
	/// Was messing around and found a pretty weird formula for counting how many 1s appear between
	/// the inclusive start and end of a binary length range (ex. between 1000 and 1111, or 100000 and 111111).
	/// </summary>
	private static BigInteger CountOnesInBinaryLength( BigInteger binaryLength )
	{
		if (binaryLength <= 0)
		{
			return 0;
		}

		BigInteger output = binaryLength + 1; // stripping out the first and last counts in the range to avoid division by zero.

		BigInteger lastValue = 1;
		for (BigInteger i = 1; i < binaryLength - 1; i++)
		{
			lastValue = (binaryLength - i) / i * lastValue;
			output += lastValue * (i + 1);
		}

		return output;
	}

	public static BigInteger BruteForceCountOnes( long left, long right )
	{
		BigInteger onesCount = 0;
		for (long i = left; i <= right; i++)
		{
			onesCount += Convert.ToString( i, 2 ).Count( _x => _x == '1' );
		}

		Console.WriteLine( $"Brute Force Ones Count: {onesCount}" );
		return onesCount;
	}
}
