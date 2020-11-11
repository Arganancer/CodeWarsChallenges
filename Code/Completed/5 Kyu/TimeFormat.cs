namespace Codewars
{
	public static class TimeFormat
	{
		/// <summary>
		/// https://www.codewars.com/kata/52685f7382004e774f0001f7/train/csharp
		/// </summary>
		public static string GetReadableTime(int seconds)
		{
			var secs = seconds % 60;
			var minutes = ((seconds - secs) / 60) % 60;
			var hours = seconds / 3600;
			return $"{hours:D2}:{minutes:D2}:{secs:D2}";
		}
	}
}
