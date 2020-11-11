using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Codewars;

public class Transpiler
{
	public static string transpile(string expression)
	{
		int firstBraceOrBracketIndex = expression.IndexOfAny(new[] {'(', '{'});
		if (firstBraceOrBracketIndex == -1)
		{
			return "";
		}

		StringBuilder output = new StringBuilder();
		string functionName = firstBraceOrBracketIndex > 0 ? expression.Substring(0, firstBraceOrBracketIndex).RemoveWhitespace() : "";
		if (!IsNameValid(functionName))
		{
			return "";
		}

		int openingBraceCount = 0;
		output.Append(functionName);
		bool combineLamdaWithArguments = functionName.Length > 0;

		List<string> arguments = new List<string>();

		for (int i = firstBraceOrBracketIndex; i < expression.Length; i++)
		{
			char c = expression[i];
			if (c == '{')
			{
				if (++openingBraceCount > 2)
				{
					return "";
				}
				int nextClosingBrace = expression.IndexOf('}', i + 1);
				if (nextClosingBrace == -1 || !ValidateAndTranspileLambda(expression.Substring(i + 1, nextClosingBrace - i - 1), out string lambda))
				{
					return "";
				}

				i = nextClosingBrace;

				if (arguments.Any() || combineLamdaWithArguments)
				{
					arguments.Add(lambda);
				}
				else
				{
					output.Append(lambda);
				}

				combineLamdaWithArguments = false;
			}
			else if (c == '}')
			{
				return "";
			}
			else if (c == '(')
			{
				int commas = 0;
				if (arguments.Any())
				{
					output.Append($"({string.Join(',', arguments)})");
					arguments.Clear();
				}

				int parenBalance = 1;
				StringBuilder argument = new StringBuilder();
				for (int j = i + 1; j < expression.Length; j++)
				{
					char d = expression[j];
					if (d == '(')
					{
						++parenBalance;
					}
					else if (d == ')' && --parenBalance == 0)
					{
						i = j;
						break;
					}
					else if (d == ',')
					{
						++commas;

						if (argument.Length <= 0) continue;
						string trimmedArgument = argument.ToString().Trim();
						int trimmedArgumentLength = trimmedArgument.Length;
						string argName = trimmedArgument.RemoveWhitespace();
						if (trimmedArgumentLength != argName.Length)
						{
							return "";
						}

						if (!IsNameValid(argName))
						{
							return "";
						}

						arguments.Add(argName);
						argument = new StringBuilder();
					}
					else if (d == '{')
					{
						int nextClosingBrace = expression.IndexOf('}', j + 1);
						if (nextClosingBrace == -1 || !ValidateAndTranspileLambda(expression.Substring(j + 1, nextClosingBrace - j - 1), out string lambda))
						{
							return "";
						}

						j = nextClosingBrace;
						
						arguments.Add(lambda);
					}
					else if (d == '}')
					{
						return "";
					}
					else
					{
						argument.Append(d);
					}
				}

				if (argument.Length > 0)
				{
					string trimmedArgument = argument.ToString().Trim();
					int trimmedArgumentLength = trimmedArgument.Length;
					string argName = trimmedArgument.RemoveWhitespace();
					if (trimmedArgumentLength != argName.Length)
					{
						return "";
					}
					if (!IsNameValid(argName))
					{
						return "";
					}

					if (!string.IsNullOrWhiteSpace(argName))
					{
						arguments.Add(argName);
					}
				}

				if (parenBalance != 0 || commas > 0 && arguments.Count != commas + 1)
				{
					return "";
				}

				combineLamdaWithArguments = true;
			}
			else if (c == ')')
			{
				return "";
			}
			else if (!char.IsWhiteSpace(c))
			{
				return "";
			}
		}

		if (functionName.Length == 0 && openingBraceCount == 0)
		{
			return "";
		}

		if (arguments.Any() || combineLamdaWithArguments)
		{
			output.Append($"({string.Join(',', arguments)})");
		}

		return output.ToString();
	}

	private static bool ValidateAndTranspileLambda(string expression, out string lambda)
	{
		lambda = "";
		string[] leftRight = string.IsNullOrWhiteSpace(expression) ? new string[0] : new[] { expression };
		bool containsArrow = expression.Contains("->");
		if (containsArrow)
		{
			leftRight = expression.Split("->", StringSplitOptions.RemoveEmptyEntries);
		}

		string left = "()";

		if (containsArrow && expression.Trim()[0] == '-')
		{
			return false;
		}

		if (containsArrow)
		{
			string[] leftSplit = leftRight[0].Split(',').Select(leftArg => leftArg.Trim()).ToArray();
			foreach (string leftArg in leftSplit)
			{
				int count = leftArg.Length;
				if (count != leftArg.RemoveWhitespace().Length || !IsNameValid(leftArg))
				{
					return false;
				}
			}

			left = $"({string.Join(',', leftSplit)})";
		}

		string right = "{}";
		if (!containsArrow && leftRight.Length > 0 || leftRight.Length > 1)
		{
			string[] rightSplit = leftRight[containsArrow ? 1 : 0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			if (rightSplit.Any(name => !IsNameValid(name)))
			{
				return false;
			}
			right = $"{{{string.Join(";", rightSplit)};}}";
		}

		lambda = $"{left}{right}";
		return true;
	}

	private static bool IsNameValid(string name)
	{
		bool encounteredNumeric = false;
		if(name == "_")
		{
			return true;
		}
		foreach (char c in name)
		{
			if (char.IsNumber(c))
			{
				encounteredNumeric = true;
			}
			else if (char.IsLetter(c))
			{
				if (encounteredNumeric)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		return true;
	}
}

public static class StringExtensions
{
	public static string RemoveWhitespace(this string input)
	{
		return Regex.Replace(input, @"\s+", "");
	}
}

public static class ExpressionTranspilerTests
{
	public static void RunTests()
	{
		Logger.Log("f((a,b){})", Transpiler.transpile("f(){a,b->}"), "f(){a,b->}");
		Logger.Log("f((a,b){a;})", Transpiler.transpile("f({a, b->a})"), "f({a, b->a})");
		Logger.Log("invoke((){},(){})", Transpiler.transpile("invoke({},{})"), "invoke({},{})");
		Logger.Log("", Transpiler.transpile("f( a v)"), "f( a v)");
		Logger.Log("call((){})", Transpiler.transpile("call(\n){}"), "call(\n){}");
		Logger.Log("", Transpiler.transpile("{a->a}(cde,y,z){x y d -> stuff}  "), "{a->a}(cde,y,z){x y d -> stuff}  ");
		Logger.Log("(a){a;}(cde,y,z,(x,y,d){stuff;})", Transpiler.transpile("{a->a}(cde,y,z){x,y,d -> stuff}"), "{a->a}(cde,y,z){x,y,d -> stuff}");
		Logger.Log("", Transpiler.transpile("(12,ab)"), "(12,ab)");
		Logger.Log("", Transpiler.transpile("{}{}{}"), "{}{}{}");
		Logger.Log("", Transpiler.transpile("f(a,)"), "f(a,)");
		Logger.Log("", Transpiler.transpile("f(){->a}"), "f(){->a}");
		Logger.Log("f((_){})", Transpiler.transpile("f({_->})"), "f({_->})");

		Logger.Log("run((){a;})", Transpiler.transpile("run{a}"), "run{a}");
		Logger.Log("f((a){})", Transpiler.transpile("f({a->})"), "f({a->})");
		Logger.Log("f((a){a;})", Transpiler.transpile("f({a->a})"), "f({a->a})");

		Logger.Log("call((){})", Transpiler.transpile("call({})"), "call({})");
		Logger.Log("(){}()", Transpiler.transpile("{}()"), "{}()");
		Logger.Log("(a){a;}(233)", Transpiler.transpile("{a->a}(233)"), "{a->a}(233)");

		Logger.Log("", Transpiler.transpile(""), "");
		Logger.Log("1()", Transpiler.transpile("1()"), "1()");
		Logger.Log("123()", Transpiler.transpile("123()"), "123()");
		Logger.Log("a()", Transpiler.transpile("a()"), "a()");
		Logger.Log("abc()", Transpiler.transpile("abc()"), "abc()");

		Logger.Log("call()", Transpiler.transpile("call()"), "call()");
		Logger.Log("", Transpiler.transpile("%^&*("), "%^&*(");
		Logger.Log("", Transpiler.transpile("x9x92xb29xub29bx120()!("), "x9x92xb29xub29bx120()!(");
		Logger.Log("invoke(a,b)", Transpiler.transpile("invoke  (       a    ,   b   )"), "invoke  (       a    ,   b   )");
		Logger.Log("invoke(a,b)", Transpiler.transpile("invoke(a, b)"), "invoke(a, b)");

		Logger.Log("invoke(a,b,(){})", Transpiler.transpile("invoke  (       a    ,   b   ) { } "), "invoke  (       a    ,   b   ) { } ");
		Logger.Log("f(x,(a){})", Transpiler.transpile("f(x){a->}"), "f(x){a->}");
		Logger.Log("f(a,b,(a){a;})", Transpiler.transpile("f(a,b){a->a}"), "f(a,b){a->a}");
	}
}