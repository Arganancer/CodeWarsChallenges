using System.Numerics;

/// <summary>
/// https://www.codewars.com/kata/559a28007caad2ac4e000083/train/csharp
/// </summary>
class PerimeterOfSquares
{
	public static BigInteger Perimeter( BigInteger n )
	{
		BigInteger result = 1;
		BigInteger lastLastNumber = BigInteger.Zero;
		BigInteger lastNumber = 1;

		for (int i = 0; i < n; i++)
		{
			BigInteger current = lastNumber + lastLastNumber;
			result += current;
			lastLastNumber = lastNumber;
			lastNumber = current;
		}

		return result * 4;
	}
}
