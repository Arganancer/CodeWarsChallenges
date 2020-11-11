﻿using System;

namespace Codewars
{
	public class RGBToHex
	{
		public static string Rgb(int r, int g, int b)
		{
			return $"{Math.Clamp(r, 0, 255):X2}{Math.Clamp(g, 0, 255):X2}{Math.Clamp(b, 0, 255):X2}";
		}
	}
}
