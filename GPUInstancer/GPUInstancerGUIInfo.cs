using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerGUIInfo : MonoBehaviour
{
	public bool showRenderedAmount;

	public bool showPrototypesSeparate;

	public bool showPrefabManagers = true;

	public bool showDetailManagers = true;

	public bool showTreeManagers = true;

	private static List<_E4C2> m__E000 = new List<_E4C2> { null };

	private void OnGUI()
	{
		if (GPUInstancerManager.activeManagerList == null)
		{
			return;
		}
		if (GPUInstancerManager.showRenderedAmount != showRenderedAmount)
		{
			GPUInstancerManager.showRenderedAmount = showRenderedAmount;
		}
		int num = 0;
		int num2 = 0;
		string text = "";
		Color color = GUI.color;
		GUI.color = Color.red;
		if (showRenderedAmount)
		{
			GUI.Label(new Rect(10f, Screen.height - 30, 700f, 30f), _ED3E._E000(115517));
			num += 30;
		}
		foreach (GPUInstancerManager activeManager in GPUInstancerManager.activeManagerList)
		{
			num2 = 0;
			if (activeManager is GPUInstancerPrefabManager)
			{
				if (!showPrefabManagers)
				{
					continue;
				}
				text = _ED3E._E000(115533);
				num2 = ((GPUInstancerPrefabManager)activeManager).GetEnabledPrefabCount();
			}
			else if (activeManager is GPUInstancerTreeManager)
			{
				if (!showTreeManagers)
				{
					continue;
				}
				text = _ED3E._E000(115524);
			}
			else if (activeManager is GPUInstancerDetailManager)
			{
				if (!showDetailManagers)
				{
					continue;
				}
				text = _ED3E._E000(115521);
			}
			if (showPrototypesSeparate)
			{
				foreach (_E4C2 runtimeData in activeManager.runtimeDataList)
				{
					GPUInstancerGUIInfo.m__E000[0] = runtimeData;
					_E000(runtimeData.prototype.ToString(), GPUInstancerGUIInfo.m__E000, showRenderedAmount, num, num2);
					num += _E4BF.DEBUG_INFO_SIZE;
				}
			}
			else
			{
				_E000(text, activeManager.runtimeDataList, showRenderedAmount, num, num2);
				num += _E4BF.DEBUG_INFO_SIZE;
			}
		}
		GUI.color = color;
	}

	private void OnDisable()
	{
		if (showRenderedAmount)
		{
			GPUInstancerManager.showRenderedAmount = false;
		}
	}

	private static void _E000(string name, List<_E4C2> runtimeDataList, bool showRenderedAmount, int startPos, int enabledCount)
	{
		if (runtimeDataList == null || runtimeDataList.Count == 0)
		{
			GUI.Label(new Rect(10f, Screen.height - startPos - 25, 700f, 30f), _ED3E._E000(115576) + name + _ED3E._E000(115566));
			return;
		}
		int num = 0;
		for (int i = 0; i < runtimeDataList.Count; i++)
		{
			num += runtimeDataList[i].instanceCount;
		}
		GUI.Label(new Rect(10f, Screen.height - startPos - 45, 700f, 30f), _ED3E._E000(115598) + name + _ED3E._E000(115589) + runtimeDataList.Count);
		GUI.Label(new Rect(10f, Screen.height - startPos - 25, 700f, 30f), _ED3E._E000(115598) + name + _ED3E._E000(115632) + num);
		if (showRenderedAmount)
		{
			GUI.Label(new Rect(10f, Screen.height - startPos - 65, 700f, 30f), _ED3E._E000(115618) + name + _ED3E._E000(115632) + _E001(runtimeDataList));
			GUI.Label(new Rect(10f, Screen.height - startPos - 85, 700f, 30f), _ED3E._E000(115668) + name + _ED3E._E000(115632) + _E001(runtimeDataList, isShadow: true));
		}
		if (enabledCount > 0)
		{
			GUI.Label(new Rect(10f, Screen.height - startPos - 105, 700f, 30f), _ED3E._E000(115653) + name + _ED3E._E000(115632) + enabledCount);
		}
	}

	private static string _E001<_E077>(List<_E077> runtimeData, bool isShadow = false) where _E077 : _E4C2
	{
		int num = 0;
		int num2 = 1;
		for (int i = 0; i < runtimeData.Count; i++)
		{
			if (num2 < runtimeData[i].instanceLODs.Count)
			{
				num2 = runtimeData[i].instanceLODs.Count;
			}
		}
		int[] array = new int[num2];
		for (int j = 0; j < runtimeData.Count; j++)
		{
			if (isShadow)
			{
				if (runtimeData[j].shadowArgs != null && runtimeData[j].shadowArgs.Length != 0)
				{
					for (int k = 0; k < runtimeData[j].instanceLODs.Count; k++)
					{
						array[k] += (int)runtimeData[j].shadowArgs[runtimeData[j].instanceLODs[k].argsBufferOffset + 1];
					}
				}
			}
			else if (runtimeData[j].args != null && runtimeData[j].args.Length != 0)
			{
				for (int l = 0; l < runtimeData[j].instanceLODs.Count; l++)
				{
					array[l] += (int)runtimeData[j].args[runtimeData[j].instanceLODs[l].argsBufferOffset + 1];
				}
			}
		}
		string text = "";
		for (int m = 0; m < array.Length; m++)
		{
			num += array[m];
			text = text + _ED3E._E000(115698) + m + _ED3E._E000(12201) + array[m] + ((m == array.Length - 1) ? "" : _ED3E._E000(10270));
		}
		return num + _ED3E._E000(54246) + text + _ED3E._E000(27308);
	}
}
