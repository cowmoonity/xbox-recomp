using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace SuperCow;

internal class Keymap
{
	private Dictionary<GamePadKeys, List<Keys>> bindings;

	public Keymap()
	{
		bindings = new Dictionary<GamePadKeys, List<Keys>>();
	}

	public void Add(GamePadKeys gk, Keys k)
	{
		List<Keys> list = new List<Keys>();
		list.Add(k);
		if (bindings.ContainsKey(gk))
		{
			bindings.Remove(gk);
			bindings.Add(gk, list);
		}
		else
		{
			bindings.Add(gk, list);
		}
	}

	public Keys Get(GamePadKeys gk)
	{
		return bindings[gk][0];
	}
}
