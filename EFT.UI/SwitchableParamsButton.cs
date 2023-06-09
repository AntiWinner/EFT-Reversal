using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class SwitchableParamsButton : Button
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SwitchableParamsButton _003C_003E4__this;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this._E000(!_003C_003E4__this.m__E000);
		}

		internal void _E001()
		{
			callback();
		}
	}

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _onSprite;

	[SerializeField]
	private Sprite _offSprite;

	[SerializeField]
	private CustomTextMeshProUGUI _label;

	[SerializeField]
	private string _onText;

	[SerializeField]
	private string _offText;

	[SerializeField]
	private bool _toUpper;

	private bool m__E000;

	public void AddSubscription(Action callback)
	{
		AddSubscription(callback, isOn: false);
	}

	public void AddSubscription(Action callback, bool isOn)
	{
		this.m__E000 = isOn;
		base.onClick.AddListener(delegate
		{
			_E000(!this.m__E000);
		});
		base.onClick.AddListener(delegate
		{
			callback();
		});
		_E000(isOn);
	}

	private void _E000(bool isOn)
	{
		this.m__E000 = isOn;
		if (_image != null)
		{
			_image.sprite = (isOn ? _onSprite : _offSprite);
		}
		if (!(_label == null))
		{
			string text = (isOn ? _onText.Localized() : _offText.Localized());
			if (_toUpper)
			{
				text = text.ToUpper();
			}
			_label.text = text;
		}
	}
}
