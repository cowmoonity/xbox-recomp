namespace SuperCow;

public class light_t
{
	public LightingType Type;

	public color_t Diffuse = default(color_t);

	public color_t Specular = default(color_t);

	public color_t Ambient = default(color_t);

	public vec3_t Position = default(vec3_t);

	public vec3_t Direction = default(vec3_t);

	public float Range;

	public float Falloff;

	public float Attenuation0;

	public float Attenuation1;

	public float Attenuation2;

	public float Theta;

	public float Phi;
}
