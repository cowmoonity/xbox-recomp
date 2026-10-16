using System.Text;

namespace SuperCow;

public static class wstrstuff
{
	public struct gamestringw_t
	{
		public string name;

		public string p_str;
	}

	public static int MAX_GAME_STRINGS = 1024;

	public static gamestringw_t[] ar_gamestringsw = new gamestringw_t[MAX_GAME_STRINGS];

	public static int numgamestringsw = 0;

	public static void AddGameStringW(string name, string body, int len)
	{
		gamestringw_t gamestringw_t = new gamestringw_t
		{
			name = name,
			p_str = body.Substring(0, len)
		};
		ar_gamestringsw[numgamestringsw++] = gamestringw_t;
	}

	public static void LoadStringFile(string name)
	{
		int len = 0;
		byte[] array = Store.storeLoadFileArray(name, ref len, b_errorexit: false);
		string text = Encoding.Unicode.GetString(array, 0, array.Length);
		text = text.Substring(1, text.Length - 1);
		int num2;
		int num = (num2 = 0);
		len = text.Length;
		while (true)
		{
			if (text[num] <= ' ')
			{
				if (num < text.Length - 1)
				{
					num++;
					continue;
				}
				break;
			}
			string text2;
			string name2;
			if (text[num] == '[')
			{
				num++;
				num2 = num;
				for (text2 = text; text2[num2] != '/' || text2[num2 + 1] != ']'; num2++)
				{
					if (num2 >= len - 1)
					{
						return;
					}
				}
				name2 = text.Substring(num, num2 - num);
				for (num2 += 3; text2[num2] < ' '; num2++)
				{
					if (num2 >= len - 1)
					{
						return;
					}
				}
				text = text2;
				num = num2;
				for (; text2[num2] != '['; num2++)
				{
					if (num2 >= len - 1)
					{
						return;
					}
				}
				AddGameStringW(name2, text.Substring(num, text.Length - num), num2 - num - 2);
				for (; text2[num2] != ']'; num2++)
				{
					if (num2 >= len - 1)
					{
						return;
					}
				}
				num2++;
				text = text2;
				num = num2;
				continue;
			}
			text2 = text;
			for (num2 = num; text2[num2] != '='; num2++)
			{
				if (num2 >= len - 1)
				{
					return;
				}
			}
			name2 = text.Substring(num, num2 - num);
			num2++;
			text = text2;
			num = num2;
			for (; text2[num2] >= ' ' || text2[num2] >= ' '; num2++)
			{
				if (num2 >= len - 1)
				{
					return;
				}
			}
			AddGameStringW(name2, text.Substring(num, text.Length - num), num2 - num);
			num2++;
			text = text2;
			num = num2;
		}
	}

	public static string GetStr(string name)
	{
		for (int i = 0; i < numgamestringsw; i++)
		{
			gamestringw_t gamestringw_t = ar_gamestringsw[i];
			if (gamestringw_t.name == name)
			{
				return gamestringw_t.p_str;
			}
		}
		return "";
	}

	public static string GetStr(int num)
	{
		if (num >= numgamestringsw)
		{
			return "";
		}
		return ar_gamestringsw[num].p_str;
	}

	public static int GetStrNum(string name)
	{
		for (int i = 0; i < numgamestringsw; i++)
		{
			gamestringw_t gamestringw_t = ar_gamestringsw[i];
			if (gamestringw_t.name == name)
			{
				return i;
			}
		}
		return -1;
	}

	public static string GetStrDelta(string name, int num)
	{
		return GetStr(GetStrNum(name) + num);
	}
}
