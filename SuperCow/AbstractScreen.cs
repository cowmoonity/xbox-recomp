using System.Collections.Generic;

namespace SuperCow;

public class AbstractScreen : Widget
{
	protected List<Widget> mChildList;

	private Widget mBackChild;

	private Widget mLastFocusedWidget;

	private Widget mFocusedChild;

	public Widget backChild
	{
		get
		{
			return mBackChild;
		}
		set
		{
			mBackChild = value;
		}
	}

	public Widget focusedChild
	{
		get
		{
			return mFocusedChild;
		}
		set
		{
			if (mFocusedChild != null)
			{
				mFocusedChild.LostFocus();
			}
			mFocusedChild = value;
			if (mFocusedChild != null)
			{
				mFocusedChild.GotFocus();
			}
		}
	}

	public AbstractScreen()
	{
		mChildList = new List<Widget>();
		mLastFocusedWidget = this;
	}

	public override void AddedToManager()
	{
		base.AddedToManager();
		mFocusedChild = mLastFocusedWidget;
		base.widgetManager.focusedWidget = mFocusedChild;
		foreach (Widget mChild in mChildList)
		{
			base.widgetManager.AddWidget(mChild);
		}
	}

	public override void RemovedFromManager()
	{
		base.RemovedFromManager();
		mLastFocusedWidget = mFocusedChild;
		if (base.focused)
		{
			base.widgetManager.focusedWidget = null;
		}
		foreach (Widget mChild in mChildList)
		{
			base.widgetManager.RemoveWidget(mChild);
		}
	}

	public virtual void FromDemoToFull()
	{
	}

	public void AddChild(Widget child)
	{
		if (!mChildList.Contains(child))
		{
			child.parent = this;
			mChildList.Add(child);
			if (mChildList.Count == 1)
			{
				mLastFocusedWidget = child;
				child.focused = true;
			}
		}
	}

	public void RemoveChild(Widget child)
	{
		if (child != null && mChildList.Contains(child))
		{
			mChildList.Remove(child);
		}
		mChildList.TrimExcess();
	}

	public Widget GetChildByID(int id)
	{
		foreach (Widget mChild in mChildList)
		{
			if (mChild.id == id)
			{
				return mChild;
			}
		}
		return null;
	}

	public override void LostFocus()
	{
		base.LostFocus();
	}

	public override void GotFocus()
	{
		base.GotFocus();
	}

	public void SetupOrder()
	{
		if (mChildList.Count == 0)
		{
			return;
		}
		Widget widget = mChildList[0];
		foreach (Widget mChild in mChildList)
		{
			if (mChild != mChildList[0] && mChild.visible)
			{
				widget.nextWidget = mChild;
				widget = mChild;
			}
		}
		mChildList[mChildList.Count - 1].nextWidget = mChildList[0];
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (base.focused)
		{
			if (button == GamePadKeys.A || button == GamePadKeys.Start)
			{
				Sound.Addsound("menu");
				ButtonPressed(theID);
			}
			if ((button == GamePadKeys.B || button == GamePadKeys.Back) && mBackChild != null)
			{
				Sound.Addsound("menu");
				ButtonPressed(mBackChild.id);
			}
		}
	}

	public override void JoyMoveRepeat(GamePadKeys button, int theID)
	{
		if (focusedChild == null || !base.focused)
		{
			return;
		}
		switch (button)
		{
		case GamePadKeys.Down:
			if (focusedChild.nextWidget != null)
			{
				Sound.Addsound("menu");
				focusedChild = focusedChild.nextWidget;
			}
			break;
		case GamePadKeys.Up:
			if (focusedChild.prevWidget != null)
			{
				Sound.Addsound("menu");
				focusedChild = focusedChild.prevWidget;
			}
			break;
		}
		base.widgetManager.focusedWidget = focusedChild;
	}

	public override void JoyMove(GamePadKeys button, int theID)
	{
		if (focusedChild == null || !base.focused)
		{
			return;
		}
		switch (button)
		{
		case GamePadKeys.Down:
			if (focusedChild.nextWidget != null)
			{
				Sound.Addsound("menu");
				focusedChild = focusedChild.nextWidget;
			}
			break;
		case GamePadKeys.Up:
			if (focusedChild.prevWidget != null)
			{
				Sound.Addsound("menu");
				focusedChild = focusedChild.prevWidget;
			}
			break;
		}
		base.widgetManager.focusedWidget = focusedChild;
	}

	public override void Draw()
	{
		base.Draw();
		for (int i = 0; i < mChildList.Count; i++)
		{
			if (mChildList[i].visible)
			{
				mChildList[i].Draw();
			}
		}
	}

	public virtual void ShutDown()
	{
	}

	public virtual void Activate()
	{
	}

	public virtual void Deactivate()
	{
	}

	public virtual void DrawGeometry()
	{
	}

	public virtual void CalculatePhysics()
	{
	}
}
