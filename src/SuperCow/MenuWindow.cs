using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class MenuWindow : AbstractScreen
{
	private enum Status
	{
		None,
		Showing,
		Hiding,
		Collapsing,
		Uncollapsing
	}

	private int mMenuSpeed = 1000;

	private Rectangle mBounds;

	private bool mDropDown;

	private Status mState;

	private bool mMenuVisible;

	private texture_t mBack;

	private texture_t mMenuTex;

	private float mTime;

	private float mSetTime;

	private Matrix mTransform = Matrix.Identity;

	private Rectangle DstRect = default(Rectangle);

	private Rectangle SrcRect = default(Rectangle);

	private Color aColorWhite = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private Color aColorBlack = new Color((byte)0, (byte)0, (byte)0, (byte)128);

	private WindowRect winRect = new WindowRect();

	private float mMenuPos;

	public bool menuVisible
	{
		get
		{
			return mMenuVisible;
		}
		set
		{
			mMenuVisible = value;
		}
	}

	public MenuWindow(Rectangle bounds, int speed, bool visible, bool dropDown)
	{
		mBounds = bounds;
		mMenuSpeed = speed;
		mMenuVisible = visible;
		mDropDown = dropDown;
		base.x = bounds.X;
		if (visible)
		{
			base.y = bounds.Y;
		}
		else if (dropDown)
		{
			base.y = bounds.Y;
		}
		else
		{
			base.y -= bounds.Y + bounds.Height;
		}
	}

	public void    Hide()
	{
		if (mMenuVisible && mState == Status.None)
		{
			mTime = 0f;
			mState = Status.Hiding;
		}
	}

	public void Show()
	{
		if (!mMenuVisible && mState == Status.None)
		{
			mTime = 0f;
			mState = Status.Showing;
		}
	}

	public void Collapse()
	{
		if (mMenuVisible && mState == Status.None)
		{
			mTime = 0f;
			mState = Status.Collapsing;
		}
	}

	public void Uncollapse()
	{
		if (!mMenuVisible && mState == Status.None)
		{
			mTime = 0f;
			mState = Status.Uncollapsing;
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (mState == Status.Uncollapsing)
		{
			mTime += gameTime.ElapsedGameTime.Milliseconds;
			mSetTime = 1f - mTime / (float)mMenuSpeed;
			if (mSetTime < 0f)
			{
				mSetTime = 0f;
			}
			SCGlobals.menusetk = mSetTime;
			if (mTime >= (float)mMenuSpeed)
			{
				Uncollapsed();
				mState = Status.None;
				mMenuVisible = true;
				SCGlobals.menusetk = 0f;
			}
		}
		else if (mState == Status.Collapsing)
		{
			mTime += gameTime.ElapsedGameTime.Milliseconds;
			mSetTime = mTime / (float)mMenuSpeed;
			if (mSetTime > 1f)
			{
				mSetTime = 1f;
			}
			SCGlobals.menusetk = mSetTime;
			if (mTime >= (float)mMenuSpeed)
			{
				Collapsed();
				mState = Status.None;
				mMenuVisible = false;
				SCGlobals.menusetk = 1f;
			}
		}
		else if (mState == Status.Showing)
		{
			mTime += gameTime.ElapsedGameTime.Milliseconds;
			mSetTime = mTime / (float)mMenuSpeed;
			if (mSetTime > 1f)
			{
				mSetTime = 1f;
			}
			SCGlobals.menusetk = mSetTime;
			float num = base.y;
			float num2 = mSetTime * (float)(mBounds.Y * 2 + mBounds.Height) * mSetTime;
			base.y = num2 - (float)mBounds.Height - (float)mBounds.Y;
			foreach (Widget mChild in mChildList)
			{
				mChild.y += base.y - num;
			}
			mMenuPos = SCGlobals.menusetk * SCGlobals.menusetk * 300f;
			if (mTime >= (float)mMenuSpeed)
			{
				ShowCompleted();
				mState = Status.None;
				mMenuVisible = true;
				SCGlobals.menusetk = 0f;
			}
		}
		else
		{
			if (mState != Status.Hiding)
			{
				return;
			}
			mTime += gameTime.ElapsedGameTime.Milliseconds;
			mSetTime = 1f - mTime / (float)mMenuSpeed;
			if (mSetTime < 0f)
			{
				mSetTime = 0f;
			}
			SCGlobals.menusetk = mSetTime;
			float num3 = base.y;
			float num4 = mSetTime * (float)(mBounds.Y * 2 + mBounds.Height) * mSetTime;
			base.y = num4 - (float)mBounds.Height - (float)mBounds.Y;
			foreach (Widget mChild2 in mChildList)
			{
				mChild2.y -= num3 - base.y;
			}
			mMenuPos = SCGlobals.menusetk * SCGlobals.menusetk * 300f;
			if (mTime >= (float)mMenuSpeed)
			{
				HideCompleted();
				mState = Status.None;
				mMenuVisible = false;
				SCGlobals.menusetk = 1f;
			}
		}
	}

	public override void Draw()
	{
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		SCGlobals.cam.load_identity();
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.EnableRS(7u);
		CowRender.EnableRS(5u);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// TODO: alpha test not implemented - 1f
		if (mDropDown)
		{
			matrix_t m = default(matrix_t);
			m.load_identity();
			m.scale(1f, 1f - SCGlobals.menusetk * SCGlobals.menusetk, 1f);
			CowRender.LoadMatrix(m);
			frect_t rect = default(frect_t);
			rect.assign(-(mBounds.Width / 2), mBounds.Y, mBounds.Width, mBounds.Height);
			winRect.SetRect(rect);
			winRect.Draw();
			base.Draw();
			CowRender.LoadMatrixNull();
		}
		else
		{
			int num = 32;
			num = 64;
			DstRect.X = (int)base.x;
			DstRect.Y = (int)base.y;
			DstRect.Width = num;
			DstRect.Height = num;
			SrcRect.X = 0;
			SrcRect.Y = 0;
			SrcRect.Width = num;
			SrcRect.Height = num;
			winRect.SetRect(new frect_t(base.x - (float)mBounds.Width, 600f - ((float)mBounds.Y + mMenuPos), mBounds.Width, mBounds.Height));
			winRect.Draw();
			CowRender.DisableRS(7u);
			CowRender.DisableRS(5u);
			base.Draw();
		}
	}

	public virtual void Collapsed()
	{
	}

	public virtual void Uncollapsed()
	{
	}

	public virtual void ShowCompleted()
	{
	}

	public virtual void HideCompleted()
	{
	}

	public override void JoyMove(GamePadKeys button, int theID)
	{
		if (mState == Status.None)
		{
			base.JoyMove(button, theID);
		}
	}
}
