using System.Collections.Generic;
using UnityEngine;

namespace EFT.UI.Health;

public class InventoryScreenHealthPanel : UIElement
{
	[SerializeField]
	private HealthParametersPanel _parametersPanel;

	[SerializeField]
	private GameObject _silhouette;

	[SerializeField]
	private BodyPartView _head;

	[SerializeField]
	private BodyPartView _chest;

	[SerializeField]
	private BodyPartView _stomach;

	[SerializeField]
	private BodyPartView _leftArm;

	[SerializeField]
	private BodyPartView _rightArm;

	[SerializeField]
	private BodyPartView _leftLeg;

	[SerializeField]
	private BodyPartView _rightLeg;

	[SerializeField]
	private GameObject _damageLegend;

	private readonly Dictionary<EBodyPart, BodyPartView> _E2F8 = _E3A5<EBodyPart>.GetDictWith<BodyPartView>();

	private SimpleTooltip _E02A;

	private void Awake()
	{
		_E2F8.Clear();
		_E2F8.Add(EBodyPart.Head, _head);
		_E2F8.Add(EBodyPart.Chest, _chest);
		_E2F8.Add(EBodyPart.LeftArm, _leftArm);
		_E2F8.Add(EBodyPart.RightArm, _rightArm);
		_E2F8.Add(EBodyPart.Stomach, _stomach);
		_E2F8.Add(EBodyPart.LeftLeg, _leftLeg);
		_E2F8.Add(EBodyPart.RightLeg, _rightLeg);
	}

	public void Show(_E9C4 healthController, _EAE7 inventory, _E74F skillManager, DamageHistory damageHistory = null)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		ShowGameObject();
		_parametersPanel.Show(healthController, inventory, skillManager);
		UI.AddDisposable(_parametersPanel);
		_damageLegend.SetActive(damageHistory != null);
		foreach (KeyValuePair<EBodyPart, BodyPartView> item in _E2F8)
		{
			item.Value.Show(healthController, item.Key, _E02A, damageHistory);
			UI.AddDisposable(item.Value);
		}
	}

	public void SetParametersVisibility(bool value)
	{
		_parametersPanel.ShowSecondaryParameters(value);
		_silhouette.SetActive(value);
	}
}
