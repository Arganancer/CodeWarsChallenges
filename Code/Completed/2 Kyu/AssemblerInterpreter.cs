using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/58e61f3d8ff24f774400002c/train/csharp
	/// </summary>
	public class AssemblerInterpreter
	{
		public static string Interpret(string input)
		{
			Dictionary<string, int> labels = new Dictionary<string, int>();
			Dictionary<string, int> registers = new Dictionary<string, int>();

			List<string> rawProgram = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

			// Normalize instructions, remove comments and empty entries, and find all labels
			for (int i = 0; i < rawProgram.Count; i++)
			{
				string cleanedInstruction = rawProgram[i];
				int commentOperatorIndex = rawProgram[i].IndexOf(';');
				if (commentOperatorIndex != -1)
				{
					cleanedInstruction = cleanedInstruction.Substring(0, commentOperatorIndex);
				}

				cleanedInstruction = cleanedInstruction.Trim();
				if (string.IsNullOrWhiteSpace(cleanedInstruction))
				{
					rawProgram.RemoveAt(i);
					--i;
					continue;
				}
				rawProgram[i] = cleanedInstruction;

				string[] instruction = cleanedInstruction.Split(' ');
				if (instruction[0][0] == ';' || instruction[0][^1] != ':') continue;
				labels[instruction[0].Replace(":", "")] = i;
			}

			string[] program = rawProgram.ToArray();

			// Execute program
			int lastCmp = 0;
			string msg = "";
			Stack<int> subroutineCallers = new Stack<int>();
			for (int i = 0; i < program.Length; i++)
			{
				string[] instruction = program[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				switch (instruction[0])
				{
					case "mov":
					{
						if (int.TryParse(instruction[2], out int value))
						{
							registers[instruction[1].CleanRegInstruction()] = value;
						}
						else
						{
							registers[instruction[1].CleanRegInstruction()] = registers[instruction[2]];
						}
						break;
					}
					case "inc":
						registers[instruction[1].CleanRegInstruction()]++;
						break;
					case "dec":
						registers[instruction[1].CleanRegInstruction()]--;
						break;
					case "add":
					{
						if (int.TryParse(instruction[2], out int value))
						{
							registers[instruction[1].CleanRegInstruction()] += value;
						}
						else
						{
							registers[instruction[1].CleanRegInstruction()] += registers[instruction[2]];
						}
						break;
					}
					case "sub":
					{
						if (int.TryParse(instruction[2], out int value))
						{
							registers[instruction[1].CleanRegInstruction()] -= value;
						}
						else
						{
							registers[instruction[1].CleanRegInstruction()] -= registers[instruction[2]];
						}
						break;
					}
					case "mul":
					{
						if (int.TryParse(instruction[2], out int value))
						{
							registers[instruction[1].CleanRegInstruction()] *= value;
						}
						else
						{
							registers[instruction[1].CleanRegInstruction()] *= registers[instruction[2]];
						}
						break;
					}
					case "div":
					{
						if (int.TryParse(instruction[2], out int value))
						{
							registers[instruction[1].CleanRegInstruction()] /= value;
						}
						else
						{
							registers[instruction[1].CleanRegInstruction()] /= registers[instruction[2]];
						}
						break;
					}
					case "jmp":
						i = labels[instruction[1].CleanRegInstruction()];
						break;
					case "cmp":
					{
						if (!int.TryParse(instruction[1].CleanRegInstruction(), out int x))
						{
							x = registers[instruction[1].CleanRegInstruction()];
						}
						if (!int.TryParse(instruction[2], out int y))
						{
							y = registers[instruction[2]];
						}

						lastCmp = x.CompareTo(y);
						break;
					}
					case "jne":
						if (lastCmp != 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "je":
						if (lastCmp == 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "jge":
						if (lastCmp >= 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "jg":
						if (lastCmp > 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "jle":
						if (lastCmp <= 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "jl":
						if (lastCmp < 0)
						{
							i = labels[instruction[1].CleanRegInstruction()];
						}
						break;
					case "call":
						subroutineCallers.Push(i);
						i = labels[instruction[1].CleanRegInstruction()];
						break;
					case "ret":
						if (subroutineCallers.Any())
						{
							i = subroutineCallers.Pop();
						}
						break;
					case "msg":
						string rawArguments = program[i].Substring(3).Trim();
						List<(string value, bool isRegister)> arguments = new List<(string value, bool isRegister)>();
						bool openParentheses = false;
						bool isRegister = true;
						StringBuilder currentArgument = new StringBuilder();

						foreach (char currentChar in rawArguments)
						{
							if (currentChar == '\'')
							{
								isRegister = false;
								openParentheses = !openParentheses;
							}
							else if (!openParentheses && currentChar == ',')
							{
								arguments.Add((currentArgument.ToString(), isRegister));
								currentArgument = new StringBuilder();
								isRegister = true;
							}
							else if (openParentheses || !char.IsWhiteSpace(currentChar))
							{
								currentArgument.Append(currentChar);
							}
						}

						string lastArgument = currentArgument.ToString();
						if (!string.IsNullOrWhiteSpace(lastArgument))
						{
							arguments.Add((lastArgument, isRegister));
						}

						foreach ((string value, bool isReg) in arguments)
						{
							if (isReg)
							{
								msg += registers[value];
							}
							else
							{
								msg += value;
							}
						}
						break;
					case "end":
						return msg;
				}
			}

			return null;
		}
	}

	public static class InstructionExtensions
	{
		public static string CleanRegInstruction(this string register)
		{
			return register.Trim().Trim(',');
		}
	}

	public static class AssemblerInterpreterTests
	{
		public static void RunTests()
		{
			for (int i = 0; i < Expected.Length; i++)
			{
				Console.WriteLine("\n----------------------\n");
				Logger.Log(Expected[i], AssemblerInterpreter.Interpret(Programs[i]), Programs[i]);
			}
		}
		private static readonly string[] Programs = {
			"\n; My first program\nmov  a, 5\ninc  a\ncall function\nmsg  '(5+1)/2 = ', a    ; output message\nend\n\nfunction:\n    div  a, 2\n    ret\n",
			"\nmov   a, 5\nmov   b, a\nmov   c, a\ncall  proc_fact\ncall  print\nend\n\nproc_fact:\n    dec   b\n    mul   c, b\n    cmp   b, 1\n    jne   proc_fact\n    ret\n\nprint:\n    msg   a, '! = ', c ; output text\n    ret\n",
			"\nmov   a, 8            ; value\nmov   b, 0            ; next\nmov   c, 0            ; counter\nmov   d, 0            ; first\nmov   e, 1            ; second\ncall  proc_fib\ncall  print\nend\n\nproc_fib:\n    cmp   c, 2\n    jl    func_0\n    mov   b, d\n    add   b, e\n    mov   d, e\n    mov   e, b\n    inc   c\n    cmp   c, a\n    jle   proc_fib\n    ret\n\nfunc_0:\n    mov   b, c\n    inc   c\n    jmp   proc_fib\n\nprint:\n    msg   'Term ', a, ' of Fibonacci series is: ', b        ; output text\n    ret\n",
			"\nmov   a, 11           ; value1\nmov   b, 3            ; value2\ncall  mod_func\nmsg   'mod(', a, ', ', b, ') = ', d        ; output\nend\n\n; Mod function\nmod_func:\n    mov   c, a        ; temp1\n    div   c, b\n    mul   c, b\n    mov   d, a        ; temp2\n    sub   d, c\n    ret\n",
			"\nmov   a, 81         ; value1\nmov   b, 153        ; value2\ncall  init\ncall  proc_gcd\ncall  print\nend\n\nproc_gcd:\n    cmp   c, d\n    jne   loop\n    ret\n\nloop:\n    cmp   c, d\n    jg    a_bigger\n    jmp   b_bigger\n\na_bigger:\n    sub   c, d\n    jmp   proc_gcd\n\nb_bigger:\n    sub   d, c\n    jmp   proc_gcd\n\ninit:\n    cmp   a, 0\n    jl    a_abs\n    cmp   b, 0\n    jl    b_abs\n    mov   c, a            ; temp1\n    mov   d, b            ; temp2\n    ret\n\na_abs:\n    mul   a, -1\n    jmp   init\n\nb_abs:\n    mul   b, -1\n    jmp   init\n\nprint:\n    msg   'gcd(', a, ', ', b, ') = ', c\n    ret\n",
			"\ncall  func1\ncall  print\nend\n\nfunc1:\n    call  func2\n    ret\n\nfunc2:\n    ret\n\nprint:\n    msg 'This program should return null'\n",
			"\nmov   a, 2            ; value1\nmov   b, 10           ; value2\nmov   c, a            ; temp1\nmov   d, b            ; temp2\ncall  proc_func\ncall  print\nend\n\nproc_func:\n    cmp   d, 1\n    je    continue\n    mul   c, a\n    dec   d\n    call  proc_func\n\ncontinue:\n    ret\n\nprint:\n    msg a, '^', b, ' = ', c\n    ret\n"};

		private static readonly string[] Expected = {"(5+1)/2 = 3",
			"5! = 120",
			"Term 8 of Fibonacci series is: 21",
			"mod(11, 3) = 2",
			"gcd(81, 153) = 9",
			null,
			"2^10 = 1024"};
	}
}