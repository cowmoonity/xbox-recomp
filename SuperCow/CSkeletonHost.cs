using System;

namespace SuperCow;

public class CSkeletonHost
{
	public int numskeletons;

	private skeleton_t[] ar_skl = new skeleton_t[512];

	private SVLink[] VertexLink = new SVLink[3];

	public CSkeletonHost()
	{
		numskeletons = 0;
	}

	public skeleton_t Add(string name)
	{
		skeleton_t skeleton_t2 = default(skeleton_t);
		if (numskeletons == 511)
		{
			return skeleton_t2;
		}
		skeleton_t2.name = name;
		ar_skl[numskeletons] = skeleton_t2;
		numskeletons++;
		return skeleton_t2;
	}

	public skeleton_t GetByName(string name)
	{
		for (int i = 0; i < numskeletons; i++)
		{
			if (ar_skl[i].name == name)
			{
				return ar_skl[i];
			}
		}
		return default(skeleton_t);
	}

	public skeleton_t GetByNum(int n)
	{
		return ar_skl[n];
	}

	public int GetHostSize()
	{
		return numskeletons;
	}

	public skeleton_t Load(string name)
	{
		skeleton_t byName = GetByName(name);
		if (byName.name != null)
		{
			return byName;
		}
		byName = Add(name);
		int len = 0;
		byte[] array = new byte[1];
		array = Store.storeLoadFileArray(name, ref len, b_errorexit: true);
		CBuffer cBuffer = new CBuffer();
		cBuffer.SetDataBuffer(array);
		byName.numverts = (int)cBuffer.ReadLong();
		byName.numbones = (int)cBuffer.ReadLong();
		len = byName.numbones;
		byName.p_bonevec = new vec3_t[len];
		for (int i = 0; i < byName.numbones; i++)
		{
			byName.p_bonevec[i].X = cBuffer.ReadFloat();
			byName.p_bonevec[i].Y = cBuffer.ReadFloat();
			byName.p_bonevec[i].Z = cBuffer.ReadFloat();
		}
		len = byName.numverts;
		byName.p_verlink = new SVLink[3][]
		{
			new SVLink[len],
			new SVLink[len],
			new SVLink[len]
		};
		for (int j = 0; j < byName.numverts; j++)
		{
			for (int k = 0; k < 3; k++)
			{
				byName.p_verlink[k][j].weight = cBuffer.ReadFloat();
				byName.p_verlink[k][j].ibone = (int)cBuffer.ReadLong();
				byName.p_verlink[k][j].offset.X = cBuffer.ReadFloat();
				byName.p_verlink[k][j].offset.Y = cBuffer.ReadFloat();
				byName.p_verlink[k][j].offset.Z = cBuffer.ReadFloat();
			}
		}
		len = byName.numbones;
		byName.p_bones = new skeleton_t.SBone[len];
		for (int l = 0; l < byName.numbones; l++)
		{
			byName.p_bones[l].numchild = (int)cBuffer.ReadLong();
			byName.p_bones[l].children = new int[5];
			byName.p_bones[l].children[0] = (int)cBuffer.ReadLong();
			byName.p_bones[l].children[1] = (int)cBuffer.ReadLong();
			byName.p_bones[l].children[2] = (int)cBuffer.ReadLong();
			byName.p_bones[l].children[3] = (int)cBuffer.ReadLong();
			byName.p_bones[l].children[4] = (int)cBuffer.ReadLong();
			byte[] data = (data = cBuffer.ReadBytes(32));
			byName.p_bones[l].name = Store.ConvertToString(data, 32);
		}
		byName.numfaces = (int)cBuffer.ReadLong();
		len = byName.numfaces * 3;
		byName.p_faces = new int[len];
		for (int m = 0; m < byName.numfaces * 3; m++)
		{
			byName.p_faces[m] = (int)cBuffer.ReadLong();
		}
		len = byName.numfaces * 3;
		vec2_t[] array2 = new vec2_t[len];
		for (int n = 0; n < byName.numfaces * 3; n++)
		{
			array2[n] = default(vec2_t);
			array2[n].X = cBuffer.ReadFloat();
			array2[n].Y = cBuffer.ReadFloat();
		}
		vec2_t[] array3 = new vec2_t[len];
		byName.numbufvers = 0;
		int[] array4 = new int[byName.numfaces * 3];
		int[] array5 = new int[byName.numfaces * 3];
		vec2_t vec2_t2 = default(vec2_t);
		for (int num = 0; num < byName.numverts; num++)
		{
			bool flag2;
			do
			{
				bool flag = true;
				flag2 = false;
				for (int num2 = 0; num2 < byName.numfaces * 3; num2++)
				{
					if (num == byName.p_faces[num2] && !(array2[num2].v0 > 1000f))
					{
						if (flag)
						{
							vec2_t2 = array2[num2];
							array2[num2].v0 += 10000f;
							flag = false;
							array5[num2] = byName.numbufvers;
						}
						else if (vec2_t2.v0 != array2[num2].v0 || vec2_t2.v1 != array2[num2].v1)
						{
							flag2 = true;
						}
						else
						{
							array2[num2].v0 += 10000f;
							array5[num2] = byName.numbufvers;
						}
					}
				}
				vec2_t2.v1 = 1f - vec2_t2.v1;
				array3[byName.numbufvers] = vec2_t2;
				array4[byName.numbufvers] = num;
				byName.numbufvers++;
			}
			while (flag2);
		}
		byName.p_buf_vert_indices = new int[byName.numbufvers];
		Buffer.BlockCopy(array4, 0, byName.p_buf_vert_indices, 0, byName.numbufvers * 4);
		byName.p_texcoord = new vec2_t[byName.numbufvers];
		for (int num3 = 0; num3 < byName.numbufvers; num3++)
		{
			ref vec2_t reference = ref byName.p_texcoord[num3];
			reference = array3[num3];
		}
		_ = SuperCow.DeviceManager.GraphicsDevice;
		byName.p_indices = new int[byName.numfaces * 3];
		for (int num4 = 0; num4 < byName.numfaces * 3; num4++)
		{
			byName.p_indices[num4] = array5[num4];
		}
		byName.p_vertex = new vec3_t[byName.numverts];
		byName.p_normals = new vec3_t[byName.numverts];
		byName.p_CurBoneVec = new vec3_t[byName.numbones];
		byName.p_CurBoneMatrix = new matrix_t[byName.numbones];
		ar_skl[numskeletons - 1] = byName;
		return byName;
	}
}
