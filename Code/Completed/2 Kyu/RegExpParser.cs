using System.Collections.Generic;
using System.Linq;
using Codewars;

public class RegExpParser
{
	public static Reg.Exp parse(string input)
	{
		if (input.Length == 0)
		{
			return null;
		}

		List<Reg.Exp> expressions = new List<Reg.Exp>();

		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			switch (c)
			{
				case '(':
					if (i == input.Length - 1 || input[i + 1] == ')') return null;

					int parenBalance = 1;
					for (int j = i + 1; j < input.Length; j++)
					{
						if (input[j] == '(')
						{
							++parenBalance;
							continue;
						}
						if (input[j] != ')' || --parenBalance != 0) continue;

						Reg.Exp subExpression = parse(input.Substring(i + 1, j - i - 1));
						if (subExpression == null)
						{
							return null;
						}
						expressions.Add(subExpression);
						i = j;
						break;
					}

					if (parenBalance != 0) return null;

					break;
				case ')':
					return null;
				case '*':
					if (i == 0 || input[i - 1] is { } previous && (previous == '*' || previous == '(' || previous == '|')) return null;

					var zeroOrMore = Reg.zeroOrMore(expressions.Last());
					expressions.RemoveAt(expressions.Count - 1);
					expressions.Add(zeroOrMore);

					break;
				case '|':
					expressions.Add(new TmpOr());
					break;
				case '.':
					expressions.Add(Reg.any());
					break;
				default:
					expressions.Add(Reg.normal(c));
					break;
			}
		}

		bool first = true;
		for (int i = 1; i < expressions.Count; i++)
		{
			Reg.Exp current = expressions[i];
			if (current is TmpOr) continue;

			Reg.Exp previous = expressions[i - 1];
			if (!(previous is TmpOr))
			{
				Reg.Exp add = Reg.add(previous is Reg.Str str && !first ? str : Reg.str(previous), current);
				first = false;
				expressions.RemoveAt(i);
				--i;
				expressions.RemoveAt(i);
				expressions.Insert(i, add);
			}
		}

		int index = expressions.FindIndex(exp => exp is TmpOr);

		return index >= 0
			? Reg.or(index > 0 ? expressions[index - 1] : null,
				index + 1 < expressions.Count ? expressions[index + 1] : null)
			: expressions.First();
	}
}

public class TmpOr : Reg.Exp { }

public static class RegularExpressionParserTests
{
	public static void RunTests()
	{
		// Failed Random Tests
		Logger.Log("(((utq(j|d)vy(f|p)oaf.x)lkvfyig.qkb(.k*cqiiso.b.xgvp.q(m|a)(v|i)qrbm.*c)e.nynv*wy(.ye)q)|d)",
			RegExpParser.parse("(((utq(j|d)vy(f|p)oaf.x)lkvfyig.qkb(.k*cqiiso.b.xgvp.q(m|a)(v|i)qrbm.*c)e.nynv*wy(.ye)q)|d)").ToString(),
			"(((utq(j|d)vy(f|p)oaf.x)lkvfyig.qkb(.k*cqiiso.b.xgvp.q(m|a)(v|i)qrbm.*c)e.nynv*wy(.ye)q)|d)");
		Logger.Log("(btisygv((wa(c|o)m*tms*srjclf(nhip.dgef(c(dl.lbbxdganggaf.k).wadyxttxwpgw)mtwcsvax(x|j).)habuvjwn)kh*ah(j|d)yyf(ll(l*|v*)bsp*wmt)obn)((v|g)atk(s|g)eq(y|.)lgx.kyqp(j*|s*))p*hf*lhrbe*(hv.yvwq).bg(n*(aj.fnqqbirbn*id)bnnk(yymrlq*ajk(y|a*)twx((p.n)|m)a)e.*(m|v)*)sbn)*",
			RegExpParser.parse("(btisygv((wa(c|o)m*tms*srjclf(nhip.dgef(c(dl.lbbxdganggaf.k).wadyxttxwpgw)mtwcsvax(x|j).)habuvjwn)kh*ah(j|d)yyf(ll(l*|v*)bsp*wmt)obn)((v|g)atk(s|g)eq(y|.)lgx.kyqp(j*|s*))p*hf*lhrbe*(hv.yvwq).bg(n*(aj.fnqqbirbn*id)bnnk(yymrlq*ajk(y|a*)twx((p.n)|m)a)e.*(m|v)*)sbn)*").ToString(),
			"(btisygv((wa(c|o)m*tms*srjclf(nhip.dgef(c(dl.lbbxdganggaf.k).wadyxttxwpgw)mtwcsvax(x|j).)habuvjwn)kh*ah(j|d)yyf(ll(l*|v*)bsp*wmt)obn)((v|g)atk(s|g)eq(y|.)lgx.kyqp(j*|s*))p*hf*lhrbe*(hv.yvwq).bg(n*(aj.fnqqbirbn*id)bnnk(yymrlq*ajk(y|a*)twx((p.n)|m)a)e.*(m|v)*)sbn)*");
		Logger.Log("", RegExpParser.parse("((P2$-Mn=1/mt Rr#G}TZ/{`m z?t=WQ0b)|)").ToString());
		Logger.Log("(|(+MEq))", RegExpParser.parse("(|(+MEq))").ToString(), "(|(+MEq))");

		// Complex Tests
		Logger.Log("(((aa)|(ab))*|a)", RegExpParser.parse("((aa)|ab)*|a").ToString());
		Logger.Log("(((a.)|(.b))*|a)", RegExpParser.parse("((a.)|.b)*|a").ToString());

		// Basic Tests
		Logger.Log(".", RegExpParser.parse(".").ToString());
		Logger.Log("a", RegExpParser.parse("a").ToString());
		Logger.Log("(a|b)", RegExpParser.parse("a|b").ToString());
		Logger.Log("a*", RegExpParser.parse("a*").ToString());
		Logger.Log("a", RegExpParser.parse("(a)").ToString());
		Logger.Log("a*", RegExpParser.parse("(a*)").ToString());
		Logger.Log("(a|b)*", RegExpParser.parse("(a|b)*").ToString());
		Logger.Log("(a|b*)", RegExpParser.parse("a|b*").ToString());
		Logger.Log("(abcd)", RegExpParser.parse("abcd").ToString());
		Logger.Log("((ab)|(cd))", RegExpParser.parse("ab|cd").ToString());

		// Precedence Tests
		Logger.Log("(ab*)", RegExpParser.parse("ab*").ToString());
		Logger.Log("(ab)*", RegExpParser.parse("(ab)*").ToString());
		Logger.Log("((ab)|a)", RegExpParser.parse("ab|a").ToString());
		Logger.Log("(a(b|a))", RegExpParser.parse("a(b|a)").ToString());

		// Invalid Tests
		Logger.Log(null, RegExpParser.parse(""));
		Logger.Log(null, RegExpParser.parse("("));
		Logger.Log(null, RegExpParser.parse(")("));
		Logger.Log(null, RegExpParser.parse("*"));
	}
}

public class Reg
{
	public abstract class Exp
	{
	}

	public class Normal : Exp
	{
		private readonly char _c;

		public Normal(char c)
		{
			_c = c;
		}

		public override string ToString()
		{
			return _c.ToString();
		}
	}

	public class Any : Exp
	{
		public override string ToString()
		{
			return ".";
		}
	}

	public class ZeroOrMore : Exp
	{
		private readonly Exp _exp;

		public ZeroOrMore(Exp exp)
		{
			_exp = exp;
		}

		public override string ToString()
		{
			return $"{_exp}*";
		}
	}

	public class Or : Exp
	{
		private readonly Exp _left;
		private readonly Exp _right;

		public Or(Exp left, Exp right)
		{
			_left = left;
			_right = right;
		}

		public override string ToString()
		{
			return $"({_left}|{_right})";
		}
	}

	public class Str : Exp
	{
		private readonly List<Exp> _expressions;

		public Str(Exp first)
		{
			_expressions = new List<Exp> { first };
		}

		public Str Add(Exp exp)
		{
			_expressions.Add(exp);
			return this;
		}

		public override string ToString()
		{
			return $"({string.Concat(_expressions)})";
		}
	}

	/// <summary>
	/// A character that is not in "()*|."
	/// </summary>
	public static Exp normal(char c)
	{
		return new Normal(c);
	}

	/// <summary>
	/// Any character "."
	/// </summary>
	public static Exp any()
	{
		return new Any();
	}

	/// <summary>
	/// Zero or more occurrences of the same regexp "*"
	/// </summary>
	public static Exp zeroOrMore(Exp starred)
	{
		return new ZeroOrMore(starred);
	}

	/// <summary>
	/// A choice between 2 regexps
	/// </summary>
	public static Exp or(Exp left, Exp right)
	{
		return new Or(left, right);
	}

	/// <summary>
	/// A sequence of regexps, first element
	/// </summary>
	public static Str str(Exp first)
	{
		return new Str(first);
	}

	/// <summary>
	/// A sequence of regexps, additional element
	/// </summary>
	public static Str add(Str str, Exp next)
	{
		return str.Add(next);
	}
}