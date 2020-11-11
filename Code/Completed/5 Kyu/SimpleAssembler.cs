using System.Collections.Generic;

namespace Codewars
{
	/// <summary>
	/// https://www.codewars.com/kata/58e24788e24ddee28e000053/train/csharp
	/// </summary>
	public static class SimpleAssembler
	{
		public static Dictionary<string, int> Interpret(string[] program)
		{
			Dictionary<string, int> registers = new Dictionary<string, int>();

			for (int i = 0; i < program.Length; i++)
			{
				string[] instruction = program[i].Split(' ');
				switch (instruction[0])
				{
					case "mov":
						if (int.TryParse(instruction[2], out int moveValue))
						{
							registers[instruction[1]] = moveValue;
						}
						else
						{
							registers[instruction[1]] = registers[instruction[2]];
						}
						break;
					case "inc":
						registers[instruction[1]]++;
						break;
					case "dec":
						registers[instruction[1]]--;
						break;
					case "jnz":
						if (int.TryParse(instruction[1], out int value) ? value != 0 : registers[instruction[1]] != 0)
						{
							if (int.TryParse(instruction[2], out value))
							{
								i = i + value - 1;
							}
							else
							{
								i = i + registers[instruction[2]] - 1;
							}
						}
						break;
				}
			}

			return registers;
		}
	}
}
