namespace SuperCow;

public class CMeshHost
{
	public mesh_t[] ar_mesh = new mesh_t[1024];

	public int nummeshes;

	public CMeshHost()
	{
		nummeshes = 0;
	}

	public mesh_t Add(string name)
	{
		mesh_t result = (ar_mesh[nummeshes] = default(mesh_t));
		result.name = name;
		nummeshes++;
		return result;
	}

	public mesh_t GetByName(string name)
	{
		for (int i = 0; i < nummeshes; i++)
		{
			if (name == ar_mesh[i].name)
			{
				return ar_mesh[i];
			}
		}
		return default(mesh_t);
	}

	public int GetHostSize()
	{
		return nummeshes;
	}

	public mesh_t Load(string name)
	{
		mesh_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		byName = new mesh_t
		{
			bbox = new bbox_t(),
			bsphere = new bsphere_t()
		};
		int len = 0;
		byte[] p_mem = Store.storeLoadFileArray(name, ref len, b_errorexit: false);
		LoadFromMemory(ref byName, p_mem);
		byName.name = name;
		uint vertex_format = 274u;
		CowRender.InitMesh(ref byName, vertex_format);
		ar_mesh[nummeshes] = byName;
		nummeshes++;
		return byName;
	}

	public byte[] LoadFromMemory(ref mesh_t mesh, byte[] p_mem)
	{
		CBuffer cBuffer = new CBuffer();
		cBuffer.SetDataBuffer(p_mem);
		cBuffer.SkipData(3);
		mesh.b_smooth = cBuffer.ReadBool();
		vec3_t v = cBuffer.ReadVector3();
		mesh.mshmatrix = default(matrix_t);
		mesh.mshmatrix.load_identity();
		mesh.mshmatrix.translate(v);
		quaternion quaternion2 = default(quaternion);
		v = cBuffer.ReadVector3();
		quaternion2.EulerToQuaternion(v);
		quaternion2.QuaternionToMatrix(ref mesh.mshmatrix);
		cBuffer.ReadByte();
		vec3_t[] normals = null;
		int num = cBuffer.ReadInt();
		int num2 = cBuffer.ReadInt();
		int num3 = num;
		vec3_t[] array = new vec3_t[num3];
		for (int i = 0; i < num3; i++)
		{
			ref vec3_t reference = ref array[i];
			reference = cBuffer.ReadVector3();
		}
		num3 = num2 * 3;
		int[] array2 = new int[num3];
		for (int j = 0; j < num3; j++)
		{
			array2[j] = cBuffer.ReadInt();
		}
		num3 = num2 * 3;
		vec2_t[] array3 = new vec2_t[num3];
		for (int k = 0; k < num3; k++)
		{
			ref vec2_t reference2 = ref array3[k];
			reference2 = cBuffer.ReadVector2();
		}
		mesh.bbox.Calculate(array, num);
		mesh.bsphere.Calculate(array, num);
		for (int l = 0; l < num2 * 3; l++)
		{
			array3[l].Y = 1f - array3[l].Y;
		}
		if (mesh.b_smooth)
		{
			CowRender.CalculateSmoothNormals(num, num2, array, array2, ref normals);
			mesh.numvertex = 0;
			vertex_t[] array4 = new vertex_t[num2 * 3];
			int[] array5 = new int[num2 * 3];
			vec2_t uv = default(vec2_t);
			vertex_t[] array6 = array4;
			int num4 = 0;
			for (int m = 0; m < num; m++)
			{
				bool flag2;
				do
				{
					bool flag = true;
					flag2 = false;
					for (int n = 0; n < num2 * 3; n++)
					{
						if (m == array2[n] && !(array3[n].X > 1000f))
						{
							if (flag)
							{
								uv = array3[n];
								array3[n].X += 10000f;
								flag = false;
								array5[n] = mesh.numvertex;
							}
							else if (uv.X != array3[n].X || uv.Y != array3[n].Y)
							{
								flag2 = true;
							}
							else
							{
								array3[n].X += 10000f;
								array5[n] = mesh.numvertex;
							}
						}
					}
					array6[num4].xyz = array[m];
					array6[num4].normal = normals[m];
					array6[num4].uv = uv;
					num4++;
					mesh.numvertex++;
				}
				while (flag2);
			}
			mesh.p_vertices = new vertex_t[mesh.numvertex];
			for (int num5 = 0; num5 < mesh.numvertex; num5++)
			{
				ref vertex_t reference3 = ref mesh.p_vertices[num5];
				reference3 = array4[num5];
			}
			mesh.p_faces = array5;
			mesh.numfaces = num2;
		}
		else
		{
			mesh.numfaces = num2;
			mesh.numvertex = num2 * 3;
			mesh.p_vertices = new vertex_t[num2 * 3];
			for (int num6 = 0; num6 < num2; num6++)
			{
				mesh.p_vertices[num6].xyz = array[array2[num6]];
				mesh.p_vertices[num6].uv = array3[num6];
				mesh.p_vertices[num6].normal = normals[num6 / 3];
			}
		}
		return p_mem;
	}
}
