using System;
using System.Linq;
using Code.Utilities;

/// <summary>
/// https://www.codewars.com/kata/54c1bf903f0696f04600068b/train/csharp
/// </summary>
public class Machine
{
	private readonly Cpu cpu;

	public Machine( Cpu cpu = null )
	{
		this.cpu = cpu;
	}

	public void Exec( string op )
	{
		string[] parts = op.Replace( ",", "" ).Split( ' ', StringSplitOptions.RemoveEmptyEntries );

		if (parts.Length > 1 && int.TryParse( parts[1], out int result ))
		{
			PerformCommand( parts[0] )( result, parts.Length > 2 ? parts.Skip( 2 ).ToArray() : new string[0] );
		}
		else
		{
			PerformCommand( parts[0] )( null, parts.Length > 1 ? parts.Skip( 1 ).ToArray() : new string[0] );
		}
	}

	private void Write( int? _value, params string[] _registers )
	{
		switch (_registers?.Length ?? 0)
		{
			case 1:
				if (_value is null)
				{
					cpu.WriteStack( cpu.ReadReg( _registers[0] ) );
				}
				else
				{
					cpu.WriteReg( _registers[0], _value ?? 0 );
				}

				break;
			case 2:
				cpu.WriteReg( _registers[1], cpu.ReadReg( _registers[0] ) );
				break;
			default:
				if (_value != null)
				{
					cpu.WriteStack( (int)_value );
				}

				break;
		}
	}

	private void PerformOperationOnRegistersInOrder( Action<string> _action, bool _isAscendingOrder )
	{
		foreach (string register in _isAscendingOrder ? new[] { "a", "b", "c", "d" } : new[] { "d", "c", "b", "a" })
		{
			_action( register );
		}
	}

	private void PerformArithmetic( Func<int, int, int> _operation, int? _value, string[] _registers, bool _aVariation = false )
	{
		if (_aVariation)
		{
			cpu.WriteStack( cpu.ReadReg( "a" ) );
		}

		int operationCount = _value ?? cpu.ReadReg( _registers[0] );

		int result = cpu.PopStack();
		for (int i = 1; i < operationCount; i++)
		{
			result = _operation( result, cpu.PopStack() );
		}

		string destinationRegister = _registers.Length > 0 ? _value is null ? _registers.Length > 1 ? _registers.Last() : "a" : _registers[0] : "a";

		cpu.WriteReg( destinationRegister, result );
	}

	private Action<int?, string[]> PerformCommand( string _command )
	{
		return _command switch
		{
			"mov" => Write,
			"push" => Write,
			"pop" => ( _i, _r ) =>
			{
				if (_r.Length == 0)
				{
					cpu.PopStack();
				}
				else
				{
					Write( cpu.PopStack(), _r );
				}
			},
			"pushr" => ( _i, _r ) => PerformOperationOnRegistersInOrder( _reg => Write( cpu.ReadReg( _reg ), null ), true ),
			"pushrr" => ( _i, _r ) => PerformOperationOnRegistersInOrder( _reg => Write( cpu.ReadReg( _reg ), null ), false ),
			"popr" => ( _i, _r ) => PerformOperationOnRegistersInOrder( _reg => Write( cpu.PopStack(), _reg ), false ),
			"poprr" => ( _i, _r ) => PerformOperationOnRegistersInOrder( _reg => Write( cpu.PopStack(), _reg ), true ),
			"adda" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a + _b, _i, _r, true ),
			"add" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a + _b, _i, _r ),
			"suba" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a - _b, _i, _r, true ),
			"sub" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a - _b, _i, _r ),
			"mula" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a * _b, _i, _r, true ),
			"mul" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a * _b, _i, _r ),
			"diva" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a / _b, _i, _r, true ),
			"div" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a / _b, _i, _r ),
			"anda" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a & _b, _i, _r, true ),
			"and" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a & _b, _i, _r ),
			"ora" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a | _b, _i, _r, true ),
			"or" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a | _b, _i, _r ),
			"xora" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a ^ _b, _i, _r, true ),
			"xor" => ( _i, _r ) => PerformArithmetic( ( _a, _b ) => _a ^ _b, _i, _r ),
			_ => throw new NotImplementedException( $"[{_command}] not implemented." )
		};
	}
}
