using System.Runtime.InteropServices;

namespace SuperCow;

public class bound_t
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
	public string name;

	public bool b_circle;

	public vec3_t center;

	public float radius;

	public float dx;

	public float dy;

	public bound_t()
	{
		radius = (dx = (dy = 0f));
		b_circle = false;
		center = default(vec3_t);
	}
}
