namespace SuperCow.GameSources;

public class CToiletObj : CLevelObj
{
	public override void Draw(dcolor_t dc)
	{
		rect.isRectIntersectRect(SCGlobals.viewport_rect);
	}
}
