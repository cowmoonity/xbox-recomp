using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

internal static class Program
{
	private static void Main(string[] args)
	{
		SuperCow superCow = new SuperCow();
		try
		{
			((Game)superCow).Run();
		}
		finally
		{
			((IDisposable)superCow)?.Dispose();
		}
	}
}
