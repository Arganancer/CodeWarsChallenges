using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

/// <summary>
/// https://www.codewars.com/kata/589394ae1a880832e2000092/train/csharp
/// </summary>
public class CreateClassAtRuntime
{
	private static AssemblyBuilder m_AssemblyBuilder;
	private static ModuleBuilder m_ModuleBuilder;

	public static bool DefineClass( string className, Dictionary<string, Type> properties, ref Type actualType )
	{
		if (m_AssemblyBuilder is null)
		{
			AssemblyName assemblyName = new AssemblyName( "RuntimeAssembly" );
			m_AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly( assemblyName, AssemblyBuilderAccess.Run );
			m_ModuleBuilder = m_AssemblyBuilder.DefineDynamicModule( "RuntimeModule" );
		}
		else if (m_AssemblyBuilder.DefinedTypes.Any( typeInfo => typeInfo.Name == className ))
		{
			return false;
		}

		TypeBuilder typeBuilder = m_ModuleBuilder.DefineType( className, TypeAttributes.Public );
		foreach ((string propertyName, Type propertyType) in properties)
		{
			FieldBuilder fieldBuilder = typeBuilder.DefineField( $"m_{propertyName}", propertyType, FieldAttributes.Private );

			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty( propertyName, PropertyAttributes.HasDefault, propertyType, null);

			MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

			MethodBuilder getPropertyMethodBuilder = typeBuilder.DefineMethod( $"get_{propertyName}", getSetAttr, propertyType, Type.EmptyTypes );
			ILGenerator getIL = getPropertyMethodBuilder.GetILGenerator();
			getIL.Emit( OpCodes.Ldarg_0 );
			getIL.Emit( OpCodes.Ldfld, fieldBuilder );
			getIL.Emit( OpCodes.Ret );

			MethodBuilder setPropertyMethodBuilder = typeBuilder.DefineMethod( $"set_{propertyName}", getSetAttr, null, new[] { propertyType } );
			ILGenerator setIL = setPropertyMethodBuilder.GetILGenerator();
			setIL.Emit( OpCodes.Ldarg_0 );
			setIL.Emit( OpCodes.Ldarg_1 );
			setIL.Emit( OpCodes.Stfld, fieldBuilder );
			setIL.Emit( OpCodes.Ret );

			propertyBuilder.SetGetMethod( getPropertyMethodBuilder );
			propertyBuilder.SetSetMethod( setPropertyMethodBuilder );
		}

		actualType = typeBuilder.CreateType();
		return true;
	}
}
