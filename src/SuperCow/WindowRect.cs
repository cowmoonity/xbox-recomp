using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class WindowRect
{
	private frect_t mBounds;

	private bool mDrawBack = true;

	public WindowRect()
	{
		mDrawBack = true;
	}

	public void SetBack(bool back)
	{
		mDrawBack = back;
	}

	public void SetRect(frect_t rect)
	{
		mBounds = rect;
	}

	public void Draw()
	{
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t vec3_t4 = default(vec3_t);
		vec3_t vec3_t5 = default(vec3_t);
		vec3_t2.assign(mBounds.x0, mBounds.y0, 35f);
		vec3_t3.assign(mBounds.x0 + mBounds.x1, mBounds.y0, 35f);
		vec3_t4.assign(mBounds.x0, mBounds.y0 - mBounds.y1, 35f);
		vec3_t5.assign(mBounds.x0 + mBounds.x1, mBounds.y0 - mBounds.y1, 35f);
		float num = Math.Abs(vec3_t2.x - vec3_t3.x);
		float num2 = Math.Abs(vec3_t2.y - vec3_t4.y);
		CowRender.SetBlend(0);
		vec3_t2.z = 40f;
		vec3_t3.z = 40f;
		vec3_t4.z = 40f;
		vec3_t5.z = 40f;
		frect_t vc = default(frect_t);
		vc.assign(mBounds.x0, mBounds.y0, mBounds.x0 + mBounds.x1, mBounds.y0 - mBounds.y1);
		if (mDrawBack)
		{
			CowRender.BindTexture(SCGlobals.p_white);
			frect_t tc = mBounds;
			tc.assign(0f, 0f, vc.sizex() / 128f, vc.sizey() / 208f);
			CowRender.DrawColorQuad2D(vc, tc, new dcolor_t(55, 15, 0, 200), 40f);
		}
		CowRender.BindTexture(SCGlobals.p_menutex);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// TODO: using class definitions from some fuckass static game class not good
		graphicsDevice.SamplerStates[0] = SuperCowGame.AnisotropicMirror;
		CowRender.DrawLineMI(4, vec3_t2, vec3_t3);
		CowRender.DrawLineMI(5, vec3_t2, vec3_t4);
		CowRender.DrawLineMI(5, vec3_t3, vec3_t5);
		CowRender.DrawLineMI(4, vec3_t4, vec3_t5);
		CowRender.DrawMI(0, vec3_t2);
		CowRender.DrawMI(1, vec3_t3);
		CowRender.DrawMI(2, vec3_t4);
		CowRender.DrawMI(3, vec3_t5);
		if (num > 192f)
		{
			CowRender.DrawMI(10, (vec3_t2 + vec3_t3) / 2f);
			CowRender.DrawMI(11, (vec3_t4 + vec3_t5) / 2f);
		}
		if (num2 > 192f)
		{
			CowRender.DrawMI(12, (vec3_t2 + vec3_t4) / 2f);
			CowRender.DrawMI(13, (vec3_t3 + vec3_t5) / 2f);
		}
		graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
	}
}
