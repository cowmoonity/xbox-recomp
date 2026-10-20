using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace SuperCow.Helpers;

public static class FileHelper
{
	public static ManualResetEvent StorageContainerMRE = new ManualResetEvent(initialState: true);

	private static StorageDevice xnaUserDevice = null;

	public static StorageDevice XnaUserDevice
	{
		get
		{
			if (xnaUserDevice != null && !xnaUserDevice.IsConnected)
			{
				return xnaUserDevice;
			}
			if (xnaUserDevice == null)
			{
				if (Guide.IsVisible)
				{
					return null;
				}
				// TODO: dead code, dunno actually if it needed, but if it is... fuuuuuuuuuck
				// IAsyncResult asyncResult = Guide.BeginShowStorageDeviceSelector(SuperCow.lockedController, (AsyncCallback)null, (object)null);
				// while (!asyncResult.IsCompleted)
				// {
				// 	Thread.Sleep(10);
				// 	SuperCow.graphicsManager.GraphicsDevice.Clear((ClearOptions)1, Color.Black, 1f, 0);
				// 	SuperCow.graphicsManager.GraphicsDevice.Present();
				// }
				// xnaUserDevice = Guide.EndShowStorageDeviceSelector(asyncResult);
				if (!Guide.IsVisible)
				{
					(SuperCow.GamerServicesComponent).Update(new GameTime());
				}
				if (Guide.IsVisible)
				{
					Thread.Sleep(10);
					(SuperCow.GamerServicesComponent).Update(new GameTime());
					SuperCow.graphicsManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1f, 0);
					SuperCow.graphicsManager.GraphicsDevice.Present();
				}
			}
			return xnaUserDevice;
		}
	}

	// TODO: not used in the game, like at all?
	// public static FileStream LoadGameContentFile(string relativeFilename)
	// {
	// 	string path = Path.Combine(StorageContainer.TitleLocation, relativeFilename);
	// 	if (!File.Exists(path))
	// 	{
	// 		return null;
	// 	}
	// 	return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	// }

	public static string[] GetLines(string filename)
	{
		try
		{
			StreamReader streamReader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read), Encoding.UTF8);
			List<string> list = new List<string>();
			do
			{
				list.Add(streamReader.ReadLine());
			}
			while (streamReader.Peek() > -1);
			streamReader.Close();
			return list.ToArray();
		}
		catch (FileNotFoundException)
		{
			return null;
		}
		catch (DirectoryNotFoundException)
		{
			return null;
		}
		catch (IOException)
		{
			return null;
		}
	}

	public static void WriteVector3(BinaryWriter writer, Vector3 vec)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(vec.X);
		writer.Write(vec.Y);
		writer.Write(vec.Z);
	}

	public static void WriteVector4(BinaryWriter writer, Vector4 vec)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(vec.X);
		writer.Write(vec.Y);
		writer.Write(vec.Z);
		writer.Write(vec.W);
	}

	public static void WriteMatrix(BinaryWriter writer, Matrix matrix)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(matrix.M11);
		writer.Write(matrix.M12);
		writer.Write(matrix.M13);
		writer.Write(matrix.M14);
		writer.Write(matrix.M21);
		writer.Write(matrix.M22);
		writer.Write(matrix.M23);
		writer.Write(matrix.M24);
		writer.Write(matrix.M31);
		writer.Write(matrix.M32);
		writer.Write(matrix.M33);
		writer.Write(matrix.M34);
		writer.Write(matrix.M41);
		writer.Write(matrix.M42);
		writer.Write(matrix.M43);
		writer.Write(matrix.M44);
	}

	public static Vector3 ReadVector3(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Vector4 ReadVector4(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Matrix ReadMatrix(BinaryReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		return new Matrix(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}
}
