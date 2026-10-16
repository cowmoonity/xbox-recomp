using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CGeomObj
{
	public string name;

	public int nummeshes;

	public int numbounds;

	public bool b_clone;

	public texture_t p_icontex;

	public bound_t[] p_bounds;

	public mesh_t[] p_meshes;

	public mesh_t[] p_shadowmesh;

	public texture_t[] p_textures;

	public matrix_t[] p_matrices = new matrix_t[8];

	public CGeomObj()
	{
		nummeshes = 0;
		numbounds = 0;
		p_bounds = null;
		for (int i = 0; i < 8; i++)
		{
			p_matrices[i].load_identity();
		}
	}

	public void CreateIcon()
	{
	}

	public bool Load(string name)
	{
		byte[] array = new byte[1];
		int len = 0;
		string str = "";
		b_clone = false;
		array = Store.storeLoadFileArray(name, ref len, b_errorexit: false);
		string script = SuperCowHelper.StringFromByteArray(array);
		nummeshes = SuperCowHelper.get_script_int(script, "nummeshes=", 0);
		p_meshes = new mesh_t[nummeshes];
		p_textures = new texture_t[nummeshes];
		p_shadowmesh = new mesh_t[nummeshes];
		for (int i = 0; i < nummeshes; i++)
		{
			string varname = $"mesh{i:D}=";
			SuperCowHelper.get_script_string(ref str, script, varname, 0);
			ref mesh_t reference = ref p_meshes[i];
			reference = SCGlobals.meshhost.Load(str);
			varname = $"texture{i:D}=";
			SuperCowHelper.get_script_string(ref str, script, varname, 0);
			ref texture_t reference2 = ref p_textures[i];
			reference2 = SCGlobals.texturehost.Load(str);
		}
		return true;
	}

	public void Draw(matrix_t matrix)
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		for (int i = 0; i < nummeshes; i++)
		{
			matrix_t matrix_t2 = matrix * p_meshes[i].mshmatrix;
			if (!p_matrices[i].isNull())
			{
				matrix_t2 *= p_matrices[i];
			}
			if (!SCGlobals.b_shadows)
			{
				vec3_t vec3_t2 = matrix_t2.get_trans() + matrix_t2 * p_meshes[i].bsphere.center;
				if (!SCGlobals.frustum.SphereInFrustum(vec3_t2.X, vec3_t2.Y, vec3_t2.Z, p_meshes[i].bsphere.radius))
				{
					continue;
				}
			}
			bool b_alpha = p_textures[i].b_alpha;
			if (SCGlobals.b_shadows && !b_alpha)
			{
				CowRender.LoadMatrix(matrix_t2);
				CowRender.BindTexture(default(texture_t));
				CowRender.DrawMesh(p_meshes[i]);
				continue;
			}
			CowRender.LoadMatrix(matrix_t2);
			CowRender.BindTexture(p_textures[i]);
			if (b_alpha)
			{
				CowRender.EnableRS(5u);
			}
			else
			{
				CowRender.DisableRS(5u);
			}
			if (SCGlobals.b_shadows && b_alpha)
			{
				graphicsDevice.RenderState.CullMode = (CullMode)1;
				CowRender.EnableRS(4u);
				CowRender.SetBlend(4);
			}
			if (!SCGlobals.b_shadows && b_alpha)
			{
				graphicsDevice.RenderState.CullMode = (CullMode)1;
				CowRender.EnableRS(4u);
				CowRender.SetBlend(0);
			}
			CowRender.DrawMesh(p_meshes[i]);
		}
	}
}
