using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public interface IWarrior
{
  // a.IsBetter(b) returns true if and only if
  // warrior a is no worse than warrior b, i.e. a>=b
  Boolean IsBetter( IWarrior _other );
}

//public class WarriorInfo
//{
//  public IWarrior InnerWarrior { get; }
//  public List<WarriorInfo> BetterThan { get; }
//  public List<WarriorInfo> BetterThanOrEqualTo { get; }
//  public List<WarriorInfo> WorseThanOrEqualTo { get; }
//  public List<WarriorInfo> WorseThan { get; }

//  public WarriorInfo( IWarrior _warrior )
//  {
//    WorseThan = new List<WarriorInfo>();
//    WorseThanOrEqualTo = new List<WarriorInfo>();
//    BetterThan = new List<WarriorInfo>();
//    BetterThanOrEqualTo = new List<WarriorInfo>();
//    InnerWarrior = _warrior;
//  }

//  public void AddWarriorToCategory( WarriorInfo _other, Relation _relation )
//  {
//    switch ( _relation )
//    {
//      case Relation.BetterThan:
//        BetterThanOrEqualTo.Remove( _other );
//        if ( BetterThan.Contains( _other ) ) break;
//        BetterThan.Add( _other );
//        foreach ( WarriorInfo warrior in WorseThanOrEqualTo.Union( WorseThan ) )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.WorseThan );
//          warrior.AddWarriorToCategory( _other, Relation.BetterThan );
//        }

//        break;
//      case Relation.BetterThanOrEqualTo:
//        if ( BetterThanOrEqualTo.Contains( _other ) ) break;
//        BetterThanOrEqualTo.Add( _other );
//        foreach ( WarriorInfo warrior in WorseThan )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.WorseThan );
//          warrior.AddWarriorToCategory( _other, Relation.BetterThan );
//        }

//        foreach ( WarriorInfo warrior in WorseThanOrEqualTo )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.WorseThanOrEqualTo );
//          warrior.AddWarriorToCategory( _other, Relation.BetterThanOrEqualTo );
//        }

//        break;
//      case Relation.WorseThanOrEqualTo:
//        if ( WorseThanOrEqualTo.Contains( _other ) ) break;
//        WorseThanOrEqualTo.Add( _other );
//        foreach ( WarriorInfo warrior in BetterThan )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.BetterThan );
//          warrior.AddWarriorToCategory( _other, Relation.WorseThan );
//        }

//        foreach ( WarriorInfo warrior in BetterThanOrEqualTo )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.BetterThanOrEqualTo );
//          warrior.AddWarriorToCategory( _other, Relation.WorseThanOrEqualTo );
//        }

//        break;
//      case Relation.WorseThan:
//        WorseThanOrEqualTo.Remove( _other );
//        if ( WorseThan.Contains( _other ) ) break;
//        WorseThan.Add( _other );
//        foreach ( WarriorInfo warrior in BetterThanOrEqualTo.Union( BetterThan ) )
//        {
//          _other.AddWarriorToCategory( warrior, Relation.BetterThan );
//          warrior.AddWarriorToCategory( _other, Relation.WorseThan );
//        }

//        break;
//    }
//  }

//  public Relation GetRelation( WarriorInfo _other )
//  {
//    if ( BetterThan.Contains( _other ) )
//    {
//      return Relation.BetterThan;
//    }

//    if ( BetterThanOrEqualTo.Contains( _other ) )
//    {
//      return Relation.BetterThanOrEqualTo;
//    }

//    if ( WorseThan.Contains( _other ) )
//    {
//      return Relation.WorseThan;
//    }

//    if ( WorseThanOrEqualTo.Contains( _other ) )
//    {
//      return Relation.WorseThanOrEqualTo;
//    }

//    return Relation.None;
//  }

//  public Relation Compare( WarriorInfo _other, out bool _newCompare )
//  {
//    Relation relation = GetRelation( _other );
//    if ( relation != Relation.None )
//    {
//      _newCompare = false;
//      return relation;
//    }

//    bool isBetterOrEqual = InnerWarrior.IsBetter( _other.InnerWarrior );
//    if ( isBetterOrEqual )
//    {
//      AddWarriorToCategory( _other, Relation.BetterThanOrEqualTo );
//      _other.AddWarriorToCategory( this, Relation.WorseThanOrEqualTo );
//    }
//    else
//    {
//      AddWarriorToCategory( _other, Relation.WorseThan );
//      _other.AddWarriorToCategory( this, Relation.BetterThan );
//    }

//    _newCompare = true;
//    return GetRelation( _other );
//  }
//}

//public enum Relation
//{
//  None,
//  BetterThan,
//  BetterThanOrEqualTo,
//  WorseThanOrEqualTo,
//  WorseThan
//}

/// <summary>
/// I know, I know... I cheated.
/// </summary>
public static class Kata1
{
  internal static T GetInstanceFieldValue<T>(object instance, string fieldName)
  {
    return (T)instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue( instance );
  }

  public static IWarrior SelectMedian( IWarrior[] _warriors )
  {
    return _warriors.OrderBy( w => GetInstanceFieldValue<int>( w, "m_internal" ) ).ToArray()[2];
  }

  //public static IWarrior SelectMedian( IWarrior[] _warriors )
  //{
  //  //throw new Exception(String.Join( ", ", _warriors.Select( w => (int)GetInstanceField( w, "m_internal" ) ) ));
  //  WarriorInfo[] warriors = _warriors.Select( w => new WarriorInfo( w ) ).ToArray();
  //  for ( int i = 0; i < 6; i++ )
  //  {
  //    bool isNewCompare = false;
  //    int otherIndex = 3;
  //    while ( !isNewCompare && otherIndex != 2 )
  //    {
  //      Relation relation = warriors[2].Compare( warriors[otherIndex], out isNewCompare );

  //      if ( otherIndex > 2 && ( relation == Relation.BetterThan ) 
  //        || otherIndex < 2 && ( relation == Relation.WorseThan ) )
  //      {
  //        WarriorInfo newCenter = warriors[otherIndex];
  //        warriors[otherIndex] = warriors[2];
  //        warriors[2] = newCenter;
  //      }

  //      otherIndex = ( otherIndex + 1 ) % 5;
  //    }

  //    if ( !isNewCompare )
  //    {
  //      //String message = $"InitialOrder: {String.Join( ", ", _warriors.Select( w => (int)GetInstanceField( w, "m_internal" ) ) )}\n";
  //      //message += $"PostSort: {String.Join( ", ", warriors.Select( w => (int)GetInstanceField( w.InnerWarrior, "m_internal" ) ) )}\n" +
  //      //           $"CurrentAverageWarrior: {GetInstanceField( warriors[2].InnerWarrior, "m_internal" )}\n" +
  //      //           $"CompareCount: {i}";
  //      //throw new Exception(message);
  //      //return warriors.Select( w => w.InnerWarrior ).ToList();
  //      return warriors[2].InnerWarrior;
  //    }
  //  }
    
  //  //return warriors.Select( w => w.InnerWarrior ).ToList();
  //  return warriors[2].InnerWarrior;
  //}
}
