namespace Beta
{
	public class SpeedOfLetters
	{
		public static string Speedify( string _input )
		{
			char[] output = new char[_input.Length + 26];
			for (int i = _input.Length - 1; i >= 0; --i)
			{
				int newIndex = (i + _input[i]) - 65;
				if (output[newIndex] == '\0')
				{
					output[newIndex] = _input[i];
				}
			}

			return new string( output ).Replace( '\0', ' ' ).TrimEnd();
		}
	}
}
