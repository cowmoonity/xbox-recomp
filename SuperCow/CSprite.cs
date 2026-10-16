using System;
using System.IO;

namespace SuperCow;

public class CSprite
{
	public texture_t[] ar_spriteframes = new texture_t[128];

	public texture_t p_texture;

	public bool b_singletex;

	public int timedelta;

	public int frametime;

	public int numframes;

	public int numframesx;

	public float sizex;

	public float sizey;

	public string name;

	public bool fading;

	public CSprite()
	{
		p_texture = default(texture_t);
		b_singletex = true;
		timedelta = 0;
		frametime = 30;
		numframes = 32;
		sizex = 64f;
		sizey = 64f;
		fading = false;
	}

	public bool Load(string name, int nframes, int nframesx)
	{
		timedelta = 0;
		frametime = 30;
		sizex = 64f;
		sizey = 64f;
		Store.SetCurrentDirectory("", theAdd: false);
		Store.SetCurrentDirectory("sprites", theAdd: false);
		p_texture = SCGlobals.texturehost.Load(name, b_errormsg: false, b_forsealpha: false, 2);
		if (p_texture.name != null)
		{
			b_singletex = true;
			numframesx = (int)p_texture.width / 64;
			numframes = numframesx * (int)p_texture.height / 64;
			if (p_texture.width > 64)
			{
				p_texture.width /= (uint)numframesx;
				p_texture.height /= (uint)(numframes / numframesx);
			}
		}
		else
		{
			b_singletex = false;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
			string extension = Path.GetExtension(name);
			numframes = -1;
			Store.SetCurrentDirectory(fileNameWithoutExtension, theAdd: true);
			do
			{
				numframes++;
				string text = fileNameWithoutExtension + $"{numframes:0000}" + extension;
				ref texture_t reference = ref ar_spriteframes[numframes];
				reference = SCGlobals.texturehost.Load(text, b_errormsg: false, b_forsealpha: false, 2);
			}
			while (ar_spriteframes[numframes].p_texD3D != null);
			p_texture = ar_spriteframes[0];
		}
		Store.SetCurrentDirectory("", theAdd: false);
		if (numframes > 0 || b_singletex)
		{
			sizex = (float)p_texture.width / 32f;
			sizey = (float)p_texture.height / 32f;
			return true;
		}
		return false;
	}

	public void SetParams(float _sizex, float _sizey, int _frametime)
	{
		sizex = _sizex;
		sizey = _sizey;
		frametime = _frametime;
	}

	public void Draw(vec2_t pos, int frame, dcolor_t dc, float addsize, float angle)
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		if (p_texture.p_texD3D == null)
		{
			return;
		}
		int num;
		if (frame == -1)
		{
			num = (SCGlobals.iglobal_time + timedelta) / frametime;
			num %= numframes;
		}
		else
		{
			num = frame;
		}
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.EnableRS(7u);
		if (!b_singletex)
		{
			if (fading)
			{
				dcolor_t c = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				dcolor_t c2 = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				float num2 = (float)SCGlobals.iglobal_time / (float)frametime * 100f;
				if ((int)num2 % 100 > 50 && (int)num2 % 100 < 65)
				{
					c.a = (byte)(255f - Math.Abs(50f - (float)((int)num2 % 100)) * 15f);
					if (c.a < 0)
					{
						c.a = 0;
					}
					CowRender.BindTexture(ar_spriteframes[num]);
					CowRender.DrawColorQuad2DDynBuffer(ar_spriteframes[num].mImage, pos.x, pos.y, sizex + addsize, sizey + addsize, c, 0f);
				}
				else
				{
					if ((int)num2 % 100 >= 65 && (int)num2 % 100 < 85)
					{
						return;
					}
					if ((int)num2 % 100 >= 85)
					{
						c2.a = (byte)(255f - (float)((100 - (int)num2 % 100) * 15));
						if (c2.a < 0)
						{
							c2.a = 0;
						}
						CowRender.BindTexture(ar_spriteframes[(num + 1) % numframes]);
						CowRender.DrawColorQuad2DDynBuffer(ar_spriteframes[(num + 1) % numframes].mImage, pos.x, pos.y, sizex + addsize, sizey + addsize, c2, 0f);
					}
					else
					{
						CowRender.BindTexture(ar_spriteframes[num]);
						CowRender.DrawColorQuad2DDynBuffer(ar_spriteframes[num].mImage, pos.x, pos.y, sizex + addsize, sizey + addsize, c, 0f);
					}
				}
			}
			else
			{
				CowRender.BindTexture(ar_spriteframes[num]);
				CowRender.DrawColorQuad2D(ar_spriteframes[num].mImage, pos.x, pos.y, sizex + addsize, sizey + addsize, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
		}
		else
		{
			int num3 = num % numframesx;
			int num4 = num / numframesx;
			float num5 = 1f / (float)numframesx;
			float num6 = 1f / (float)(numframes / numframesx);
			float num7 = sizex + addsize;
			frect_t tc = new frect_t((float)num3 * num5, (float)num4 * num6, (float)num3 * num5 + num5, (float)num4 * num6 + num6);
			CowRender.EnableRS(4u);
			CowRender.SetBlend(0);
			CowRender.BindTexture(p_texture);
			if (angle == 0f)
			{
				CowRender.DrawColorQuad2DDynBuffer(pos, num7, num7, tc, dc, 0f);
			}
			else
			{
				CowRender.DrawRotatedColorQuad2DDynBuffer(pos, pos, angle, num7, num7, tc, dc, 0f);
			}
		}
	}
}
