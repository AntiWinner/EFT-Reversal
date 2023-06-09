using System;
using System.Collections.Generic;

namespace EFT.Hideout;

[Serializable]
public sealed class PanelVisibilitySettings
{
	public List<EPanelType> CurrentLevel;

	public List<EPanelType> NextLevel;

	private EPanelType _E000()
	{
		return (EPanelType)CurrentLevel.Count;
	}

	private EPanelType _E001()
	{
		return (EPanelType)NextLevel.Count;
	}
}
