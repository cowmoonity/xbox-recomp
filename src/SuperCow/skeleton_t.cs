using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct skeleton_t
{
	public struct SBone
	{
		public int numchild;

		public int[] children;

		public string name;
	}

	public string name;

	public int numverts;

	public int numbones;

	public vec3_t[] p_bonevec;

	public SBone[] p_bones;

	public SVLink[][] p_verlink;

	public int[] p_faces;

	public int numfaces;

	public vec2_t[] p_texcoord;

	public int numbufvers;

	public int[] p_buf_vert_indices;

	public int[] p_indices;

	public DynamicIndexBuffer index_buffer;

	public DynamicVertexBuffer vertex_buffer;

	public vec3_t[] p_CurBoneVec;

	public matrix_t[] p_CurBoneMatrix;

	public vec3_t[] p_vertex;

	public vec3_t[] p_normals;
}
