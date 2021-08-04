using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class BefungeInterpreter
{
	private static List<int> m_Stack;
	private static StringBuilder m_Output;
	private static Random m_Rand;
	private static Direction m_PointerDirection;
	private static bool m_IsAsciiMode;
	private static bool m_SkipNext;
	private static char[][] m_Code;

	private static int m_InstructionCount;

	public string Interpret( string code )
	{
		//Console.WriteLine($"Input Code:\n{code}\n");
		m_PointerDirection = Direction.Right;
		m_Code = code.Split( '\n', StringSplitOptions.RemoveEmptyEntries ).Select( _x => _x.ToCharArray() ).ToArray();
		m_IsAsciiMode = false;
		m_SkipNext = false;
		m_Rand = new Random();
		m_Stack = new List<int>();
		m_Output = new StringBuilder();
		int pointerX = 0;
		int pointerY = 0;

		while (EvaluateInstruction( m_Code[pointerY][pointerX] ))
		{
			//UpdateConsole( pointerX, pointerY, code );
			switch (m_PointerDirection)
			{
				case Direction.Left:
					if (--pointerX < 0)
					{
						pointerX = m_Code[pointerY].Length - 1;
					}

					break;
				case Direction.Right:
					if (++pointerX >= m_Code[pointerY].Length)
					{
						pointerX = 0;
					}

					break;
				case Direction.Up:
					if (--pointerY < 0)
					{
						pointerY = m_Code.Length - 1;
					}

					break;
				case Direction.Down:
					if (++pointerY > m_Code.Length - 1)
					{
						pointerY = 0;
					}

					break;
			}
		}

		//UpdateConsole( pointerX, pointerY, code );

		return m_Output.ToString();
	}

	private static void UpdateConsole( int _x, int _y, string _code )
	{
		Console.CursorVisible = false;
		int maxCodeLength = m_Code.Max( _v => _v.Length );
		Console.SetCursorPosition( 0, 0 );
		Console.Write( _code );

		Console.SetCursorPosition( _x, _y );
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.BackgroundColor = ConsoleColor.DarkCyan;
		Console.Write( m_Code[_y][_x] );

		Console.ForegroundColor = ConsoleColor.White;
		Console.BackgroundColor = ConsoleColor.Black;

		Console.SetCursorPosition( 0, m_Code.Length );
		Console.WriteLine( $"\nInstruction Count: {++m_InstructionCount}" );
		Console.WriteLine( $"Output: {m_Output}".PadRight( 150, ' ' ) );
		Console.WriteLine( $"Stack: {string.Join( ", ", m_Stack )}".PadRight( 150, ' ' ) );
		//Thread.Sleep( 100 );
		//Console.ReadLine();
	}

	private static bool EvaluateInstruction( char _instruction )
	{
		string instructionDefinition = "";

		if (m_SkipNext)
		{
			m_SkipNext = false;
			return true;
		}

		if (m_IsAsciiMode && _instruction != '"')
		{
			Push( _instruction );
			return true;
		}

		if (char.IsNumber( _instruction ))
		{
			//Console.WriteLine( "Push this number onto the stack.".PadRight( 100, ' ' ) );
			Push( int.Parse( _instruction.ToString() ) );
			return true;
		}

		switch (_instruction)
		{
			case '"':
				instructionDefinition = $"{(m_IsAsciiMode ? "Exiting" : "Entering")} Ascii Mode";
				m_IsAsciiMode = !m_IsAsciiMode;
				break;
			case '@':
				return false;
			case '+':
				instructionDefinition = "Addition: Pop a and b, then push a+b.";
				Add();
				break;
			case '-':
				instructionDefinition = "Subtraction: Pop a and b, then push b-a.";
				Subtract();
				break;
			case '*':
				instructionDefinition = "Multiplication: Pop a and b, then push a*b.";
				Multiply();
				break;
			case '/':
				instructionDefinition = "Integer division: Pop a and b, then push b/a, rounded down. If a is zero, push zero.";
				Divide();
				break;
			case '%':
				instructionDefinition = "Modulo: Pop a and b, then push the b%a. If a is zero, push zero.";
				Modulo();
				break;
			case '!':
				instructionDefinition = "Logical NOT: Pop a value. If the value is zero, push 1; otherwise, push zero.";
				Not();
				break;
			case '`':
				instructionDefinition = "(backtick) Greater than: Pop a and b, then push 1 if b>a, otherwise push zero.";
				GreaterThan();
				break;
			case '>':
				instructionDefinition = "Start moving right.";
				m_PointerDirection = Direction.Right;
				break;
			case '<':
				instructionDefinition = "Start moving left.";
				m_PointerDirection = Direction.Left;
				break;
			case '^':
				instructionDefinition = "Start moving up.";
				m_PointerDirection = Direction.Up;
				break;
			case 'v':
				instructionDefinition = "Start moving down.";
				m_PointerDirection = Direction.Down;
				break;
			case '?':
				instructionDefinition = "Start moving in a random cardinal direction.";
				m_PointerDirection = (Direction)m_Rand.Next( 4 );
				break;
			case '_':
				instructionDefinition = "Pop a value; move right if value = 0, left otherwise.";
				m_PointerDirection = Pop() == 0 ? Direction.Right : Direction.Left;
				break;
			case '|':
				instructionDefinition = "Pop a value; move down if value = 0, up otherwise.";
				m_PointerDirection = Pop() == 0 ? Direction.Down : Direction.Up;
				break;
			case ':':
				instructionDefinition = "Duplicate value on top of the stack. If there is nothing on top of the stack, push a 0.";
				Push( Peek() ?? 0 );
				break;
			case '\\':
				instructionDefinition = "Swap two values on top of the stack. If there is only one value, pretend there is an extra 0 on bottom of the stack.";
				Swap();
				break;
			case '$':
				instructionDefinition = "Pop value from the stack and discard it.";
				Pop();
				break;
			case '.':
				instructionDefinition = "Pop value and output as an integer.";
				m_Output.Append( Pop() );
				break;
			case ',':
				instructionDefinition = "Pop value and output the ASCII character represented by the integer code that is stored in the value.";
				m_Output.Append( Convert.ToChar( Pop() ) );
				break;
			case '#':
				instructionDefinition = "Trampoline: Skip next cell.";
				m_SkipNext = true;
				break;
			case 'p':
				instructionDefinition = "A put call (a way to store a value for later use). Pop y, x and v, then change the character at the position (x,y) in the program to the character with ASCII value v.";
				Put();
				break;
			case 'g':
				instructionDefinition = "A get call (a way to retrieve data in storage). Pop y and x, then push ASCII value of the character at that position in the program.";
				Get();
				break;
		}

		//Console.WriteLine( instructionDefinition.PadRight( 150, ' ' ) );
		return true;
	}

	private static void Put()
	{
		int y = Pop();
		int x = Pop();
		m_Code[y][x] = Convert.ToChar( Pop() );
	}

	private static void Get()
	{
		int y = Pop();
		int x = Pop();
		Push( m_Code[y][x] );
	}

	private static int Pop()
	{
		int output = m_Stack[^1];
		m_Stack.RemoveAt( m_Stack.Count - 1 );
		return output;
	}

	private static int? Peek()
	{
		return m_Stack.Count == 0 ? (int?)null : m_Stack[^1];
	}

	private static (int A, int B) PopAB()
	{
		return (Pop(), Pop());
	}

	private static void Push( int _value )
	{
		m_Stack.Add( _value );
	}

	private static void Add()
	{
		(int a, int b) = PopAB();
		Push( a + b );
	}

	private static void Subtract()
	{
		(int a, int b) = PopAB();
		Push( b - a );
	}

	private static void Multiply()
	{
		(int a, int b) = PopAB();
		Push( a * b );
	}

	private static void Divide()
	{
		(int a, int b) = PopAB();
		if (a == 0)
		{
			Push( 0 );
		}
		else
		{
			Push( b / a );
		}
	}

	private static void Modulo()
	{
		(int a, int b) = PopAB();
		if (a == 0)
		{
			Push( 0 );
		}
		else
		{
			Push( b % a );
		}
	}

	private static void Not()
	{
		Push( Pop() == 0 ? 1 : 0 );
	}

	private static void GreaterThan()
	{
		(int a, int b) = PopAB();
		Push( b > a ? 1 : 0 );
	}

	private static void Swap()
	{
		if (m_Stack.Count > 1)
		{
			(int a, int b) = PopAB();
			Push( a );
			Push( b );
		}
		else
		{
			Push( 0 );
		}
	}

	private enum Direction
	{
		Left, Right, Up, Down
	}
}
