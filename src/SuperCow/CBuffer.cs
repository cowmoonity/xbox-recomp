using System;

namespace SuperCow;

public class CBuffer
{
	private byte[] dataBuffer;

	private int currentPtr;

	public CBuffer()
	{
		currentPtr = 0;
	}

	public void SetDataBuffer(byte[] InDataBuffer)
	{
		dataBuffer = InDataBuffer;
		currentPtr = 0;
	}

	public void ResetDataBuffer()
	{
		currentPtr = 0;
		Array.Clear(dataBuffer, 0, 0);
	}

	public ushort ReadWord()
	{
		ushort result = (ushort)BitConverter.ToInt16(GetMachineTypeData(2), 0);
		MoveArray(2);
		return result;
	}

	public long ReadLong()
	{
		int num = BitConverter.ToInt32(GetMachineTypeData(4), 0);
		MoveArray(4);
		return num;
	}

	public float ReadFloat()
	{
		float num = BitConverter.ToSingle(GetMachineTypeData(4), 0);
		MoveArray(4);
		return num;
	}

	public bool ReadBool()
	{
		bool result = BitConverter.ToBoolean(GetMachineTypeData(1), 0);
		MoveArray(1);
		return result;
	}

	public vec3_t ReadVector3()
	{
		return new vec3_t
		{
			x = ReadFloat(),
			y = ReadFloat(),
			z = ReadFloat()
		};
	}

	public vec2_t ReadVector2()
	{
		return new vec2_t
		{
			x = ReadFloat(),
			y = ReadFloat()
		};
	}

	public byte ReadByte()
	{
		char c = BitConverter.ToChar(dataBuffer, 0);
		MoveArray(1);
		return (byte)c;
	}

	public int ReadInt()
	{
		int result = BitConverter.ToInt32(GetMachineTypeData(4), 0);
		MoveArray(4);
		return result;
	}

	public byte[] ReadBytes(int size)
	{
		byte[] array = new byte[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = dataBuffer[i];
		}
		MoveArray(size);
		return array;
	}

	public string ReadString(int stringSize)
	{
		byte[] array = new byte[stringSize];
		int num = 0;
		for (int i = 0; i < stringSize; i++)
		{
			if (dataBuffer[i] > 0)
			{
				num++;
				array[i] = dataBuffer[i];
			}
		}
		MoveArray(stringSize);
		byte[] array2 = new byte[num];
		Buffer.BlockCopy(array, 0, array2, 0, num);
		return SuperCowHelper.StringFromByteArray(array2);
	}

	public void SkipData(int size)
	{
		MoveArray(size);
	}

	private void MoveArray(int stepSize)
	{
		int num = dataBuffer.Length;
		Buffer.BlockCopy(dataBuffer, stepSize, dataBuffer, 0, num - stepSize);
		currentPtr += stepSize;
	}

	public bool IsEnfOfBuffer()
	{
		return currentPtr >= dataBuffer.Length;
	}

	private byte[] GetMachineTypeData(int size)
	{
		byte[] array = new byte[size];
		Buffer.BlockCopy(dataBuffer, 0, array, 0, size);
		if (!BitConverter.IsLittleEndian)
		{
			Array.Reverse(array);
		}
		return array;
	}
}
