using System.Collections.Generic;
using UnityEngine;

public class AICalculationsInfo : MonoBehaviour
{
	public List<BaseAICalcSystemInfo> Infos = new List<BaseAICalcSystemInfo>();

	public void Save(BaseAICalcSystemInfo system)
	{
		foreach (BaseAICalcSystemInfo info in Infos)
		{
			if (info.Name != system.Name)
			{
				continue;
			}
			Infos.Remove(info);
			break;
		}
		Infos.Add(system);
	}
}
