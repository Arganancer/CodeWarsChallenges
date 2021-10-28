using System.Collections.Generic;
using System.Linq;

public class ShoppingCalculations
{
	public static List<(string, string, string)> ShoppingCalculation( List<string> _input )
	{
		Dictionary<string, (int money, string boughtProducts)> customers = new Dictionary<string, (int money, string boughtProducts)>();
		Dictionary<string, int> products = new Dictionary<string, int>();

		for (int i = 0; i < _input.Count; i++)
		{
			string[] splitInput = _input[i].Split( " " );
			switch (splitInput[1])
			{
				case "is":
					products.Add( splitInput[0], int.Parse( string.Concat( splitInput[2].Where( char.IsDigit ) ) ) );
					break;
				case "has":
					customers.Add( splitInput[0], (int.Parse( string.Concat( splitInput[2].Where( char.IsDigit ) ) ), "") );
					break;
				case "buys":
					// Instructions did not specify that inputs will always be ordered, although the tests seemed to indicate that they are.
					// I decided to handle them being unordered just in case, but it should have been specified in the instructions.
					string product = splitInput[3].TrimEnd( 's', '.' );
					product = $"{char.ToUpper( product[0] )}{product.Substring( 1 )}";
					if (!customers.ContainsKey( splitInput[0] ) || !products.ContainsKey( product ))
					{
						string input = _input[i];
						_input.RemoveAt( i );
						_input.Add( input );
					}
					else
					{
						(int money, string boughtProducts) = customers[splitInput[0]];
						money -= products[product] * int.Parse( splitInput[2] );
						boughtProducts += $"{(string.IsNullOrEmpty( boughtProducts ) ? "" : ", ")}{splitInput[2]} {splitInput[3].TrimEnd( '.' )}";
						customers[splitInput[0]] = (money, boughtProducts);
					}

					break;
			}
		}

		return customers.Select( c => (c.Key, $"{c.Value.money}$", c.Value.boughtProducts) ).ToList();
	}
}
