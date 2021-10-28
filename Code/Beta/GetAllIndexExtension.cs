using System.Collections.Generic;

namespace Beta
{
	public static class GetAllIndexExtension
	{
		public static int[] GetAllIndex<T>( this List<T> data, T item )
		{
			List<int> indices = new List<int>();
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i].Equals( item ))
				{
					indices.Add( i );
				}
			}

			return indices.ToArray();
		}
	}
}
