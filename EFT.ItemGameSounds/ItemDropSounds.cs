using System.Collections.Generic;
using JsonType;
using UnityEngine;

namespace EFT.ItemGameSounds;

public class ItemDropSounds : ScriptableObject
{
	public AnimationCurve EnergyToVolumeCurve;

	public BaseBallistic.ESurfaceSound DefaultSurfaceSound;

	public ItemDropSurfaceSet[] SurfaceSets;

	private readonly Dictionary<BaseBallistic.ESurfaceSound, ItemDropSurfaceSet> _E000 = new Dictionary<BaseBallistic.ESurfaceSound, ItemDropSurfaceSet>(_E3A5<BaseBallistic.ESurfaceSound>.EqualityComparer);

	public SoundBank GetSoundBank(BaseBallistic.ESurfaceSound surfaceSound, EItemDropSoundType dropSoundType)
	{
		if (_E000.Count != SurfaceSets.Length)
		{
			_E000.Clear();
			ItemDropSurfaceSet[] surfaceSets = SurfaceSets;
			foreach (ItemDropSurfaceSet itemDropSurfaceSet in surfaceSets)
			{
				if (!_E000.ContainsKey(itemDropSurfaceSet.Surface))
				{
					_E000.Add(itemDropSurfaceSet.Surface, itemDropSurfaceSet);
				}
				else
				{
					Debug.LogError(string.Concat(itemDropSurfaceSet.Surface, _ED3E._E000(159849)));
				}
			}
		}
		if (!_E000.ContainsKey(surfaceSound))
		{
			surfaceSound = DefaultSurfaceSound;
		}
		return _E000[surfaceSound].GetBank(dropSoundType);
	}
}
