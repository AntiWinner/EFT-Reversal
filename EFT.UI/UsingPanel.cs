using System;
using System.Collections;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class UsingPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _itemName;

	[SerializeField]
	private Image _progressBarImage;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _examineColor;

	private DateTime _E0DA;

	private DateTime _E0E3;

	private Coroutine _E0E4;

	private _E992 _E0E5;

	public void Init(_E9C4 healthController, _EAE5 itemController)
	{
		healthController.EffectStartedEvent -= _E001;
		healthController.EffectRemovedEvent -= _E002;
		healthController.EffectStartedEvent += _E001;
		healthController.EffectRemovedEvent += _E002;
		itemController.PlayerExamineEvent -= _E003;
		itemController.PlayerExamineEvent += _E003;
	}

	private void _E000(string itemName, float useTime)
	{
		ShowGameObject();
		if (_E0E4 != null)
		{
			StopCoroutine(_E0E4);
		}
		_itemName.text = itemName;
		_E0E4 = StartCoroutine(_E004(useTime));
	}

	private void _E001(_E992 effect)
	{
		_caption.text = _ED3E._E000(253461).Localized();
		_progressBarImage.color = _defaultColor;
		if (effect is _E9BF && EFTHardSettings.Instance.MED_EFFECT_USING_PANEL)
		{
			_E0E5 = effect;
			Item medItem = ((_E9BF)effect).MedItem;
			_E000(medItem.Name.Localized(), effect.WorkStateTime);
		}
	}

	private void _E002(_E992 effect)
	{
		if (_E0E5 == effect)
		{
			_E0E5 = null;
			Close();
		}
	}

	private void _E003(_EAF6 args)
	{
		if (args.Status == CommandStatus.Begin)
		{
			_caption.text = _ED3E._E000(253459).Localized();
			_progressBarImage.color = _examineColor;
			_E000(args.Item.Name.Localized(), args.ExamineTime);
		}
	}

	private IEnumerator _E004(float duration)
	{
		_E0DA = _E5AD.Now;
		_E0E3 = _E0DA.AddSeconds(duration);
		while (_E5AD.Now < _E0E3)
		{
			_progressBarImage.fillAmount = (float)((_E5AD.Now - _E0DA).TotalSeconds / (_E0E3 - _E0DA).TotalSeconds);
			yield return null;
		}
		Close();
	}

	private void _E005()
	{
		_E0E4 = null;
	}

	public override void Close()
	{
		base.Close();
		_E005();
	}
}
