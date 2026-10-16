using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperCow;

public class SuperCow : Game
{
	public static bool locked = false;

	private static PlayerIndex LockedController = (PlayerIndex)0;

	private static PlayerIndex CurrentController = (PlayerIndex)0;

	private string errorMsg = "";

	private int rspf;

	private int updateCount;

	private bool needLoad = true;

	public static bool isTrial = true;

	private static ManagedThread loadingThread;

	private static GraphicsDeviceManager graphics;

	private static ContentManager contentManager;

	private static KeyboardState keyState;

	private static WidgetManager mWidgetManager = null;

	private static ScreenManager mScreenManager = null;

	private static SpriteBatch mBatch = null;

	public static Settings settings = new Settings();

	private static bool IsSlowly = false;

	private static int tempTrophie = 0;

	public static int FixedDrawingWidth = 1280;

	public static int FixedDrawingHeight = 720;

	private static bool IsHDTVMode;

	public static bool needDraw = false;

	public static int n = 0;

	public static uint dwTime = 0u;

	public static float fps = 0f;

	public static int LastNumPolsPerFrame = 0;

	public static int LastNumDrawsPerFrame = 0;

	public static int LastNumObjsPerFrame = 0;

	public static PerfCounter pf = new PerfCounter();

	private static GamerServicesComponent gamerServicesComponent = null;

	private static float elapsedTimeThisFrameInMs = 0.001f;

	private static float totalTimeMs = 0f;

	private static float lastFrameTotalTimeMs = 0f;

	private static float lastFrameTime = 0f;

	private static int frameCountThisSecond = 0;

	private static int totalFrameCount = 0;

	private static int fpsLastSecond = 60;

	private static int updatesPerSecond = 0;

	private static int drawsPerSecond = 0;

	private static float startTimeThisSecond = 0f;

	private static float fpsInterpolated = 100f;

	private static float elapsedTimeThisFrameInMs2 = 0.001f;

	private static bool TwoFrameCounter = false;

	public static GamerServicesComponent GamerServicesComponent => gamerServicesComponent;

	public static GraphicsDeviceManager graphicsManager => graphics;

	public static float FPS => fps;

	public static Settings Settings => settings;

	public static PlayerIndex lockedController
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return LockedController;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			LockedController = value;
		}
	}

	public static PlayerIndex currentController
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return CurrentController;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			CurrentController = value;
		}
	}

	public static int getWidth => FixedDrawingWidth;

	public static int getHeight => FixedDrawingHeight;

	public static int TotalFrames => totalFrameCount;

	public static float ElapsedTimeThisFrameInMilliseconds => elapsedTimeThisFrameInMs;

	public static int Fps => fpsLastSecond;

	public static float TotalTime => totalTimeMs / 1000f;

	public static float TotalTimeMilliseconds => totalTimeMs;

	public static ContentManager ContentManager => contentManager;

	public static GraphicsDeviceManager DeviceManager => graphics;

	public static SpriteBatch GetBatch => mBatch;

	public static bool IsHDTV => IsHDTVMode;

	public static bool IsRunningSlowly => IsSlowly;

	public SuperCow()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		gamerServicesComponent = new GamerServicesComponent((Game)(object)this);
		((Collection<IGameComponent>)(object)((Game)this).Components).Add((IGameComponent)(object)gamerServicesComponent);
		graphics = new GraphicsDeviceManager((Game)(object)this);
		IsHDTVMode = GraphicsAdapter.DefaultAdapter.IsWideScreen;
		graphics.IsFullScreen = true;
		DisplayMode currentDisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
		if (((DisplayMode)(ref currentDisplayMode)).Height >= 720)
		{
			if (IsHDTVMode)
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
			else
			{
				graphics.PreferredBackBufferWidth = 960;
				graphics.PreferredBackBufferHeight = 720;
			}
		}
		else
		{
			GraphicsDeviceManager obj2 = graphics;
			DisplayMode currentDisplayMode2 = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			obj2.PreferredBackBufferWidth = ((DisplayMode)(ref currentDisplayMode2)).Width;
			GraphicsDeviceManager obj3 = graphics;
			DisplayMode currentDisplayMode3 = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			obj3.PreferredBackBufferHeight = ((DisplayMode)(ref currentDisplayMode3)).Height;
		}
		if (IsHDTVMode)
		{
			FixedDrawingWidth = 1280;
			FixedDrawingHeight = 720;
		}
		else
		{
			FixedDrawingWidth = 800;
			FixedDrawingHeight = 600;
		}
		graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
		((Game)this).Content.RootDirectory = "Content";
		CRand.randomize(0);
		mWidgetManager = new WidgetManager();
		mScreenManager = new ScreenManager(mWidgetManager);
	}

	public void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		if (Environment.OSVersion.Platform != PlatformID.Win32NT)
		{
			PresentationParameters presentationParameters = e.GraphicsDeviceInformation.PresentationParameters;
			e.GraphicsDeviceInformation.PresentationParameters.EnableAutoDepthStencil = true;
			e.GraphicsDeviceInformation.PresentationParameters.SwapEffect = (SwapEffect)1;
			presentationParameters.MultiSampleQuality = 0;
			if (graphics.PreferredBackBufferHeight == 720)
			{
				presentationParameters.MultiSampleType = (MultiSampleType)0;
				presentationParameters.PresentationInterval = (PresentInterval)1;
			}
			else
			{
				presentationParameters.MultiSampleType = (MultiSampleType)0;
				presentationParameters.PresentationInterval = (PresentInterval)1;
			}
		}
		else
		{
			e.GraphicsDeviceInformation.PresentationParameters.EnableAutoDepthStencil = true;
			e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = (PresentInterval)1;
			e.GraphicsDeviceInformation.PresentationParameters.AutoDepthStencilFormat = graphics.PreferredDepthStencilFormat;
			IsHDTVMode = true;
		}
	}

	public static AbstractScreen GetScreen(int id)
	{
		if (mScreenManager != null)
		{
			return mScreenManager.GetScreenByID(id);
		}
		return null;
	}

	public static AbstractScreen GetScreenInTrash(int id)
	{
		if (mScreenManager != null)
		{
			return mScreenManager.GetScreenByIDInTrashCan(id);
		}
		return null;
	}

	public static void AddScreen(AbstractScreen screen)
	{
		if (mScreenManager != null && screen != null)
		{
			mScreenManager.AddScreen(screen);
		}
	}

	public static void AddScreen(int id, bool activate)
	{
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		if (mScreenManager == null)
		{
			return;
		}
		AbstractScreen abstractScreen = GetScreenInTrash(id);
		if (abstractScreen != null)
		{
			mScreenManager.AddScreen(id, abstractScreen);
			if (activate)
			{
				abstractScreen.Activate();
			}
			return;
		}
		int num = 470;
		if (!IsHDTV)
		{
			num -= 120;
		}
		switch (id)
		{
		case 16:
			abstractScreen = new MainMenu(new Rectangle(num, 45, 300, 250), SCGlobals.menuSpeed, visible: false, dropDown: false);
			break;
		case 4:
			abstractScreen = new MenuScreen();
			break;
		case 10:
			abstractScreen = new TrophiesScreeen();
			break;
		case 7:
			abstractScreen = new MapScreen();
			break;
		case 22:
			abstractScreen = new HelpAndOptionsMenu(new Rectangle(num, 45, 300, 250), SCGlobals.menuSpeed, visible: false, dropDown: false);
			break;
		case 11:
			abstractScreen = new GameScreen();
			break;
		case 15:
			abstractScreen = new GameSettingsMenu(new Rectangle(200, 140, 400, 280), SCGlobals.menuSpeed, visible: false, dropDown: true);
			break;
		case 17:
			abstractScreen = new GameMenu(new Rectangle(num, 45, 300, 250), SCGlobals.menuSpeed, visible: false, dropDown: false);
			break;
		case 18:
			abstractScreen = new StartMenu(new Rectangle(100, 100, 400, 300), SCGlobals.menuSpeed, visible: false, dropDown: true);
			break;
		case 8:
			abstractScreen = new StartComicsScreen();
			break;
		case 14:
			abstractScreen = new StageCommics();
			break;
		case 9:
			abstractScreen = new CreditsScreen();
			break;
		case 5:
			abstractScreen = new UpsellScreen();
			break;
		case 1:
			abstractScreen = new CantBuyScreen();
			break;
		case 6:
			abstractScreen = new UnlockedScreen();
			break;
		case 0:
			abstractScreen = new MUError();
			break;
		case 23:
			abstractScreen = new GameOver(new Rectangle(100, 100, 400, 300), SCGlobals.menuSpeed, visible: false, dropDown: true);
			break;
		}
		mScreenManager.AddScreen(id, abstractScreen);
		if (activate)
		{
			abstractScreen.Activate();
		}
	}

	public static void AddScreen(int id, AbstractScreen screen)
	{
		if (mScreenManager != null && screen != null)
		{
			mScreenManager.AddScreen(id, screen);
		}
	}

	public void RemoveScreen(AbstractScreen screen, bool clear)
	{
		if (mScreenManager != null && screen != null)
		{
			mScreenManager.RemoveScreen(screen, clear);
		}
	}

	public static void PurgeInTrash(int id)
	{
		if (mScreenManager != null)
		{
			mScreenManager.PurgeInTrash(id);
		}
	}

	public static void RemoveScreen(int id, bool clear)
	{
		if (mScreenManager != null)
		{
			mScreenManager.RemoveScreen(id, clear);
		}
	}

	protected override void Initialize()
	{
		((Game)this).Initialize();
		Sound.Initialize();
		CowRender.InitShaders();
		CowRender.InitVertexDeclarations();
		Store.InitStore();
		settings.LoadGameSettings();
		XInputHelper.SetDelay(200);
		((Game)this).IsMouseVisible = true;
		graphicsManager.DeviceReset += graphics_DeviceReset;
		SCGlobals.lasttick = (uint)Environment.TickCount;
		Store.SetCurrentDirectory("txt\\us", theAdd: false);
		wstrstuff.LoadStringFile("strings.txt");
		wstrstuff.LoadStringFile("menutext.txt");
		wstrstuff.LoadStringFile("tips.txt");
		wstrstuff.LoadStringFile("introcomics.txt");
		wstrstuff.LoadStringFile("diary.txt");
	}

	protected override void LoadContent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		((Game)this).LoadContent();
		IGraphicsDeviceService val = (IGraphicsDeviceService)((Game)this).Services.GetService(typeof(IGraphicsDeviceService));
		GraphicsDevice graphicsDevice = val.GraphicsDevice;
		mBatch = new SpriteBatch(graphicsDevice);
		contentManager = new ContentManager((IServiceProvider)((Game)this).Services);
		SCGlobals.FontVerdana14 = ContentManager.Load<SpriteFont>("Content\\Fonts\\verdana14");
		SCGlobals.FontVerdana20 = ContentManager.Load<SpriteFont>("Content\\Fonts\\verdana20");
		SCGlobals.FontComicSans14 = ContentManager.Load<SpriteFont>("Content\\Fonts\\ComicSans");
		((Game)this).IsFixedTimeStep = false;
	}

	protected override void UnloadContent()
	{
	}

	protected void CalculateFPS(GameTime gameTime)
	{
		uint tickCount = (uint)Environment.TickCount;
		SCGlobals.tickperframe = (uint)gameTime.ElapsedGameTime.TotalMilliseconds;
		SCGlobals.lasttick = tickCount;
		if (!SCGlobals.b_stopphysics)
		{
			SCGlobals.TimeRemainder += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
		}
		SCGlobals.lasttime = tickCount;
		SCGlobals.frameinterp = (float)SCGlobals.TimeRemainder / (float)SCGlobals.ENGINE_FREQ;
		SCGlobals.ftickperframe = SCGlobals.engine_speed * (float)SCGlobals.tickperframe;
		if (SCGlobals.ftickperframe > 80f)
		{
			SCGlobals.ftickperframe = 80f;
		}
	}

	public void CalculatePhysics()
	{
		if (!SCGlobals.b_stopphysics)
		{
			SCGlobals.engine_frequency = (uint)((float)SCGlobals.ENGINE_FREQ * SCGlobals.engine_speed);
			SCGlobals.physics_speed = SCGlobals.engine_frequency / SCGlobals.ENGINE_FREQ;
			SCGlobals.frameinterp = 0f;
			if (mScreenManager != null)
			{
				mScreenManager.CalculatePhysics();
			}
			SCGlobals.lasttime = (uint)Environment.TickCount;
			SCGlobals.TimeRemainder = 0;
		}
	}

	protected override void Update(GameTime gameTime)
	{
		((Game)this).Update(gameTime);
		UpdateInput(gameTime);
		lastFrameTotalTimeMs = totalTimeMs;
		elapsedTimeThisFrameInMs = (float)gameTime.ElapsedRealTime.TotalMilliseconds;
		elapsedTimeThisFrameInMs2 += (float)gameTime.ElapsedRealTime.TotalMilliseconds;
		totalTimeMs += elapsedTimeThisFrameInMs;
		if (elapsedTimeThisFrameInMs <= 0f)
		{
			elapsedTimeThisFrameInMs = 0.001f;
		}
		frameCountThisSecond++;
		totalFrameCount++;
		if (totalTimeMs - startTimeThisSecond > 1000f)
		{
			fpsLastSecond = (int)((float)frameCountThisSecond * 1000f / (totalTimeMs - startTimeThisSecond));
			startTimeThisSecond = totalTimeMs;
			frameCountThisSecond = 0;
			fpsInterpolated = MathHelper.Lerp(fpsInterpolated, (float)fpsLastSecond, 0.1f);
		}
		updatesPerSecond++;
		SCGlobals.ftickperframe = (elapsedTimeThisFrameInMs + lastFrameTime) / 2f;
		SCGlobals.physics_speed = SCGlobals.ftickperframe / (float)SCGlobals.ENGINE_FREQ;
		SCGlobals.ftickperframe2 = elapsedTimeThisFrameInMs2;
		TwoFrameCounter = !TwoFrameCounter;
		if (!TwoFrameCounter)
		{
			SCGlobals.physics_speed2 = elapsedTimeThisFrameInMs2 / (float)SCGlobals.ENGINE_FREQ;
			elapsedTimeThisFrameInMs2 = 0f;
		}
		SCGlobals.tickperframe = (uint)gameTime.ElapsedGameTime.TotalMilliseconds;
		SCGlobals.engine_frequency = SCGlobals.tickperframe;
		SCGlobals.global_timer += SCGlobals.ftickperframe;
		SCGlobals.iglobal_time += (int)SCGlobals.tickperframe;
		lastFrameTime = SCGlobals.ftickperframe;
		SCGlobals.frameinterp = SCGlobals.physics_speed;
		if (SCGlobals.ftickperframe > 100f)
		{
			SCGlobals.ftickperframe = 100f;
		}
		if (((Game)this).IsActive)
		{
			_ = SCGlobals.supercowgame;
			Sound.Update();
			if (mWidgetManager != null)
			{
				mWidgetManager.Update(gameTime);
			}
			if (isTrial != Guide.IsTrialMode)
			{
				FromDemoToFull();
			}
			isTrial = Guide.IsTrialMode;
			if (SCGlobals.bGameLoaded && SCGlobals.bMUError && !SCGlobals.bMUErrorVisible && (SCGlobals.menusetk == 0f || SCGlobals.menusetk == 1f))
			{
				AddScreen(0, activate: true);
			}
			if (SCGlobals.b_exitGame)
			{
				((Game)this).Exit();
			}
		}
	}

	private void graphics_DeviceReset(object sender, EventArgs e)
	{
	}

	protected override void Draw(GameTime gameTime)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (!Guide.IsVisible)
		{
			graphics.GraphicsDevice.Clear((ClearOptions)3, Color.CornflowerBlue, 1f, 0);
		}
		else
		{
			graphics.GraphicsDevice.Clear((ClearOptions)2, Color.Red, 1f, 0);
		}
		try
		{
			drawsPerSecond++;
			if (mScreenManager != null)
			{
				mScreenManager.DrawGeometry();
			}
			if (mScreenManager != null && SCGlobals.b_pause)
			{
				((GameScreen)GetScreen(11))?.DrawGeometry();
			}
			if (mScreenManager != null)
			{
				mScreenManager.Draw();
			}
			LastNumPolsPerFrame = CowRender.PolygonsPerFrame;
			LastNumDrawsPerFrame = CowRender.DrawsPerFrame;
			LastNumObjsPerFrame = CowRender.ObjectPerFrame;
			rspf = SCGlobals.CountRenderStateChanges;
			CowRender.EndFrame();
		}
		catch (Exception ex)
		{
			errorMsg = ex.Message;
		}
		((Game)this).Draw(gameTime);
	}

	protected override void BeginRun()
	{
		AddScreen(2, new SplashScreen());
		((Game)this).BeginRun();
	}

	public void UpdateInput(GameTime updateTime)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		XInputHelper.Update((Game)(object)this, updateTime);
		for (int i = 0; i < 4; i++)
		{
			currentController = (PlayerIndex)i;
			if (!locked || lockedController == currentController)
			{
				if (XInputHelper.GamePads[(PlayerIndex)i].APressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyPress(GamePadKeys.A, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].XPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyPress(GamePadKeys.X, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].StartPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyPress(GamePadKeys.Start, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].BackPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyPress(GamePadKeys.Back, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].BPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyPress(GamePadKeys.B, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].LeftPressed || XInputHelper.GamePads[(PlayerIndex)i].ThumbLeftPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMove(GamePadKeys.Left, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].RightPressed || XInputHelper.GamePads[(PlayerIndex)i].ThumbRightPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMove(GamePadKeys.Right, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].UpPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMove(GamePadKeys.Up, -1);
				}
				if (XInputHelper.GamePads[(PlayerIndex)i].DownPressed && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMove(GamePadKeys.Down, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].UpPressedRepeat || XInputHelper.GamePads[(PlayerIndex)i].ThumbUpPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMoveRepeat(GamePadKeys.Up, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].DownPressedRepeat || XInputHelper.GamePads[(PlayerIndex)i].ThumbDownPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMoveRepeat(GamePadKeys.Down, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].LeftPressedRepeat || XInputHelper.GamePads[(PlayerIndex)i].ThumbLeftPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMoveRepeat(GamePadKeys.Left, -1);
				}
				if ((XInputHelper.GamePads[(PlayerIndex)i].RightPressedRepeat || XInputHelper.GamePads[(PlayerIndex)i].ThumbRightPressedRepeat) && mWidgetManager != null && mWidgetManager.focusedWidget != null)
				{
					mWidgetManager.focusedWidget.JoyMoveRepeat(GamePadKeys.Right, -1);
				}
			}
		}
	}

	public static void StartLevelLoadingThread()
	{
		if (mScreenManager != null)
		{
			MapScreen mapScreen = (MapScreen)GetScreen(7);
			loadingThread = new ManagedThread(4, ThreadPriority.Highest);
			loadingThread.AddTask(new ThreadTask(mapScreen.ThreadProc, mapScreen.ThreadFinished));
		}
	}

	public static void KillLevelLoadingThread()
	{
		if (loadingThread != null)
		{
			loadingThread.Kill();
		}
	}

	public static void GetCursorPoint(float x, float y)
	{
	}

	public void StopGame()
	{
		((Game)this).Exit();
	}

	public void FromDemoToFull()
	{
		mScreenManager.GetScreenByID(16)?.FromDemoToFull();
		if (mScreenManager != null)
		{
			mScreenManager.FromDemoToFull();
		}
		PlayerProfile.curlevelstage = 0;
		PlayerProfile.curlevel = 0;
		PlayerProfile.maxlevel = 0;
		PlayerProfile.maxlevelstage = 0;
	}

	public static void ShowMarketPlace()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (Guide.IsVisible)
		{
			return;
		}
		SignedInGamer val = Gamer.SignedInGamers[lockedController];
		if (val != null)
		{
			if (val.IsSignedInToLive)
			{
				if (!val.Privileges.AllowPurchaseContent)
				{
					AddScreen(1, activate: true);
				}
				else
				{
					Guide.ShowMarketplace(lockedController);
				}
			}
			else
			{
				Guide.ShowSignIn(1, true);
			}
		}
		else
		{
			Guide.ShowSignIn(1, true);
		}
	}
}
