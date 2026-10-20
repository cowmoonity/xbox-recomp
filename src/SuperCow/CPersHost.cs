using System;

namespace SuperCow;

public class CPersHost
{
	public int numobjs;

	public CMonster[] ar_pers = new CMonster[1024];

	public CMonster[] ar_persprototype = new CMonster[1024];

	private string[] ar_monsternames = new string[16]
	{
		"snail", "mole", "dog", "bird", "spider", "spider2", "hedgehog", "boss_pig", "frog", "goat",
		"bat", "wasp", "bug", "bird2", "flycatcher", "prof"
	};

	public CPersHost()
	{
		numobjs = 0;
		SCGlobals.nummonsters = 0;
		SCGlobals.ar_monsters = ar_pers;
		for (int i = 0; i < 64; i++)
		{
			SCGlobals.ar_CPersSnail[i] = new CPersSnail();
			SCGlobals.ar_CPersMole[i] = new CPersMole();
			SCGlobals.ar_CPersDog[i] = new CPersDog();
			SCGlobals.ar_CPersBird[i] = new CPersBird();
			SCGlobals.ar_CPersBird2[i] = new CPersBird2();
			SCGlobals.ar_CPersSpider[i] = new CPersSpider();
			SCGlobals.ar_CPersSpider2[i] = new CPersSpider();
			SCGlobals.ar_CPersHedgehog[i] = new CPersHedgehog();
			SCGlobals.ar_CPersFrog[i] = new CPersFrog();
			SCGlobals.ar_CPersBat[i] = new CPersBat();
			SCGlobals.ar_CPersWasp[i] = new CPersWasp();
			SCGlobals.ar_CPersBug[i] = new CPersBug();
			SCGlobals.ar_CPersFlyCatcher[i] = new CPersFlyCatcher();
		}
		for (int j = 0; j < 3; j++)
		{
			SCGlobals.ar_CPersPig[j] = new CPersPig();
			SCGlobals.ar_CPersGoat[j] = new CPersGoat();
			SCGlobals.ar_CPersProf[j] = new CPersProf();
		}
	}

	public void FreePers()
	{
		numobjs = 0;
		SCGlobals.nummonsters = 0;
		Array.Clear(SCGlobals.ar_nummonsterstype, 0, SCGlobals.ar_nummonsterstype.Length);
	}

	public void LoadAll()
	{
		for (int i = 0; i < 16; i++)
		{
			CMonster cMonster = CreatePersObj(i);
			string filename = ((i != 5) ? (ar_monsternames[i] + ".chr") : "spider.chr");
			cMonster.Load(filename);
			cMonster.monsterID = i;
			ar_persprototype[i] = cMonster;
		}
	}

	public CMonster AddPersObj(int id)
	{
		CMonster cMonster = null;
		int num = SCGlobals.ar_nummonsterstype[id];
		int num2 = SCGlobals.ar_nummonsterstype[id];
		cMonster = id switch
		{
			0 => SCGlobals.ar_CPersSnail[num], 
			1 => SCGlobals.ar_CPersMole[num], 
			2 => SCGlobals.ar_CPersDog[num], 
			3 => SCGlobals.ar_CPersBird[num], 
			13 => SCGlobals.ar_CPersBird2[num], 
			4 => SCGlobals.ar_CPersSpider[num], 
			5 => SCGlobals.ar_CPersSpider2[num], 
			6 => SCGlobals.ar_CPersHedgehog[num], 
			7 => SCGlobals.ar_CPersPig[num], 
			8 => SCGlobals.ar_CPersFrog[num], 
			9 => SCGlobals.ar_CPersGoat[num], 
			10 => SCGlobals.ar_CPersBat[num], 
			11 => SCGlobals.ar_CPersWasp[num], 
			12 => SCGlobals.ar_CPersBug[num], 
			14 => SCGlobals.ar_CPersFlyCatcher[num], 
			15 => SCGlobals.ar_CPersProf[num], 
			_ => SCGlobals.ar_CPersSnail[num], 
		};
		if (id == 9 || id == 15 || id == 7)
		{
			num2 = (num + 1) % 3;
			SCGlobals.ar_nummonsterstype[id] = num2;
		}
		else
		{
			num = (num + 1) % 64;
			SCGlobals.ar_nummonsterstype[id] = num;
		}
		return cMonster;
	}

	public CMonster CreatePersObj(int id)
	{
		CMonster cMonster = null;
		switch (id)
		{
		case 0:
			return new CPersSnail();
		case 1:
			return new CPersMole();
		case 2:
			return new CPersDog();
		case 3:
			return new CPersBird();
		case 13:
			return new CPersBird2();
		case 4:
		case 5:
			return new CPersSpider();
		case 6:
			return new CPersHedgehog();
		case 7:
			return new CPersPig();
		case 8:
			return new CPersProf();
		case 9:
			return new CPersGoat();
		case 10:
			return new CPersBat();
		case 11:
			return new CPersWasp();
		case 12:
			return new CPersBug();
		case 14:
			return new CPersFlyCatcher();
		case 15:
			return new CPersProf();
		default:
			return new CPersSnail();
		}
	}

	public CMonster AddMonster(string name)
	{
		return AddMonster(GetIdByName(name));
	}

	public CMonster AddMonster(int id)
	{
		if (numobjs == 1023)
		{
			return new CMonster();
		}
		CMonster cMonster = AddPersObj(id);
		string filename = ((id != 5) ? (ar_monsternames[id] + ".chr") : "spider.chr");
		cMonster.monsterID = id;
		Store.SetCurrentDirectory("characters", theAdd: false);
		cMonster.Load(filename);
		Store.SetCurrentDirectory("", theAdd: false);
		cMonster.coord_z = -20f - 6f * (float)(numobjs % 4);
		ar_pers[numobjs] = cMonster;
		numobjs++;
		SCGlobals.nummonsters = numobjs;
		return cMonster;
	}

	public CMonster GetPers(int id)
	{
		return ar_persprototype[id];
	}

	public CMonster GetPers(string name)
	{
		return ar_persprototype[GetIdByName(name)];
	}

	public int GetIdByName(string name)
	{
		for (int i = 0; i < 16; i++)
		{
			if (name == ar_monsternames[i])
			{
				return i;
			}
		}
		return 0;
	}

	public string GetNameById(int id)
	{
		return ar_monsternames[id];
	}

	public int GetHostSize()
	{
		return numobjs;
	}

	public void PhysicPers()
	{
		for (int i = 0; i < numobjs; i++)
		{
			if (!ar_pers[i].b_alive)
			{
				for (int j = i; j < numobjs - 1; j++)
				{
					ar_pers[j] = ar_pers[j + 1];
				}
				numobjs--;
				SCGlobals.nummonsters = numobjs;
			}
		}
	}

	public void DrawPers()
	{
		for (int i = 0; i < numobjs; i++)
		{
			ar_pers[i].Calculate();
			ar_pers[i].Draw();
		}
	}
}
