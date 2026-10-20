using System.Collections.Generic;

namespace SuperCow;

public class Level
{
	public int objcount;

	public string levelname;

	public int curlevelback;

	public int curleveltask;

	public int curlevelmusic;

	public List<obj> objects = new List<obj>();

	public sbyte[,,] ar_ground = new sbyte[6, 256, 64];

	public sbyte[,,] ar_groundtype = new sbyte[6, 256, 64];
}
