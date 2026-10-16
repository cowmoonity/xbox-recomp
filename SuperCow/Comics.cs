using System.Globalization;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Comics
{
	public struct pers_t
	{
		public string name;

		public string tganame;
	}

	public struct comicsfile_t
	{
		public string str;
	}

	public struct comics_t
	{
		public string str;

		public int type;

		public int stage;

		public int num;
	}

	public enum CM_TYPE
	{
		CMT_NAME,
		CMT_PERSL,
		CMT_PERSR,
		CMT_H,
		CMT_V,
		CMT_TASK,
		CMT_LABEL,
		CMT_GOTO,
		CMT_EFFECT,
		CMT_SOUND,
		CMT_LIGHT,
		CMT_HERO
	}

	private static Cue mVoice;

	private static Cue mSound;

	private static int comLevelStage;

	private static int comCurLevel;

	public static int COMICS_PERS_DISAPPEAR = 300;

	public static int COMICS_PERS_APPEAR_ONE = 500;

	public static int COMICS_PERS_APPEAR_TWO = 2500;

	public static int COMICS_PERS_MAX = 16;

	public static int COMICS_TEXT_DIS = 250;

	public static int COMICS_MAX_STRING = 1024;

	public static matrix_t gamecamera;

	public static float f_darknesstimer = 0f;

	public static float f_heroapptimer = 0f;

	public static bool b_comicson;

	public static bool b_comicsstagename;

	public static bool b_comicston1;

	public static bool b_comicston2;

	public static bool b_comicst1;

	public static bool b_comicst2;

	public static bool b_comicspersleft;

	public static bool b_comicslight;

	public static bool b_comicshero;

	public static bool b_voiceactive = false;

	public static float f_comics;

	public static float f_comicst1;

	public static float f_comicst2;

	public static float f_comicseffect;

	public static int comicsfileindex;

	public static int comicsindex;

	public static int comicslevel;

	public static int comicsstage;

	public static int comicscur;

	public static int comicsnext;

	public static int comicsnum;

	public static int comicsselect;

	public static int comicspersindex;

	public static int comicspersnum;

	public static int ivoicerateV = 44100;

	public static int ivoicerateH = 44100;

	public static int v_counter;

	public static int h_counter;

	public static string curvoicename = "";

	public static string str_comics_stagename;

	public static string str_comics_pers;

	public static string str_comics_hero;

	public static frect_t[] comics_rect = new frect_t[4];

	public static comicsfile_t[] ar_comicsfile = new comicsfile_t[COMICS_MAX_STRING];

	public static comics_t[] ar_comics = new comics_t[COMICS_MAX_STRING];

	public static pers_t[] ar_comicspers = new pers_t[COMICS_PERS_MAX];

	public static texture_t[] p_pers = new texture_t[COMICS_PERS_MAX];

	public static texture_t p_curpers;

	public static texture_t p_nextpers;

	public static void ComicsEnd()
	{
		f_comics = -1f;
		Sound.Stop(mVoice);
		Sound.Stop(mSound);
	}

	public static void ComicsFileReset()
	{
		Store.SetCurrentDirectory("txt\\us", theAdd: false);
		int len = 0;
		Store.storeLoadFileArray("comics.txt", ref len, b_errorexit: false);
		ComicsFree();
		ComicsLoad();
	}

	public static void ComicsLoadPersFile()
	{
		int len = 0;
		Store.SetCurrentDirectory("", theAdd: false);
		byte[] array = Store.storeLoadFileArray("pers.txt", ref len, b_errorexit: false);
		string text = Encoding.Unicode.GetString(array, 0, array.Length);
		int i = 1;
		len = text.Length;
		while (true)
		{
			if (text[i] <= ' ')
			{
				if (i >= len - 1)
				{
					break;
				}
				i++;
				continue;
			}
			int num = i;
			for (; text[i] != '='; i++)
			{
				if (i >= len - 1)
				{
					goto end_IL_004c;
				}
			}
			string name = text.Substring(num, i - num);
			ar_comicspers[comicspersindex].name = name;
			i++;
			num = i;
			for (; text[i] != '\r' || text[i + 1] != '\n'; i++)
			{
				if (i >= len - 1)
				{
					goto end_IL_004c;
				}
			}
			name = text.Substring(num, i - num);
			ar_comicspers[comicspersindex].tganame = name;
			comicspersindex++;
			if (comicspersindex >= COMICS_PERS_MAX)
			{
				break;
			}
			continue;
			end_IL_004c:
			break;
		}
		Store.SetCurrentDirectory("pers", theAdd: false);
		for (int j = 0; j < comicspersindex; j++)
		{
			ref texture_t reference = ref p_pers[j];
			reference = SCGlobals.texturehost.Load(ar_comicspers[j].tganame);
		}
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void ComicsLoadComicsFile()
	{
		int len = 0;
		int num = 0;
		int num2 = 0;
		comicsfileindex = 0;
		Store.SetCurrentDirectory("txt\\us", theAdd: false);
		byte[] array = Store.storeLoadFileArray("comics.txt", ref len, b_errorexit: false);
		string text = Encoding.Unicode.GetString(array, 0, array.Length);
		int num3 = 1;
		int i = num3;
		while (true)
		{
			num3 = i;
			if (i == text.Length)
			{
				break;
			}
			for (; text[i] != '\r' || text[i + 1] != '\n'; i++)
			{
				if (i >= text.Length - 3)
				{
					return;
				}
			}
			num = i - num3;
			if (num > 0)
			{
				string text2 = text.Substring(num3, num);
				ar_comicsfile[comicsfileindex].str = text2;
				for (num2 = 0; num2 < num; num2++)
				{
					if (text2[num2] > ' ')
					{
						comicsfileindex++;
						break;
					}
				}
			}
			i += 2;
		}
	}

	public static void ComicsMakeScript()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int stage = 0;
		bool flag = false;
		comicsindex = 0;
		int num6 = 0;
		int num7 = 0;
		for (num = 0; num < comicsfileindex; num++)
		{
			int num8 = 0;
			int i = num8;
			num2 = ar_comicsfile[num].str.Length - num8;
			string str = "";
			string str2 = "";
			num5 = 0;
			num4 = 0;
			flag = false;
			string text = "";
			if (ar_comicsfile[num].str[num8] == '[')
			{
				for (; ar_comicsfile[num].str[i] != '/'; i++)
				{
				}
				if (i - num8 > 6)
				{
					num8 += 6;
					text = ar_comicsfile[num].str.Substring(num8, i - num8);
					stage = int.Parse(text, CultureInfo.InvariantCulture);
				}
				else
				{
					stage = 666;
				}
				continue;
			}
			if (ar_comicsfile[num].str[num8 + num2 - 1] == ':')
			{
				str = ar_comicsfile[num].str.Substring(num8, num2);
				num5 = 6;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 4) == "name")
			{
				i += 5;
				num2 += num8 - i;
				num2++;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 0;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 4) == "task")
			{
				i += 5;
				num2 += num8 - i;
				num2++;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 5;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 5) == "persL")
			{
				for (; ar_comicsfile[num].str[i] != '='; i++)
				{
				}
				num3 = i - num8;
				if (num3 > 5)
				{
					text = ar_comicsfile[num].str.Substring(num8 + 5, num3 - 5);
					num4 = int.Parse(text, CultureInfo.InvariantCulture);
				}
				i++;
				num2 += num8 - i;
				num2++;
				str = (text = ar_comicsfile[num].str.Substring(i, num2));
				num5 = 1;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 5) == "persR")
			{
				for (; ar_comicsfile[num].str[i] != '='; i++)
				{
				}
				num3 = i - num8;
				if (num3 > 5)
				{
					text = ar_comicsfile[num].str.Substring(num8 + 5, num3 - 5);
					num4 = int.Parse(text, CultureInfo.InvariantCulture);
				}
				i++;
				num2 += num8 - i;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 2;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 4) == "pers")
			{
				for (; ar_comicsfile[num].str[i] != '='; i++)
				{
				}
				num3 = i - num8;
				if (num3 > 4)
				{
					text = ar_comicsfile[num].str.Substring(num8 + 4, num3 - 4);
					num4 = int.Parse(text, CultureInfo.InvariantCulture);
				}
				i++;
				num2 += num8 - i;
				str = (text = ar_comicsfile[num].str.Substring(i, num2));
				num5 = 1;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 4) == "goto")
			{
				i += 5;
				num2 += num8 - i;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 7;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 6) == "effect")
			{
				i += 7;
				text += ar_comicsfile[num].str[i];
				num4 = int.Parse(text, CultureInfo.InvariantCulture);
				str = "";
				num5 = 8;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 5) == "hero=")
			{
				num4 = ((ar_comicsfile[num].str[i + 5] == '1') ? 1 : 0);
				num5 = 11;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 1) == "v")
			{
				for (; ar_comicsfile[num].str[i] != '='; i++)
				{
				}
				i++;
				num2 += num8 - i;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 4;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 1) == "h")
			{
				if (ar_comicsfile[num].str[i + 1] == '1')
				{
					num4 = 1;
				}
				else if (ar_comicsfile[num].str[i + 1] == '2')
				{
					num4 = 2;
				}
				else if (ar_comicsfile[num].str[i + 1] == '3')
				{
					num4 = 3;
				}
				else if (ar_comicsfile[num].str[i + 1] == '4')
				{
					num4 = 4;
				}
				if (ar_comicsfile[num].str[i + 1] == '(')
				{
					num6 = i + 2;
					for (num7 = num8; ar_comicsfile[num].str[num7] != ')'; num7++)
					{
					}
					num3 = num7 - num6;
					str2 = ar_comicsfile[num].str.Substring(num6, num3);
					flag = true;
				}
				if (ar_comicsfile[num].str[i + 2] == '(')
				{
					num6 = i + 3;
					for (num7 = num8; ar_comicsfile[num].str[num7] != ')'; num7++)
					{
					}
					num3 = num7 - num6;
					str2 = ar_comicsfile[num].str.Substring(num6, num3);
					flag = true;
				}
				for (; ar_comicsfile[num].str[i] != '='; i++)
				{
				}
				i++;
				num2 += num8 - i;
				str = ar_comicsfile[num].str.Substring(i, num2);
				num5 = 3;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 6) == "sound=")
			{
				i += 6;
				num2 += num8 - i;
				str = ar_comicsfile[num].str.Substring(i, num2);
				str = str ?? "";
				num5 = 9;
			}
			else if (ar_comicsfile[num].str.Substring(num8, 1) == "light=")
			{
				num4 = ((ar_comicsfile[num].str[i + 6] == '1') ? 1 : 0);
				num5 = 10;
			}
			ar_comics[comicsindex].str = str;
			ar_comics[comicsindex].type = num5;
			ar_comics[comicsindex].num = num4;
			ar_comics[comicsindex].stage = stage;
			comicsindex++;
			if (flag)
			{
				ar_comics[comicsindex].str = str2;
				ar_comics[comicsindex].type = 7;
				ar_comics[comicsindex].stage = stage;
				comicsindex++;
			}
		}
	}

	public static void ComicsLoad()
	{
		ComicsLoadPersFile();
		ComicsLoadComicsFile();
		ComicsMakeScript();
		for (int i = 0; i < COMICS_MAX_STRING; i++)
		{
			ar_comicsfile[i].str = "";
		}
		ivoicerateH = 22050;
		ivoicerateV = 20050;
	}

	public static void ComicsFree()
	{
		for (int i = 0; i < comicsindex; i++)
		{
			ar_comics[i].str = "";
		}
	}

	public static void ComicsFreeAll()
	{
		ComicsFree();
		for (int i = 0; i < COMICS_MAX_STRING; i++)
		{
			ar_comicsfile[i].str = "";
		}
	}

	public static void ComicsNext()
	{
		int stage = ar_comics[comicscur].stage;
		comicscur++;
		if (stage != ar_comics[comicscur].stage || comicscur >= comicsindex)
		{
			comicscur = -1;
			return;
		}
		if (ar_comics[comicscur].type == 7)
		{
			for (int i = 0; i < comicsindex; i++)
			{
				if (ar_comics[i].stage == comicsstage && ar_comics[i].type == 6 && ar_comics[i].str == ar_comics[comicscur].str)
				{
					comicscur = i + 1;
					break;
				}
			}
		}
		comicsnext = comicscur;
	}

	public static void ComicsGetFirst()
	{
		for (int i = 0; i < comicsindex; i++)
		{
			if (ar_comics[i].stage != comicsstage)
			{
				continue;
			}
			if (comicslevel == 0)
			{
				comicscur = i;
				return;
			}
			int num = 0;
			for (int j = i; j < comicsindex - 1; j++)
			{
				if (ar_comics[j].type == 5)
				{
					num++;
				}
				if (num == comicslevel)
				{
					if (ar_comics[j + 1].stage == comicsstage)
					{
						comicscur = j + 1;
					}
					else
					{
						comicscur = -1;
					}
					return;
				}
			}
		}
		comicscur = -1;
	}

	public static void ComicsTypeTask()
	{
	}

	public static void ComicsTypeName()
	{
		b_comicsstagename = true;
		str_comics_stagename = ar_comics[comicscur].str;
	}

	public static void ComicsTypePers()
	{
		int type = ar_comics[comicscur].type;
		int num = ar_comics[comicscur].num;
		string str = ar_comics[comicscur].str;
		comicspersnum = num;
		if (f_comics == 0f)
		{
			p_curpers = ComicsGetPersTex(str);
			if (type == 1)
			{
				b_comicspersleft = true;
			}
			else
			{
				b_comicspersleft = false;
			}
			switch (num)
			{
			case 4:
				Drawpers.PersSetEffect(1);
				break;
			case 5:
				Drawpers.PersSetEffect(2);
				break;
			default:
				Drawpers.PersSetEffect(0);
				break;
			}
			b_comicson = true;
			return;
		}
		p_nextpers = ComicsGetPersTex(str);
		if (str == "void")
		{
			switch (num)
			{
			case 4:
				Drawpers.PersSetEffect(3);
				break;
			case 5:
				Drawpers.PersSetEffect(4);
				break;
			default:
				Drawpers.PersSetEffect(0);
				break;
			}
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				f_comics = 500f;
				break;
			case 4:
			case 5:
				f_comics = 3000f;
				break;
			}
		}
		else
		{
			comicspersnum = 0;
			f_comics = 500f;
		}
		b_comicson = false;
		b_comicst1 = false;
		b_comicst2 = false;
	}

	public static void ComicsTypeEffect()
	{
		Vertexeffect.StartVertexEffect(ar_comics[comicscur].num);
		f_comicseffect = 0f;
	}

	public static void ComicsTypeH()
	{
		b_comicst2 = true;
		b_comicston1 = false;
		b_comicston2 = true;
		f_comicst1 = COMICS_TEXT_DIS;
		f_comicst2 = 0f;
		Sound.Stop(mVoice);
		string sound = $"{comLevelStage:D}_h{h_counter + 1:D}";
		b_voiceactive = true;
		mVoice = Sound.Play(sound);
		h_counter++;
		str_comics_hero = ar_comics[comicscur].str;
		if (ar_comics[comicscur].num == 1)
		{
			int i;
			for (i = 1; ar_comics[comicscur + i * 2].num == i + 1; i++)
			{
				str_comics_hero = ar_comics[comicscur + i * 2].str;
			}
			comicsnum = i;
		}
		else
		{
			comicsnum = 0;
		}
	}

	public static void ComicsTypeV()
	{
		b_comicst1 = true;
		b_comicston1 = true;
		b_comicston2 = false;
		f_comicst1 = 0f;
		f_comicst2 = COMICS_TEXT_DIS;
		Sound.Stop(mVoice);
		string sound = $"{comLevelStage:D}_v{v_counter + 1:D}";
		b_voiceactive = true;
		mVoice = Sound.Play(sound);
		v_counter++;
		str_comics_pers = ar_comics[comicscur].str;
	}

	public static void ComicsTypeSound()
	{
	}

	public static void ComicsTypeLight()
	{
		if (ar_comics[comicscur].num == 1)
		{
			b_comicslight = true;
		}
		else
		{
			b_comicslight = false;
			f_darknesstimer = 0f;
		}
		ComicsGo();
	}

	public static void ComicsTypeHero()
	{
		if (ar_comics[comicscur].num == 1)
		{
			b_comicshero = true;
		}
		else
		{
			b_comicshero = false;
			f_heroapptimer = 0f;
		}
		ComicsGo();
	}

	public static void ComicsInit()
	{
		v_counter = 0;
		h_counter = 0;
		b_comicson = true;
		b_comicsstagename = false;
		f_comics = 0f;
		f_comicseffect = -1f;
		comicsstage = PlayerProfile.curlevelstage - 1;
		comicsnum = 0;
		b_comicslight = true;
		b_comicshero = true;
		b_comicst1 = false;
		b_comicst2 = false;
		Sound.Stop(mSound);
		mSound = Sound.Play("backfarm");
		comLevelStage = PlayerProfile.curlevelstage;
		comCurLevel = PlayerProfile.curlevel;
		if (comCurLevel == 0)
		{
			p_curpers.mImage = null;
			p_nextpers.mImage = null;
		}
		else
		{
			comicslevel = 0;
			ComicsGetFirst();
			int stage = ar_comics[comicscur].stage;
			while (true)
			{
				if (comicscur >= comicsindex || stage != ar_comics[comicscur].stage)
				{
					comicscur = -1;
					break;
				}
				if (ar_comics[comicscur].type == 1)
				{
					p_curpers = ComicsGetPersTex(ar_comics[comicscur].str);
					b_comicspersleft = true;
				}
				else if (ar_comics[comicscur].type == 2)
				{
					p_curpers = ComicsGetPersTex(ar_comics[comicscur].str);
					b_comicspersleft = false;
				}
				else if (ar_comics[comicscur].type == 5)
				{
					break;
				}
				comicscur++;
			}
			comicsnext = comicscur;
		}
		Drawpers.PersSetEffect(0);
		comicslevel = PlayerProfile.curlevel;
		ComicsGetFirst();
		if (comicscur < 0)
		{
			ComicsEnd();
		}
		else
		{
			ComicsSetState();
		}
	}

	public static void ComicsSetState()
	{
		if (comicscur < 0)
		{
			ComicsScreen comicsScreen = (ComicsScreen)SuperCow.GetScreen(13);
			if (comicsScreen != null)
			{
				comicsScreen.StageEnd();
				ComicsEnd();
				return;
			}
			StageCommics stageCommics = (StageCommics)SuperCow.GetScreen(14);
			if (stageCommics != null)
			{
				stageCommics.StageEnd();
				ComicsEnd();
			}
			return;
		}
		switch (ar_comics[comicscur].type)
		{
		case 6:
			ComicsGo();
			break;
		case 0:
			ComicsTypeName();
			break;
		case 5:
			ComicsTypeTask();
			break;
		case 1:
		case 2:
			ComicsTypePers();
			break;
		case 8:
			ComicsTypeEffect();
			break;
		case 3:
			ComicsTypeH();
			break;
		case 4:
			ComicsTypeV();
			break;
		case 9:
			ComicsTypeSound();
			break;
		case 10:
			ComicsTypeLight();
			break;
		case 11:
			ComicsTypeHero();
			break;
		}
	}

	public static void ComicsGo()
	{
		ComicsNext();
		ComicsSetState();
	}

	public static void ComicsAbort()
	{
		Sound.Stop(mSound);
		Sound.Stop(mVoice);
	}

	public static void ComicsClick()
	{
		if ((comicsnum <= 0 || comicsselect >= 0) && !(f_comics < 600f) && (!(f_comicst1 < (float)COMICS_TEXT_DIS) || !(f_comicst1 > 0f)) && (!(f_comicst2 < (float)COMICS_TEXT_DIS) || !(f_comicst2 > 0f)) && !b_comicsstagename && ar_comics[comicscur].type != 1 && ar_comics[comicscur].type != 2 && !(f_comicseffect >= 0f))
		{
			ComicsGo();
		}
	}

	public static void CalculateComics()
	{
		if (f_comics < 0f)
		{
			return;
		}
		float num = 0f;
		int num2 = ar_comics[comicscur].num;
		if (f_comicseffect >= 0f)
		{
			f_comicseffect += SCGlobals.ftickperframe;
			if (f_comicseffect > 2500f)
			{
				ComicsGo();
				f_comicseffect = -1f;
				return;
			}
		}
		if (b_comicsstagename)
		{
			f_comics += SCGlobals.ftickperframe;
			if (f_comics > 1500f)
			{
				f_comics = 0f;
				b_comicsstagename = false;
				ComicsGo();
			}
			return;
		}
		if (b_comicson)
		{
			f_comics += SCGlobals.ftickperframe;
			if (ar_comics[comicscur].type == 1 || ar_comics[comicscur].type == 2)
			{
				switch (num2)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					num = 500f;
					break;
				case 4:
				case 5:
					num = 3000f;
					break;
				}
				if (f_comics >= num && f_comics - SCGlobals.ftickperframe < num)
				{
					Drawpers.PersSetEffect(0);
					ComicsGo();
				}
			}
		}
		else
		{
			f_comics -= SCGlobals.ftickperframe;
			if (f_comics < 0f)
			{
				if (ar_comics[comicscur].type == 1)
				{
					b_comicspersleft = true;
				}
				else
				{
					b_comicspersleft = false;
				}
				switch (num2)
				{
				case 4:
					Drawpers.PersSetEffect(1);
					break;
				case 5:
					Drawpers.PersSetEffect(2);
					break;
				default:
					Drawpers.PersSetEffect(0);
					break;
				}
				p_curpers = p_nextpers;
				b_comicson = true;
				f_comics = 0f;
				comicspersnum = num2;
				if (p_curpers.mImage == null)
				{
					f_comics = 6000f;
					ComicsGo();
				}
			}
		}
		if (b_comicston1)
		{
			f_comicst1 += SCGlobals.ftickperframe;
		}
		else
		{
			f_comicst1 -= SCGlobals.ftickperframe;
			if (f_comicst1 < 0f)
			{
				b_comicst1 = false;
				f_comicst1 = 0f;
			}
		}
		if (b_comicston2)
		{
			f_comicst2 += SCGlobals.ftickperframe;
		}
		else
		{
			f_comicst2 -= SCGlobals.ftickperframe;
			if (f_comicst2 < 0f)
			{
				comicsnum = 0;
				b_comicst2 = false;
				f_comicst2 = 0f;
			}
		}
		if (comicsnext >= 0 && ar_comics[comicsnext].type == 3)
		{
			int num3 = 0;
			if (num3 < comicsnum)
			{
				comicsselect = num3;
				comicscur = comicsnext + comicsselect * 2;
				return;
			}
			comicsselect = -1;
		}
		if (mVoice != null && !mVoice.IsPlaying && b_voiceactive && comicscur >= 0)
		{
			ComicsClick();
		}
	}

	public static texture_t ComicsGetPersTex(string name)
	{
		if (name == "void")
		{
			return default(texture_t);
		}
		for (int i = 0; i < comicspersindex; i++)
		{
			if (ar_comicspers[i].name == name)
			{
				return p_pers[i];
			}
		}
		return default(texture_t);
	}

	public static texture_t ComicsGetPersTex(int num)
	{
		return p_pers[num];
	}

	public static void ComicsResume()
	{
		Drawpers.PersSetEffect(0);
		b_comicson = true;
		b_comicsstagename = false;
		f_comics = 0f;
		f_comicseffect = -1f;
		comicsnum = 0;
		b_comicslight = true;
		b_comicst1 = false;
		b_comicst2 = false;
		ComicsGo();
	}

	public static void ComicsSkip()
	{
		Sound.Stop(mSound);
		int stage = ar_comics[comicscur].stage;
		while (true)
		{
			comicscur++;
			if (comicscur >= comicsindex || stage != ar_comics[comicscur].stage)
			{
				comicscur = -1;
				break;
			}
			if (ar_comics[comicscur].type == 1)
			{
				p_curpers = ComicsGetPersTex(ar_comics[comicscur].str);
				b_comicspersleft = true;
				Drawpers.PersSetEffect(0);
			}
			else if (ar_comics[comicscur].type == 2)
			{
				p_curpers = ComicsGetPersTex(ar_comics[comicscur].str);
				b_comicspersleft = false;
				Drawpers.PersSetEffect(0);
			}
			else if (ar_comics[comicscur].type == 5)
			{
				break;
			}
		}
		comicsnext = comicscur;
		ComicsSetState();
	}

	public static void DrawComicsHero()
	{
		if (b_comicshero)
		{
			float num = 0.3f;
			CowRender.SetOrtho(16f * num, 12f * num, 1f, 300f);
			CowRender.EnableRS(0u);
			CowRender.EnableRS(1u);
			CowRender.EnableRS(2u);
			CowRender.EnableRS(7u);
			CowRender.EnableRS(5u);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			graphicsDevice.RenderState.CullMode = (CullMode)1;
			SCGlobals.hero.animspeed = 0.8f;
			SCGlobals.hero.matrix.load_identity();
			SCGlobals.hero.matrix.translate((float)(b_comicspersleft ? 1 : (-1)) * 5.2f * num, 0f, -9f * num);
			SCGlobals.hero.matrix.zrot_delta(b_comicspersleft ? (-80f) : 40f);
			SCGlobals.hero.b_offscreen = false;
			SCGlobals.hero.Draw();
			CowRender.EnableRS(4u);
			CowRender.DisableRS(5u);
			CowRender.DisableRS(1u);
			CowRender.DisableRS(0u);
			CowRender.LoadMatrixNull();
			CowRender.SetOrtho(800f, 600f, 1f, 300f);
		}
	}

	public static void DrawComicsPers()
	{
		if (p_curpers.mImage == null)
		{
			return;
		}
		float num = 0f;
		color_t col = default(color_t);
		dcolor_t c = default(dcolor_t);
		matrix_t m = default(matrix_t);
		frect_t vc = default(frect_t);
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		float num2 = -50f;
		float num3 = -201f;
		if (p_curpers.name.Contains("goat") && SuperCow.IsHDTV)
		{
			num3 -= 100f;
		}
		if (!b_comicspersleft)
		{
			num3 = 0f - num3;
		}
		m.load_identity();
		m.translate(num3, num2, 0f);
		if (b_comicson)
		{
			num = f_comics / 500f;
			if (num > 1f)
			{
				num = 1f;
			}
			else if (num < 0f)
			{
				num = 0f;
			}
			switch (comicspersnum)
			{
			case 0:
			case 1:
				c.a = (byte)(255f * num);
				break;
			case 2:
				m.translate(2f * num3 * (1f - num), 0f, 0f);
				break;
			case 3:
				m.translate(0f, num2 * 18f * (1f - num), 0f);
				break;
			case 4:
			case 5:
				num = f_comics / 2900f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				break;
			}
		}
		else
		{
			switch (comicspersnum)
			{
			case 0:
				num = f_comics / 300f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				c.a = (byte)(255f * num);
				break;
			case 1:
				num = f_comics / 500f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				c.a = (byte)(255f * num);
				break;
			case 2:
				num = f_comics / 500f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				m.translate(2f * num3 * (1f - num), 0f, 0f);
				break;
			case 3:
				num = f_comics / 500f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				m.translate(0f, num2 * 18f * (1f - num), 0f);
				break;
			case 4:
			case 5:
				num = f_comics / 2900f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				break;
			}
		}
		CowRender.LoadMatrix(m);
		vc.assign(new vec2_t(0f, 0f), 511f, 511f);
		CowRender.BindTexture(p_curpers);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.SamplerStates[0].AddressV = (TextureAddressMode)4;
		graphicsDevice.SamplerStates[0].AddressU = (TextureAddressMode)4;
		if (comicspersnum < 4)
		{
			if (num == 1f || comicspersnum == 0 || comicspersnum == 1)
			{
				CowRender.DisableRS(7u);
			}
			CowRender.DrawColorQuad2D(vc, SCGlobals.tc_normal, c, 0f);
			CowRender.EnableRS(7u);
		}
		else if (num == 1f)
		{
			CowRender.DisableRS(7u);
			CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			CowRender.EnableRS(7u);
		}
		else
		{
			col.assign(1f, 1f, 1f, 1f);
			num2 = -9.9f;
			num3 = -40f;
			if (!b_comicspersleft)
			{
				num3 = 0f - num3;
			}
			m.load_identity();
			m.translate(num3, num2, 0f);
			Drawpers.PersSetParams(m, col);
			Drawpers.PersDraw();
		}
		graphicsDevice.SamplerStates[0].AddressV = (TextureAddressMode)1;
		graphicsDevice.SamplerStates[0].AddressU = (TextureAddressMode)1;
		CowRender.LoadMatrixNull();
	}

	public static void DrawComicsPersText()
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (b_comicst1)
		{
			float num = f_comicst1 / (float)COMICS_TEXT_DIS;
			float num2 = 300f;
			float num3 = 14f;
			float num4 = 1.2f;
			float num5 = 1.2f;
			frect_t rect = default(frect_t);
			matrix_t m = default(matrix_t);
			if (num < 0f)
			{
				num = 0f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			float num6 = Text.PrepareWindowText(num2, str_comics_pers, num3);
			float y = 70f + 30f * num - 30f;
			float num7 = (0f - num2) * 0.5f * num + (180f + num2 * 0.5f) * num;
			if (!b_comicspersleft)
			{
				num7 = 0f - num7;
			}
			m.load_identity();
			m.translate(num7, y, 0f);
			m.scale(num, num, 1f);
			CowRender.LoadMatrix(m);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			graphicsDevice.RenderState.ReferenceAlpha = 128;
			CowRender.EnableRS(1u);
			CowRender.EnableRS(2u);
			graphicsDevice.Clear((ClearOptions)2, Color.Black, 1f, 0);
			vec2_t vec2_t2 = default(vec2_t);
			if (b_comicspersleft)
			{
				vec2_t2.assign(-100f, 150f - num6 / 2f);
			}
			else
			{
				vec2_t2.assign(100f, 150f - num6 / 2f);
			}
			vec2_t pos = vec2_t2;
			rect.assign(pos, num2 * num4, num6 * num5 + 50f);
			Balloon.DrawBalloonRect(rect);
			if (b_comicspersleft)
			{
				pos += new vec2_t(-170f, -15f);
				num = 200f;
			}
			else
			{
				pos += new vec2_t(170f, -15f);
				num = -20f;
			}
			CowRender.DisableRS(1u);
			CowRender.DisableRS(2u);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
			num7 = vec2_t2.x;
			y = vec2_t2.y + num6 * 0.5f - num3;
			Text.DrawPreparedTextCenterXY(num7, y, num3);
			CowRender.LoadMatrixNull();
		}
	}

	public static void DrawComicsHeroText()
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (b_comicst2)
		{
			float num = f_comicst2 / (float)COMICS_TEXT_DIS;
			float num2 = 300f;
			float num3 = 14f;
			float num4 = 1.2f;
			float num5 = 1.2f;
			frect_t rect = default(frect_t);
			matrix_t m = default(matrix_t);
			if (num < 0f)
			{
				num = 0f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			float num6 = Text.PrepareWindowText(num2, str_comics_hero, num3);
			float y = 70f + 30f * num;
			float num7 = (0f - num2) * 0.5f * num + (180f + num2 * 0.5f) * num;
			if (!b_comicspersleft)
			{
				num7 = 0f - num7;
			}
			m.load_identity();
			m.translate(num7, y, 0f);
			m.scale(num, num, 1f);
			CowRender.LoadMatrix(m);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			graphicsDevice.RenderState.ReferenceAlpha = 128;
			CowRender.EnableRS(1u);
			CowRender.EnableRS(2u);
			graphicsDevice.Clear((ClearOptions)2, Color.Black, 1f, 0);
			vec2_t vec2_t2 = default(vec2_t);
			if (b_comicspersleft)
			{
				vec2_t2.assign(-350f, -320f + num6 / 2f);
			}
			else
			{
				vec2_t2.assign(350f, -320f + num6 / 2f);
			}
			vec2_t pos = vec2_t2;
			rect.assign(pos, num2 * num4, num6 * num5 + 50f);
			Balloon.DrawBalloonRect(rect);
			if (b_comicspersleft)
			{
				pos += new vec2_t(165f, 0f);
				num = 20f;
			}
			else
			{
				pos += new vec2_t(-165f, 0f);
				num = 160f;
			}
			CowRender.DisableRS(1u);
			CowRender.DisableRS(2u);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
			num7 = vec2_t2.x;
			y = vec2_t2.y + num6 * 0.5f - num3;
			Text.DrawPreparedTextCenterXY(num7, y, num3);
			CowRender.LoadMatrixNull();
		}
	}

	public static void DrawComics()
	{
		if (!(f_comics < 0f))
		{
			if (b_comicsstagename)
			{
				Text.OutTextXY(0f, 0f, 30f, str_comics_stagename, b_center: true);
				return;
			}
			DrawComicsPers();
			DrawComicsHero();
			DrawComicsPersText();
			DrawComicsHeroText();
		}
	}
}
