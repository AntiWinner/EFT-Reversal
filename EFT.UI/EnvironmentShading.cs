using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.UI;

public sealed class EnvironmentShading : SerializedMonoBehaviour
{
	private const float m__E000 = 0.8f;

	[SerializeField]
	private Dictionary<EShadingType, CanvasGroup> _environmentShadings;

	private bool _E001;

	private CanvasGroup _E002;

	private EShadingType _E003;

	public void SetDefaultShading(CanvasGroup canvasGroup)
	{
		if (_environmentShadings.TryGetValue(EShadingType.None, out var value))
		{
			_E000(value);
		}
		_environmentShadings[EShadingType.None] = canvasGroup;
		ResetShading();
	}

	public void ResetShading()
	{
		SetShading(_E003, force: true);
	}

	public void SetShading(EShadingType type, bool force = false)
	{
		if (!force && _E003 == type)
		{
			return;
		}
		foreach (var (eShadingType2, canvasGroup2) in _environmentShadings)
		{
			if (eShadingType2 == type)
			{
				_E003 = type;
				_E002 = canvasGroup2;
				SetShadingVisibility(_E001, 0f);
				_E002.gameObject.SetActive(value: true);
			}
			else
			{
				_E000(canvasGroup2);
			}
		}
	}

	public void SetShadingVisibility(bool visible, float duration = 0.8f)
	{
		_E001 = visible;
		if (!(_E002 == null))
		{
			_E002.DOKill();
			float num = (visible ? 1f : 0f);
			if (duration.IsZero())
			{
				_E002.alpha = num;
				return;
			}
			float num2 = Math.Abs(_E002.alpha - num);
			_E002.DOFade(num, duration * num2);
		}
	}

	private static void _E000(CanvasGroup canvasGroup)
	{
		canvasGroup.DOKill();
		canvasGroup.alpha = 0f;
		canvasGroup.gameObject.SetActive(value: false);
	}
}
