using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

// REMARK: vertex shader, unused now
// internal class VSReader : ContentTypeReader<VertexShader>
// {
// 	protected override VertexShader Read(ContentReader input, VertexShader existingInstance)
// 	{
// 		int count = ((BinaryReader)(object)input).ReadInt32();
// 		byte[] array = ((BinaryReader)(object)input).ReadBytes(count);
// 		IGraphicsDeviceService val = (IGraphicsDeviceService)input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
// 		return new VertexShader(val.GraphicsDevice, array);
// 	}
// }
