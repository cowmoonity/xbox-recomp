using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Font
{
	private struct FontInfo
	{
		public string Filename;

		public string Characters;

		public int StartOffset;

		public int CharacterSpacing;

		public int CharacterWidth;

		public int CharacterHeight;

		public FontInfo(string fileName, string characters, int startOffset, int characterSpacing, int characterWidth, int characterHeight)
		{
			Filename = fileName;
			Characters = characters;
			CharacterHeight = characterHeight;
			StartOffset = startOffset;
			CharacterSpacing = characterSpacing;
			CharacterWidth = characterWidth;
		}
	}

	private static FontInfo[] _fontInfo = new FontInfo[2]
	{
		new FontInfo("fonts\\weapon_large_font", "0123456789$,ptsx=", 0, 20, 18, 35),
		new FontInfo("fonts\\weapon_small_font", "0123456789$,", 0, 15, 13, 30)
	};

	public static void Init(Game game)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (game != null)
		{
			_ = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
		}
	}

	public static void Dispose()
	{
	}

	public static void Begin()
	{
	}

	public static void End()
	{
	}

	public static void Draw(FontStyle fontStyle, int x, int y, int number, Vector4 color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Draw(fontStyle, x, y, number.ToString(), color);
	}

	public static void Draw(FontStyle fontStyle, int x, int y, int number)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Draw(fontStyle, x, y, number.ToString(), new Vector4(1f, 1f, 1f, 1f));
	}

	public static void Draw(FontStyle fontStyle, int x, int y, string digits)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Draw(fontStyle, x, y, digits, new Vector4(1f, 1f, 1f, 1f));
	}

	public static void Draw(FontStyle fontStyle, int x, int y, string digits, Vector4 color)
	{
		float num = x;
		FontInfo fontInfo = _fontInfo[(int)fontStyle];
		for (int i = 0; i < digits.Length; i++)
		{
			if (digits[i] != ' ')
			{
				fontInfo.Characters.IndexOf(digits[i]);
			}
			num += (float)((digits[i] == ',' || digits[i] == ':') ? (fontInfo.CharacterWidth / 2) : fontInfo.CharacterWidth);
		}
	}
}
