using System.Collections.Generic;

namespace SuperCow;

public class ScreenManager
{
	private WidgetManager mWidgetManager;

	private List<AbstractScreen> mScreenList;

	private List<AbstractScreen> mTrashCan;

	public ScreenManager(WidgetManager widgetManager)
	{
		mWidgetManager = widgetManager;
		mScreenList = new List<AbstractScreen>();
		mTrashCan = new List<AbstractScreen>();
	}

	public virtual void AddScreen(AbstractScreen screen)
	{
		AddScreen(screen.id, screen);
	}

	public virtual void AddScreen(int id, AbstractScreen screen)
	{
		AbstractScreen abstractScreen = screen;
		if (GetScreenByID(id) != null)
		{
			return;
		}
		if (abstractScreen == null)
		{
			abstractScreen = GetScreenByIDInTrashCan(id);
			if (abstractScreen != null)
			{
				mTrashCan.Remove(abstractScreen);
				mTrashCan.TrimExcess();
			}
		}
		if (abstractScreen != null)
		{
			abstractScreen.id = id;
			mScreenList.Add(abstractScreen);
			abstractScreen.focused = true;
			mWidgetManager.AddWidget(abstractScreen);
		}
	}

	public virtual void RemoveScreen(AbstractScreen screen, bool clear)
	{
		RemoveScreen(screen.id, clear);
	}

	public virtual void PurgeInTrash(int id)
	{
		for (AbstractScreen screenByIDInTrashCan = GetScreenByIDInTrashCan(id); screenByIDInTrashCan != null; screenByIDInTrashCan = GetScreenByIDInTrashCan(id))
		{
			mTrashCan.Remove(screenByIDInTrashCan);
		}
	}

	public virtual void RemoveScreen(int id, bool clear)
	{
		AbstractScreen screenByID = GetScreenByID(id);
		if (screenByID != null)
		{
			screenByID.focused = false;
			mScreenList.Remove(screenByID);
			mWidgetManager.RemoveWidget(screenByID);
			screenByID.ShutDown();
			if (!clear)
			{
				mTrashCan.Add(screenByID);
			}
			if (mScreenList.Count > 0)
			{
				mScreenList[mScreenList.Count - 1].focused = true;
				mWidgetManager.focusedWidget = mScreenList[mScreenList.Count - 1].focusedChild;
			}
			else
			{
				mWidgetManager.focusedWidget = null;
			}
			mScreenList.TrimExcess();
		}
	}

	public virtual AbstractScreen GetScreenByID(int id)
	{
		foreach (AbstractScreen mScreen in mScreenList)
		{
			if (mScreen.id == id)
			{
				return mScreen;
			}
		}
		return null;
	}

	public virtual AbstractScreen GetScreenByIDInTrashCan(int id)
	{
		foreach (AbstractScreen item in mTrashCan)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public virtual void Draw()
	{
		foreach (AbstractScreen mScreen in mScreenList)
		{
			mScreen.Draw();
		}
	}

	public virtual void DrawGeometry()
	{
		if (mScreenList.Count != 0)
		{
			AbstractScreen abstractScreen = mScreenList[mScreenList.Count - 1];
			abstractScreen.DrawGeometry();
		}
	}

	public virtual void CalculatePhysics()
	{
		if (mScreenList.Count != 0)
		{
			AbstractScreen abstractScreen = mScreenList[mScreenList.Count - 1];
			abstractScreen.CalculatePhysics();
		}
	}

	public virtual void FromDemoToFull()
	{
		if (mScreenList.Count != 0)
		{
			AbstractScreen abstractScreen = mScreenList[mScreenList.Count - 1];
			abstractScreen.FromDemoToFull();
		}
	}
}
