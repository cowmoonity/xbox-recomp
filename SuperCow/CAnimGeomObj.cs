using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CAnimGeomObj
{
	public struct SCharacter
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sklname;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string texname;
	}

	public string name;

	public vec3_t position = default(vec3_t);

	public vec3_t position0 = default(vec3_t);

	public float rotation;

	public float fscale;

	public static int tempzz;

	public matrix_t matrix = default(matrix_t);

	public matrix_t scalematrix = default(matrix_t);

	public matrix_t rotmatrix = default(matrix_t);

	public vec3_t correct_pos = default(vec3_t);

	public DynamicVertexBuffer vertex_buffer;

	public int numbufverts;

	public int[] p_buf_vert_indices;

	public vec3_t[] p_vertex;

	public vec3_t[] p_normals;

	public skeleton_t p_skl;

	public texture_t p_texture;

	public int IndexSpline1;

	public int IndexLThigh;

	public animstage_t p_stage;

	public animstage_t p_nextstage;

	public animstage_t p_stage_up;

	public bool b_divide;

	public vec3_t[] p_CurBoneVec;

	public matrix_t[] p_CurBoneMatrix;

	public float interstage;

	public float scale;

	public float scale_up;

	public int interval;

	public int interval_up;

	public float stage_timef;

	public float stageup_timef;

	public matrix_t leye_matrix;

	public matrix_t reye_matrix;

	public matrix_t head_matrix;

	public matrix_t hohol_matrix;

	public int leye_index;

	public int reye_index;

	public int head_index;

	public int hohol_index;

	public float coord_z;

	public bool b_overlap;

	public bool b_rotation;

	public bool b_offscreen;

	public float foverlap;

	public float total_overlap;

	public vec3_t move = default(vec3_t);

	public vec3_t dstep = default(vec3_t);

	public vec3_t curpos = default(vec3_t);

	public vec3_t framepos = default(vec3_t);

	public float dest_rotation;

	public float move_rot;

	public int numdirection;

	public float animspeed;

	public float currot;

	public float aRot;

	public static bool bOnce;

	private GraphicsDevice device;

	private vec3_t p1 = default(vec3_t);

	private vec3_t p2 = default(vec3_t);

	private vec3_t p3 = default(vec3_t);

	private vec3_t q1 = default(vec3_t);

	private vec3_t t1 = default(vec3_t);

	private vec3_t t2 = default(vec3_t);

	private vec3_t pass1 = default(vec3_t);

	private vec3_t pass2 = default(vec3_t);

	private vec3_t pass3 = default(vec3_t);

	private vec3_t pass4 = default(vec3_t);

	private matrix_t lm = default(matrix_t);

	private vertex_t_d3d[] p_vertexbuf3d;

	public CAnimGeomObj()
	{
		matrix = default(matrix_t);
		matrix.load_identity();
		scalematrix = default(matrix_t);
		scalematrix.load_identity();
		leye_matrix = default(matrix_t);
		leye_matrix.load_identity();
		reye_matrix = default(matrix_t);
		reye_matrix.load_identity();
		head_matrix = default(matrix_t);
		head_matrix.load_identity();
		Init();
	}

	public void Init()
	{
		matrix.load_identity();
		scalematrix.load_identity();
		leye_matrix.load_identity();
		reye_matrix.load_identity();
		head_matrix.load_identity();
		rotation = 0f;
		animspeed = 1f;
		foverlap = 300f;
		scale = 0f;
		move_rot = 0f;
		total_overlap = 200f;
		b_overlap = (b_divide = false);
		stage_timef = (stageup_timef = 0f);
		b_rotation = false;
		b_offscreen = false;
		animspeed = 0.8f;
		device = SuperCow.DeviceManager.GraphicsDevice;
	}

	public int GetBoneIndexByName(string bonename)
	{
		int num = 0;
		for (num = 0; num < p_skl.numbones; num++)
		{
			if (p_skl.p_bones[num].name == bonename)
			{
				return num;
			}
		}
		return -1;
	}

	public virtual bool Load(string filename)
	{
		SCharacter sCharacter = default(SCharacter);
		Init();
		name = filename;
		int len = 0;
		byte[] bytes = Store.storeLoadFileArray(filename, ref len, b_errorexit: false);
		sCharacter.sklname = SuperCowHelper.StripTrash(Encoding.ASCII.GetString(bytes, 0, 32));
		sCharacter.texname = SuperCowHelper.StripTrash(Encoding.ASCII.GetString(bytes, 32, 32));
		p_skl = SCGlobals.skeletonhost.Load(sCharacter.sklname);
		p_texture = SCGlobals.texturehost.Load(sCharacter.texname);
		IndexSpline1 = GetBoneIndexByName("Bip01 Spline1");
		IndexLThigh = GetBoneIndexByName("Bip01 L Thigh");
		leye_index = -1;
		reye_index = -1;
		leye_index = GetBoneIndexByName("Bip01 _L G");
		reye_index = GetBoneIndexByName("Bip01 _R G");
		head_index = GetBoneIndexByName("Bip01 Head");
		hohol_index = -1;
		numbufverts = p_skl.numbufvers;
		p_buf_vert_indices = p_skl.p_buf_vert_indices;
		vertex_buffer = p_skl.vertex_buffer;
		p_vertex = p_skl.p_vertex;
		p_normals = p_skl.p_normals;
		p_CurBoneVec = p_skl.p_CurBoneVec;
		p_CurBoneMatrix = p_skl.p_CurBoneMatrix;
		p_vertexbuf3d = new vertex_t_d3d[numbufverts];
		return true;
	}

	public virtual void Draw()
	{
		if (numbufverts != 0 && !b_offscreen)
		{
			CowRender.LoadMatrix(rotmatrix * matrix * scalematrix);
			if (SCGlobals.b_shadows)
			{
				CowRender.BindTextureNull();
			}
			else
			{
				CowRender.BindTexture(p_texture);
			}
			CowRender.SetVertexPixelShadersData(274uL);
			device.DrawUserIndexedPrimitives<vertex_t_d3d>((PrimitiveType)4, p_vertexbuf3d, 0, numbufverts, p_skl.p_indices, 0, p_skl.numfaces);
			CowRender.DrawsPerFrame++;
		}
	}

	public void CalculateRotation()
	{
		if (b_rotation)
		{
			float num = move_rot * SCGlobals.ftickperframe;
			rotation += num;
			if (Math.Abs(dest_rotation - rotation) <= Math.Abs(num))
			{
				rotation = dest_rotation;
				b_rotation = false;
				move_rot = 0f;
			}
		}
	}

	public virtual void CalcMath()
	{
		CalculateRotation();
		if (b_overlap)
		{
			if (foverlap >= total_overlap)
			{
				stage_timef = foverlap - total_overlap;
				p_stage = p_nextstage;
				b_overlap = false;
			}
			else
			{
				foverlap += SCGlobals.ftickperframe * animspeed;
				interstage = foverlap / total_overlap;
			}
		}
		stage_timef += animspeed * SCGlobals.ftickperframe;
		if (stage_timef >= (float)p_stage.iStageTime)
		{
			stage_timef -= p_stage.iStageTime;
		}
		interval = (int)stage_timef / p_stage.iFrameTime;
		scale = stage_timef / (float)p_stage.iFrameTime - (float)interval;
		PrepareBone(0, 0);
		int numverts = p_skl.numverts;
		SVLink[][] p_verlink = p_skl.p_verlink;
		for (int i = 0; i < numverts; i++)
		{
			p_vertex[i].x = 0f;
			p_vertex[i].y = 0f;
			p_vertex[i].z = 0f;
			for (int j = 0; j < 3; j++)
			{
				float weight = p_verlink[j][i].weight;
				if (weight != 0f)
				{
					int ibone = p_verlink[j][i].ibone;
					p_vertex[i] += (p_verlink[j][i].offset * p_CurBoneMatrix[ibone] + p_CurBoneVec[ibone]) * weight;
				}
			}
		}
		matrix.load_identity();
		curpos = position + move * SCGlobals.frameinterp;
		framepos = curpos;
		curpos.z = curpos.y;
		curpos.y = coord_z;
		currot = rotation;
		int num = ((interval != p_stage.iFrameCount - 1) ? (interval + 1) : 0);
		dstep = p_stage.p_moving[interval].interp(p_stage.p_moving[num], scale);
		if (b_overlap)
		{
			dstep = dstep * (1f - interstage) + p_nextstage.p_moving[0] * interstage;
		}
		matrix.translate(curpos);
		matrix.zrot_delta(currot + 90f);
		matrix.translate((dstep + correct_pos) * fscale);
		CalculateNormals();
	}

	public virtual void Calculate()
	{
		bool flag = false;
		CalcMath();
		for (int i = 0; i < numbufverts; i++)
		{
			p_vertexbuf3d[i].uv.X = p_skl.p_texcoord[i].v0;
			p_vertexbuf3d[i].uv.Y = p_skl.p_texcoord[i].v1;
			p_vertexbuf3d[i].xyz.X = p_vertex[p_buf_vert_indices[i]].v0;
			p_vertexbuf3d[i].xyz.Y = p_vertex[p_buf_vert_indices[i]].v1;
			p_vertexbuf3d[i].xyz.Z = p_vertex[p_buf_vert_indices[i]].v2;
			p_vertexbuf3d[i].normal.X = p_normals[p_buf_vert_indices[i]].v0;
			p_vertexbuf3d[i].normal.Y = p_normals[p_buf_vert_indices[i]].v1;
			p_vertexbuf3d[i].normal.Z = p_normals[p_buf_vert_indices[i]].v2;
		}
	}

	protected void PrepareBone(int ibone, int pbone)
	{
		if (ibone == 0)
		{
			p_CurBoneVec[0].assign(0f, 0f, 0f);
			BoneRotationMatrix(ref p_CurBoneMatrix[0], 0);
		}
		else
		{
			BoneRotationMatrix(ref lm, ibone);
			if (reye_index == ibone)
			{
				lm *= reye_matrix;
			}
			if (leye_index == ibone)
			{
				lm *= leye_matrix;
			}
			if (head_index == ibone)
			{
				lm *= head_matrix;
			}
			if (hohol_index == ibone)
			{
				lm *= hohol_matrix;
			}
			ref matrix_t reference = ref p_CurBoneMatrix[ibone];
			reference = p_CurBoneMatrix[pbone] * lm;
			ref vec3_t reference2 = ref p_CurBoneVec[ibone];
			reference2 = p_skl.p_bonevec[ibone] * p_CurBoneMatrix[pbone] + p_CurBoneVec[pbone];
		}
		for (int i = 0; i < p_skl.p_bones[ibone].numchild; i++)
		{
			PrepareBone(p_skl.p_bones[ibone].children[i], ibone);
		}
	}

	protected void CalculateNormals()
	{
		int numverts = p_skl.numverts;
		int numfaces = p_skl.numfaces;
		int[] p_faces = p_skl.p_faces;
		Array.Clear(p_normals, 0, p_normals.Length);
		for (int i = 0; i < numfaces; i++)
		{
			p1 = p_vertex[p_faces[i * 3]];
			p2 = p_vertex[p_faces[i * 3 + 1]];
			p3 = p_vertex[p_faces[i * 3 + 2]];
			t1.v0 = p3.v0 - p1.v0;
			t1.v1 = p3.v1 - p1.v1;
			t1.v2 = p3.v2 - p1.v2;
			t2.v0 = p2.v0 - p1.v0;
			t2.v1 = p2.v1 - p1.v1;
			t2.v2 = p2.v2 - p1.v2;
			q1 = t1.cross(t2);
			q1.normalize();
			p_normals[p_faces[i * 3]] += q1;
			p_normals[p_faces[i * 3 + 1]] += q1;
			p_normals[p_faces[i * 3 + 2]] += q1;
		}
		for (int i = 0; i < numverts; i++)
		{
			p_normals[i].normalize();
		}
	}

	protected void BoneRotationMatrix(ref matrix_t m, int ibone)
	{
		quaternion quaternion2 = default(quaternion);
		int num;
		animstage_t animstage_t2;
		float t;
		int num2;
		if (b_divide)
		{
			if (ibone < IndexSpline1)
			{
				num = ibone;
				num2 = interval;
				animstage_t2 = p_stage;
				t = scale;
			}
			else if (ibone >= IndexLThigh)
			{
				num = ibone - IndexLThigh + IndexSpline1;
				num2 = interval;
				animstage_t2 = p_stage;
				t = scale;
			}
			else
			{
				num = ibone - IndexSpline1;
				num2 = interval_up;
				animstage_t2 = p_stage_up;
				t = scale_up;
			}
		}
		else
		{
			num = ibone;
			num2 = interval;
			animstage_t2 = p_stage;
			t = scale;
		}
		quaternion quaternion3 = animstage_t2.p_bonequat[num * animstage_t2.iFrameCount + num2];
		num2 = ((num2 != animstage_t2.iFrameCount - 1) ? (num2 + 1) : 0);
		quaternion p = animstage_t2.p_bonequat[num * animstage_t2.iFrameCount + num2];
		quaternion2 = quaternion3.Slerp(p, t);
		if (b_overlap)
		{
			quaternion3 = p_nextstage.p_bonequat[num * p_nextstage.iFrameCount];
			quaternion2 = quaternion2.Slerp(quaternion3, interstage);
		}
		m.load_identity();
		quaternion2.QuaternionToMatrix(ref m);
	}

	public void Update(TimeSpan time)
	{
	}

	public void SetRotation(float angle)
	{
		if (Math.Abs(rotation - angle) > 180f)
		{
			if (rotation > angle)
			{
				rotation -= 360f;
			}
			else
			{
				rotation += 360f;
			}
		}
		dest_rotation = angle;
		move_rot = (angle - rotation) * animspeed / 200f;
		b_rotation = true;
	}

	public void SetRotation(float angle, float rot_time)
	{
		if (Math.Abs(rotation - angle) > 180f)
		{
			if (rotation > angle)
			{
				rotation -= 360f;
			}
			else
			{
				rotation += 360f;
			}
		}
		dest_rotation = angle;
		move_rot = (angle - rotation) * animspeed / rot_time;
		b_rotation = true;
	}

	public void SetAnimStage(animstage_t newstage)
	{
		p_nextstage = newstage;
		total_overlap = 200f;
		foverlap = 0f;
		b_overlap = true;
	}

	public void SetAnimStage(animstage_t newstage, float overlap_time)
	{
		p_nextstage = newstage;
		total_overlap = overlap_time;
		foverlap = 0f;
		b_overlap = true;
	}
}
