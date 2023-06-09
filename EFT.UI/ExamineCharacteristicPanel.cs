using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ExamineCharacteristicPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EAE6 controller;

		public ExamineCharacteristicPanel _003C_003E4__this;

		internal void _E000()
		{
			controller.ExamineEvent -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private Slider _examineProgressSlider;

	[SerializeField]
	private CustomTextMeshProUGUI _examineLabel;

	private readonly Color32 _E128 = Color.red;

	private readonly Color32 _E129 = new Color32(84, 193, byte.MaxValue, byte.MaxValue);

	private Item _E085;

	private bool _E12A;

	private DateTime _E12B;

	private float _E12C;

	public void Show(Item item, _EAE6 controller)
	{
		UI.Dispose();
		_E085 = item;
		_examineLabel.text = _ED3E._E000(247086).Localized();
		_examineLabel.color = _E128;
		_examineProgressSlider.maxValue = 1f;
		controller.ExamineEvent += _E000;
		UI.AddDisposable(delegate
		{
			controller.ExamineEvent -= _E000;
		});
	}

	private void _E000(_EAF6 examineArgs)
	{
		if (_E085 == examineArgs.Item)
		{
			_E12A = examineArgs.Status == CommandStatus.Begin;
			if (_E12A)
			{
				_E12B = _E5AD.Now;
				_E12C = examineArgs.ExamineTime;
			}
		}
	}

	private void Update()
	{
		if (_E12A)
		{
			double num = (_E5AD.Now - _E12B).TotalSeconds / (double)_E12C;
			int num2 = Mathf.Clamp((int)(num * 100.0), 0, 100);
			_examineProgressSlider.value = (float)num;
			_examineLabel.text = _ED3E._E000(247124).Localized() + _ED3E._E000(54246) + num2 + _ED3E._E000(247104);
			if (_examineLabel.color != _E129)
			{
				_examineLabel.color = _E129;
			}
		}
	}
}
