using System;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class ClientWatermark : UIElement
{
	private const float _E0B6 = 4f;

	[SerializeField]
	private TextMeshProUGUI _label;

	private Profile _E0B7;

	private Coroutine _E0B8;

	public void Show(Profile profile)
	{
		_E0B7 = profile;
		Stop();
		ShowGameObject();
		_E000();
	}

	public void Stop()
	{
		HideGameObject();
		if (_E0B8 != null)
		{
			StopCoroutine(_E0B8);
		}
	}

	private void _E000()
	{
		RectTransform obj = (RectTransform)base.transform;
		Rect rect = obj.rect;
		int hours = _E5AD.Now.TimeOfDay.Hours;
		bool flag = hours >= 13 && (hours <= 24 || hours <= 0);
		obj.anchoredPosition = _E001(Screen.height, Screen.width, (int)rect.height, (int)rect.width);
		_label.text = _E0B7.AccountId + _ED3E._E000(27331) + _E0B7.Nickname + _ED3E._E000(2540) + _E5AD.Now.ToString(_ED3E._E000(250624) + _E5AD.Now.ToString(_ED3E._E000(148382))) + _ED3E._E000(18502) + (flag ? _ED3E._E000(250605) : _ED3E._E000(250610));
		_E0B8 = this.WaitSeconds(4f, _E000);
	}

	private static Vector2 _E001(int screenHeight, int screenWidth, int rectHeight, int rectWidth)
	{
		System.Random random = new System.Random();
		int num = screenWidth / 2 - rectWidth / 2;
		int num2 = screenHeight / 2 - rectHeight / 2;
		int num3 = ((-num < num) ? random.Next(-num, num) : random.Next(num, -num));
		int num4 = ((-num2 < num2) ? random.Next(-num2, num2) : random.Next(num2, -num2));
		return new Vector2(num3, num4);
	}
}
