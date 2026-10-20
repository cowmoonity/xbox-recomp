using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct mesh_t
{
	public bool b_smooth;

	public bool b_environ;

	public bool b_2sided;

	public string name;

	public int numvertex;

	public int numfaces;

	public matrix_t mshmatrix;

	public vertex_t[] p_vertices;

	public int[] p_faces;

	public bbox_t bbox;

	public bsphere_t bsphere;

	public ulong vertex_format;

	public DynamicVertexBuffer vertex_buffer;

	public DynamicIndexBuffer index_buffer;

	public int vertex_size;
}
