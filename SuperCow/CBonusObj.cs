using System;
using System.Globalization;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CBonusObj : CLevelObj
{
	public float par1;

	public float par2;

	public float reanimation_time;

	public bool mReanimated;

	public override void Init()
	{
		if (extratextparam.Length > 0)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(extratextparam);
			reanimation_time = float.Parse(SuperCowHelper.StringFromByteArray(bytes), CultureInfo.InvariantCulture);
			reanimation_time *= 1000f;
		}
		else
		{
			reanimation_time = -1f;
		}
		mReanimated = false;
		base.Init();
	}

	public void DrawParticles(bool toBuffer)
	{
		float num = frandom1 * (float)Math.PI + obj_time * 0.005f;
		float num2 = (float)Math.Sin(num);
		float num3 = (num2 + 1f) * 0.5f;
		byte b = (byte)(num3 * 215f);
		Particles.AddParticle(color: new dcolor_t(b, b, b, byte.MaxValue), p: position, rot: num3 * 90f, size: 0.5f * num2 + 3f, blendtype: 0, num: 12);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(1);
		CowRender.EnableRS(7u);
		CowRender.BindTexture(SCGlobals.p_podkladka);
		if (toBuffer)
		{
			CowRender.DrawColorQuad2DDynBuffer(position, 5f, 5f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(105, 85, 5, byte.MaxValue), 0f);
		}
		else
		{
			CowRender.DrawColorQuad2D(position, 5f, 5f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(105, 85, 5, byte.MaxValue), 0f);
		}
		CowRender.DisableRS(7u);
		CowRender.SetBlend(0);
		if (IsObjFromTask(SCGlobals.curleveltask))
		{
			vec3_t p = new vec3_t(position);
			p.z = 200f;
			CowRender.SetBlend(1);
			CowRender.EnableRS(7u);
			Particles.DrawParticle(p, num * 10f, 8f, new dcolor_t(130, 130, 130, byte.MaxValue), 15, toBuffer);
			Particles.DrawParticle(p, (0f - num) * 10f, 8f, new dcolor_t(130, 130, 130, byte.MaxValue), 15, toBuffer);
			CowRender.SetBlend(0);
			CowRender.DisableRS(7u);
		}
	}

	public override void DrawStatic(dcolor_t dc)
	{
	}

	public override void Draw(dcolor_t dc)
	{
		if (!b_offscreen && b_switch == 0)
		{
			obj_time += SCGlobals.ftickperframe;
			vec2_t v = position;
			matrix_t matrix_t2 = default(matrix_t);
			matrix_t2.load_identity();
			matrix_t2.translate(v);
			matrix_t2.xrot(-90f);
			float num = frandom1 * 360f + obj_time * 0.1f;
			if (objID == 29 || objID == 30)
			{
				matrix_t2.zrot_delta(0f - num);
				matrix_t2.yrot_delta(num * 0.4f);
			}
			else if (objID == 32 || objID == 33)
			{
				matrix_t2.yrot_delta(20f * (float)Math.Sin(num * 0.01f));
				matrix_t2.zrot_delta(num);
			}
			else
			{
				matrix_t2.yrot_delta(20f * (float)Math.Sin(num * 0.01f));
				matrix_t2.zrot_delta(num);
			}
			if (p_sprite != null)
			{
				p_sprite.frametime = 40;
				p_sprite.timedelta = (int)(Math.Abs(frandom1) * 32f * 20f);
				p_sprite.Draw(position, -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
			}
			else
			{
				CowRender.BindTexture(p_texture);
				frect_t tc = (b_invertobj ? new frect_t(1f, 0f, 0f, 1f) : new frect_t(0f, 0f, 1f, 1f));
				CowRender.DrawColorQuad2DDynBuffer(frame_rect, tc, dc, 0f);
				GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
				graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
			}
		}
	}

	public override void Physics()
	{
		if (b_active == 0)
		{
			return;
		}
		if (b_switch != 0)
		{
			obj_time2 += SCGlobals.engine_frequency;
			if (obj_time2 > reanimation_time)
			{
				Particles.AddDynamicParticle(position, 26, 1, -1);
				mReanimated = true;
				b_switch = 0;
			}
		}
		else
		{
			if (SCGlobals.hero.behavior == behaviors_t.BT_DEATHFALL || SCGlobals.hero.behavior == behaviors_t.BT_DEATH || !SCGlobals.hero.bound_rect.isRectTouchRect(rect))
			{
				return;
			}
			b_active = 0;
			if (reanimation_time > 0f)
			{
				b_switch = byte.MaxValue;
				b_active = byte.MaxValue;
				obj_time2 = 0f;
			}
			int num = 7;
			if (objID == 29)
			{
				num = objsubtype;
				string sound = "bonuscoin" + (num + 1);
				Sound.Addsound(sound);
			}
			else if (objID == 32)
			{
				num = 3 + objsubtype;
				Sound.Addsound("bonusgem");
			}
			else if (objID == 31)
			{
				num = 6;
				Sound.Addsound("bonustreasure");
			}
			if (!mReanimated)
			{
				Bonus.AddScoreFromBonus(position, num);
			}
			SCGlobals.hero.numbonuses++;
			if (objID == 33)
			{
				SCGlobals.supercowgame.AddHeroLife();
				SCGlobals.supercowgame.AddHeroHealth(100f);
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup1"), 4);
				Sound.Addsound("poweruplife");
			}
			else if (name == "pack")
			{
				int num2 = SCGlobals.supercowgame.AddHeroHealth(15f);
				if (num2 > 0 && !mReanimated)
				{
					SCGlobals.supercowgame.AddScore(position + new vec2_t(0f, 1f), num2 * 10);
				}
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup2"), 4);
				Sound.Addsound("powerupkefir");
			}
			else if (name == "aid")
			{
				int num3 = SCGlobals.supercowgame.AddHeroHealth(30f);
				if (num3 > 0 && !mReanimated)
				{
					SCGlobals.supercowgame.AddScore(position + new vec2_t(0f, 1f), num3 * 10);
				}
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup3"), 4);
				Sound.Addsound("poweruphealth");
			}
			else if (name == "megajump")
			{
				SCGlobals.hero.megajumptimer += 20000f;
				if (SCGlobals.hero.megajumptimer > 40000f)
				{
					SCGlobals.hero.megajumptimer = 40000f;
				}
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup4"), 4);
				Sound.Addsound("powerupmegajump");
			}
			else if (name == "horseshoe")
			{
				SCGlobals.hero.hypershoetimer += 20000f;
				if (SCGlobals.hero.hypershoetimer > 40000f)
				{
					SCGlobals.hero.hypershoetimer = 40000f;
				}
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup5"), 4);
				Sound.Addsound("powerupshoe");
			}
			else if (name == "firmness")
			{
				SCGlobals.hero.firmnesstimer += 20000f;
				if (SCGlobals.hero.firmnesstimer > 40000f)
				{
					SCGlobals.hero.firmnesstimer = 40000f;
				}
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup6"), 4);
				Sound.Addsound("powerupfirmness");
			}
			else if (name == "key")
			{
				SCGlobals.levelstat.keys++;
				Particles.AddDynamicText(position, wstrstuff.GetStr("powerup7"), 4);
				Sound.Addsound("key");
			}
		}
	}
}
