using System.Collections;
using UnityEngine;

namespace EFT.UI;

public sealed class BattleUIPanelExtraction : BattleUIPanel
{
	[SerializeField]
	private CustomTextMeshProUGUI _extractionLabel;

	private Coroutine _E09E;

	private Coroutine _E09F;

	private string _E0A0;

	public void Show(string text)
	{
		ShowGameObject();
		_E0A0 = text;
		if (_E09E != null)
		{
			StopCoroutine(_E09E);
			_E09E = null;
		}
		_E09E = StartCoroutine(Co_ShowInfoPanel(0f, 1f));
		_extractionLabel.text = _E0A0;
	}

	public void Show(string text, float duration)
	{
		ShowGameObject();
		_E0A0 = text;
		if (_E09E != null)
		{
			StopCoroutine(_E09E);
			_E09E = null;
		}
		if (_E09F != null)
		{
			StopCoroutine(_E09F);
			_E09F = null;
		}
		_E09E = StartCoroutine(Co_ShowInfoPanel(0f, 1f));
		_E09F = StartCoroutine(_E000(duration));
	}

	private IEnumerator _E000(float duration)
	{
		float num = duration;
		while (num > 0f)
		{
			_extractionLabel.text = string.Format(_E0A0.Localized(), num);
			num -= Time.deltaTime;
			if (num <= 0f)
			{
				Close();
				break;
			}
			yield return null;
		}
	}
}
