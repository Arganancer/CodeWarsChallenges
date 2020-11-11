using System;
using System.Diagnostics;
using System.Linq;

namespace Codewars
{
  class Program
  {
    public class Warrior : IWarrior
    {
      public int m_internal;

      public static int CompareCount { get; private set; }

      public Warrior( int rank = 0 )
      {
        m_internal = rank;
      }

      public bool IsBetter( IWarrior other )
      {
        ++CompareCount;
        if ( other == null || !( other is Warrior ) ) return false;
        return m_internal >= ( other as Warrior ).m_internal;
      }

      public static void ResetCompareCount()
      {
        CompareCount = 0;
      }
    }

    static void Main( string[] args )
    {
      Console.SetWindowSize( 200, 60 );

      IWarrior[] input = {
          new Warrior(1),
          new Warrior(4),
          new Warrior(5),
          new Warrior(3),
          new Warrior(2),
        };
      Warrior.ResetCompareCount();
      Console.WriteLine( Kata.SelectMedian( input ) );

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine( "++ All tests executed." );
      Console.Read();
    }
  }
}
