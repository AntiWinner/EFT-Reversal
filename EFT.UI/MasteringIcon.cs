using System;
using System.Collections.Generic;
using Comfort.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class MasteringIcon : ItemIconView
{
	[SerializeField]
	private CustomTextMeshProUGUI _progress;

	[SerializeField]
	private Image _levelUpGlow;

	[SerializeField]
	private GameObject _mastering;

	[SerializeField]
	private CustomTextMeshProUGUI _masteringText;

	public void Show(KeyValuePair<string, _E750> masteringSkill, Action<bool, PointerEventData> onHover)
	{
		ShowGameObject();
		_mastering.gameObject.SetActive(value: true);
		UpdateProgress(masteringSkill.Value);
		Show(Singleton<_E63B>.Instance.GetPresetItem(masteringSkill.Key), onHover);
	}

	public void UpdateProgress(_E750 mastering)
	{
		_progress.text = string.Format(_ED3E._E000(171743), (int)(mastering.LevelProgress * (float)mastering.LevelingThreshold), mastering.LevelingThreshold);
		_masteringText.text = (mastering.Level + 1).ToString();
	}

	protected override void OnHoverEnd(PointerEventData eventData)
	{
		_levelUpGlow.gameObject.SetActive(value: false);
		base.OnHoverEnd(eventData);
	}
}
