using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AreaPanelSettings : SerializedScriptableObject
{
	public Dictionary<EAreaStatus, Sprite> StatusAreaMap;

	public Sprite this[EAreaStatus status] => StatusAreaMap[status];
}
