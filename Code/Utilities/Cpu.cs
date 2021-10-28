namespace Code.Utilities
{
	public class Cpu
	{
		/// <summary>
		/// Returns the value of the named register.
		/// </summary>
		public int ReadReg( string name ) { return 0; }

		/// <summary>
		/// Stores the value into the given register.
		/// </summary>
		public void WriteReg( string name, int value ) { }

		/// <summary>
		/// Pops the top element of the stack, returning the value.
		/// </summary>
		public int PopStack() { return 0; }

		/// <summary>
		/// Pushes an element onto the stack.
		/// </summary>
		public void WriteStack( int value ) { }

		/// <summary>
		/// Prints the contents of the stack to System.Console.
		/// </summary>
		public void PrintStack() { }
	}
}
