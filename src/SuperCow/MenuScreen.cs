using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class MenuScreen : AbstractScreen
{
	private Cue mMusic;

	private bool bFirstActivate = true;

	public MenuScreen()
	{
		CowRender.SetMenuLight();
	}

	public override void Draw()
	{
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(5u);
		// TODO: alpha test not implemented - 1f
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		CowRender.LoadMatrixNull();
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.DisableRS(1u);
		CowRender.DisableRS(0u);
		if (SuperCow.IsHDTV)
		{
			CowRender.SetOrtho(597.0149f, 447.76117f, 1f, 300f);
		}
		graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		graphicsDevice.Textures[0] = SuperCowGame.mainTex;
		SCGlobals.animbacks.DrawAnimateBacks();
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadMatrixNull();
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		float num = 0.8f;
		CowRender.SetOrtho(16f * num, 12f * num, 1f, 300f);
		CowRender.EnableRS(0u);
		CowRender.EnableRS(1u);
		CowRender.EnableRS(2u);
		CowRender.EnableRS(7u);
		CowRender.EnableRS(5u);
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		SCGlobals.heroMenu.animspeed = 0.8f;
		SCGlobals.heroMenu.Calculate();
		SCGlobals.heroMenu.matrix.load_identity();
		SCGlobals.heroMenu.matrix.translate(-3.6f * num, 0f, -2f * num);
		SCGlobals.heroMenu.matrix.zrot_delta(160f);
		SCGlobals.heroMenu.Draw();
		SCGlobals.p_banjocgo.Draw(SCGlobals.heroMenu.rotmatrix * SCGlobals.heroMenu.matrix);
		CowRender.DisableRS(0u);
		CowRender.DisableRS(1u);
		CowRender.DisableRS(2u);
		CowRender.DisableRS(7u);
		CowRender.DisableRS(5u);
		CowRender.LoadMatrixNull();
	}

	public override void Activate()
	{
		base.Activate();
		SCGlobals.animbacks.NewAnimateBackgroundColor(2);
		SCGlobals.b_gamemenu = false;
		SCGlobals.b_pause = false;
		SCGlobals.b_stopphysics = false;
		if (mMusic != null && !mMusic.IsPlaying)
		{
			mMusic = Sound.Play(Sounds.menu_music);
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Sound.Stop(mMusic);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public void SetMusic(Cue music)
	{
		mMusic = music;
	}
}
