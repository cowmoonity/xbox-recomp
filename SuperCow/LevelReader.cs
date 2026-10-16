using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SuperCow;

public class LevelReader : ContentTypeReader<Level>
{
	protected override Level Read(ContentReader input, Level existingInstance)
	{
		Level level = new Level();
		level.levelname = ((BinaryReader)(object)input).ReadString();
		level.objcount = ((BinaryReader)(object)input).ReadInt32();
		level.curlevelback = ((BinaryReader)(object)input).ReadInt32();
		level.curleveltask = ((BinaryReader)(object)input).ReadInt32();
		level.curlevelmusic = ((BinaryReader)(object)input).ReadInt32();
		for (int i = 0; i < level.objcount; i++)
		{
			obj obj2 = new obj();
			obj2.name = ((BinaryReader)(object)input).ReadString();
			obj2.rect[0] = ((BinaryReader)(object)input).ReadDouble();
			obj2.rect[1] = ((BinaryReader)(object)input).ReadDouble();
			obj2.rect[2] = ((BinaryReader)(object)input).ReadDouble();
			obj2.rect[3] = ((BinaryReader)(object)input).ReadDouble();
			obj2.drawlayer = ((BinaryReader)(object)input).ReadInt32();
			obj2.rotation = ((BinaryReader)(object)input).ReadDouble();
			obj2.endpos[0] = ((BinaryReader)(object)input).ReadDouble();
			obj2.endpos[1] = ((BinaryReader)(object)input).ReadDouble();
			obj2.invertobj = ((BinaryReader)(object)input).ReadBoolean();
			obj2.extraparam = ((BinaryReader)(object)input).ReadString();
			level.objects.Add(obj2);
		}
		for (int j = 0; j < 6; j++)
		{
			for (int num = 63; num >= 0; num--)
			{
				for (int k = 0; k < 256; k++)
				{
					level.ar_ground[j, k, num] = ((BinaryReader)(object)input).ReadSByte();
					level.ar_groundtype[j, k, num] = ((BinaryReader)(object)input).ReadSByte();
				}
			}
		}
		return level;
	}
}
