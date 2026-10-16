using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

internal class TrophiesScreeen : AbstractScreen
{
	private enum WidgetList
	{
		BUTTON_BACK,
		WIDGET_COUNT
	}

	public float f_trophyroom;

	private texture_t mBackground;

	private Cue mMusic;

	public TrophiesScreeen()
	{
		SetupOrder();
	}

	public override void Draw()
	{
		f_trophyroom += SCGlobals.ftickperframe;
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.EnableRS(5u);
		graphicsDevice.RenderState.ReferenceAlpha = 1;
		CowRender.DisableRS(1u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(0u);
		CowRender.EnableRS(7u);
		CowRender.SetOrtho(800f, 600f, 1f, 500f);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 120f);
		CowRender.LoadMatrixNull();
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		graphicsDevice.RenderState.CullMode = (CullMode)1;
		TrophyRoom.CalculateTrophyRoom();
		TrophyRoom.DrawTrophyRoom();
		CowRender.EnableRS(7u);
		Particles.CalculateParticles();
		Particles.DrawParticles();
		CowRender.EnableRS(7u);
		CowRender.DisableRS(5u);
		CowRender.SetBlend(0);
		Balloon.DrawBalloonRects();
		base.Draw();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void Activate()
	{
		base.Activate();
		SCGlobals.bInTrophyRoom = true;
		if (!TrophyRoom.b_ingame)
		{
			mMusic = Sound.Play(Sounds.map);
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		if (!TrophyRoom.b_ingame)
		{
			Sound.Stop(mMusic);
		}
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (TrophyRoom.b_presentation)
		{
			if (button == GamePadKeys.A || button == GamePadKeys.B || button == GamePadKeys.Back || button == GamePadKeys.Start)
			{
				TrophyRoom.SkipWindow();
			}
			return;
		}
		if (button == GamePadKeys.B || button == GamePadKeys.Back)
		{
			ButtonPressed(0);
		}
		base.JoyPress(button, theID);
	}

	public override void JoyMoveRepeat(GamePadKeys button, int theID)
	{
		if (!TrophyRoom.b_presentation)
		{
			TrophyRoom.ProcessTrophyRoomCommand((uint)button);
		}
	}

	public override void ButtonPressed(int theID)
	{
		if (theID == 0)
		{
			Deactivate();
			SuperCow.Settings.SaveSettings();
			SuperCow.RemoveScreen(base.id, clear: true);
			SCGlobals.bInTrophyRoom = false;
			if (TrophyRoom.b_ingame)
			{
				SCGlobals.b_stopphysics = false;
				SCGlobals.b_pause = false;
				Balloon.visible = false;
			}
			else
			{
				SuperCow.AddScreen(4, activate: true);
				SuperCow.AddScreen(16, activate: true);
			}
		}
	}
}
