using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CTextureHost
{
	public struct texcoords
	{
		public float ux;

		public float uy;
	}

	public static Dictionary<string, List<texture_t>> texList;

	private int numtextures;

	private texture_t[] ar_texture = new texture_t[1024];

	public CTextureHost()
	{
		numtextures = 0;
	}

	public void ReleaseVideoMemory()
	{
	}

	public void RestoreVideoMemory()
	{
	}

	public texture_t GetByName(string name)
	{
		return GetTex(name);
	}

	public texture_t GetTex(string name)
	{
		if (texList == null)
		{
			return default(texture_t);
		}
		name = name.ToUpper();
		if (texList.ContainsKey(name))
		{
			return texList[name][0];
		}
		return default(texture_t);
	}

	public int GetHostSize()
	{
		return numtextures;
	}

	public void LoadAllTextures()
	{
		texList = new Dictionary<string, List<texture_t>>();
		Texture2D val = SuperCow.ContentManager.Load<Texture2D>("content\\data\\textures");
		for (int i = 0; i < 243; i++)
		{
			texture_t item = default(texture_t);
			item.width = SCGlobals.gtexlist.textures[i].width;
			item.height = SCGlobals.gtexlist.textures[i].height;
			item.b_alpha = true;
			item.b_render = false;
			item.name = SCGlobals.gtexlist.textures[i].name;
			item.mImage = val;
			item.p_texD3D = (Texture)(object)val;
			item.tex_coord0.ux = (float)SCGlobals.gtexlist.textures[i].ux / (float)val.Width;
			item.tex_coord0.uy = (float)SCGlobals.gtexlist.textures[i].uy / (float)val.Height;
			item.tex_coord1.ux = (float)(SCGlobals.gtexlist.textures[i].ux + item.width) / (float)val.Width;
			item.tex_coord1.uy = (float)(SCGlobals.gtexlist.textures[i].uy + item.height) / (float)val.Height;
			List<texture_t> list = new List<texture_t>();
			list.Add(item);
			texList.Add(SCGlobals.gtexlist.textures[i].name.ToUpper(), list);
		}
	}

	public texture_t Load(string name)
	{
		return Load(name, b_errormsg: true, b_forsealpha: false, 2);
	}

	public texture_t Load(string name, bool b_errormsg, bool b_forsealpha, int miplevels)
	{
		texture_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		if (name.Contains(".tga") || name.Contains(".TGA"))
		{
			string text = "_a_" + name;
			text = text.Substring(0, text.IndexOf('.'));
			text += ".jpg";
			byName = LoadTGAJPG(text, b_errormsg: false, b_notusealpha: false);
			if (byName.name != null)
			{
				return byName;
			}
			text = "_a_" + name;
			text = text.Substring(0, text.IndexOf('.'));
			text += ".png";
			byName = LoadTGAJPG(text, b_errormsg: false, b_notusealpha: false);
			if (byName.name != null)
			{
				return byName;
			}
		}
		Texture2D val = Store.LoadTexture2D(name);
		if (val == null)
		{
			return default(texture_t);
		}
		byName = AddTexture(name);
		byName.p_texD3D = (Texture)(object)val;
		byName.mImage = val;
		byName.height = (uint)val.Height;
		byName.width = (uint)val.Width;
		byName.b_alpha = (b_forsealpha ? true : false);
		byName.b_render = false;
		byName.tex_coord0.ux = 0f;
		byName.tex_coord0.uy = 0f;
		byName.tex_coord1.ux = 1f;
		byName.tex_coord1.uy = 1f;
		ar_texture[numtextures - 1] = byName;
		return byName;
	}

	public texture_t LoadTGAJPGWithoutScale(string name, int width, int height)
	{
		GetByName(name);
		texture_t result = SCGlobals.texturehost.Create(name, (uint)get_nearest_2Sq(width), (uint)get_nearest_2Sq(height), (SurfaceFormat)1, render: false, theFake: true);
		Texture2D val = Store.LoadTexture2D(name);
		if (val == null)
		{
			return default(texture_t);
		}
		int len = 0;
		byte[] array = Store.storeLoadFileArray(name + ".a", ref len, b_errorexit: false);
		if (len > 0)
		{
			SuperCowHelper.DebugOut("INFO: merging alpha channel for " + name);
			ColorDataReader[] array2 = new ColorDataReader[val.Height * val.Width];
			val.GetData<ColorDataReader>(array2);
			for (int i = 0; i < array2.Length; i++)
			{
				if (BitConverter.IsLittleEndian)
				{
					array2[i].a = array[i];
				}
				else
				{
					array2[i].r = array[i];
				}
			}
			val.SetData<ColorDataReader>(array2);
		}
		result.p_texD3D = (Texture)(object)val;
		result.mImage = val;
		return result;
	}

	public texture_t LoadTGAJPG(string name, bool b_errormsg, bool b_notusealpha)
	{
		texture_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		Texture2D val = Store.LoadTexture2D(name);
		if (val == null)
		{
			return byName;
		}
		byName.width = (uint)val.Width;
		byName.height = (uint)val.Height;
		byName.b_alpha = true;
		byName.b_render = false;
		byName.tex_coord0.ux = 0f;
		byName.tex_coord0.uy = 0f;
		byName.tex_coord1.ux = 1f;
		byName.tex_coord1.uy = 1f;
		if (!b_notusealpha)
		{
			int len = 0;
			byte[] array = new byte[1];
			try
			{
				array = Store.storeLoadFileArray(name + ".a", ref len, b_errorexit: false);
			}
			catch
			{
				SuperCowHelper.DebugOut("ERROR: file not found :" + name);
			}
			if (len > 0)
			{
				SuperCowHelper.DebugOut("INFO: merging alpha channel for " + name);
				ColorDataReader[] array2 = new ColorDataReader[val.Height * val.Width];
				val.GetData<ColorDataReader>(array2);
				for (int i = 0; i < array2.Length; i++)
				{
					if (BitConverter.IsLittleEndian)
					{
						array2[i].a = array[i];
					}
					else
					{
						array2[i].r = array[i];
					}
				}
				val.SetData<ColorDataReader>(array2);
			}
		}
		byName.name = name;
		byName.mImage = val;
		byName.p_texD3D = (Texture)(object)val;
		return byName;
	}

	public texture_t LoadTextureWithoutScale(string name, int width, int height)
	{
		texture_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		byName = SCGlobals.texturehost.Create(name, (uint)get_nearest_2Sq(width), (uint)get_nearest_2Sq(height), (SurfaceFormat)1, render: false, theFake: true);
		Texture2D val = Store.LoadTexture2D(name);
		if (val == null)
		{
			return byName;
		}
		byName.mImage = val;
		byName.p_texD3D = (Texture)(object)val;
		return byName;
	}

	public texture_t Create(string name, uint width, uint height, SurfaceFormat d3dformat, bool render, bool theFake)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		texture_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		Texture2D p_texD3D = (theFake ? ((Texture2D)null) : new Texture2D(graphicsDevice, (int)width, (int)height, 1, (TextureUsage)0, d3dformat));
		texture_t result = AddTexture(name);
		result.p_texD3D = (Texture)(object)p_texD3D;
		result.height = height;
		result.width = width;
		result.format = d3dformat;
		result.b_render = render;
		ar_texture[numtextures - 1] = byName;
		return result;
	}

	public int get_nearest_2Sq(int n)
	{
		int num = 1;
		for (int i = 2; i < 12; i++)
		{
			num *= 2;
			if (n <= num)
			{
				return num;
			}
		}
		return 0;
	}

	public texture_t AddTexture(string name)
	{
		if (numtextures == 1023)
		{
			return default(texture_t);
		}
		texture_t texture_t2 = new texture_t
		{
			name = name.ToUpper()
		};
		ar_texture[numtextures] = texture_t2;
		numtextures++;
		return texture_t2;
	}
}
