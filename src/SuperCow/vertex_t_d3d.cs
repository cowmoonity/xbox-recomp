using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct vertex_t_d3d : IVertexType
{
	public Vector3 xyz;
	public Vector3 normal;
	public Vector2 uv;
	
	public VertexDeclaration VertexDeclaration => new(
		new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
		new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
		new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
		);
}
