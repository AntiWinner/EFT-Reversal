using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AreaSubstrateSettings : SerializedScriptableObject
{
	[SerializeField]
	private Dictionary<EPanelType, _E833> _areaRelatedPanels;

	[SerializeField]
	private Dictionary<EAreaStatus, PanelVisibilitySettings> _panelsVisibilitySettings;

	public Dictionary<EPanelType, _E833> Panels => _areaRelatedPanels;

	public Dictionary<EAreaStatus, PanelVisibilitySettings> VisibilitySettings => _panelsVisibilitySettings;
}
