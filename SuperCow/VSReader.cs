using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

internal class VSReader : ContentTypeReader<VertexShader>
{
	protected override VertexShader Read(ContentReader input, VertexShader existingInstance)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		int count = ((BinaryReader)(object)input).ReadInt32();
		byte[] array = ((BinaryReader)(object)input).ReadBytes(count);
		IGraphicsDeviceService val = (IGraphicsDeviceService)input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
		return new VertexShader(val.GraphicsDevice, array);
	}
}
