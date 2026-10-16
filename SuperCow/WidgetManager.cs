using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SuperCow;

public class WidgetManager
{
	private Widget mFocusedWidget;

	private List<Widget> mWidgetList;

	public Widget focusedWidget
	{
		get
		{
			return mFocusedWidget;
		}
		set
		{
			mFocusedWidget = value;
		}
	}

	public WidgetManager()
	{
		mWidgetList = new List<Widget>();
	}

	public virtual void AddWidget(Widget widget)
	{
		if (widget != null)
		{
			widget.Init();
			widget.widgetManager = this;
			mWidgetList.Add(widget);
			widget.AddedToManager();
		}
	}

	public virtual void RemoveWidget(Widget widget)
	{
		if (widget != null && mWidgetList.Contains(widget))
		{
			mWidgetList.Remove(widget);
			widget.RemovedFromManager();
		}
	}

	public virtual void Update(GameTime gameTime)
	{
		for (int num = mWidgetList.Count - 1; num >= 0; num--)
		{
			mWidgetList[num].Update(gameTime);
		}
	}

	public virtual void Draw()
	{
		foreach (Widget mWidget in mWidgetList)
		{
			if (mWidget.visible)
			{
				mWidget.Draw();
			}
		}
	}

	public virtual void ProcessInput()
	{
		foreach (Widget mWidget in mWidgetList)
		{
			mWidget.ProcessInput();
		}
	}
}
