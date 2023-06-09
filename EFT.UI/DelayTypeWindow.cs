using System;
using System.Collections;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class DelayTypeWindow : MessageWindow
{
	private const float _E002 = 0.02f;

	[SerializeField]
	private Image _traderImage;

	private Coroutine _E003;

	private bool _E004;

	protected override bool CloseOnAccept => true;

	public void Show(string title, string message, string traderId, Action acceptAction, float time = 0f)
	{
		Show(title, message, acceptAction, null, time);
		bool flag = !string.IsNullOrEmpty(traderId);
		if (flag)
		{
			Singleton<_E5CB>.Instance.TradersSettings[traderId].GetAndAssignAvatar(_traderImage, UI.CancellationToken).HandleExceptions();
		}
		_traderImage.gameObject.SetActive(flag);
		if (_E003 != null)
		{
			StopCoroutine(_E003);
		}
		_E003 = StartCoroutine(_E000(message));
	}

	private IEnumerator _E000(string text)
	{
		int num = 0;
		base.Message.text = string.Empty;
		_E004 = false;
		while (num < text.Length)
		{
			base.Message.text += text[num++];
			yield return new WaitForSeconds(0.02f);
		}
		_E004 = true;
	}

	public void FinishAnimating()
	{
		if (!_E004)
		{
			if (_E003 != null)
			{
				StopCoroutine(_E003);
			}
			_E004 = true;
			base.Message.text = base.Description;
		}
		else
		{
			Close();
		}
	}
}
