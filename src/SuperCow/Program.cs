using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

internal static class Program
{
	private static void Main(string[] args)
	{
		FNALoggerEXT.LogInfo  = msg => Console.WriteLine("[FNA] " + msg);
		FNALoggerEXT.LogWarn  = msg => Console.WriteLine("[FNA WARN] " + msg);
		FNALoggerEXT.LogError = msg => Console.WriteLine("[FNA ERROR] " + msg);
		using var game = new SuperCow();
		game.Run();
	}
}
