using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class Slider : Widget
{
	private string mText = "";

	private float mWidth = 100f;

	private int mValue;

	private texture_t mMenuTex;

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

	public float sliderWidth
	{
		get
		{
			return mWidth;
		}
		set
		{
			mWidth = value;
		}
	}

	public int sliderValue
	{
		get
		{
			return mValue;
		}
		set
		{
			mValue = value;
		}
	}

	public Slider(int buttonID, Widget listener)
	{
		base.id = buttonID;
		Store.SetCurrentDirectory("ui", theAdd: false);
		mMenuTex = SCGlobals.texturehost.LoadTGAJPG("_a_menutex.jpg", b_errormsg: true, b_notusealpha: false);
	}

	public override void Draw()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		base.Draw();
		new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		int num = (int)(sliderWidth / 10f * (float)mValue);
		CowRender.EnableRS(7u);
		Text.SetFont(0);
		CowRender.BindTexture(SCGlobals.p_menutex);
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t vec3_t4 = default(vec3_t);
		vec3_t vec3_t5 = default(vec3_t);
		vec3_t vec3_t6 = default(vec3_t);
		vec3_t vec3_t7 = default(vec3_t);
		vec3_t vec3_t8 = default(vec3_t);
		vec3_t vec3_t9 = default(vec3_t);
		vec3_t6.assign(0f - sliderWidth / 2f, base.y - 30f, 40f);
		vec3_t7.assign(0f + sliderWidth / 2f, base.y - 30f, 40f);
		vec3_t8 = vec3_t7 - vec3_t6;
		float num2 = vec3_t8.len();
		vec3_t8 /= num2;
		vec3_t2 = vec3_t6 - vec3_t8 * 64f;
		vec3_t3 = vec3_t7 + vec3_t8 * 64f;
		vec3_t4 = vec3_t6 - vec3_t8 * 64f * 0.7f;
		vec3_t5 = vec3_t7 + vec3_t8 * 64f * 0.7f;
		vec3_t9 = vec3_t6;
		vec3_t9.x = vec3_t6.x + (float)num;
		CowRender.mi_size = 0.5f;
		CowRender.DrawLineMI(4, vec3_t2, vec3_t3);
		CowRender.mi_size = 1f;
		CowRender.DrawMI(14, vec3_t4);
		CowRender.DrawMI(15, vec3_t5);
		CowRender.DrawMI(19, vec3_t9);
		if (base.focused)
		{
			Text.SetTextColor(0, byte.MaxValue, 0, byte.MaxValue);
		}
		else
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		Text.OutTextXY(base.x, base.y, mFontSize, mText, b_center: true);
	}

	public override void JoyMove(GamePadKeys button, int theID)
	{
		switch (button)
		{
		case GamePadKeys.Left:
			if (mValue >= 1)
			{
				mValue--;
			}
			break;
		case GamePadKeys.Right:
			if (mValue <= 9)
			{
				mValue++;
			}
			break;
		}
		base.JoyMove(button, base.id);
	}
}
