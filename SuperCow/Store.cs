using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class Store
{
	public static int numstoreobjs = 0;

	public static store_obj[] p_store = new store_obj[1024];

	private static string curDir = "";

	private static string curPath = "content\\data";

	private static string strPath = "";

	public static string CurrentDirectory
	{
		get
		{
			string text = curDir + "\\";
			if (strPath.Length != 0)
			{
				text = text + strPath + "\\";
			}
			return text;
		}
	}

	public static string ContentDirectory
	{
		get
		{
			string text = curPath + "\\";
			if (strPath.Length != 0)
			{
				text = text + strPath + "\\";
			}
			return text;
		}
	}

	public static void InitStore()
	{
		string text = "\\";
		curDir = text + "\\" + curPath;
	}

	public static void SetCurrentDirectory(string strDir, bool theAdd)
	{
		if (theAdd && strPath.Length > 0)
		{
			strPath = strPath + "\\" + strDir;
		}
		else
		{
			strPath = strDir;
		}
	}

	public static Texture2D LoadTexture2D(string name)
	{
		try
		{
			return SuperCow.ContentManager.Load<Texture2D>(ContentDirectory + StipExtension(name));
		}
		catch
		{
			SuperCowHelper.DebugOut("Content not found: " + StipExtension(name));
			return null;
		}
	}

	public static byte[] LoadBinary(string name)
	{
		Array array;
		try
		{
			array = SuperCow.ContentManager.Load<Array>(ContentDirectory + StipExtension(name));
		}
		catch
		{
			SuperCowHelper.DebugOut("Content not found: " + StipExtension(name));
			return null;
		}
		return (byte[])array;
	}

	public static byte[] storeLoadFileArray(string filename, ref int len, bool b_errorexit)
	{
		store_obj store_obj2 = storeObjByName(filename);
		if (store_obj2.name != null && store_obj2.pointer != null)
		{
			len = store_obj2.pointer.Length;
			return store_obj2.pointer;
		}
		len = store_obj2.size;
		byte[] array = LoadBinary(filename);
		len = array.Length;
		storeAddStockObj(filename, len, array, file: true);
		return array;
	}

	public static byte[] storeLoadFileLeveArray(string filename, ref int len)
	{
		byte[] array = LoadBinary(filename);
		len = array.Length;
		return array;
	}

	public static int storeGetFileSize(string filename)
	{
		return -1;
	}

	public static store_obj storeObjByName(string name)
	{
		string text = name.ToUpper();
		for (int i = 0; i < numstoreobjs; i++)
		{
			if (p_store[i].name == text)
			{
				return p_store[i];
			}
		}
		return default(store_obj);
	}

	public static object storeAddStockObj(string fullname, int size, byte[] pointer, bool file)
	{
		store_obj store_obj2 = new store_obj
		{
			name = fullname.ToUpper(),
			pointer = pointer,
			size = size,
			file = file
		};
		p_store[numstoreobjs] = store_obj2;
		numstoreobjs++;
		SCGlobals.mProgress += size;
		return store_obj2;
	}

	public static string ConvertToString(byte[] data, int dataSize)
	{
		int count = 0;
		for (int i = 0; i < dataSize; i++)
		{
			if (data[i] == 0)
			{
				count = i;
				break;
			}
		}
		return Encoding.ASCII.GetString(data, 0, count);
	}

	public static string StipExtension(string filename)
	{
		string text = Path.GetExtension(filename);
		if (text.Length > 0)
		{
			text = text.Substring(1, text.Length - 1);
		}
		return Path.GetFileNameWithoutExtension(filename) + "_" + text;
	}
}
