using UnityEngine;

public class AIDebugSettings : ScriptableObject
{
	private static AIDebugSettings _E000;

	public bool CoverPointDrawEnable;

	public Texture2D BearingPointIcon;

	public Texture2D SelectedBearingPointIcon;

	public static AIDebugSettings Instance
	{
		get
		{
			if (!_E000)
			{
				return _E000 = _E3A2.Load<AIDebugSettings>(_ED3E._E000(32215));
			}
			return _E000;
		}
	}
}
