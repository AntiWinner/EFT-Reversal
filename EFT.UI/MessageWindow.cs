using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public class MessageWindow : DialogWindow<_EC7B>
{
	private const float _E00C = 0.15f;

	private const int _E00D = 8;

	[SerializeField]
	private TextMeshProUGUI _message;

	private Coroutine _E00E;

	[CompilerGenerated]
	private string _E00F;

	public TextAlignmentOptions MessageAlignment
	{
		set
		{
			_message.alignment = value;
		}
	}

	protected string Description
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		private set
		{
			_E00F = value;
		}
	}

	protected TextMeshProUGUI Message => _message;

	public _EC7B Show(string title, string message, Action acceptAction, [CanBeNull] Action cancelAction, float time = 0f)
	{
		_EC7B result = Show(title, acceptAction, cancelAction);
		Description = message;
		if (_message != null)
		{
			_message.text = Description;
		}
		if (_E00E != null)
		{
			StopCoroutine(_E00E);
		}
		if (time > 0f)
		{
			_E00E = StartCoroutine(_E001(_E5AD.Now.AddSeconds(time), Decline));
		}
		return result;
	}

	private void _E000(string title, string description)
	{
		base.Caption.text = title;
		if (_message != null)
		{
			_message.text = description;
		}
	}

	protected async Task AnimateText(Color color)
	{
		base.Caption.DOColor(color, 0.15f).SetLoops(8, LoopType.Yoyo);
		await _message.DOColor(color, 0.15f).SetLoops(8, LoopType.Yoyo);
	}

	private IEnumerator _E001(DateTime endTime, [CanBeNull] Action onFinished)
	{
		while (_E5AD.Now < endTime)
		{
			int num = (int)(endTime - _E5AD.Now).TotalSeconds;
			_message.text = Description + _ED3E._E000(248003) + num.ToString(_ED3E._E000(215469)) + _ED3E._E000(124724).Localized();
			yield return null;
		}
		onFinished?.Invoke();
	}
}
