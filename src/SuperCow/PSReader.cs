using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

// REMARK: pixel shader, unused now
// internal class PSReader : ContentTypeReader<PixelShader>
// {
// 	protected override PixelShader Read(ContentReader input, PixelShader existingInstance)
// 	{
// 		int count = ((BinaryReader)(object)input).ReadInt32();
// 		byte[] array = ((BinaryReader)(object)input).ReadBytes(count);
// 		IGraphicsDeviceService val = (IGraphicsDeviceService)input.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
// 		return new PixelShader(val.GraphicsDevice, array);
// 	}
// }
