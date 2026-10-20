using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class Button : Widget
{
	private string mText = "";

	private Texture2D mNormalImage;

	private Texture2D mOverImage;

	private float mFontSize = 20f;

	private int mFont;

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			mText = value;
		}
	}

	public float fontSize
	{
		get
		{
			return mFontSize;
		}
		set
		{
			mFontSize = value;
		}
	}

	public int font
	{
		get
		{
			return mFont;
		}
		set
		{
			mFont = value;
		}
	}

	public Texture2D normalImage
	{
		get
		{
			return mNormalImage;
		}
		set
		{
			mNormalImage = value;
		}
	}

	public Texture2D overImage
	{
		get
		{
			return mOverImage;
		}
		set
		{
			mOverImage = value;
		}
	}

	public Button(int buttonID, Widget listener)
	{
		base.id = buttonID;
	}

	public override void Draw()
	{
		base.Draw();
		if (mNormalImage != null && !base.focused)
		{
			new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)100);
			new Vector2(base.x, base.y);
		}
		if (mOverImage != null && base.focused)
		{
			new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)100);
			new Vector2(base.x, base.y);
		}
		CowRender.EnableRS(7u);
		Text.SetFont(0);
		if (base.focused)
		{
			Text.SetTextColor(0, byte.MaxValue, 0, byte.MaxValue);
		}
		else
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		Text.OutTextXY(base.x, 0f - base.y, mFontSize, mText, b_center: true);
	}
}
