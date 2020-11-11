using System;
using System.Numerics;

/// <summary>
/// https://www.codewars.com/kata/5324945e2ece5e1f32000370/train/csharp
/// </summary>
public class SumStringsAsNumbers
{
  public static String sumStrings( String a, String b )
  {
    return $"{( BigInteger.TryParse( a, out BigInteger i ) ? i : 0 ) + ( BigInteger.TryParse( b, out BigInteger j ) ? j : 0 )}";
  }
}