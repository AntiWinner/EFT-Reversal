using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class BattleUIPanelExitTrigger : BattleUIPanel
{
	[SerializeField]
	private CustomTextMeshProUGUI _extractionLabel;

	[SerializeField]
	private Color _countdownColor;

	[SerializeField]
	private Color _requirementsColor;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Image _glow;

	private Coroutine _E09E;

	private Coroutine _E09F;

	public void Show(float duration)
	{
		ShowGameObject();
		_E000();
		_E09E = StartCoroutine(Co_ShowInfoPanel(0.5f, 1f));
		Image background = _background;
		Color color = (_glow.color = _countdownColor);
		background.color = color;
		_E09F = StartCoroutine(_E001(duration));
	}

	public void ShowRequirements(string tip)
	{
		ShowGameObject();
		_E000();
		Image background = _background;
		Color color = (_glow.color = _requirementsColor);
		background.color = color;
		_extractionLabel.SetMonospaceText(string.Format(tip));
		_E09E = StartCoroutine(Co_ShowInfoPanel(0.5f, 1f));
	}

	public override void Close()
	{
		_E000();
		base.Close();
	}

	private void _E000()
	{
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
	}

	private IEnumerator _E001(float duration)
	{
		float num = duration;
		while (num > 0f)
		{
			string text = string.Format(_ED3E._E000(250432).Localized(), num);
			_extractionLabel.SetMonospaceText(text);
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
