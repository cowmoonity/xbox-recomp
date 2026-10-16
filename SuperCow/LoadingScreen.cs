using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using SuperCow.Properties;

namespace SuperCow;

public class LoadingScreen : AbstractScreen, ThreadInterface
{
	private texture_t textureBackground;

	private texture_t loadingTex;

	private texture_t loadingTex2;

	private float mTimer;

	private float mAlpha;

	private bool transitionInProgress = true;

	private bool mLoaded;

	private ManagedThread loadingThread;

	public Cue mMusic;

	public void ThreadFinished(int taskId)
	{
		mLoaded = true;
	}

	public void ThreadFinished()
	{
	}

	public void ThreadProc()
	{
		try
		{
			SCGlobals.texturehost.LoadAllTextures();
			GlowFXHost.gfxhLoad();
			Text.LoadFont("font");
			Text.LoadFont("font2");
			Text.SetFont(0);
			Store.SetCurrentDirectory("", theAdd: false);
			Particles.InitParticles();
			Comics.ComicsFileReset();
			Store.SetCurrentDirectory("", theAdd: false);
			SCGlobals.animbacks.InitAnimateBacks();
			SCGlobals.animbacks.InitAnimateBacksAtInitLevel(0);
			Store.SetCurrentDirectory("animbacks", theAdd: false);
			Store.SetCurrentDirectory("characters", theAdd: false);
			SCGlobals.heroMenu.Load("cow.chr");
			SCGlobals.pershost.LoadAll();
			SCGlobals.heroMenu.Refresh();
			SCGlobals.hero.Load("cow.chr");
			SCGlobals.hero.Refresh();
			SCGlobals.heroMenu.SetAnimStage(SCGlobals.heroMenu.p_banjo, 10f);
			SCGlobals.heroMenu.animspeed = 2f;
			SCGlobals.p_banjocgo = SCGlobals.geomobjhost.Load("banjo.cgo");
			for (int i = 0; i < 3; i++)
			{
				SCGlobals.menugem[i] = new CSprite();
			}
			SCGlobals.menugem[0].Load("coin3.tga", 32, 8);
			SCGlobals.menugem[0].SetParams(40f, 40f, 30);
			SCGlobals.menugem[1].Load("diamond01.tga", 32, 8);
			SCGlobals.menugem[1].SetParams(40f, 40f, 30);
			SCGlobals.menugem[2].Load("gold.tga", 32, 8);
			SCGlobals.menugem[2].SetParams(40f, 40f, 30);
			SCGlobals.p_magglass = SCGlobals.texturehost.Load("magglass.tga");
			CGameLevel.SetupGameObj();
			Store.SetCurrentDirectory("", theAdd: false);
			SCGlobals.flaretexture = SCGlobals.texturehost.Load("gradient.tga");
			Store.SetCurrentDirectory("ui", theAdd: false);
			SCGlobals.p_menuballoon = SCGlobals.texturehost.LoadTGAJPG("_a_baloon.jpg", b_errormsg: true, b_notusealpha: false);
			SCGlobals.p_menutex = SCGlobals.texturehost.LoadTGAJPG("_a_menutex.jpg", b_errormsg: true, b_notusealpha: false);
			Store.SetCurrentDirectory("levelobjs", theAdd: false);
			SCGlobals.p_white = SCGlobals.texturehost.Load("white.jpg");
			Store.SetCurrentDirectory("", theAdd: false);
			SCGlobals.p_happyendback = SCGlobals.texturehost.Load("happyendback.jpg");
			SCGlobals.p_happyend0 = SCGlobals.texturehost.Load("happyend0.jpg");
			SCGlobals.p_happyend1 = SCGlobals.texturehost.Load("happyend1.jpg");
			SCGlobals.p_a = SCGlobals.texturehost.Load("a.png");
			SCGlobals.p_b = SCGlobals.texturehost.Load("b.png");
			SCGlobals.p_x = SCGlobals.texturehost.Load("x.png");
			SCGlobals.p_start = SCGlobals.texturehost.Load("start.png");
			CGameLevel.InitLevelBacks();
			CLevelObj.LoadLevelObjStuff();
			SuperCowHelper.SetWinTexCoord(0, 0f, 0f, 1f, 1f);
			SuperCowHelper.SetWinTexCoord(1, 1f, 0f, 1f, 1f);
			SuperCowHelper.SetWinTexCoord(2, 0f, 1f, 1f, 1f);
			SuperCowHelper.SetWinTexCoord(3, 1f, 1f, 1f, 1f);
			SuperCowHelper.SetWinTexCoord(4, 2f, 0f, 1f, 1f);
			SuperCowHelper.SetWinTexCoord(5, 3f, 0f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(0, 0f, 0f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(1, 1f, 0f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(2, 0f, 1f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(3, 1f, 1f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(4, 0f, 2f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(5, 1f, 2f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(6, 2f, 0f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(7, 3f, 0f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(8, 2f, 1f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(9, 3f, 1f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(10, 0f, 3f, 2f, 1f);
			SuperCowHelper.SetMITexCoord(11, 0f, 4f, 2f, 1f);
			SuperCowHelper.SetMITexCoord(12, 2f, 3f, 1f, 2f);
			SuperCowHelper.SetMITexCoord(13, 3f, 3f, 1f, 2f);
			SuperCowHelper.SetMITexCoord(14, 0f, 7f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(15, 1f, 7f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(16, 0f, 5f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(17, 0f, 6f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(19, 3f, 5f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(18, 2f, 5f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(20, 1f, 6f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(21, 1f, 5f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(22, 3f, 7f, 1f, 1f);
			SuperCowHelper.SetMITexCoord(23, 2f, 7f, 1f, 1f);
			GameMap.GameMapInit();
			LoadCredits();
			TrophyRoom.InitTrophiesConf();
			TrophyRoom.InitTrophiesTex();
		}
		catch
		{
			SCGlobals.b_exitGame = true;
		}
	}

	public LoadingScreen()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		textureBackground = SCGlobals.texturehost.Load("loadscreen.jpg");
		loadingTex = SCGlobals.texturehost.Load("loadbar.png");
		loadingTex2 = SCGlobals.texturehost.Load("press_a.png");
		loadingThread = new ManagedThread(4, ThreadPriority.Highest);
		loadingThread.AddTask(new ThreadTask(ThreadProc, ThreadFinished));
		mMusic = Sound.Play(Sounds.menu_music);
	}

	public override void ShutDown()
	{
		base.ShutDown();
	}

	public override void Draw()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		SCGlobals.cam.load_identity();
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.BindTexture(textureBackground);
		CowRender.DrawColorQuad2D(textureBackground.mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)mAlpha), 10f);
		new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)mAlpha);
		float num = SCGlobals.mProgress / 6462952f;
		if (num > 1f)
		{
			num = 1f;
		}
		CowRender.BindTexture(loadingTex);
		frect_t vc = default(frect_t);
		float num2 = 310f;
		float num3 = 48f;
		float num4 = 6f;
		float num5 = -217f;
		vc.assign(num4 - num2 / 2f, num5, num4 - num2 / 2f + num * num2, num5 - num3);
		CowRender.DrawColorQuad2D(vc, new frect_t(0.001f, 0.006f, num * num2 / 310f, num3 / 48f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)mAlpha), 10f);
		if (!transitionInProgress && mLoaded)
		{
			CowRender.BindTexture(loadingTex2);
			CowRender.DrawColorQuad2D(loadingTex2.mImage, 0f, -240f, 310f, 48f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		}
	}

	public override void ButtonPressed(int theID)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (!transitionInProgress && mLoaded && theID == -1)
		{
			bool flag = false;
			SignedInGamer val = Gamer.SignedInGamers[SuperCow.currentController];
			if (val == null || !val.IsSignedInToLive)
			{
				flag = true;
			}
			if (flag)
			{
				Guide.ShowSignIn(1, true);
				return;
			}
			SuperCow.lockedController = SuperCow.currentController;
			SuperCow.locked = true;
			GameSettings.Initialize();
			SuperCow.settings.LoadSettings();
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.AddScreen(4, activate: true);
			SuperCow.AddScreen(16, activate: true);
			MenuScreen menuScreen = (MenuScreen)SuperCow.GetScreen(4);
			menuScreen.SetMusic(mMusic);
			SCGlobals.bGameLoaded = true;
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (transitionInProgress)
		{
			mTimer += gameTime.ElapsedGameTime.Milliseconds;
			float num = mTimer / 3000f;
			if (num > 1f)
			{
				num = 1f;
			}
			mAlpha = num * num * 255f;
			if (mTimer >= 3000f)
			{
				transitionInProgress = false;
				SuperCow.RemoveScreen(2, clear: true);
				loadingThread.Kill();
			}
		}
	}

	public void ExitGame()
	{
	}

	public void LoadCredits()
	{
		SCGlobals.gamecredits[0].position = "SUPERCOW";
		SCGlobals.gamecredits[0].name = "a nevosoft game";
		SCGlobals.gamecredits[1].text = ":";
		SCGlobals.gamecredits[1].persname = "donkey";
		SCGlobals.gamecredits[1].position = "Directed by";
		SCGlobals.gamecredits[1].name = "ALEXEY SEREBROV";
		SCGlobals.gamecredits[2].text = ":";
		SCGlobals.gamecredits[2].persname = "goat";
		SCGlobals.gamecredits[2].position = "Lead Artist";
		SCGlobals.gamecredits[2].name = "YURI VORONOV";
		SCGlobals.gamecredits[3].text = "As Kevin the Goose";
		SCGlobals.gamecredits[3].persname = "goose";
		SCGlobals.gamecredits[3].position = "Head Designer";
		SCGlobals.gamecredits[3].name = "ARTEM BONDARENKO";
		SCGlobals.gamecredits[4].text = "As Marrie the Hen";
		SCGlobals.gamecredits[4].persname = "han";
		SCGlobals.gamecredits[4].position = "Models and Animation";
		SCGlobals.gamecredits[4].name = "АLEXANDRA RYABCHIKOVA";
		SCGlobals.gamecredits[5].text = ":";
		SCGlobals.gamecredits[5].persname = "rabbit";
		SCGlobals.gamecredits[5].position = "Artist and Designer";
		SCGlobals.gamecredits[5].name = "ANASTASIA BORZYKH";
		SCGlobals.gamecredits[6].text = ":";
		SCGlobals.gamecredits[6].persname = "sheep";
		SCGlobals.gamecredits[6].position = "Artist";
		SCGlobals.gamecredits[6].name = "KSENIA MAMAEVA";
		SCGlobals.gamecredits[7].text = ":";
		SCGlobals.gamecredits[7].persname = "pig";
		SCGlobals.gamecredits[7].position = "Music by";
		SCGlobals.gamecredits[7].name = "MICHAEL IVASHCHENKO";
		SCGlobals.gamecredits[8].text = ":";
		SCGlobals.gamecredits[8].persname = "chicken";
		SCGlobals.gamecredits[8].position = "Producer";
		SCGlobals.gamecredits[8].name = "VITALY ROMANOV";
		SCGlobals.gamecredits[9].text = ":";
		SCGlobals.gamecredits[9].persname = "cock";
		SCGlobals.gamecredits[9].position = "Co-Producer";
		SCGlobals.gamecredits[9].name = "NICK OSHURKOV";
		SCGlobals.gamecredits[10].position = "ALSO APPEARING";
		SCGlobals.gamecredits[10].name = "";
		SCGlobals.gamecredits[11].position = "Written by";
		SCGlobals.gamecredits[11].name = "ALEXEY SEREBROV";
		SCGlobals.gamecredits[12].position = "Level Designers";
		SCGlobals.gamecredits[12].name = "ANASTASIA BORZYKH\nYURI VORONOV\nVALERY NOSOV";
		SCGlobals.gamecredits[13].position = "Programming";
		SCGlobals.gamecredits[13].name = "ALEXEY SEREBROV\nALEXANDER VASILEV\nSERGE SMIRNOV";
		SCGlobals.gamecredits[14].position = "Testers";
		SCGlobals.gamecredits[14].name = "DANILA VORONOV\nSERGEY SELEZNEV";
		SCGlobals.gamecredits[15].position = "Sound Producers";
		SCGlobals.gamecredits[15].name = "EUGENE MAXIYANOV\nKONSTANTIN ELGAZIN";
		SCGlobals.gamecredits[16].position = "VO Production";
		SCGlobals.gamecredits[16].name = "RICHARD WAWMAN\nBEN YUONG";
		SCGlobals.gamecredits[17].position = "VO cast";
		SCGlobals.gamecredits[17].name = "BETH CHARMES\nPAUL SILK\nIAN SWANN\nNICK OSHURKOV";
		SCGlobals.gamecredits[18].position = "Creative Assistant";
		SCGlobals.gamecredits[18].name = "PAVEL RIAIKKONEN";
		SCGlobals.gamecredits[19].position = "With the help and support of";
		SCGlobals.gamecredits[19].name = "OUR FAMILY AND FRIENDS";
		SCGlobals.gamecredits[20].position = "And of course";
		SCGlobals.gamecredits[20].name = "ORDER SLUNOPIVEC";
		SCGlobals.gamecredits[21].position = "NO ANIMALS WERE HARMED";
		SCGlobals.gamecredits[21].name = "IN THE MAKING OF THIS GAME";
		SCGlobals.gamecredits[22].position = "25 barrels of tea and\n4 bottles of champagne";
		SCGlobals.gamecredits[22].name = "were consumed during the production of this game";
		SCGlobals.gamecredits[23].position = "3500 kilowatts of energy\nand a lot of nerves";
		SCGlobals.gamecredits[23].name = "were spent during the production of this game";
		SCGlobals.gamecredits[24].position = "DO NOT ATTEMPT THIS AT HOME";
		SCGlobals.gamecredits[24].name = "";
		SCGlobals.gamecredits[25].position = "WE LOVE YOU!";
		SCGlobals.gamecredits[25].name = "";
		SCGlobals.gamecredits[26].position = "XNA port by CTXM";
		SCGlobals.gamecredits[26].name = "";
		SCGlobals.gamecredits[27].position = "Programming";
		SCGlobals.gamecredits[27].name = "ALEXEY VOLKOV\nALEXANDER SHIROV";
		SCGlobals.gamecredits[28].position = "Artist";
		SCGlobals.gamecredits[28].name = "GUNAARS MIEZIS";
		SCGlobals.gamecredits[29].position = "Testing";
		SCGlobals.gamecredits[29].name = "IGOR DEVEL\nMIKHAIL GUBKO";
		SCGlobals.gamecredits[30].position = "All rights reserved © 2007";
		SCGlobals.gamecredits[30].name = "NEVOSOFT";
		SCGlobals.gamecredits[31].position = "Press    to continue";
		SCGlobals.gamecredits[31].name = "";
	}
}
