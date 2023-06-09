using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoverPointCreatorPresetCollection", menuName = "Scriptable objects/AI/CoverPointCreator Preset Collection")]
public class CoverPointCreatorPresetCollection : ScriptableObject
{
	[SerializeField]
	private List<CoverPointCreatorPreset> _presets;

	public List<CoverPointCreatorPreset> Presets => _presets;

	public CoverPointCreatorPreset GetPresetByName(string name)
	{
		for (int i = 0; i < _presets.Count; i++)
		{
			if (_presets[i].Name == name)
			{
				return _presets[i];
			}
		}
		for (int j = 0; j < _presets.Count; j++)
		{
			if (_presets[j].Name == _ED3E._E000(30808))
			{
				return _presets[j];
			}
		}
		return null;
	}

	public void DrawPresetsInspector()
	{
	}
}
