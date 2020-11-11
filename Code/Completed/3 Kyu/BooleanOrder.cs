using System.Linq;
using System.Numerics;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/59eb1e4a0863c7ff7e000008/train/csharp
	/// </summary>
	public class BooleanOrder
	{
		private readonly bool[] _operands;
		private readonly char[] _operators;
		private readonly BigInteger[,] _cacheTrue;
		private readonly BigInteger[,] _cacheFalse;

		public BooleanOrder(string operands, string operators)
		{
			_operands = operands.Select(operand => operand == 't').ToArray();
			_operators = operators.ToCharArray();
			_cacheTrue = new BigInteger[operands.Length, operands.Length];
			_cacheFalse = new BigInteger[operands.Length, operands.Length];
		}

		public BigInteger Solve()
		{
			for (int s = 0; s < _operands.Length; ++s)
			{
				for (int i = 0; i < _operands.Length - s; ++i)
				{
					int j = s + i;
					(BigInteger t, BigInteger f) = CalculateTrueFalse(i, j);
					_cacheTrue[i, j] = t;
					_cacheFalse[i, j] = f;
				}
			}

			return _cacheTrue[0, _operands.Length - 1];
		}

		private (BigInteger t, BigInteger f) CalculateTrueFalse(int i, int j)
		{
			if (i == j)
			{
				return (_operands[i] ? BigInteger.One : BigInteger.Zero, _operands[i] ? BigInteger.Zero : BigInteger.One);
			}

			(BigInteger t, BigInteger f) output = (BigInteger.Zero, BigInteger.Zero);

			for (int k = i; k < j; k++)
			{
				int leftI = i;
				int leftJ = k;
				int rightI = k + 1;
				int rightJ = j;

				BigInteger leftTrue = _cacheTrue[leftI, leftJ];
				BigInteger leftFalse = _cacheFalse[leftI, leftJ];
				BigInteger rightTrue = _cacheTrue[rightI, rightJ];
				BigInteger rightFalse = _cacheFalse[rightI, rightJ];

				char op = _operators[k];
				switch (op)
				{
					case '&':
						output.t += leftTrue * rightTrue;
						output.f += leftFalse * rightFalse + leftFalse * rightTrue + leftTrue * rightFalse;
						break;
					case '|':
						output.t += leftTrue * rightTrue + leftFalse * rightTrue + leftTrue * rightFalse;
						output.f += leftFalse * rightFalse;
						break;
					default:
						output.t += leftTrue * rightFalse + leftFalse * rightTrue;
						output.f += leftTrue * rightTrue + leftFalse * rightFalse;
						break;
				}
			}

			return output;
		}
	}

	public static class BooleanOrderTests
	{
		public static void RunTests()
		{
			Logger.Log(new BigInteger(2), new BooleanOrder("tft", "^&").Solve(), "\"tft\", \" ^ &\"");
			Logger.Log(new BigInteger(16), new BooleanOrder("ttftff", "|&^&&").Solve(), "");
			Logger.Log(new BigInteger(339), new BooleanOrder("ttftfftf", "|&^&&||").Solve(), "");
			Logger.Log(new BigInteger(851), new BooleanOrder("ttftfftft", "|&^&&||^").Solve(), "");
			Logger.Log(new BigInteger(2434), new BooleanOrder("ttftfftftf", "|&^&&||^&").Solve(), "");
			Logger.Log(new BigInteger(945766470799), new BooleanOrder("ttftfftftffttfftftftfftft", "|&^&&||^&&^^|&&||^&&||&^").Solve(), "");
		}
	}
}
