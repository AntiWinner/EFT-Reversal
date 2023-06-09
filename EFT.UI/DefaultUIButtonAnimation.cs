using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class DefaultUIButtonAnimation : SerializedMonoBehaviour, _EC42, _EC43<EButtonAnimationState>
{
	private const float m__E000 = 0.15f;

	private const float m__E001 = 0.3f;

	[SerializeField]
	public RectTransform Background;

	[SerializeField]
	public Image Image;

	[SerializeField]
	public Image Icon;

	[SerializeField]
	public TMP_Text Label;

	[SerializeField]
	private Dictionary<EButtonAnimationState, GameObject> _stateAttachments = new Dictionary<EButtonAnimationState, GameObject>();

	private readonly List<Tween> m__E002 = new List<Tween>();

	[SerializeField]
	private bool _changeColors;

	[SerializeField]
	private Color _normalLabelColor = new Color32(231, 229, 212, byte.MaxValue);

	[SerializeField]
	private Color _highlightedLabelColor = Color.black;

	private Color m__E003 = Color.white;

	private Color m__E004 = Color.white;

	private Color m__E005 = Color.white;

	private Color m__E006 = Color.white;

	public void TransitionToState(EButtonAnimationState state)
	{
		_E000(state, animated: true);
	}

	public void SetState(EButtonAnimationState state)
	{
		_E000(state, animated: false);
	}

	private void _E000(EButtonAnimationState state, bool animated)
	{
		switch (state)
		{
		case EButtonAnimationState.Normal:
			_E001(animated);
			break;
		case EButtonAnimationState.Highlighted:
			_E002(animated).HandleExceptions();
			break;
		case EButtonAnimationState.Pressed:
			_E003(animated);
			break;
		case EButtonAnimationState.Disabled:
			_E004(animated);
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(250735), state.ToString());
		}
		foreach (var (eButtonAnimationState2, gameObject2) in _stateAttachments)
		{
			if (eButtonAnimationState2 != state && gameObject2 != null)
			{
				gameObject2.SetActive(value: false);
			}
		}
		if (_stateAttachments.TryGetValue(state, out var value) && value != null)
		{
			value.SetActive(value: true);
		}
	}

	public void Stop()
	{
		foreach (Tween item in this.m__E002)
		{
			item.Kill();
		}
		this.m__E002.Clear();
	}

	private void _E001(bool animated)
	{
		Stop();
		if (!animated)
		{
			Label.color = _normalLabelColor;
			Background.anchoredPosition = new Vector2(0f, Background.anchoredPosition.y);
			Image.color = this.m__E003.SetAlpha(0f);
			if (Icon != null)
			{
				Icon.color = this.m__E005.SetAlpha(0f);
			}
			return;
		}
		if (Icon != null)
		{
			_E006(Icon.DOColor(this.m__E005.SetAlpha(0f), 0.15f)).HandleExceptions();
		}
		_E005(Background.DOAnchorPosX(0f, 0.15f), Label.DOColor(_normalLabelColor, 0.15f), Image.DOColor(this.m__E003.SetAlpha(0f), 0.15f)).HandleExceptions();
	}

	private async Task _E002(bool animated)
	{
		Stop();
		if (!animated)
		{
			Label.color = _highlightedLabelColor;
			Background.anchoredPosition = new Vector2(0f, Background.anchoredPosition.y);
			Image.color = this.m__E004;
			if (Icon != null)
			{
				Icon.color = this.m__E006;
			}
			return;
		}
		float duration = 0.2f;
		Image.color = this.m__E004.SetAlpha(0f);
		Background.anchoredPosition = new Vector2(-15f, Background.anchoredPosition.y);
		_E005(Background.DOAnchorPosX(0f, duration), Image.DOFade(1f, duration)).HandleExceptions();
		duration = 0.1f;
		if (Icon != null)
		{
			_E006(Icon.DOFade(0f, duration)).HandleExceptions();
		}
		if (await _E006(Label.DOFade(0f, duration)))
		{
			Label.color = _highlightedLabelColor.SetAlpha(0f);
			if (Icon != null)
			{
				Icon.color = this.m__E006.SetAlpha(0f);
			}
			duration = 0.3f - duration;
			if (Icon != null)
			{
				_E006(Icon.DOFade(1f, duration)).HandleExceptions();
			}
			_E006(Label.DOFade(1f, duration)).HandleExceptions();
		}
	}

	private void _E003(bool animated)
	{
	}

	private void _E004(bool animated)
	{
		_E001(animated);
	}

	private async Task<bool> _E005(params Tween[] tweens)
	{
		Task<bool>[] array = new Task<bool>[tweens.Length];
		for (int i = 0; i < tweens.Length; i++)
		{
			array[i] = _E006(tweens[i]);
		}
		return (await Task.WhenAll(array)).All((bool result) => result);
	}

	private async Task<bool> _E006(Tween tween, Ease ease = Ease.OutQuad)
	{
		this.m__E002.Add(tween);
		await tween.SetEase(ease);
		this.m__E002.Remove(tween);
		return tween.IsComplete();
	}
}
