using System;
using System.Numerics;

/// <summary>
/// https://www.codewars.com/kata/53d40c1e2f13e331fc000c26/train/csharp
/// </summary>
public class Fibonacci
{
	public static BigInteger fib( int _n )
	{
		if (_n == 0) return 0;

		BigInteger[,] f = { { 1, 1 }, { 1, 0 } };
		Power( f, Math.Abs( _n ) - 1 );
		return f[0, 0] * (_n < 0 && _n % 2 == 0 ? -1 : 1);
	}

	private static void Power( BigInteger[,] _f, int _n )
	{
		if (_n == 0 || _n == 1) return;

		BigInteger[,] m = { { 1, 1 }, { 1, 0 } };
		Power( _f, _n / 2 );
		Multiply( _f, _f );

		if (_n % 2 != 0) Multiply( _f, m );
	}

	private static void Multiply( BigInteger[,] _f, BigInteger[,] _m )
	{
		BigInteger x = (_f[0, 0] * _m[0, 0]) + (_f[0, 1] * _m[1, 0]);
		BigInteger y = (_f[0, 0] * _m[0, 1]) + (_f[0, 1] * _m[1, 1]);
		BigInteger z = (_f[1, 0] * _m[0, 0]) + (_f[1, 1] * _m[1, 0]);
		BigInteger w = (_f[1, 0] * _m[0, 1]) + (_f[1, 1] * _m[1, 1]);

		_f[0, 0] = x;
		_f[0, 1] = y;
		_f[1, 0] = z;
		_f[1, 1] = w;
	}
}
