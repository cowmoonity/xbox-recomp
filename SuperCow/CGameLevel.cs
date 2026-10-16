using System.Globalization;
using Microsoft.Xna.Framework.GamerServices;

namespace SuperCow;

public class CGameLevel
{
	public string str_levelname;

	public CLevelObj[,] ar_drawplanes = new CLevelObj[6, 4096];

	public int[,] ar_drawplaneindexes = new int[6, 4096];

	public int[] numplaneobjs = new int[6];

	public CLevelObj[] ar_levelobjs = new CLevelObj[4096];

	public string[] ar_levelobjnames = new string[4096];

	public int numobjnames;

	public int clipboardsize;

	public vec2_t centerselection;

	public int numundocount;

	public int undocountindex;

	public int curundoindex;

	public int firstundoindex;

	public int lastundoindex;

	public int curredoindex;

	public int redocountindex;

	public int numlevelobjs;

	public bool b_levelchanged;

	public int numlevelbacks;

	public int curlevelback;

	public int curlevelmusic;

	public static string le_p_file = "";

	public static int le_len = 0;

	public static bool bSprite = false;

	public CGameLevel()
	{
		numlevelobjs = 0;
		firstundoindex = 0;
		curundoindex = 0;
		lastundoindex = 0;
		numundocount = 0;
		undocountindex = 0;
		clipboardsize = 0;
		redocountindex = -1;
		curredoindex = -1;
	}

	public CLevelObj GetFirstSelected()
	{
		for (int i = 0; i < numlevelobjs; i++)
		{
			if (ar_levelobjs[i].b_selected)
			{
				return ar_levelobjs[i];
			}
		}
		return null;
	}

	public void AddName(string name)
	{
		for (int i = 0; i < numobjnames; i++)
		{
			if (ar_levelobjnames[i] == name)
			{
				return;
			}
		}
		ar_levelobjnames[numobjnames] = name;
		numobjnames++;
	}

	public CLevelObj AddLevelObj(string objname, commonsaveobj_t saveobj)
	{
		if (numlevelobjs == 4096)
		{
			return null;
		}
		int i;
		for (i = 0; i < SCGlobals.numgameobjs && !(SCGlobals.ar_gameobjs[i].name == objname); i++)
		{
		}
		if (i == SCGlobals.numgameobjs)
		{
			return null;
		}
		CLevelObj cLevelObj = CLevelObj.CreateLevelObjFromGameObj(i);
		ar_levelobjs[numlevelobjs] = cLevelObj;
		cLevelObj.b_selected = false;
		cLevelObj.SetSaveData(saveobj);
		frect_t rect = cLevelObj.rect;
		cLevelObj.sizex = rect.sizex();
		cLevelObj.sizey = rect.sizey();
		cLevelObj.position = rect.pos();
		cLevelObj.startpos = cLevelObj.position;
		numlevelobjs++;
		AddName(objname);
		return cLevelObj;
	}

	public void SortDrawPlanes(bool b_game)
	{
		for (int i = 0; i < numplaneobjs.Length; i++)
		{
			numplaneobjs[i] = 0;
		}
		for (int j = 0; j < 6; j++)
		{
			for (int k = 0; k < numlevelobjs; k++)
			{
				if ((ar_levelobjs[k].objID != 6 || !b_game) && ((ar_levelobjs[k].objID != 18 && ar_levelobjs[k].objID != 20 && ar_levelobjs[k].objID != 19) || !b_game) && ar_levelobjs[k].draw_plane == j)
				{
					if (numplaneobjs[j] == 4096)
					{
						break;
					}
					ar_drawplanes[j, numplaneobjs[j]] = ar_levelobjs[k];
					numplaneobjs[j]++;
				}
			}
		}
		for (int l = 0; l < 6; l++)
		{
			int num = 0;
			int num2 = 0;
			for (int m = 0; m < numobjnames; m++)
			{
				for (int n = 0; n < numplaneobjs[l]; n++)
				{
					if (ar_drawplanes[l, n].name == ar_levelobjnames[num2])
					{
						ar_drawplaneindexes[l, num] = n;
						num++;
					}
				}
				num2++;
			}
		}
	}

	public void DeleteObjByPointer(CLevelObj pl)
	{
		int i;
		for (i = 0; i < numlevelobjs && ar_levelobjs[i] != pl; i++)
		{
		}
		if (i < numlevelobjs)
		{
			for (int j = i; j < numlevelobjs - 1; j++)
			{
				ar_levelobjs[j] = ar_levelobjs[j + 1];
			}
			numlevelobjs--;
		}
	}

	public void ClearLevel()
	{
		numlevelobjs = 0;
		firstundoindex = 0;
		curundoindex = 0;
		lastundoindex = 0;
		numundocount = 0;
		undocountindex = 0;
		redocountindex = -1;
		curredoindex = -1;
		Ground.InitGround();
	}

	public bool Load(int nstage, int nlevel)
	{
		string text = ((!Guide.IsTrialMode) ? $"level{nlevel + 1:00}.lev" : $"level{nlevel + 1:00}trial.lev");
		string text2 = $"{nstage + 1:D}";
		Store.SetCurrentDirectory("levels", theAdd: false);
		Store.SetCurrentDirectory(text2, theAdd: true);
		ClearLevel();
		string filename = text;
		b_levelchanged = false;
		Level level = SuperCow.ContentManager.Load<Level>("content\\data\\levels\\" + text2 + "\\" + Store.StipExtension(filename));
		int objcount = level.objcount;
		SCGlobals.curlevelback = level.curlevelback;
		SCGlobals.curleveltask = level.curleveltask;
		SCGlobals.curlevelmusic = level.curlevelmusic;
		commonsaveobj_t saveobj = default(commonsaveobj_t);
		for (int i = 0; i < objcount; i++)
		{
			saveobj.rect = new frect_t((float)level.objects[i].rect[0], (float)level.objects[i].rect[1], (float)level.objects[i].rect[2], (float)level.objects[i].rect[3]);
			saveobj.draw_plane = level.objects[i].drawlayer;
			saveobj.rotation = (float)level.objects[i].rotation;
			saveobj.endpos = new vec2_t((float)level.objects[i].endpos[0], (float)level.objects[i].endpos[1]);
			saveobj.b_invertobj = level.objects[i].invertobj;
			saveobj.str = level.objects[i].extraparam;
			AddLevelObj(level.objects[i].name, saveobj);
		}
		for (int j = 0; j < 6; j++)
		{
			for (int num = 63; num >= 0; num--)
			{
				for (int k = 0; k < 256; k++)
				{
					Ground.ar_ground[j, k, num] = level.ar_ground[j, k, num];
					Ground.ar_groundtype[j, k, num] = level.ar_groundtype[j, k, num];
				}
			}
		}
		Ground.RecalculateGround();
		SortDrawPlanes(b_game: false);
		level = null;
		return true;
	}

	public static void SetupGameObj()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		byte[] array = Store.storeLoadFileArray("gameobjs.txt", ref le_len, b_errorexit: false);
		if (array.Length > 0)
		{
			le_p_file = SuperCowHelper.StringFromByteArray(array);
			AddGameObjGroup(le_p_file, -1);
			Store.SetCurrentDirectory("", theAdd: false);
		}
	}

	public static string AddGameObjGroup(string c0, int parent)
	{
		string text = null;
		while (true)
		{
			int num2;
			int num = (num2 = 0);
			while (true)
			{
				if (c0[num] <= ' ')
				{
					if (num < c0.Length - 1)
					{
						num++;
						continue;
					}
				}
				else
				{
					if (c0[num] == '[' && c0[num + 1] != '/')
					{
						num++;
						text = c0;
						num2 = num;
						while (text[num2] != '/')
						{
							if (num2 < text.Length - 1)
							{
								num2++;
								continue;
							}
							goto IL_0666;
						}
						break;
					}
					if (c0[num] == '[' && c0[num + 1] == '/')
					{
						num += 2;
						text = c0;
						num2 = num;
						while (true)
						{
							if (text[num2] != ']')
							{
								if (num2 >= text.Length - 1)
								{
									break;
								}
								num2++;
								continue;
							}
							num2++;
							text = text.Substring(num2, text.Length - num2);
							num2 = 0;
							return text;
						}
					}
					else
					{
						while (true)
						{
							if (c0[num] <= ' ')
							{
								if (num2 >= text.Length - 1)
								{
									break;
								}
								num++;
								continue;
							}
							text = c0;
							num2 = num;
							while (true)
							{
								c0 = text;
								num = num2;
								for (; text[num2] != '='; num2++)
								{
									if (num2 >= text.Length - 1)
									{
										goto end_IL_0187;
									}
								}
								string name = c0.Substring(num, num2 - num);
								for (num2++; text[num2] <= ' '; num2++)
								{
									if (num2 >= text.Length - 1)
									{
										goto end_IL_0187;
									}
								}
								gameobject_t gameobject_t2 = SCGlobals.ar_gameobjs[SCGlobals.numgameobjs++];
								gameobject_t2.name = name;
								gameobject_t2.group = SCGlobals.numobjgroups - 1;
								c0 = text;
								num = num2;
								while (text[num2] > ' ' && text[num2] != ',')
								{
									if (num2 < text.Length - 1)
									{
										num2++;
										continue;
									}
									goto IL_0239;
								}
								string text2 = c0.Substring(num, num2 - num);
								if (text2.Contains("coin"))
								{
									int num3 = 5;
									_ = num3 / 2;
								}
								gameobject_t2.p_cgo = null;
								if (text2.Contains(".tga") || text2.Contains(".jpg"))
								{
									Store.SetCurrentDirectory("", theAdd: false);
									gameobject_t2.p_tex = SCGlobals.texturehost.Load(text2, b_errormsg: false, b_forsealpha: false, 1);
									if (text2 == "white.jpg")
									{
										int num4 = 0;
										num4 += 10;
									}
									bSprite = false;
									if (gameobject_t2.p_tex.name != null && (gameobject_t2.p_tex.name.Contains("_a_coin") || gameobject_t2.p_tex.name.Contains("_a_diamond") || gameobject_t2.p_tex.name.Contains("_a_aid") || gameobject_t2.p_tex.name.Contains("_a_box") || gameobject_t2.p_tex.name.Contains("_a_key") || gameobject_t2.p_tex.name.Contains("_a_life") || gameobject_t2.p_tex.name.Contains("_a_locket") || gameobject_t2.p_tex.name.Contains("_a_bracelet") || gameobject_t2.p_tex.name.Contains("_a_brooch") || gameobject_t2.p_tex.name.Contains("_a_goblet") || gameobject_t2.p_tex.name.Contains("_a_gold") || gameobject_t2.p_tex.name.Contains("_a_nugget") || gameobject_t2.p_tex.name.Contains("_a_pack") || gameobject_t2.p_tex.name.Contains("_a_perl") || gameobject_t2.p_tex.name.Contains("_a_pot") || gameobject_t2.p_tex.name.Contains("_a_rings") || gameobject_t2.p_tex.name.Contains("_a_sack") || gameobject_t2.p_tex.name.Contains("_a_crown") || gameobject_t2.p_tex.name.Contains("_a_scepter")))
									{
										bSprite = true;
									}
									if (gameobject_t2.p_tex.name == null || bSprite)
									{
										Store.SetCurrentDirectory("levelobjs", theAdd: false);
										gameobject_t2.p_tex = SCGlobals.texturehost.Load(text2, b_errormsg: false, b_forsealpha: false, 1);
										if (gameobject_t2.p_tex.name == null || bSprite)
										{
											gameobject_t2.p_sprite = new CSprite();
											if (!gameobject_t2.p_sprite.Load(text2, 32, 8))
											{
												gameobject_t2.p_sprite = null;
											}
											else
											{
												gameobject_t2.p_tex = gameobject_t2.p_sprite.p_texture;
											}
										}
									}
								}
								else if (text2.Contains(".cgo"))
								{
									Store.SetCurrentDirectory("", theAdd: false);
									gameobject_t2.p_cgo = SCGlobals.geomobjhost.Load(text2);
									gameobject_t2.p_tex = gameobject_t2.p_cgo.p_textures[0];
								}
								SCGlobals.ar_gameobjs[SCGlobals.numgameobjs - 1] = gameobject_t2;
								for (; text[num2] <= ' '; num2++)
								{
									if (num2 >= text.Length - 1)
									{
										goto end_IL_0187;
									}
								}
								if (text[num2] != '[' || text[++num2] != '/')
								{
									continue;
								}
								while (true)
								{
									if (text[num2] != ']')
									{
										if (num2 >= text.Length - 1)
										{
											break;
										}
										num2++;
										continue;
									}
									num2++;
									text = text.Substring(num2, text.Length - num2);
									num2 = 0;
									return text;
								}
								break;
								IL_0239:
								SCGlobals.ar_gameobjs[SCGlobals.numgameobjs - 1] = gameobject_t2;
								break;
								continue;
								end_IL_0187:
								break;
							}
							break;
						}
					}
				}
				goto IL_0666;
				IL_0666:
				c0 = c0.Substring(num, c0.Length - num);
				num = 0;
				return c0;
			}
			string name2 = c0.Substring(num, num2 - num);
			SCGlobals.group_names[SCGlobals.numobjgroups].name = name2;
			SCGlobals.group_names[SCGlobals.numobjgroups].parent = parent;
			c0 = text;
			num = num2;
			num += 2;
			SCGlobals.numobjgroups++;
			c0 = c0.Substring(num, c0.Length - num);
			num2 = 0;
			text = c0;
			c0 = AddGameObjGroup(c0, SCGlobals.numobjgroups - 1);
		}
	}

	public static void InitLevelBacks()
	{
		int len = 0;
		for (int i = 0; i < 32; i++)
		{
			SCGlobals.ar_levelbacks[i] = default(levelback_t);
			SCGlobals.ar_levelbacks[i].ar_planes = new levelbackplane_t[16];
			for (int j = 0; j < 16; j++)
			{
				SCGlobals.ar_levelbacks[i].ar_planes[j] = default(levelbackplane_t);
			}
		}
		byte[] array = Store.storeLoadFileArray("backgrounds.txt", ref len, b_errorexit: false);
		if (array.Length == 0)
		{
			return;
		}
		string text = SuperCowHelper.StringFromByteArray(array);
		SCGlobals.numlevelbacks = 0;
		string text2 = text;
		int num = 0;
		Store.SetCurrentDirectory("levelbacks", theAdd: false);
		int num2 = 0;
		int num3 = 0;
		while (true)
		{
			levelback_t levelback_t2 = SCGlobals.ar_levelbacks[SCGlobals.numlevelbacks++];
			string text3;
			while (true)
			{
				if (text2[num2] != '[')
				{
					if (num2 < text2.Length - 1)
					{
						num2++;
						continue;
					}
				}
				else
				{
					num2++;
					text3 = text2;
					num3 = num2;
					while (text3[num3] != '/')
					{
						if (num3 < text3.Length - 1)
						{
							num3++;
							continue;
						}
						goto IL_052e;
					}
					string levelbackname = text2.Substring(num2, num3 - num2);
					levelback_t2.levelbackname = levelbackname;
					num3 += 2;
					while (text3[num3] <= ' ')
					{
						if (num3 < text3.Length - 1)
						{
							num3++;
							continue;
						}
						goto IL_052e;
					}
					num = -1;
					levelbackplane_t levelbackplane_t2 = levelback_t2.ar_planes[0];
					while (true)
					{
						text2 = text3;
						num2 = num3;
						for (; text3[num3] != '='; num3++)
						{
							if (num3 >= text3.Length - 1)
							{
								goto end_IL_0176;
							}
						}
						string text4 = text2.Substring(num2, num3 - num2);
						for (num3++; text3[num3] <= ' '; num3++)
						{
							if (num3 >= text3.Length - 1)
							{
								goto end_IL_0176;
							}
						}
						text2 = text3;
						num2 = num3;
						for (; text3[num3] >= ' '; num3++)
						{
							if (num3 >= text3.Length - 1)
							{
								goto end_IL_0176;
							}
						}
						string text5 = text2.Substring(num2, num3 - num2);
						switch (text4)
						{
						case "texture":
							num++;
							levelbackplane_t2 = levelback_t2.ar_planes[num];
							levelbackplane_t2.p_tex = SCGlobals.texturehost.Load(text5, b_errormsg: false, b_forsealpha: false, 1);
							levelbackplane_t2.color = default(color_t);
							levelbackplane_t2.color.assign(1f, 1f, 1f, 1f);
							levelback_t2.ar_planes[num] = levelbackplane_t2;
							break;
						case "speed":
							levelbackplane_t2.speed = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "wind":
							levelbackplane_t2.wind = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "height":
							levelbackplane_t2.height = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "width":
							levelbackplane_t2.width = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "vertdelta":
							levelbackplane_t2.vertdelta = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "vertical":
							levelbackplane_t2.vertical = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "single":
							levelbackplane_t2.b_single = text5 == "true";
							break;
						case "noblend":
							levelbackplane_t2.b_noblend = text5 == "true";
							break;
						case "distance":
							levelbackplane_t2.distance = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "color":
						{
							float[] array2 = new float[4];
							SuperCowHelper.get_float_array(text5, ref array2);
							levelbackplane_t2.color = new color_t(array2[0], array2[1], array2[2], array2[3]);
							levelback_t2.ar_planes[num] = levelbackplane_t2;
							break;
						}
						case "zastup_up":
							levelbackplane_t2.zastup_up = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						case "zastup_down":
							levelbackplane_t2.zastup_down = float.Parse(text5, CultureInfo.InvariantCulture);
							break;
						}
						levelback_t2.ar_planes[num] = levelbackplane_t2;
						for (; text3[num3] <= ' '; num3++)
						{
							if (num3 >= text3.Length - 1)
							{
								goto end_IL_0176;
							}
						}
						if (text3[num3] != '[' || text3[++num3] != '/')
						{
							continue;
						}
						goto IL_04dc;
						continue;
						end_IL_0176:
						break;
					}
				}
				goto IL_052e;
				IL_04dc:
				levelback_t2.numplanes = num + 1;
				SCGlobals.ar_levelbacks[SCGlobals.numlevelbacks - 1] = levelback_t2;
				while (text3[num3] > ' ')
				{
					if (num3 < text3.Length - 1)
					{
						num3++;
						continue;
					}
					goto IL_052e;
				}
				break;
				IL_052e:
				SCGlobals.ar_levelbacks[SCGlobals.numlevelbacks - 1] = levelback_t2;
				Store.SetCurrentDirectory("", theAdd: false);
				return;
			}
			text2 = text3;
			num2 = num3;
		}
	}
}
