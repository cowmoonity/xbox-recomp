namespace SuperCow;

public class CGeomObjHost
{
	public int numobj;

	public CGeomObj[] ar_obj = new CGeomObj[1024];

	public CGeomObjHost()
	{
		numobj = 0;
	}

	public CGeomObj GetByNum(int num)
	{
		if (num < 1024)
		{
			return ar_obj[num];
		}
		return new CGeomObj();
	}

	public CGeomObj GetByName(string name)
	{
		for (int i = 0; i < numobj; i++)
		{
			if (ar_obj[i].name == name)
			{
				return ar_obj[i];
			}
		}
		return null;
	}

	public CGeomObj Load(string name)
	{
		CGeomObj byName = GetByName(name);
		if (byName != null)
		{
			return byName;
		}
		byName = Add(name);
		Store.SetCurrentDirectory("geomobjs", theAdd: false);
		byName.Load(name);
		ar_obj[numobj - 1] = byName;
		return byName;
	}

	public int GetHostSize()
	{
		return numobj;
	}

	public CGeomObj Add(string name)
	{
		CGeomObj cGeomObj = null;
		if (name.Contains(".cgo"))
		{
			cGeomObj = new CGeomObj();
		}
		cGeomObj.name = name;
		ar_obj[numobj] = cGeomObj;
		numobj++;
		return cGeomObj;
	}
}
