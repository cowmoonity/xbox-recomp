using Microsoft.Xna.Framework;

namespace SuperCow;

public class Widget
{
	private int mID = -1;

	private float mX;

	private float mY;

	private int mWidth;

	private int mHeight;

	private bool mVisible = true;

	private bool mIsFocused;

	private Widget mParent;

	private WidgetManager mWidgetManager;

	private Widget mPrevWidget;

	private Widget mNextWidget;

	private Matrix mDrawMatrix = Matrix.Identity;

	private byte mAlpha = byte.MaxValue;

	public int id
	{
		get
		{
			return mID;
		}
		set
		{
			mID = value;
		}
	}

	public float x
	{
		get
		{
			return mX;
		}
		set
		{
			mX = value;
		}
	}

	public float y
	{
		get
		{
			return mY;
		}
		set
		{
			mY = value;
		}
	}

	public int width
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

	public int height
	{
		get
		{
			return mHeight;
		}
		set
		{
			mHeight = value;
		}
	}

	public bool visible
	{
		get
		{
			return mVisible;
		}
		set
		{
			mVisible = value;
		}
	}

	public bool focused
	{
		get
		{
			return mIsFocused;
		}
		set
		{
			mIsFocused = value;
		}
	}

	public Widget parent
	{
		get
		{
			return mParent;
		}
		set
		{
			mParent = value;
		}
	}

	public WidgetManager widgetManager
	{
		get
		{
			return mWidgetManager;
		}
		set
		{
			mWidgetManager = value;
		}
	}

	public Widget prevWidget
	{
		get
		{
			return mPrevWidget;
		}
		set
		{
			mPrevWidget = value;
			if (value != null)
			{
				value.mNextWidget = this;
			}
		}
	}

	public Widget nextWidget
	{
		get
		{
			return mNextWidget;
		}
		set
		{
			mNextWidget = value;
			if (value != null)
			{
				value.mPrevWidget = this;
			}
		}
	}

	public Matrix drawMatrix
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return mDrawMatrix;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			mDrawMatrix = value;
		}
	}

	public byte setAlpha
	{
		get
		{
			return mAlpha;
		}
		set
		{
			mAlpha = value;
		}
	}

	public virtual void Init()
	{
	}

	public virtual void AddedToManager()
	{
	}

	public virtual void RemovedFromManager()
	{
	}

	public virtual void Update(GameTime gameTime)
	{
	}

	public virtual void UpdateF(float delta)
	{
	}

	public virtual void Draw()
	{
	}

	public virtual void ChildGotFocus(int childID)
	{
	}

	public virtual void ChildLostFocus(int childID)
	{
	}

	public virtual void LostFocus()
	{
		focused = false;
		if (parent != null)
		{
			parent.ChildLostFocus(id);
		}
	}

	public virtual void GotFocus()
	{
		focused = true;
		if (parent != null)
		{
			parent.ChildGotFocus(id);
		}
	}

	public virtual void Resize(int theX, int theY, int theWidth, int theHeight)
	{
		x = theX;
		y = theY;
		width = theWidth;
		height = theHeight;
	}

	public virtual void JoyPressRepeat(GamePadKeys button, int theID)
	{
		if (parent != null)
		{
			parent.JoyPressRepeat(button, (id == -1) ? theID : id);
		}
	}

	public virtual void JoyPress(GamePadKeys button, int theID)
	{
		if (parent != null)
		{
			parent.JoyPress(button, (id == -1) ? theID : id);
		}
	}

	public virtual void JoyMove(GamePadKeys button, int theID)
	{
		if (parent != null)
		{
			parent.JoyMove(button, (id == -1) ? theID : id);
		}
	}

	public virtual void JoyMoveRepeat(GamePadKeys button, int theID)
	{
		if (parent != null)
		{
			parent.JoyMoveRepeat(button, (id == -1) ? theID : id);
		}
	}

	public virtual void ProcessInput()
	{
	}

	public virtual void ButtonPressed(int theID)
	{
	}
}
