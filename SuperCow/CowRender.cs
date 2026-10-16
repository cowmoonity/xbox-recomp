using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class CowRender
{
	public struct Quad_VertexF
	{
		public vec3_t xyz;

		public vec2_t uw;
	}

	public struct Quad_VertexF2
	{
		public vec3_t xyz;

		public vec3_t n;

		public vec2_t uw;
	}

	public struct Quad_VertexF3
	{
		public vec3_t xyz;

		public Color c;

		public vec2_t uw;
	}

	public struct d3d_vertF3
	{
		public Vector2 uw;

		public Vector3 xyz;

		public Color c;
	}

	private struct d3d_vertF3_tex1_xyz_diffuse
	{
		public Vector2 uw;

		public Vector3 xyz;

		public Color c;
	}

	public struct Quad_Vertex_SS
	{
		public vec3_t xyz;

		public vec3_t c;

		public vec2_t uw;
	}

	private const int MaxPolys = 3000;

	public static int dynBufferOffset = 0;

	public static int dynBufferOffsetPrev = 0;

	public static int polyCount = 0;

	public static int[] layerPolyCount = new int[6];

	public static int groundpolyCount = 0;

	public static int sizeofVertex = 20 + Marshal.SizeOf(typeof(Color));

	public static int bufferMax = 1500;

	public static bool[] renderStates = new bool[8];

	public static int blendType = -1;

	public static VertexShader vertexShader;

	public static PixelShader pixelShader;

	public static ulong mTextVertexSize;

	public static ulong mCurrentVertexFormat = 0uL;

	private static VertexDeclaration vertex_xyz_normal_tex1;

	private static VertexDeclaration vertex_tex1_xyz_diffuse;

	private static VertexDeclaration vertex_xyz;

	private static bool mDrawingParticles = false;

	private static CTextureHost.texcoords gTexCoords0 = default(CTextureHost.texcoords);

	private static CTextureHost.texcoords gTexCoords1 = default(CTextureHost.texcoords);

	public static vec2_t ux0 = default(vec2_t);

	public static vec2_t ux1 = default(vec2_t);

	public static Quad_VertexF[] p_Quad_vertices;

	public static Quad_VertexF2[] p_Quad_vertices2;

	public static Quad_VertexF3[] p_Quad_vertices3;

	public static Quad_Vertex_SS[] p_Quad_Vertex_SS;

	public static d3d_vertF3[] m_BuffVertices = new d3d_vertF3[10000];

	public static int m_BuffFaces = 0;

	public static d3d_vertF3[] m_GroundVertices = new d3d_vertF3[10000];

	public static int m_GroundFaces = 0;

	public static d3d_vertF3[][] m_Layers = new d3d_vertF3[6][]
	{
		new d3d_vertF3[30000],
		new d3d_vertF3[30000],
		new d3d_vertF3[44000],
		new d3d_vertF3[20000],
		new d3d_vertF3[4000],
		new d3d_vertF3[4000]
	};

	private static SpriteBatch batch;

	private static vec3_t p1 = default(vec3_t);

	private static vec3_t p2 = default(vec3_t);

	private static vec3_t p3 = default(vec3_t);

	private static vec3_t p4 = default(vec3_t);

	private static vec3_t p5 = default(vec3_t);

	private static vec3_t p6 = default(vec3_t);

	private static vec2_t t1 = default(vec2_t);

	private static vec2_t t2 = default(vec2_t);

	private static vec2_t t3 = default(vec2_t);

	private static vec2_t t4 = default(vec2_t);

	private static vec2_t t5 = default(vec2_t);

	private static vec2_t t6 = default(vec2_t);

	private static d3d_vertF3_tex1_xyz_diffuse[] vertices = new d3d_vertF3_tex1_xyz_diffuse[6];

	private static d3d_vertF3_tex1_xyz_diffuse[] vertices2 = new d3d_vertF3_tex1_xyz_diffuse[6000];

	private static float[] mFloatArray;

	private static Matrix matrixWorld = default(Matrix);

	private static Matrix matrixView = default(Matrix);

	private static Matrix matrixProj = default(Matrix);

	private static vec2_t[] delta = new vec2_t[6];

	private static Color MyColor;

	private static Vector4 MyVColor;

	public static int PolygonsPerFrame = 0;

	public static int DrawsPerFrame = 0;

	public static int ObjectPerFrame = 0;

	public static Vector4 vcc = default(Vector4);

	public static RenderTarget2D drawBuffer;

	public static DepthStencilBuffer drawDepthBuffer;

	private static float[] color_array = new float[4];

	private static bool diableDrawing = false;

	private static Texture2D cachedTexture = null;

	public static Quad_VertexF3[] letters = new Quad_VertexF3[4];

	public static float mi_size = 1f;

	public static void DrawMI(int id, vec3_t pos)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		letters[0].uw = SCGlobals.mitexcoords[id, 0];
		letters[1].uw = SCGlobals.mitexcoords[id, 1];
		letters[3].uw = SCGlobals.mitexcoords[id, 2];
		letters[2].uw = SCGlobals.mitexcoords[id, 3];
		letters[0].c = Color.White;
		letters[1].c = Color.White;
		letters[2].c = Color.White;
		letters[3].c = Color.White;
		((Color)(ref letters[0].c)).A = (byte)(SCGlobals.global_2d_color.a * 255f);
		((Color)(ref letters[1].c)).A = (byte)(SCGlobals.global_2d_color.a * 255f);
		((Color)(ref letters[2].c)).A = (byte)(SCGlobals.global_2d_color.a * 255f);
		((Color)(ref letters[3].c)).A = (byte)(SCGlobals.global_2d_color.a * 255f);
		float num = mi_size * SCGlobals.misizes[id].x / 2f;
		float num2 = mi_size * SCGlobals.misizes[id].y / 2f;
		letters[0].xyz = pos + new vec3_t(0f - num, num2, 0f);
		letters[1].xyz = pos + new vec3_t(num, num2, 0f);
		letters[2].xyz = pos + new vec3_t(0f - num, 0f - num2, 0f);
		letters[3].xyz = pos + new vec3_t(num, 0f - num2, 0f);
		DrawVertexArray(ref letters, 2, 322uL, (PrimitiveType)5);
	}

	public static void DrawLineMI(int id, vec3_t p1, vec3_t p2)
	{
		BindTexture(SCGlobals.p_menutex);
		letters[0].uw = SCGlobals.mitexcoords[id, 0];
		letters[1].uw = SCGlobals.mitexcoords[id, 1];
		letters[2].uw = SCGlobals.mitexcoords[id, 3];
		letters[3].uw = SCGlobals.mitexcoords[id, 2];
		float num = mi_size * 64f;
		vec2_t vec2_t2 = new vec2_t(p2 - p1);
		float num2 = vec2_t2.len();
		vec2_t2 /= num2;
		int num3 = (int)(num2 / num);
		float num4 = num;
		float num5 = num2 - (float)num3 * num;
		frect_t tc = new frect_t(letters[0].uw.X, letters[0].uw.Y, letters[3].uw.X, letters[3].uw.Y);
		vec2_t vec2_t3 = new vec2_t(p1);
		for (int i = 0; i < num3 + 1; i++)
		{
			vec2_t3 = new vec2_t(p1) + vec2_t2 * num4 * ((float)i + 0.5f);
			if (i == num3)
			{
				vec2_t3 = new vec2_t(p1) + vec2_t2 * num4 * ((float)i + 0.5f * num5 / num4);
				num4 = num5;
			}
			if (id == 5)
			{
				DrawColorQuad2D(vec2_t3, num, num4, tc, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 40f);
			}
			else
			{
				DrawColorQuad2D(vec2_t3, num4, num, tc, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 40f);
			}
		}
	}

	public static void InitVertexDeclarations()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		VertexElement[] array = (VertexElement[])(object)new VertexElement[3]
		{
			new VertexElement((short)0, (short)0, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)0, (byte)0),
			new VertexElement((short)0, (short)12, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)3, (byte)0),
			new VertexElement((short)0, (short)24, (VertexElementFormat)1, (VertexElementMethod)0, (VertexElementUsage)5, (byte)0)
		};
		vertex_xyz_normal_tex1 = new VertexDeclaration(graphicsDevice, array);
		VertexElement[] array2 = (VertexElement[])(object)new VertexElement[3]
		{
			new VertexElement((short)0, (short)0, (VertexElementFormat)1, (VertexElementMethod)0, (VertexElementUsage)5, (byte)0),
			new VertexElement((short)0, (short)8, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)0, (byte)0),
			new VertexElement((short)0, (short)20, (VertexElementFormat)4, (VertexElementMethod)0, (VertexElementUsage)10, (byte)0)
		};
		vertex_tex1_xyz_diffuse = new VertexDeclaration(graphicsDevice, array2);
		VertexElement[] array3 = (VertexElement[])(object)new VertexElement[1]
		{
			new VertexElement((short)0, (short)0, (VertexElementFormat)2, (VertexElementMethod)0, (VertexElementUsage)0, (byte)0)
		};
		vertex_xyz = new VertexDeclaration(graphicsDevice, array3);
		gTexCoords0.ux = 0f;
		gTexCoords0.uy = 0f;
		gTexCoords1.ux = 1f;
		gTexCoords1.uy = 1f;
	}

	public static void CreateRenderTarget(PresentationParameters pp)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		drawBuffer = new RenderTarget2D(SuperCow.DeviceManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1, (SurfaceFormat)5, pp.MultiSampleType, pp.MultiSampleQuality, pp.RenderTargetUsage);
		drawDepthBuffer = new DepthStencilBuffer(SuperCow.DeviceManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, pp.AutoDepthStencilFormat, pp.MultiSampleType, pp.MultiSampleQuality);
	}

	public static void setFVF(ulong vertex_format)
	{
		if (mCurrentVertexFormat == vertex_format)
		{
			return;
		}
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (graphicsDevice != null && (vertex_format != 274 || graphicsDevice.VertexDeclaration != vertex_xyz_normal_tex1) && (vertex_format != 322 || graphicsDevice.VertexDeclaration != vertex_tex1_xyz_diffuse) && (vertex_format != 2 || graphicsDevice.VertexDeclaration != vertex_xyz))
		{
			switch (vertex_format)
			{
			case 274uL:
				graphicsDevice.VertexDeclaration = vertex_xyz_normal_tex1;
				break;
			case 322uL:
				graphicsDevice.VertexDeclaration = vertex_tex1_xyz_diffuse;
				break;
			case 2uL:
				graphicsDevice.VertexDeclaration = vertex_xyz;
				break;
			default:
				throw new ArgumentException("Unknown vertex_format");
			}
			mCurrentVertexFormat = vertex_format;
		}
	}

	public static void InitShaders()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		pixelShader = SuperCow.ContentManager.Load<PixelShader>("content\\data\\shaders\\pshader");
		vertexShader = SuperCow.ContentManager.Load<VertexShader>("content\\data\\shaders\\vshader");
		p_Quad_vertices3 = new Quad_VertexF3[6];
		for (int i = 0; i < 6; i++)
		{
			p_Quad_vertices3[i] = default(Quad_VertexF3);
			p_Quad_vertices3[i].xyz = default(vec3_t);
			p_Quad_vertices3[i].uw = default(vec2_t);
		}
		batch = new SpriteBatch(graphicsDevice);
		mFloatArray = new float[4];
		graphicsDevice.VertexShader = vertexShader;
		graphicsDevice.PixelShader = pixelShader;
		for (int j = 0; j < 8; j++)
		{
			renderStates[j] = false;
		}
	}

	public static void DrawMesh(mesh_t p_mesh)
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.Vertices[0].SetSource((VertexBuffer)(object)p_mesh.vertex_buffer, 0, p_mesh.vertex_size);
		SetVertexPixelShadersData(p_mesh.vertex_format);
		if (p_mesh.b_smooth)
		{
			graphicsDevice.Indices = (IndexBuffer)(object)p_mesh.index_buffer;
			graphicsDevice.DrawIndexedPrimitives((PrimitiveType)4, 0, 0, p_mesh.numvertex, 0, p_mesh.numfaces);
			DrawsPerFrame++;
		}
		else
		{
			graphicsDevice.DrawPrimitives((PrimitiveType)4, 0, p_mesh.numfaces);
			DrawsPerFrame++;
		}
	}

	public static void DrawColorQuad2D(Texture2D theImage, float x, float y, float sizex, float sizey, dcolor_t c, float z)
	{
		if (theImage != null)
		{
			frect_t vc = new frect_t(x - sizex / 2f, y + sizey / 2f, x + sizex / 2f, y - sizey / 2f);
			frect_t tc_normal = SCGlobals.tc_normal;
			DrawColorQuad2D(vc, tc_normal, c, z);
		}
	}

	public static void DrawColorQuad2DDynBuffer(Texture2D theImage, float x, float y, float sizex, float sizey, dcolor_t c, float z)
	{
		if (theImage != null)
		{
			frect_t vc = new frect_t(x - sizex / 2f, y + sizey / 2f, x + sizex / 2f, y - sizey / 2f);
			frect_t tc_normal = SCGlobals.tc_normal;
			DrawColorQuad2DDynBuffer(vc, tc_normal, c, z);
		}
	}

	public static void DrawColorQuad2D(float x, float y, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		DrawColorQuad2D(new vec2_t(x, y), sizex, sizey, tc, c, z);
	}

	public static void DrawColorQuad2D(Texture2D theImage, dcolor_t c, float z)
	{
		frect_t vc = new frect_t((0f - (float)theImage.Width) / 2f, (float)theImage.Height / 2f, (float)theImage.Width / 2f, (0f - (float)theImage.Height) / 2f);
		frect_t tc_normal = SCGlobals.tc_normal;
		DrawColorQuad2D(vc, tc_normal, c, z);
	}

	public static void DrawColorQuad2D(frect_t vc, frect_t tc, dcolor_t c, float z)
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			p1.v0 = vc.x0;
			p1.v1 = vc.y0;
			p1.v2 = z;
			p2.v0 = vc.x1;
			p2.v1 = vc.y0;
			p2.v2 = z;
			p3.v0 = vc.x1;
			p3.v1 = vc.y1;
			p3.v2 = z;
			p4.v0 = vc.x0;
			p4.v1 = vc.y1;
			p4.v2 = z;
			t1.v0 = tc.x0;
			t1.v1 = tc.y0;
			t2.v0 = tc.x1;
			t2.v1 = tc.y0;
			t3.v0 = tc.x1;
			t3.v1 = tc.y1;
			t4.v0 = tc.x0;
			t4.v1 = tc.y1;
			p_Quad_vertices3[0].xyz = p2;
			p_Quad_vertices3[1].xyz = p3;
			p_Quad_vertices3[2].xyz = p1;
			p_Quad_vertices3[3].xyz = p4;
			p_Quad_vertices3[0].uw = t2;
			p_Quad_vertices3[1].uw = t3;
			p_Quad_vertices3[2].uw = t1;
			p_Quad_vertices3[3].uw = t4;
			MyColor = new Color((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
			vertices[0].c = MyColor;
			vertices[1].c = MyColor;
			vertices[2].c = MyColor;
			vertices[3].c = MyColor;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].xyz.X = p_Quad_vertices3[i].xyz.v0;
				vertices[i].xyz.Y = p_Quad_vertices3[i].xyz.v1;
				vertices[i].xyz.Z = p_Quad_vertices3[i].xyz.v2;
				vertices[i].uw.X = p_Quad_vertices3[i].uw.v0;
				vertices[i].uw.Y = p_Quad_vertices3[i].uw.v1;
			}
			SetVertexPixelShadersData(322uL);
			if (graphicsDevice != null)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>((PrimitiveType)5, vertices, 0, 2);
			}
			DrawsPerFrame++;
			PolygonsPerFrame += 2;
		}
	}

	public static void DrawDynBuffer()
	{
		if (polyCount != 0)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SetVertexPixelShadersData(322uL);
			graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_BuffVertices, 0, polyCount);
			DrawsPerFrame++;
			PolygonsPerFrame += polyCount;
			polyCount = 0;
		}
	}

	public static void DrawStaticToBuffer()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
	}

	public static void DrawStaticBuffer()
	{
		if (layerPolyCount[SCGlobals.currentLayer] != 0)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SetVertexPixelShadersData(322uL);
			if (layerPolyCount[SCGlobals.currentLayer] > 7000 && layerPolyCount[SCGlobals.currentLayer] < 10000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 0, 3000);
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 9000, layerPolyCount[SCGlobals.currentLayer] - 3000);
				DrawsPerFrame += 2;
			}
			else if (layerPolyCount[SCGlobals.currentLayer] >= 10000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 0, 5000);
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 15000, 5000);
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 30000, layerPolyCount[SCGlobals.currentLayer] - 10000);
				DrawsPerFrame += 3;
			}
			else
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_Layers[SCGlobals.currentLayer], 0, layerPolyCount[SCGlobals.currentLayer]);
				DrawsPerFrame++;
			}
			PolygonsPerFrame += layerPolyCount[SCGlobals.currentLayer];
		}
	}

	public static void DrawGroundColorQuad2DDynBuffer(frect_t vc, frect_t tc, dcolor_t c, float z)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		Color d3DColor = c.GetD3DColor();
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x0, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y0);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x1, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y1);
		m_GroundVertices[groundpolyCount * 3].xyz.X = vc.x0;
		m_GroundVertices[groundpolyCount * 3].xyz.Y = vc.y0;
		m_GroundVertices[groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[groundpolyCount * 3].uw.X = ux0.x;
		m_GroundVertices[groundpolyCount * 3].uw.Y = ux0.y;
		m_GroundVertices[groundpolyCount * 3].c = d3DColor;
		m_GroundVertices[1 + groundpolyCount * 3].xyz.X = vc.x1;
		m_GroundVertices[1 + groundpolyCount * 3].xyz.Y = vc.y0;
		m_GroundVertices[1 + groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[1 + groundpolyCount * 3].uw.X = ux1.x;
		m_GroundVertices[1 + groundpolyCount * 3].uw.Y = ux0.y;
		m_GroundVertices[1 + groundpolyCount * 3].c = d3DColor;
		m_GroundVertices[2 + groundpolyCount * 3].xyz.X = vc.x0;
		m_GroundVertices[2 + groundpolyCount * 3].xyz.Y = vc.y1;
		m_GroundVertices[2 + groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[2 + groundpolyCount * 3].uw.X = ux0.x;
		m_GroundVertices[2 + groundpolyCount * 3].uw.Y = ux1.y;
		m_GroundVertices[2 + groundpolyCount * 3].c = d3DColor;
		m_GroundVertices[3 + groundpolyCount * 3].xyz.X = vc.x0;
		m_GroundVertices[3 + groundpolyCount * 3].xyz.Y = vc.y1;
		m_GroundVertices[3 + groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[3 + groundpolyCount * 3].uw.X = ux0.x;
		m_GroundVertices[3 + groundpolyCount * 3].uw.Y = ux1.y;
		m_GroundVertices[3 + groundpolyCount * 3].c = d3DColor;
		m_GroundVertices[4 + groundpolyCount * 3].xyz.X = vc.x1;
		m_GroundVertices[4 + groundpolyCount * 3].xyz.Y = vc.y0;
		m_GroundVertices[4 + groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[4 + groundpolyCount * 3].uw.X = ux1.x;
		m_GroundVertices[4 + groundpolyCount * 3].uw.Y = ux0.y;
		m_GroundVertices[4 + groundpolyCount * 3].c = d3DColor;
		m_GroundVertices[5 + groundpolyCount * 3].xyz.X = vc.x1;
		m_GroundVertices[5 + groundpolyCount * 3].xyz.Y = vc.y1;
		m_GroundVertices[5 + groundpolyCount * 3].xyz.Z = z;
		m_GroundVertices[5 + groundpolyCount * 3].uw.X = ux1.x;
		m_GroundVertices[5 + groundpolyCount * 3].uw.Y = ux1.y;
		m_GroundVertices[5 + groundpolyCount * 3].c = d3DColor;
		groundpolyCount += 2;
	}

	public static void DrawColorQuad2DStaticBuffer(frect_t vc, frect_t tc, dcolor_t c, float z)
	{
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		Color c2 = default(Color);
		((Color)(ref c2))._002Ector((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x0, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y0);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x1, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y1);
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x0;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y0;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x1;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y0;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x0;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y1;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x0;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y1;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x1;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y0;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = vc.x1;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = vc.y1;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		layerPolyCount[SCGlobals.currentLayer] += 2;
	}

	public static void DrawColorQuad2DDynBuffer(frect_t vc, frect_t tc, dcolor_t c, float z)
	{
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		Color c2 = default(Color);
		((Color)(ref c2))._002Ector((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x0, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y0);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x1, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y1);
		m_BuffVertices[polyCount * 3].xyz.X = vc.x0;
		m_BuffVertices[polyCount * 3].xyz.Y = vc.y0;
		m_BuffVertices[polyCount * 3].xyz.Z = z;
		m_BuffVertices[polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[polyCount * 3].c = c2;
		m_BuffVertices[1 + polyCount * 3].xyz.X = vc.x1;
		m_BuffVertices[1 + polyCount * 3].xyz.Y = vc.y0;
		m_BuffVertices[1 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[1 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[1 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[1 + polyCount * 3].c = c2;
		m_BuffVertices[2 + polyCount * 3].xyz.X = vc.x0;
		m_BuffVertices[2 + polyCount * 3].xyz.Y = vc.y1;
		m_BuffVertices[2 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[2 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[2 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[2 + polyCount * 3].c = c2;
		m_BuffVertices[3 + polyCount * 3].xyz.X = vc.x0;
		m_BuffVertices[3 + polyCount * 3].xyz.Y = vc.y1;
		m_BuffVertices[3 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[3 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[3 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[3 + polyCount * 3].c = c2;
		m_BuffVertices[4 + polyCount * 3].xyz.X = vc.x1;
		m_BuffVertices[4 + polyCount * 3].xyz.Y = vc.y0;
		m_BuffVertices[4 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[4 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[4 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[4 + polyCount * 3].c = c2;
		m_BuffVertices[5 + polyCount * 3].xyz.X = vc.x1;
		m_BuffVertices[5 + polyCount * 3].xyz.Y = vc.y1;
		m_BuffVertices[5 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[5 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[5 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[5 + polyCount * 3].c = c2;
		polyCount += 2;
	}

	public static void DrawColorQuad2DDynBuffer(vec2_t p, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		sizex *= 0.5f;
		sizey *= 0.5f;
		frect_t vc = new frect_t(p.x - sizex, p.y + sizey, p.x + sizex, p.y - sizey);
		DrawColorQuad2DDynBuffer(vc, tc, c, z);
	}

	public static void DrawColorQuad2D(vec2_t p, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		sizex *= 0.5f;
		sizey *= 0.5f;
		frect_t vc = new frect_t(p.x - sizex, p.y + sizey, p.x + sizex, p.y - sizey);
		DrawColorQuad2D(vc, tc, c, z);
	}

	public static void DrawRotatedColorQuad2DStaticBuffer(vec2_t p, vec2_t center, float angle, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		float num = SuperCowHelper.fast_sin(angle);
		float num2 = SuperCowHelper.fast_cos(angle);
		sizex *= 0.5f;
		sizey *= 0.5f;
		delta[0].v0 = p.v0 - sizex;
		delta[0].v1 = p.v1 + sizey;
		delta[1].v0 = p.v0 + sizex;
		delta[1].v1 = p.v1 + sizey;
		delta[2].v0 = p.v0 + sizex;
		delta[2].v1 = p.v1 - sizey;
		delta[3].v0 = p.v0 - sizex;
		delta[3].v1 = p.v1 - sizey;
		for (int i = 0; i < 6; i++)
		{
			vec2_t vec2_t2 = delta[i] - center;
			vec2_t2.assign(vec2_t2.v0 * num2 - vec2_t2.v1 * num, vec2_t2.v0 * num + vec2_t2.v1 * num2);
			ref vec2_t reference = ref delta[i];
			reference = vec2_t2 + center;
		}
		Color c2 = default(Color);
		((Color)(ref c2))._002Ector((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x0, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y0);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x1, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y1);
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[1].v0;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[1].v1;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[2].v0;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[2].v1;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[0].v0;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[0].v1;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[0].v0;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[0].v1;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[2].v0;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[2].v1;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = delta[3].v0;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = delta[3].v1;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = z;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].c = c2;
		layerPolyCount[SCGlobals.currentLayer] += 2;
	}

	public static void DrawRotatedColorQuad2DDynBuffer(vec2_t p, vec2_t center, float angle, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		float num = SuperCowHelper.fast_sin(angle);
		float num2 = SuperCowHelper.fast_cos(angle);
		sizex *= 0.5f;
		sizey *= 0.5f;
		delta[0].v0 = p.v0 - sizex;
		delta[0].v1 = p.v1 + sizey;
		delta[1].v0 = p.v0 + sizex;
		delta[1].v1 = p.v1 + sizey;
		delta[2].v0 = p.v0 + sizex;
		delta[2].v1 = p.v1 - sizey;
		delta[3].v0 = p.v0 - sizex;
		delta[3].v1 = p.v1 - sizey;
		for (int i = 0; i < 6; i++)
		{
			vec2_t vec2_t2 = delta[i] - center;
			vec2_t2.assign(vec2_t2.v0 * num2 - vec2_t2.v1 * num, vec2_t2.v0 * num + vec2_t2.v1 * num2);
			ref vec2_t reference = ref delta[i];
			reference = vec2_t2 + center;
		}
		Color c2 = default(Color);
		((Color)(ref c2))._002Ector((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x0, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y0);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * tc.x1, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * tc.y1);
		m_BuffVertices[polyCount * 3].xyz.X = delta[1].v0;
		m_BuffVertices[polyCount * 3].xyz.Y = delta[1].v1;
		m_BuffVertices[polyCount * 3].xyz.Z = z;
		m_BuffVertices[polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[polyCount * 3].c = c2;
		m_BuffVertices[1 + polyCount * 3].xyz.X = delta[2].v0;
		m_BuffVertices[1 + polyCount * 3].xyz.Y = delta[2].v1;
		m_BuffVertices[1 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[1 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[1 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[1 + polyCount * 3].c = c2;
		m_BuffVertices[2 + polyCount * 3].xyz.X = delta[0].v0;
		m_BuffVertices[2 + polyCount * 3].xyz.Y = delta[0].v1;
		m_BuffVertices[2 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[2 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[2 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[2 + polyCount * 3].c = c2;
		m_BuffVertices[3 + polyCount * 3].xyz.X = delta[0].v0;
		m_BuffVertices[3 + polyCount * 3].xyz.Y = delta[0].v1;
		m_BuffVertices[3 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[3 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[3 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[3 + polyCount * 3].c = c2;
		m_BuffVertices[4 + polyCount * 3].xyz.X = delta[2].v0;
		m_BuffVertices[4 + polyCount * 3].xyz.Y = delta[2].v1;
		m_BuffVertices[4 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[4 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[4 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[4 + polyCount * 3].c = c2;
		m_BuffVertices[5 + polyCount * 3].xyz.X = delta[3].v0;
		m_BuffVertices[5 + polyCount * 3].xyz.Y = delta[3].v1;
		m_BuffVertices[5 + polyCount * 3].xyz.Z = z;
		m_BuffVertices[5 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[5 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[5 + polyCount * 3].c = c2;
		polyCount += 2;
	}

	public static void DrawRotatedColorQuad2D(vec2_t p, vec2_t center, float angle, float sizex, float sizey, frect_t tc, dcolor_t c, float z)
	{
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			float num = SuperCowHelper.fast_sin(angle);
			float num2 = SuperCowHelper.fast_cos(angle);
			sizex *= 0.5f;
			sizey *= 0.5f;
			delta[0].v0 = p.v0 - sizex;
			delta[0].v1 = p.v1 + sizey;
			delta[1].v0 = p.v0 + sizex;
			delta[1].v1 = p.v1 + sizey;
			delta[2].v0 = p.v0 + sizex;
			delta[2].v1 = p.v1 - sizey;
			delta[3].v0 = p.v0 - sizex;
			delta[3].v1 = p.v1 - sizey;
			for (int i = 0; i < 4; i++)
			{
				vec2_t vec2_t2 = delta[i] - center;
				vec2_t2.assign(vec2_t2.v0 * num2 - vec2_t2.v1 * num, vec2_t2.v0 * num + vec2_t2.v1 * num2);
				ref vec2_t reference = ref delta[i];
				reference = vec2_t2 + center;
			}
			p1.v0 = delta[0].v0;
			p1.v1 = delta[0].v1;
			p1.v2 = z;
			p2.v0 = delta[1].v0;
			p2.v1 = delta[1].v1;
			p2.v2 = z;
			p3.v0 = delta[2].v0;
			p3.v1 = delta[2].v1;
			p3.v2 = z;
			p4.v0 = delta[3].v0;
			p4.v1 = delta[3].v1;
			p4.v2 = z;
			t1.v0 = tc.x0;
			t1.v1 = tc.y0;
			t2.v0 = tc.x1;
			t2.v1 = tc.y0;
			t3.v0 = tc.x1;
			t3.v1 = tc.y1;
			t4.v0 = tc.x0;
			t4.v1 = tc.y1;
			p_Quad_vertices3[0].xyz = p2;
			p_Quad_vertices3[1].xyz = p3;
			p_Quad_vertices3[2].xyz = p1;
			p_Quad_vertices3[3].xyz = p4;
			p_Quad_vertices3[0].uw = t2;
			p_Quad_vertices3[1].uw = t3;
			p_Quad_vertices3[2].uw = t1;
			p_Quad_vertices3[3].uw = t4;
			MyColor = new Color((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
			vertices[0].c = MyColor;
			vertices[1].c = MyColor;
			vertices[2].c = MyColor;
			vertices[3].c = MyColor;
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j].xyz.X = p_Quad_vertices3[j].xyz.X;
				vertices[j].xyz.Y = p_Quad_vertices3[j].xyz.Y;
				vertices[j].xyz.Z = p_Quad_vertices3[j].xyz.Z;
				vertices[j].uw.X = p_Quad_vertices3[j].uw.X;
				vertices[j].uw.Y = p_Quad_vertices3[j].uw.Y;
			}
			SetVertexPixelShadersData(322uL);
			if (DrawsPerFrame < 500000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>((PrimitiveType)5, vertices, 0, 2);
			}
			DrawsPerFrame++;
			PolygonsPerFrame += 2;
		}
	}

	public static void DrawColorQuadStaticBuffer(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, vec2_t t1, vec2_t t2, vec2_t t3, vec2_t t4, dcolor_t c)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		Color d3DColor = c.GetD3DColor();
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * t3.x, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * t3.y);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * t1.x, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * t1.y);
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p4.x;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p4.y;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p4.z;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p1.x;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p1.y;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p1.z;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][1 + layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p3.x;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p3.y;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p3.z;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][2 + layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p3.x;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p3.y;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p3.z;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux0.x;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][3 + layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p1.x;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p1.y;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p1.z;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux1.y;
		m_Layers[SCGlobals.currentLayer][4 + layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.X = p2.x;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Y = p2.y;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].xyz.Z = p2.z;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.X = ux1.x;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].uw.Y = ux0.y;
		m_Layers[SCGlobals.currentLayer][5 + layerPolyCount[SCGlobals.currentLayer] * 3].c = d3DColor;
		layerPolyCount[SCGlobals.currentLayer] += 2;
	}

	public static void DrawColorQuadDynBuffer(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, vec2_t t1, vec2_t t2, vec2_t t3, vec2_t t4, dcolor_t c)
	{
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		Color c2 = default(Color);
		((Color)(ref c2))._002Ector((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		ux0.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * t3.x, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * t3.y);
		ux1.assign(gTexCoords0.ux + (gTexCoords1.ux - gTexCoords0.ux) * t1.x, gTexCoords0.uy + (gTexCoords1.uy - gTexCoords0.uy) * t1.y);
		m_BuffVertices[polyCount * 3].xyz.X = p4.x;
		m_BuffVertices[polyCount * 3].xyz.Y = p4.y;
		m_BuffVertices[polyCount * 3].xyz.Z = p4.z;
		m_BuffVertices[polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[polyCount * 3].c = c2;
		m_BuffVertices[1 + polyCount * 3].xyz.X = p1.x;
		m_BuffVertices[1 + polyCount * 3].xyz.Y = p1.y;
		m_BuffVertices[1 + polyCount * 3].xyz.Z = p1.z;
		m_BuffVertices[1 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[1 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[1 + polyCount * 3].c = c2;
		m_BuffVertices[2 + polyCount * 3].xyz.X = p3.x;
		m_BuffVertices[2 + polyCount * 3].xyz.Y = p3.y;
		m_BuffVertices[2 + polyCount * 3].xyz.Z = p3.z;
		m_BuffVertices[2 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[2 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[2 + polyCount * 3].c = c2;
		m_BuffVertices[3 + polyCount * 3].xyz.X = p3.x;
		m_BuffVertices[3 + polyCount * 3].xyz.Y = p3.y;
		m_BuffVertices[3 + polyCount * 3].xyz.Z = p3.z;
		m_BuffVertices[3 + polyCount * 3].uw.X = ux0.x;
		m_BuffVertices[3 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[3 + polyCount * 3].c = c2;
		m_BuffVertices[4 + polyCount * 3].xyz.X = p1.x;
		m_BuffVertices[4 + polyCount * 3].xyz.Y = p1.y;
		m_BuffVertices[4 + polyCount * 3].xyz.Z = p1.z;
		m_BuffVertices[4 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[4 + polyCount * 3].uw.Y = ux1.y;
		m_BuffVertices[4 + polyCount * 3].c = c2;
		m_BuffVertices[5 + polyCount * 3].xyz.X = p2.x;
		m_BuffVertices[5 + polyCount * 3].xyz.Y = p2.y;
		m_BuffVertices[5 + polyCount * 3].xyz.Z = p2.z;
		m_BuffVertices[5 + polyCount * 3].uw.X = ux1.x;
		m_BuffVertices[5 + polyCount * 3].uw.Y = ux0.y;
		m_BuffVertices[5 + polyCount * 3].c = c2;
		polyCount += 2;
	}

	public static void DrawColorQuad(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, vec2_t t1, vec2_t t2, vec2_t t3, vec2_t t4, dcolor_t c)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			p_Quad_vertices3[0].xyz = p2;
			p_Quad_vertices3[1].xyz = p3;
			p_Quad_vertices3[2].xyz = p1;
			p_Quad_vertices3[3].xyz = p4;
			p_Quad_vertices3[0].uw = t2;
			p_Quad_vertices3[1].uw = t3;
			p_Quad_vertices3[2].uw = t1;
			p_Quad_vertices3[3].uw = t4;
			MyColor = new Color((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
			vertices[0].c = MyColor;
			vertices[1].c = MyColor;
			vertices[2].c = MyColor;
			vertices[3].c = MyColor;
			for (int i = 0; i < 4; i++)
			{
				vertices[i].xyz.X = p_Quad_vertices3[i].xyz.X;
				vertices[i].xyz.Y = p_Quad_vertices3[i].xyz.Y;
				vertices[i].xyz.Z = p_Quad_vertices3[i].xyz.Z;
				vertices[i].uw.X = p_Quad_vertices3[i].uw.X;
				vertices[i].uw.Y = p_Quad_vertices3[i].uw.Y;
			}
			SetVertexPixelShadersData(322uL);
			if (DrawsPerFrame < 500000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>((PrimitiveType)5, vertices, 0, 2);
			}
			DrawsPerFrame++;
			PolygonsPerFrame += 2;
		}
	}

	public static void DrawColorQuad(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, dcolor_t c)
	{
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		p_Quad_vertices3[0].xyz = p1;
		p_Quad_vertices3[1].xyz = p2;
		p_Quad_vertices3[2].xyz = p3;
		p_Quad_vertices3[3].xyz = p1;
		p_Quad_vertices3[4].xyz = p3;
		p_Quad_vertices3[5].xyz = p4;
		p_Quad_vertices3[0].uw.v0 = 0f;
		p_Quad_vertices3[0].uw.v1 = 0f;
		p_Quad_vertices3[1].uw.v0 = 0f;
		p_Quad_vertices3[1].uw.v1 = 1f;
		p_Quad_vertices3[2].uw.v0 = 1f;
		p_Quad_vertices3[2].uw.v1 = 1f;
		p_Quad_vertices3[3].uw.v0 = 0f;
		p_Quad_vertices3[3].uw.v1 = 0f;
		p_Quad_vertices3[4].uw.v0 = 1f;
		p_Quad_vertices3[4].uw.v1 = 1f;
		p_Quad_vertices3[5].uw.v0 = 1f;
		p_Quad_vertices3[5].uw.v1 = 0f;
		MyColor = new Color((byte)((float)(int)c.r * SCGlobals.global_2d_color.r), (byte)((float)(int)c.g * SCGlobals.global_2d_color.g), (byte)((float)(int)c.b * SCGlobals.global_2d_color.b), (byte)((float)(int)c.a * SCGlobals.global_2d_color.a));
		p_Quad_vertices3[0].c = MyColor;
		p_Quad_vertices3[1].c = MyColor;
		p_Quad_vertices3[2].c = MyColor;
		p_Quad_vertices3[3].c = MyColor;
		p_Quad_vertices3[4].c = MyColor;
		p_Quad_vertices3[5].c = MyColor;
		DrawVertexArray(ref p_Quad_vertices3, 2, 322uL, (PrimitiveType)4);
		PolygonsPerFrame += 2;
	}

	public static void DrawColorQuad(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, dcolor_t c1, dcolor_t c2, dcolor_t c3, dcolor_t c4)
	{
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		p_Quad_vertices3[0].xyz = p1;
		p_Quad_vertices3[1].xyz = p2;
		p_Quad_vertices3[2].xyz = p3;
		p_Quad_vertices3[3].xyz = p1;
		p_Quad_vertices3[4].xyz = p3;
		p_Quad_vertices3[5].xyz = p4;
		p_Quad_vertices3[0].uw.v0 = 0f;
		p_Quad_vertices3[0].uw.v1 = 0f;
		p_Quad_vertices3[1].uw.v0 = 0f;
		p_Quad_vertices3[1].uw.v1 = -1f;
		p_Quad_vertices3[2].uw.v0 = -1f;
		p_Quad_vertices3[2].uw.v1 = -1f;
		p_Quad_vertices3[3].uw.v0 = 0f;
		p_Quad_vertices3[3].uw.v1 = 0f;
		p_Quad_vertices3[4].uw.v0 = -1f;
		p_Quad_vertices3[4].uw.v1 = -1f;
		p_Quad_vertices3[5].uw.v0 = -1f;
		p_Quad_vertices3[5].uw.v1 = 0f;
		color_t global_2d_color = SCGlobals.global_2d_color;
		p_Quad_vertices3[0].c = new Color((byte)((float)(int)c1.r * global_2d_color.r), (byte)((float)(int)c1.g * global_2d_color.g), (byte)((float)(int)c1.b * global_2d_color.b), (byte)((float)(int)c1.a * global_2d_color.a));
		p_Quad_vertices3[1].c = new Color((byte)((float)(int)c2.r * global_2d_color.r), (byte)((float)(int)c2.g * global_2d_color.g), (byte)((float)(int)c2.b * global_2d_color.b), (byte)((float)(int)c2.a * global_2d_color.a));
		p_Quad_vertices3[2].c = new Color((byte)((float)(int)c3.r * global_2d_color.r), (byte)((float)(int)c3.g * global_2d_color.g), (byte)((float)(int)c3.b * global_2d_color.b), (byte)((float)(int)c3.a * global_2d_color.a));
		p_Quad_vertices3[3].c = new Color((byte)((float)(int)c1.r * global_2d_color.r), (byte)((float)(int)c1.g * global_2d_color.g), (byte)((float)(int)c1.b * global_2d_color.b), (byte)((float)(int)c1.a * global_2d_color.a));
		p_Quad_vertices3[4].c = new Color((byte)((float)(int)c3.r * global_2d_color.r), (byte)((float)(int)c3.g * global_2d_color.g), (byte)((float)(int)c3.b * global_2d_color.b), (byte)((float)(int)c3.a * global_2d_color.a));
		p_Quad_vertices3[5].c = new Color((byte)((float)(int)c4.r * global_2d_color.r), (byte)((float)(int)c4.g * global_2d_color.g), (byte)((float)(int)c4.b * global_2d_color.b), (byte)((float)(int)c4.a * global_2d_color.a));
		DrawVertexArray(ref p_Quad_vertices3, 2, 322uL, (PrimitiveType)4);
		PolygonsPerFrame += 2;
	}

	public static void DrawColorQuadDynBuffer(vec3_t p1, vec3_t p2, vec3_t p3, vec3_t p4, dcolor_t c1, dcolor_t c2, dcolor_t c3, dcolor_t c4)
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		_ = SuperCow.DeviceManager.GraphicsDevice;
		p_Quad_vertices3[0].xyz = p1;
		p_Quad_vertices3[1].xyz = p2;
		p_Quad_vertices3[2].xyz = p3;
		p_Quad_vertices3[3].xyz = p1;
		p_Quad_vertices3[4].xyz = p3;
		p_Quad_vertices3[5].xyz = p4;
		p_Quad_vertices3[0].uw.v0 = 0f;
		p_Quad_vertices3[0].uw.v1 = 0f;
		p_Quad_vertices3[1].uw.v0 = 0f;
		p_Quad_vertices3[1].uw.v1 = -1f;
		p_Quad_vertices3[2].uw.v0 = -1f;
		p_Quad_vertices3[2].uw.v1 = -1f;
		p_Quad_vertices3[3].uw.v0 = 0f;
		p_Quad_vertices3[3].uw.v1 = 0f;
		p_Quad_vertices3[4].uw.v0 = -1f;
		p_Quad_vertices3[4].uw.v1 = -1f;
		p_Quad_vertices3[5].uw.v0 = -1f;
		p_Quad_vertices3[5].uw.v1 = 0f;
		color_t global_2d_color = SCGlobals.global_2d_color;
		p_Quad_vertices3[0].c = new Color((byte)((float)(int)c1.r * global_2d_color.r), (byte)((float)(int)c1.g * global_2d_color.g), (byte)((float)(int)c1.b * global_2d_color.b), (byte)((float)(int)c1.a * global_2d_color.a));
		p_Quad_vertices3[1].c = new Color((byte)((float)(int)c2.r * global_2d_color.r), (byte)((float)(int)c2.g * global_2d_color.g), (byte)((float)(int)c2.b * global_2d_color.b), (byte)((float)(int)c2.a * global_2d_color.a));
		p_Quad_vertices3[2].c = new Color((byte)((float)(int)c3.r * global_2d_color.r), (byte)((float)(int)c3.g * global_2d_color.g), (byte)((float)(int)c3.b * global_2d_color.b), (byte)((float)(int)c3.a * global_2d_color.a));
		p_Quad_vertices3[3].c = new Color((byte)((float)(int)c1.r * global_2d_color.r), (byte)((float)(int)c1.g * global_2d_color.g), (byte)((float)(int)c1.b * global_2d_color.b), (byte)((float)(int)c1.a * global_2d_color.a));
		p_Quad_vertices3[4].c = new Color((byte)((float)(int)c3.r * global_2d_color.r), (byte)((float)(int)c3.g * global_2d_color.g), (byte)((float)(int)c3.b * global_2d_color.b), (byte)((float)(int)c3.a * global_2d_color.a));
		p_Quad_vertices3[5].c = new Color((byte)((float)(int)c4.r * global_2d_color.r), (byte)((float)(int)c4.g * global_2d_color.g), (byte)((float)(int)c4.b * global_2d_color.b), (byte)((float)(int)c4.a * global_2d_color.a));
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i].xyz.X = p_Quad_vertices3[i].xyz.X;
			vertices[i].xyz.Y = p_Quad_vertices3[i].xyz.Y;
			vertices[i].xyz.Z = p_Quad_vertices3[i].xyz.Z;
			vertices[i].uw.X = p_Quad_vertices3[i].uw.X;
			vertices[i].uw.Y = p_Quad_vertices3[i].uw.Y;
			vertices[i].c = p_Quad_vertices3[i].c;
		}
	}

	public static void DrawTextDynBuffer(ref Quad_VertexF3[] p_vertices, int numfaces)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		m_BuffVertices[polyCount * 3].xyz.X = p_vertices[2].xyz.x;
		m_BuffVertices[polyCount * 3].xyz.Y = p_vertices[2].xyz.y;
		m_BuffVertices[polyCount * 3].xyz.Z = p_vertices[2].xyz.z;
		m_BuffVertices[polyCount * 3].uw.X = p_vertices[2].uw.x;
		m_BuffVertices[polyCount * 3].uw.Y = p_vertices[2].uw.y;
		m_BuffVertices[polyCount * 3].c = p_vertices[0].c;
		m_BuffVertices[1 + polyCount * 3].xyz.X = p_vertices[3].xyz.x;
		m_BuffVertices[1 + polyCount * 3].xyz.Y = p_vertices[3].xyz.y;
		m_BuffVertices[1 + polyCount * 3].xyz.Z = p_vertices[3].xyz.z;
		m_BuffVertices[1 + polyCount * 3].uw.X = p_vertices[3].uw.x;
		m_BuffVertices[1 + polyCount * 3].uw.Y = p_vertices[3].uw.y;
		m_BuffVertices[1 + polyCount * 3].c = p_vertices[0].c;
		m_BuffVertices[2 + polyCount * 3].xyz.X = p_vertices[0].xyz.x;
		m_BuffVertices[2 + polyCount * 3].xyz.Y = p_vertices[0].xyz.y;
		m_BuffVertices[2 + polyCount * 3].xyz.Z = p_vertices[0].xyz.z;
		m_BuffVertices[2 + polyCount * 3].uw.X = p_vertices[0].uw.x;
		m_BuffVertices[2 + polyCount * 3].uw.Y = p_vertices[0].uw.y;
		m_BuffVertices[2 + polyCount * 3].c = p_vertices[0].c;
		m_BuffVertices[3 + polyCount * 3].xyz.X = p_vertices[0].xyz.x;
		m_BuffVertices[3 + polyCount * 3].xyz.Y = p_vertices[0].xyz.y;
		m_BuffVertices[3 + polyCount * 3].xyz.Z = p_vertices[0].xyz.z;
		m_BuffVertices[3 + polyCount * 3].uw.X = p_vertices[0].uw.x;
		m_BuffVertices[3 + polyCount * 3].uw.Y = p_vertices[0].uw.y;
		m_BuffVertices[3 + polyCount * 3].c = p_vertices[0].c;
		m_BuffVertices[4 + polyCount * 3].xyz.X = p_vertices[3].xyz.x;
		m_BuffVertices[4 + polyCount * 3].xyz.Y = p_vertices[3].xyz.y;
		m_BuffVertices[4 + polyCount * 3].xyz.Z = p_vertices[3].xyz.z;
		m_BuffVertices[4 + polyCount * 3].uw.X = p_vertices[3].uw.x;
		m_BuffVertices[4 + polyCount * 3].uw.Y = p_vertices[3].uw.y;
		m_BuffVertices[4 + polyCount * 3].c = p_vertices[0].c;
		m_BuffVertices[5 + polyCount * 3].xyz.X = p_vertices[1].xyz.x;
		m_BuffVertices[5 + polyCount * 3].xyz.Y = p_vertices[1].xyz.y;
		m_BuffVertices[5 + polyCount * 3].xyz.Z = p_vertices[1].xyz.z;
		m_BuffVertices[5 + polyCount * 3].uw.X = p_vertices[1].uw.x;
		m_BuffVertices[5 + polyCount * 3].uw.Y = p_vertices[1].uw.y;
		m_BuffVertices[5 + polyCount * 3].c = p_vertices[0].c;
		polyCount += 2;
	}

	public static void DrawText(ref Quad_VertexF3[] p_vertices, int numfaces)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			for (int i = 0; i < p_vertices.Length; i++)
			{
				vertices[i].xyz.X = p_vertices[i].xyz.X;
				vertices[i].xyz.Y = p_vertices[i].xyz.Y;
				vertices[i].xyz.Z = p_vertices[i].xyz.Z;
				vertices[i].uw.X = p_vertices[i].uw.X;
				vertices[i].uw.Y = p_vertices[i].uw.Y;
				vertices[i].c = p_vertices[i].c;
			}
			if (DrawsPerFrame < 500000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>((PrimitiveType)5, vertices, 0, numfaces);
			}
			DrawsPerFrame++;
			PolygonsPerFrame += numfaces;
		}
	}

	public static void DrawVertexArray(Vertexeffect.cnvertexf_t[] p_vertices, int numfaces, ulong vertex_format, PrimitiveType primitive_type)
	{
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			ulong num = 12uL;
			if ((vertex_format & 0x10) != 0)
			{
				num += 12;
			}
			if ((vertex_format & 0x100) != 0)
			{
				num += 8;
			}
			if ((vertex_format & 0x20) != 0)
			{
				num += 4;
			}
			if ((vertex_format & 0x40) != 0)
			{
				num += 4;
			}
			if ((vertex_format & 0x80) != 0)
			{
				num += 4;
			}
			for (int i = 0; i < p_vertices.Length; i++)
			{
				vertices2[i].xyz.X = p_vertices[i].xyz.X;
				vertices2[i].xyz.Y = p_vertices[i].xyz.Y;
				vertices2[i].xyz.Z = p_vertices[i].xyz.Z;
				vertices2[i].uw.X = p_vertices[i].uv.x;
				vertices2[i].uw.Y = p_vertices[i].uv.y;
				vertices2[i].c = p_vertices[i].c.GetD3DColor();
			}
			SetVertexPixelShadersData(vertex_format);
			if (DrawsPerFrame < 500000)
			{
				graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>(primitive_type, vertices2, 0, numfaces);
			}
			DrawsPerFrame++;
			PolygonsPerFrame += numfaces;
		}
	}

	public static void DrawVertexArray(ref Quad_VertexF3[] p_vertices, int numfaces, ulong vertex_format, PrimitiveType primitive_type)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			ulong num = 12uL;
			if ((vertex_format & 0x10) != 0)
			{
				num += 12;
			}
			if ((vertex_format & 0x100) != 0)
			{
				num += 8;
			}
			if ((vertex_format & 0x20) != 0)
			{
				num += 4;
			}
			if ((vertex_format & 0x40) != 0)
			{
				num += 4;
			}
			if ((vertex_format & 0x80) != 0)
			{
				num += 4;
			}
			for (int i = 0; i < p_vertices.Length; i++)
			{
				vertices[i].xyz.X = p_vertices[i].xyz.X;
				vertices[i].xyz.Y = p_vertices[i].xyz.Y;
				vertices[i].xyz.Z = p_vertices[i].xyz.Z;
				vertices[i].uw.X = p_vertices[i].uw.X;
				vertices[i].uw.Y = p_vertices[i].uw.Y;
				vertices[i].c = p_vertices[i].c;
			}
			SetVertexPixelShadersData(vertex_format);
			graphicsDevice.DrawUserPrimitives<d3d_vertF3_tex1_xyz_diffuse>(primitive_type, vertices, 0, numfaces);
			PolygonsPerFrame += numfaces;
		}
	}

	public static void GroundStart()
	{
		m_GroundFaces = 0;
	}

	public static void GroundDraw()
	{
		if (groundpolyCount != 0)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SetVertexPixelShadersData(322uL);
			graphicsDevice.DrawUserPrimitives<d3d_vertF3>((PrimitiveType)4, m_GroundVertices, 0, groundpolyCount);
			if (DrawsPerFrame < 500000)
			{
				DrawsPerFrame++;
			}
			PolygonsPerFrame += groundpolyCount;
		}
	}

	public static void GroundEnd()
	{
	}

	public static void GroundAddQuad(frect_t pos, frect_t uw, dcolor_t color, float z)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		Color c = default(Color);
		((Color)(ref c))._002Ector((byte)((float)(int)color.r * SCGlobals.global_2d_color.r), (byte)((float)(int)color.g * SCGlobals.global_2d_color.g), (byte)((float)(int)color.b * SCGlobals.global_2d_color.b), (byte)((float)(int)color.a * SCGlobals.global_2d_color.a));
		m_GroundVertices[m_GroundFaces].c = c;
		m_GroundVertices[m_GroundFaces].xyz.X = pos.x0;
		m_GroundVertices[m_GroundFaces].xyz.Y = pos.y0;
		m_GroundVertices[m_GroundFaces].xyz.Z = z;
		m_GroundVertices[m_GroundFaces].uw.X = uw.x0;
		m_GroundVertices[m_GroundFaces].uw.Y = uw.y0;
		m_GroundVertices[m_GroundFaces + 1].c = c;
		m_GroundVertices[m_GroundFaces + 1].xyz.X = pos.x1;
		m_GroundVertices[m_GroundFaces + 1].xyz.Y = pos.y1;
		m_GroundVertices[m_GroundFaces + 1].xyz.Z = z;
		m_GroundVertices[m_GroundFaces + 1].uw.X = uw.x1;
		m_GroundVertices[m_GroundFaces + 1].uw.Y = uw.y1;
		m_GroundVertices[m_GroundFaces + 2].c = c;
		m_GroundVertices[m_GroundFaces + 2].xyz.X = pos.x0;
		m_GroundVertices[m_GroundFaces + 2].xyz.Y = pos.y1;
		m_GroundVertices[m_GroundFaces + 2].xyz.Z = z;
		m_GroundVertices[m_GroundFaces + 2].uw.X = uw.x0;
		m_GroundVertices[m_GroundFaces + 2].uw.Y = uw.y1;
		m_GroundFaces += 3;
		m_GroundVertices[m_GroundFaces].c = c;
		m_GroundVertices[m_GroundFaces].xyz.X = pos.x0;
		m_GroundVertices[m_GroundFaces].xyz.Y = pos.y0;
		m_GroundVertices[m_GroundFaces].xyz.Z = z;
		m_GroundVertices[m_GroundFaces].uw.X = uw.x0;
		m_GroundVertices[m_GroundFaces].uw.Y = uw.y0;
		m_GroundVertices[m_GroundFaces + 1].c = c;
		m_GroundVertices[m_GroundFaces + 1].xyz.X = pos.x1;
		m_GroundVertices[m_GroundFaces + 1].xyz.Y = pos.y0;
		m_GroundVertices[m_GroundFaces + 1].xyz.Z = z;
		m_GroundVertices[m_GroundFaces + 1].uw.X = uw.x1;
		m_GroundVertices[m_GroundFaces + 1].uw.Y = uw.y0;
		m_GroundVertices[m_GroundFaces + 2].c = c;
		m_GroundVertices[m_GroundFaces + 2].xyz.X = pos.x1;
		m_GroundVertices[m_GroundFaces + 2].xyz.Y = pos.y1;
		m_GroundVertices[m_GroundFaces + 2].xyz.Z = z;
		m_GroundVertices[m_GroundFaces + 2].uw.X = uw.x1;
		m_GroundVertices[m_GroundFaces + 2].uw.Y = uw.y1;
		m_GroundFaces += 3;
	}

	public static void DrawLine(vec3_t p1, vec3_t p2)
	{
		if (!diableDrawing)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			vec3_t[] array = new vec3_t[2] { p1, p2 };
			Vector3[] array2 = (Vector3[])(object)new Vector3[2];
			array2[0].X = array[0].x;
			array2[0].Y = array[0].y;
			array2[0].Z = array[0].z;
			array2[1].X = array[1].x;
			array2[1].Y = array[1].y;
			array2[1].Z = array[1].z;
			SetVertexPixelShadersData(2uL);
			graphicsDevice.DrawUserPrimitives<Vector3>((PrimitiveType)2, array2, 0, 1);
			if (DrawsPerFrame < 500000)
			{
				DrawsPerFrame++;
			}
			PolygonsPerFrame++;
		}
	}

	public static void DrawLine(vec3_t p1, vec3_t p2, float thick, dcolor_t col)
	{
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t2 = (p1 - p2).cross(SCGlobals.cam.forward_vector() * -1f);
		vec3_t2.normalize();
		vec3_t2 *= thick;
		DrawColorQuad(vec3_t2 * -1f + p2, vec3_t2 + p2, vec3_t2 + p1, vec3_t2 * -1f + p1, col);
	}

	public static void DrawEmptyRect(frect_t rect)
	{
		BindTextureNull();
		float x = rect.p0.x;
		float y = rect.p0.y;
		float x2 = rect.p1.x;
		float y2 = rect.p1.y;
		p1.assign(x, y, 0f);
		p2.assign(x, y2, 0f);
		DrawLine(p1, p2);
		p1.assign(x, y, 0f);
		p2.assign(x2, y, 0f);
		DrawLine(p1, p2);
		p1.assign(x, y2, 0f);
		p2.assign(x2, y2, 0f);
		DrawLine(p1, p2);
		p1.assign(x2, y, 0f);
		p2.assign(x2, y2, 0f);
		DrawLine(p1, p2);
	}

	public static void DrawEmptyRect(frect_t rect, float thick, dcolor_t c)
	{
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = default(vec3_t);
		float x = rect.p0.x;
		float y = rect.p0.y;
		float x2 = rect.p1.x;
		float y2 = rect.p1.y;
		vec3_t2.assign(x, y, 0f);
		vec3_t3.assign(x, y2, 0f);
		DrawLine(vec3_t2, vec3_t3, thick, c);
		vec3_t2.assign(x, y, 0f);
		vec3_t3.assign(x2, y, 0f);
		DrawLine(vec3_t2, vec3_t3, thick, c);
		vec3_t2.assign(x, y2, 0f);
		vec3_t3.assign(x2, y2, 0f);
		DrawLine(vec3_t2, vec3_t3, thick, c);
		vec3_t2.assign(x2, y, 0f);
		vec3_t3.assign(x2, y2, 0f);
		DrawLine(vec3_t2, vec3_t3, thick, c);
	}

	public static void SetVertexPixelShadersData(ulong vertex_format)
	{
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		setFVF(vertex_format);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (graphicsDevice != null)
		{
			graphicsDevice.VertexShader = vertexShader;
			graphicsDevice.PixelShader = pixelShader;
			mFloatArray[0] = 0f;
			mFloatArray[1] = 0f;
			mFloatArray[2] = 0f;
			mFloatArray[3] = 0f;
			if ((vertex_format & 0x10) != 0)
			{
				mFloatArray[0] = 1f;
			}
			if ((vertex_format & 0x20) != 0)
			{
				mFloatArray[1] = 1f;
			}
			if ((vertex_format & 0x40) != 0)
			{
				mFloatArray[2] = 1f;
			}
			if ((vertex_format & 0x80) != 0)
			{
				mFloatArray[3] = 1f;
			}
			graphicsDevice.SetVertexShaderConstant(0, mFloatArray);
			SCGlobals.gMaterial.Diffuse.ToArray(ref color_array);
			graphicsDevice.SetVertexShaderConstant(1, color_array);
			SCGlobals.gMaterial.Ambient.ToArray(ref color_array);
			graphicsDevice.SetVertexShaderConstant(2, color_array);
			SCGlobals.gMaterial.Specular.ToArray(ref color_array);
			graphicsDevice.SetVertexShaderConstant(3, color_array);
			SCGlobals.gMaterial.Emissive.ToArray(ref color_array);
			graphicsDevice.SetVertexShaderConstant(4, color_array);
			mFloatArray[0] = SCGlobals.gMaterial.Power;
			mFloatArray[1] = 0f;
			mFloatArray[2] = 0f;
			mFloatArray[3] = 0f;
			graphicsDevice.SetVertexShaderConstant(5, mFloatArray);
			SCGlobals.gLight.Diffuse.ToVector4(ref MyVColor);
			graphicsDevice.SetVertexShaderConstant(6, MyVColor);
			SCGlobals.gLight.Ambient.ToVector4(ref MyVColor);
			graphicsDevice.SetVertexShaderConstant(7, MyVColor);
			SCGlobals.gLight.Specular.ToVector4(ref MyVColor);
			graphicsDevice.SetVertexShaderConstant(6, MyVColor);
			mFloatArray[0] = SCGlobals.gLight.Direction.x;
			mFloatArray[1] = SCGlobals.gLight.Direction.y;
			mFloatArray[2] = SCGlobals.gLight.Direction.z;
			mFloatArray[3] = 0f;
			graphicsDevice.SetVertexShaderConstant(9, mFloatArray);
			mFloatArray[0] = ((SCGlobals.gLightEnable && SCGlobals.gLighting) ? 1f : 0f);
			mFloatArray[1] = SCGlobals.gLight.Range;
			mFloatArray[2] = SCGlobals.gLight.Attenuation0;
			mFloatArray[3] = 0f;
			graphicsDevice.SetVertexShaderConstant(10, mFloatArray);
			SuperCowHelper.LocalMatrixToMatrix(SCGlobals.gWorldMatrix, ref matrixWorld);
			SuperCowHelper.LocalMatrixToMatrix(SCGlobals.gViewMatrix, ref matrixView);
			SuperCowHelper.LocalMatrixToMatrix(SCGlobals.gProjectionMatrix, ref matrixProj);
			graphicsDevice.SetVertexShaderConstant(12, matrixWorld);
			graphicsDevice.SetVertexShaderConstant(16, matrixView);
			graphicsDevice.SetVertexShaderConstant(20, matrixProj);
			mFloatArray[0] = 0f;
			if (SCGlobals.bTextureExists)
			{
				mFloatArray[0] = 1f;
			}
			graphicsDevice.SetPixelShaderConstant(0, mFloatArray);
		}
	}

	public static void SetBlend(int bt)
	{
		if (blendType != bt)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SCGlobals.CountRenderStateChanges++;
			switch (bt)
			{
			case 0:
				graphicsDevice.RenderState.SourceBlend = (Blend)5;
				graphicsDevice.RenderState.DestinationBlend = (Blend)6;
				break;
			case 1:
				graphicsDevice.RenderState.SourceBlend = (Blend)3;
				graphicsDevice.RenderState.DestinationBlend = (Blend)2;
				break;
			case 2:
				graphicsDevice.RenderState.SourceBlend = (Blend)1;
				graphicsDevice.RenderState.DestinationBlend = (Blend)2;
				break;
			case 5:
				graphicsDevice.RenderState.SourceBlend = (Blend)1;
				graphicsDevice.RenderState.DestinationBlend = (Blend)6;
				break;
			case 3:
				graphicsDevice.RenderState.SourceBlend = (Blend)5;
				graphicsDevice.RenderState.DestinationBlend = (Blend)2;
				break;
			case 4:
				graphicsDevice.RenderState.SourceBlend = (Blend)2;
				graphicsDevice.RenderState.DestinationBlend = (Blend)5;
				break;
			}
			blendType = bt;
		}
	}

	public static void ClearAllStates()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		SetLighting(theEnable: false);
		graphicsDevice.RenderState.DepthBufferEnable = false;
		graphicsDevice.RenderState.DepthBufferWriteEnable = true;
		graphicsDevice.RenderState.AlphaBlendEnable = false;
		graphicsDevice.RenderState.AlphaTestEnable = false;
		graphicsDevice.RenderState.PointSpriteEnable = false;
	}

	public static void EnableRS(uint rs)
	{
		if (!renderStates[rs])
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SCGlobals.CountRenderStateChanges++;
			switch (rs)
			{
			case 0u:
				SetLighting(theEnable: true);
				break;
			case 1u:
				graphicsDevice.RenderState.DepthBufferEnable = true;
				break;
			case 2u:
				graphicsDevice.RenderState.DepthBufferWriteEnable = true;
				break;
			case 3u:
				SetOrtho(800f, 600f, 1f, 500f);
				break;
			case 4u:
				graphicsDevice.RenderState.AlphaBlendEnable = true;
				break;
			case 5u:
				graphicsDevice.RenderState.AlphaTestEnable = true;
				break;
			case 6u:
				graphicsDevice.RenderState.PointSpriteEnable = true;
				break;
			case 7u:
				graphicsDevice.SamplerStates[0].MipFilter = (TextureFilter)2;
				graphicsDevice.SamplerStates[0].MinFilter = (TextureFilter)3;
				graphicsDevice.SamplerStates[0].MagFilter = (TextureFilter)3;
				break;
			}
			renderStates[rs] = true;
		}
	}

	public static void DisableRS(uint rs)
	{
		if (renderStates[rs])
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			SCGlobals.CountRenderStateChanges++;
			switch (rs)
			{
			case 0u:
				SetLighting(theEnable: false);
				break;
			case 1u:
				graphicsDevice.RenderState.DepthBufferEnable = false;
				break;
			case 2u:
				graphicsDevice.RenderState.DepthBufferWriteEnable = false;
				break;
			case 3u:
				SetProjection(SCGlobals.projection);
				break;
			case 4u:
				graphicsDevice.RenderState.AlphaBlendEnable = false;
				break;
			case 5u:
				graphicsDevice.RenderState.AlphaTestEnable = false;
				break;
			case 6u:
				graphicsDevice.RenderState.PointSpriteEnable = false;
				break;
			case 7u:
				graphicsDevice.SamplerStates[0].MipFilter = (TextureFilter)2;
				graphicsDevice.SamplerStates[0].MinFilter = (TextureFilter)3;
				graphicsDevice.SamplerStates[0].MagFilter = (TextureFilter)3;
				break;
			}
			renderStates[rs] = false;
		}
	}

	public static void LoadCameraNull()
	{
		SCGlobals.gViewMatrix.load_identity();
	}

	public static void LoadCamera(matrix_t m)
	{
		SCGlobals.gViewMatrix = m;
	}

	public static void LoadMatrixNull()
	{
		SCGlobals.gWorldMatrix.load_identity();
	}

	public static void LoadMatrix(matrix_t m)
	{
		SCGlobals.gWorldMatrix = m;
	}

	public static void SetProjection(matrix_t m)
	{
		SCGlobals.gProjectionMatrix = m;
	}

	public static void GetProjection(ref matrix_t m)
	{
		m = SCGlobals.gProjectionMatrix;
	}

	public static void GetCamera(ref matrix_t m)
	{
		m = SCGlobals.gViewMatrix;
	}

	public static void GetMatrix(ref matrix_t m)
	{
		m = SCGlobals.gWorldMatrix;
	}

	public static void CalculateSmoothNormals(int numvertex, int numfaces, vec3_t[] p_vertex, int[] p_faces, ref vec3_t[] normals)
	{
		vec3_t[] array = null;
		if (normals == null)
		{
			array = new vec3_t[numvertex];
		}
		normals = array;
		for (int i = 0; i < numvertex; i++)
		{
			array[i].assign(0f, 0f, 0f);
		}
		for (int i = 0; i < numfaces; i++)
		{
			vec3_t vec3_t2 = p_vertex[p_faces[i * 3]];
			vec3_t vec3_t3 = p_vertex[p_faces[i * 3 + 1]];
			vec3_t vec3_t4 = p_vertex[p_faces[i * 3 + 2]];
			vec3_t vec3_t5 = default(vec3_t);
			vec3_t v = default(vec3_t);
			vec3_t5.assign(vec3_t4.X - vec3_t2.X, vec3_t4.Y - vec3_t2.Y, vec3_t4.Z - vec3_t2.Z);
			v.assign(vec3_t3.X - vec3_t2.X, vec3_t3.Y - vec3_t2.Y, vec3_t3.Z - vec3_t2.Z);
			vec3_t vec3_t6 = vec3_t5.cross(v);
			vec3_t6.normalize();
			array[p_faces[i * 3]] += vec3_t6;
			array[p_faces[i * 3 + 1]] += vec3_t6;
			array[p_faces[i * 3 + 2]] += vec3_t6;
		}
		for (int i = 0; i < numvertex; i++)
		{
			array[i].normalize();
		}
	}

	public static void CalculateNormals(int numfaces, vec3_t[] p_vertex, int[] p_faces, ref vec3_t[] normals)
	{
		vec3_t[] array = null;
		if (normals == null)
		{
			array = new vec3_t[numfaces];
		}
		normals = array;
		for (int i = 0; i < numfaces; i++)
		{
			vec3_t vec3_t2 = p_vertex[p_faces[i * 3]];
			vec3_t vec3_t3 = p_vertex[p_faces[i * 3 + 1]];
			vec3_t vec3_t4 = p_vertex[p_faces[i * 3 + 2]];
			vec3_t vec3_t5 = default(vec3_t);
			vec3_t v = default(vec3_t);
			vec3_t5.assign(vec3_t4.X - vec3_t2.X, vec3_t4.Y - vec3_t2.Y, vec3_t4.Z - vec3_t2.Z);
			v.assign(vec3_t3.X - vec3_t2.X, vec3_t3.Y - vec3_t2.Y, vec3_t3.Z - vec3_t2.Z);
			vec3_t vec3_t6 = vec3_t5.cross(v);
			vec3_t6.normalize();
			array[i] = vec3_t6;
		}
	}

	public static void InitMesh(ref mesh_t p_mesh, uint vertex_format)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		p_mesh.vertex_format = vertex_format;
		p_mesh.vertex_size = 12;
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if ((vertex_format & 0x10) != 0)
		{
			p_mesh.vertex_size += 12;
		}
		if ((vertex_format & 0x100) != 0)
		{
			p_mesh.vertex_size += 8;
		}
		if (p_mesh.b_smooth)
		{
			DynamicVertexBuffer vertex_buffer = new DynamicVertexBuffer(graphicsDevice, p_mesh.numvertex * p_mesh.vertex_size, (BufferUsage)8);
			p_mesh.vertex_buffer = vertex_buffer;
			DynamicIndexBuffer index_buffer = new DynamicIndexBuffer(graphicsDevice, p_mesh.numfaces * 3 * 2, (BufferUsage)8, (IndexElementSize)0);
			p_mesh.index_buffer = index_buffer;
			vertex_t_d3d[] array = new vertex_t_d3d[p_mesh.numvertex];
			for (int i = 0; i < p_mesh.numvertex; i++)
			{
				ref vertex_t_d3d reference = ref array[i];
				reference = SuperCowHelper.ToVertexD3D(p_mesh.p_vertices[i]);
			}
			((VertexBuffer)p_mesh.vertex_buffer).SetData<vertex_t_d3d>(array);
			short[] array2 = new short[p_mesh.numfaces * 3];
			for (int j = 0; j < p_mesh.numfaces * 3; j++)
			{
				array2[j] = (short)p_mesh.p_faces[j];
			}
			((IndexBuffer)p_mesh.index_buffer).SetData<short>(array2);
		}
		else
		{
			DynamicVertexBuffer vertex_buffer2 = new DynamicVertexBuffer(graphicsDevice, p_mesh.numfaces * 3 * p_mesh.vertex_size, (BufferUsage)8);
			p_mesh.vertex_buffer = vertex_buffer2;
			vertex_t_d3d[] array3 = new vertex_t_d3d[p_mesh.numfaces * 3];
			for (int k = 0; k < p_mesh.numfaces * 3; k++)
			{
				ref vertex_t_d3d reference2 = ref array3[k];
				reference2 = SuperCowHelper.ToVertexD3D(p_mesh.p_vertices[k]);
			}
			((VertexBuffer)p_mesh.vertex_buffer).SetData<vertex_t_d3d>(array3);
		}
	}

	public static void SetOrtho(float width, float height, float zn, float zf)
	{
		float num = 1.3333334f;
		if (SuperCow.IsHDTV)
		{
			num = 1.7777778f;
		}
		width = height * num;
		SCGlobals.gProjectionMatrix.load_identity();
		SCGlobals.gProjectionMatrix.ortho(width, height, zn, zf);
	}

	public static void SetMaterial(material_t material)
	{
		SCGlobals.gMaterial = material;
	}

	public static void BindTextureNull()
	{
		SuperCow.DeviceManager.GraphicsDevice.Textures[0] = null;
		SCGlobals.bTextureExists = false;
		cachedTexture = null;
	}

	public static void BindTextureForced(texture_t p_tex)
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (p_tex.mImage != null)
		{
			graphicsDevice.Textures[0] = (Texture)(object)p_tex.mImage;
		}
		else
		{
			graphicsDevice.Textures[0] = null;
		}
		SCGlobals.bTextureExists = p_tex.mImage != null;
	}

	public static void BindTexture(texture_t p_tex)
	{
		gTexCoords0.ux = p_tex.tex_coord0.ux;
		gTexCoords0.uy = p_tex.tex_coord0.uy;
		gTexCoords1.ux = p_tex.tex_coord1.ux;
		gTexCoords1.uy = p_tex.tex_coord1.uy;
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (p_tex.mImage != null)
		{
			graphicsDevice.Textures[0] = (Texture)(object)p_tex.mImage;
		}
		else
		{
			graphicsDevice.Textures[0] = null;
		}
		SCGlobals.bTextureExists = p_tex.mImage != null;
		cachedTexture = p_tex.mImage;
	}

	public static void BindHWTexture(Texture2D p_tex)
	{
		if (cachedTexture != p_tex)
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			if (p_tex != null)
			{
				graphicsDevice.Textures[0] = (Texture)(object)p_tex;
			}
			else
			{
				p_tex = null;
			}
			SCGlobals.bTextureExists = p_tex != null;
			cachedTexture = p_tex;
		}
	}

	public static void SetGlobal2DColor(color_t gc)
	{
		SCGlobals.global_2d_color = gc;
		SCGlobals.global_2d_color.check_max();
	}

	public static void PrepareTextFormat(ulong vertex_format)
	{
		SetVertexPixelShadersData(vertex_format);
		ulong num = 12uL;
		if ((vertex_format & 0x10) != 0)
		{
			num += 12;
		}
		if ((vertex_format & 0x100) != 0)
		{
			num += 8;
		}
		if ((vertex_format & 0x20) != 0)
		{
			num += 4;
		}
		if ((vertex_format & 0x40) != 0)
		{
			num += 4;
		}
		if ((vertex_format & 0x80) != 0)
		{
			num += 4;
		}
		mTextVertexSize = num;
	}

	public static void PrepareParticles(ulong vertex_format)
	{
		SetVertexPixelShadersData(vertex_format);
		mDrawingParticles = true;
	}

	public static void EndFrame()
	{
		PolygonsPerFrame = 0;
		DrawsPerFrame = 0;
		ObjectPerFrame = 0;
		SCGlobals.CountRenderStateChanges = 0;
	}

	public static void EndParticles()
	{
		mDrawingParticles = false;
	}

	public static void SetTextMode()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		DisableRS(1u);
		SetBlend(0);
		EnableRS(4u);
		EnableRS(5u);
		graphicsDevice.RenderState.ReferenceAlpha = 1;
		EnableRS(7u);
		LoadMatrixNull();
		LoadCameraNull();
		SetOrtho(800f, 600f, 1f, 100f);
	}

	public static void SetLight(uint index, light_t light)
	{
		SCGlobals.gLight = light;
	}

	public static void SetLighting(bool theEnable)
	{
		SCGlobals.gLighting = theEnable;
	}

	public static void EnableLight(uint index)
	{
		SCGlobals.gLightEnable = true;
	}

	public static void DisableLight(uint index)
	{
		SCGlobals.gLightEnable = false;
	}

	public static void SetMenuLight()
	{
		SCGlobals.matcommon.Diffuse.assign(1f, 1f, 1f, 1f);
		SCGlobals.matcommon.Ambient.assign(0.4f, 0.4f, 0.4f, 1f);
		SCGlobals.matselected.Diffuse.assign(1f, 1f, 1f, 1f);
		SCGlobals.matselected.Ambient.assign(0.4f, 0.4f, 0.4f, 1f);
		SCGlobals.color_selected.assign(byte.MaxValue, 0, 0, byte.MaxValue);
		SCGlobals.light.Ambient.assign(0.7f, 0.7f, 0.7f, 1f);
		SCGlobals.light.Diffuse.assign(0.9f, 0.9f, 0.9f, 0f);
		SCGlobals.light.Specular.assign(1f, 1f, 1f, 1f);
		SCGlobals.light.Direction.assign(1f, 1f, 0f);
		SCGlobals.light.Type = LightingType.LightDirectional;
		SCGlobals.light.Range = 500f;
		SCGlobals.light.Attenuation0 = 1f;
		SetLighting(theEnable: false);
		SetLight(0u, SCGlobals.light);
		EnableLight(0u);
		SetMaterial(SCGlobals.matcommon);
	}
}
