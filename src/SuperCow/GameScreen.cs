using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SuperCow;

public class GameScreen : AbstractScreen
{
	private float mTimer = -10f;

	private Cue mMusic;

	public override void Activate()
	{
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void JoyMove(GamePadKeys button, int theID)
	{
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (button == GamePadKeys.Start && SCGlobals.hero.behavior != behaviors_t.BT_WOW && !SCGlobals.gLevelLoading)
		{
			SuperCow.AddScreen(17, activate: true);
			SCGlobals.b_gamemenu = true;
			SCGlobals.b_pause = true;
			SCGlobals.setpausetimer = 1f;
		}
	}

	public override void Update(GameTime gameTime)
	{
		SCGlobals.supercowgame.CalculateGame2();
		base.Update(gameTime);
	}

	public override void DrawGeometry()
	{
		SCGlobals.supercowgame.DrawGame();
		Text.DrawFunnyTexts();
		Balloon.DrawBalloonRects();
	}

	public override void CalculatePhysics()
	{
		base.CalculatePhysics();
	}

	public override void FromDemoToFull()
	{
		Deactivate();
		SCGlobals.supercowgame.FromDemoToFull();
		SCGlobals.supercowgame.FreeGame();
		SCGlobals.supercowgame = null;
		base.FromDemoToFull();
	}
}
