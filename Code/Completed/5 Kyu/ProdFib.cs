public class ProdFib
{
	/// <summary>
	/// https://www.codewars.com/kata/5541f58a944b85ce6d00006a/solutions/csharp
	/// </summary>
	public static ulong[] productFib( ulong _prod )
	{
		ulong previous = 0;
		ulong current = 1;
		while ( previous * current < _prod )
		{
			ulong next = previous + current;
			previous = current;
			current = next;
		}

		return new[] { previous, current, previous * current == _prod ? 1UL : 0UL };
	}
}