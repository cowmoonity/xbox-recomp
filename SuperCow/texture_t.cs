using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct texture_t
{
	public bool exists;

	public string name;

	public uint width;

	public uint height;

	public Texture p_texD3D;

	public bool b_alpha;

	public bool b_render;

	public SurfaceFormat format;

	public CTextureHost.texcoords tex_coord0;

	public CTextureHost.texcoords tex_coord1;

	public Texture2D mImage;
}
