namespace SuperCow;

public class CAnimHost
{
	public int numstages;

	public animstage_t[] ar_stages = new animstage_t[1024];

	public CAnimHost()
	{
		numstages = 0;
	}

	public animstage_t Add(string name)
	{
		animstage_t animstage_t2 = new animstage_t();
		animstage_t2.name = "not loaded";
		if (numstages == 1023)
		{
			return animstage_t2;
		}
		animstage_t2.name = name;
		ar_stages[numstages] = animstage_t2;
		numstages++;
		return animstage_t2;
	}

	public void FreeAll()
	{
		numstages = 0;
	}

	public animstage_t GetByName(string name)
	{
		for (int i = 0; i <= numstages; i++)
		{
			if (ar_stages[i] != null && ar_stages[i].name == name)
			{
				return ar_stages[i];
			}
		}
		return null;
	}

	public animstage_t GetByNum(int n)
	{
		return ar_stages[n];
	}

	public int GetHostSize()
	{
		return numstages;
	}

	public animstage_t Load(string name)
	{
		animstage_t byName = GetByName(name);
		if (byName != null && byName.name != "not loaded")
		{
			return byName;
		}
		byName = Add(name);
		int len = 0;
		byte[] array = Store.storeLoadFileArray(name, ref len, b_errorexit: false);
		if (array.Length > 0)
		{
			CBuffer cBuffer = new CBuffer();
			cBuffer.SetDataBuffer(array);
			byName.numbones = cBuffer.ReadInt();
			byName.iFrameTime = cBuffer.ReadInt();
			byName.iFrameCount = cBuffer.ReadInt();
			byName.iStageTime = byName.iFrameCount * byName.iFrameTime;
			byName.p_bonequat = new quaternion[byName.numbones * byName.iFrameCount];
			byName.p_moving = new vec3_t[byName.iFrameCount];
			for (int i = 0; i < byName.iFrameCount; i++)
			{
				byName.p_moving[i] = default(vec3_t);
				ref vec3_t reference = ref byName.p_moving[i];
				reference = cBuffer.ReadVector3();
			}
			vec2_t vec2_t2 = new vec2_t
			{
				X = byName.p_moving[byName.iFrameCount - 1].X,
				Y = byName.p_moving[byName.iFrameCount - 1].Y
			};
			byName.step = vec2_t2.len() * (float)SCGlobals.ENGINE_FREQ / (float)(byName.iFrameTime * byName.iFrameCount);
			byName.physicframes = byName.iFrameCount * byName.iFrameTime / SCGlobals.ENGINE_FREQ + 2;
			byName.p_physicmove = new vec3_t[byName.physicframes];
			_ = (float)(byName.iFrameCount - 1) / (float)byName.physicframes;
			float num = 0f;
			for (int j = 0; j < byName.physicframes; j++)
			{
				ref vec3_t reference2 = ref byName.p_physicmove[j];
				reference2 = byName.p_moving[(int)num].interp(byName.p_moving[(int)num + 1], num - (float)(int)num);
			}
			for (int k = 0; k < byName.numbones; k++)
			{
				for (int l = 0; l < byName.iFrameCount; l++)
				{
					vec3_t vec3_t2 = default(vec3_t);
					vec3_t2 = cBuffer.ReadVector3();
					byName.p_bonequat[k * byName.iFrameCount + l] = default(quaternion);
					byName.p_bonequat[k * byName.iFrameCount + l].EulerToQuaternion(vec3_t2);
				}
			}
		}
		ar_stages[numstages - 1] = byName;
		return byName;
	}

	public int GetNumStage(string name)
	{
		for (int i = 0; i < numstages; i++)
		{
			if (name == ar_stages[i].name)
			{
				return i;
			}
		}
		return -1;
	}
}
