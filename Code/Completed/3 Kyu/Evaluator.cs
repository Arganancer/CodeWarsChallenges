using System;
using System.Collections.Generic;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/5235c913397cbf2508000048/train/csharp
	/// </summary>

	/// <summary>
	/// Using dataTable.Compute(expression); felt like cheating, soooo...
	/// </summary>
	public class Evaluator
	{
		public double Evaluate(string expression)
		{
			Expression result = new Expression(expression);

			return result.Resolve();
		}
	}

	public class Expression
	{
		private readonly LinkedList<Expression> _subExpressions = new LinkedList<Expression>();

		public Expression() { }

		public Expression(string expression)
		{
			expression = expression.Replace(" ", "");

			string number = "";
			for (int i = 0; i < expression.Length; i++)
			{
				char currentCharacter = expression[i];
				if (char.IsNumber(currentCharacter) || currentCharacter == '.')
				{
					number += currentCharacter;
				}
				else if (currentCharacter == '-' && (i == 0 || _subExpressions.Last?.Value is Operator && number.Length == 0))
				{
					_subExpressions.AddLast(new Term(-1));
					_subExpressions.AddLast(new Operator('*', 2));
				}
				else
				{
					if (number.Length > 0)
					{
						_subExpressions.AddLast(new Term(number));
						number = "";
					}

					if (currentCharacter == '(')
					{
						int openParentheses = 1;
						int closedParentheses = 0;
						for (int j = i + 1; j < expression.Length; j++)
						{
							if (expression[j] == '(')
							{
								++openParentheses;
							}
							else if (expression[j] == ')')
							{
								++closedParentheses;
								if (closedParentheses == openParentheses)
								{
									_subExpressions.AddLast(new Expression(expression.Substring(i + 1, j - i - 1)));
									i = j;
									break;
								}
							}
						}
					}
					else
					{
						_subExpressions.AddLast(new Operator(currentCharacter));
					}
				}
			}

			if (number.Length <= 0) return;
			_subExpressions.AddLast(new Term(number));
		}

		public virtual double Resolve()
		{
			ApplyOperatorsWithPriority(2);
			ApplyOperatorsWithPriority(1);
			ApplyOperatorsWithPriority(0);

			return _subExpressions.First?.Value.Resolve() ?? throw new Exception();

			void ApplyOperatorsWithPriority(int priority)
			{
				LinkedListNode<Expression> currentNode = _subExpressions.First;
				while (currentNode != null)
				{
					if (currentNode.Value is Operator op && op.Priority == priority)
					{
						LinkedListNode<Expression> previousNode = currentNode.Previous;
						LinkedListNode<Expression> nextNode = currentNode.Next;
						currentNode.Value = new Term(op.Apply(previousNode?.Value, nextNode?.Value));
						_subExpressions.Remove(previousNode!);
						_subExpressions.Remove(nextNode!);
					}

					currentNode = currentNode.Next;
				}
			}
		}
	}

	public class Term : Expression
	{
		private readonly double _value;

		public Term(double value)
		{
			_value = value;
		}

		public Term(string expression)
		{
			_value = double.Parse(expression);
		}

		public override double Resolve()
		{
			return _value;
		}
	}

	public class Operator : Expression
	{
		private readonly char _op;

		public int Priority { get; }

		public Operator(char op, int priority = -1)
		{
			_op = op;
			if (priority == -1)
			{
				Priority = _op == '+' || _op == '-' ? 0 : 1;
			}
			else
			{
				Priority = priority;
			}
		}

		public double Apply(Expression a, Expression b)
		{
			switch (_op)
			{
				case '+': return a.Resolve() + b.Resolve();
				case '-': return a.Resolve() - b.Resolve();
				case '*': return a.Resolve() * b.Resolve();
				case '/': return a.Resolve() / b.Resolve();
				default: throw new Exception($"'{_op}' is not a valid operator.");
			}
		}
	}
}
