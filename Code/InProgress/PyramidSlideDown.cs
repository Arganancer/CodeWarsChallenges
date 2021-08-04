public class PyramidSlideDown
{
	/// <summary>
	/// https://www.codewars.com/kata/551f23362ff852e2ab000037
	/// </summary>
	public static int LongestSlideDown( int[][] pyramid )
	{
		int sum = pyramid[0][0];
		int currentPeak = 0;

		for (int i = 1; i < pyramid.Length; i++)
		{
			int leftSum = 0;
			int rightSum = 0;
			for (int j = i; j < pyramid.Length; j++)
			{
				leftSum += pyramid[j][currentPeak];
				rightSum += pyramid[j][currentPeak + j - i + 1];
			}

			if (leftSum < rightSum)
			{
				currentPeak += 1;
			}

			sum += pyramid[i][currentPeak];
		}

		return sum;
	}



	private static void PrintPyramid()
	{

	}
}
