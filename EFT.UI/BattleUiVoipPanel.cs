using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class BattleUiVoipPanel : UIElement
{
	[Serializable]
	private sealed class BanContainer
	{
		public CanvasGroup Group;

		public CustomTextMeshProUGUI LeftTimeText;

		public void Show()
		{
			Group.alpha = 1f;
		}

		public void Hide()
		{
			Group.alpha = 0f;
		}
	}

	[Serializable]
	private sealed class StatusContainer
	{
		public CanvasGroup Group;

		public CustomTextMeshProUGUI LeftTimeText;

		public Image StaticImage;

		public Sprite BlockedSprite;

		public Sprite BannedSprite;

		public Sprite TalkDetectedSprite;

		public Image[] TalkingStates;

		public float TweenDuration = 0.5f;

		public Ease TweenEase = Ease.Linear;

		private Sequence _talkingAnimation;

		private Action _talkDetectionBinding;

		public void StartTalkDetection(_E7B3 controller)
		{
			_E000(TalkDetectedSprite);
			LeftTimeText.gameObject.SetActive(value: false);
			_talkDetectionBinding?.Invoke();
			_talkDetectionBinding = controller.TalkDetected.Bind(_E001);
		}

		public void StartTalkingAnimation(float duration)
		{
			StaticImage.ChangeImageAlpha(0f);
			LeftTimeText.gameObject.SetActive(value: true);
			Group.DOFade(1f, duration);
			_E002().Restart();
		}

		public void ShowBlock(float duration)
		{
			LeftTimeText.gameObject.SetActive(value: true);
			_E000(BlockedSprite, duration);
		}

		public void ShowBan()
		{
			LeftTimeText.gameObject.SetActive(value: false);
			_E000(BannedSprite);
		}

		public void Hide(float duration = -1f)
		{
			if (duration <= 0f)
			{
				Group.alpha = 0f;
				_talkingAnimation?.Rewind();
			}
			else
			{
				Group.DOFade(0f, duration);
				_talkingAnimation?.SmoothRewind();
			}
		}

		private void _E000(Sprite sprite, float duration = -1f)
		{
			_E003();
			StaticImage.sprite = sprite;
			StaticImage.ChangeImageAlpha(1f);
			Group.DOFade(1f, duration);
		}

		private void _E001(bool detected)
		{
			Group.DOKill();
			float duration = TweenDuration * 2f;
			if (detected)
			{
				Group.alpha = 0f;
				Group.DOFade(1f, duration).SetEase(TweenEase).SetLoops(-1, LoopType.Yoyo);
			}
			else
			{
				Group.DOFade(0f, duration).SetEase(TweenEase);
			}
		}

		private Sequence _E002()
		{
			TalkingStates[0].ChangeImageAlpha(1f);
			if (_talkingAnimation != null)
			{
				return _talkingAnimation;
			}
			_talkingAnimation = DOTween.Sequence();
			for (int i = 1; i < TalkingStates.Length; i++)
			{
				Image image = TalkingStates[i];
				image.ChangeImageAlpha(0f);
				_talkingAnimation.Append((i > 0) ? image.DOFade(1f, TweenDuration).SetEase(TweenEase) : image.DOFade(1f, 0.1f));
			}
			_talkingAnimation.SetLoops(-1, LoopType.Yoyo);
			return _talkingAnimation;
		}

		private void _E003()
		{
			if (_talkingAnimation != null)
			{
				TalkingStates[0].ChangeImageAlpha(0f);
				_talkingAnimation?.Rewind();
				return;
			}
			Image[] talkingStates = TalkingStates;
			for (int i = 0; i < talkingStates.Length; i++)
			{
				talkingStates[i].ChangeImageAlpha(0f);
			}
		}

		public void Clear()
		{
			_talkingAnimation?.Kill();
			_talkingAnimation = null;
			Group.DOKill();
			_talkDetectionBinding?.Invoke();
			_talkDetectionBinding = null;
		}
	}

	[SerializeField]
	private StatusContainer _status;

	[SerializeField]
	private BanContainer _ban;

	public float ShowHideDuration = 0.5f;

	private _E7B3 _E0A3;

	private int _E0A4;

	public void Show(_E7B3 controller)
	{
		if (controller == null)
		{
			HideGameObject();
			return;
		}
		_E0A3 = controller;
		UI.BindState(_E0A3.Status, _E000);
		ShowGameObject();
	}

	private void Update()
	{
		if (_E0A3 != null)
		{
			EVoipControllerStatus value = _E0A3.Status.Value;
			if ((uint)(value - 2) <= 3u)
			{
				_E001(force: false);
			}
		}
	}

	public override void Close()
	{
		base.Close();
		_status.Clear();
		_E0A3 = null;
	}

	private void _E000(EVoipControllerStatus status)
	{
		_status.Group.DOKill();
		_E0A4 = -1;
		switch (status)
		{
		case EVoipControllerStatus.Off:
		case EVoipControllerStatus.MicrophoneFail:
			_status.StartTalkDetection(_E0A3);
			_ban.Hide();
			break;
		case EVoipControllerStatus.Banned:
			_E001(force: true);
			_status.ShowBan();
			_ban.Show();
			break;
		case EVoipControllerStatus.Ready:
			_status.Hide(ShowHideDuration);
			_ban.Hide();
			break;
		case EVoipControllerStatus.Talking:
			_E001(force: true);
			_status.StartTalkingAnimation(ShowHideDuration);
			_ban.Hide();
			break;
		case EVoipControllerStatus.Limited:
		case EVoipControllerStatus.Blocked:
			_E001(force: true);
			_status.ShowBlock(ShowHideDuration);
			_ban.Hide();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(250587), status, null);
		}
	}

	private void _E001(bool force)
	{
		TimeSpan timeToNextStatus = _E0A3.TimeToNextStatus;
		if (force || _E0A4 != timeToNextStatus.Seconds)
		{
			if (_E0A3.Status.Value == EVoipControllerStatus.Banned)
			{
				_ban.LeftTimeText.text = timeToNextStatus.TimeLeftShortFormat();
			}
			else
			{
				_status.LeftTimeText.text = timeToNextStatus.TotalSeconds.ToString(_ED3E._E000(250578));
			}
			_E0A4 = timeToNextStatus.Seconds;
		}
	}
}
