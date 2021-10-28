using System.Text;

public class AddingBigNumbers
{
	public static string Add( string a, string b )
	{
		StringBuilder output = new StringBuilder( "" );
		int carry = 0;

		string biggerValue = a.Length >= b.Length ? a : b;
		string smallerValue = a.Length >= b.Length ? b : a;
		int smallerValueLengthDifference = biggerValue.Length - smallerValue.Length;
		for (int i = biggerValue.Length - 1; i >= 0; i--)
		{
			int currentValue = carry + int.Parse( biggerValue[i].ToString() );
			int smallerValueIndex = i - smallerValueLengthDifference;
			if (smallerValueIndex >= 0)
			{
				currentValue += int.Parse( smallerValue[smallerValueIndex].ToString() );
			}

			carry = currentValue / 10;
			output.Insert( 0, currentValue % 10 );
		}

		if (carry > 0)
		{
			output.Insert( 0, carry );
		}

		return output.ToString();
	}
}
