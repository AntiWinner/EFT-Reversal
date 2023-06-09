using System.Runtime.CompilerServices;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class LoadingScreen : UIScreen
{
	[SerializeField]
	private float _rotationAngle = 60f;

	[SerializeField]
	private float _rotationSpeed = 1f;

	[SerializeField]
	private Image _hideoutImage;

	[SerializeField]
	private RectTransform _hideoutIcon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Animation _hideoutLogoAnimation;

	[SerializeField]
	private CanvasGroup _centerLogo;

	[SerializeField]
	private CanvasGroup _topLeftLogo;

	private Coroutine _E059;

	public void Show()
	{
		ShowGameObject();
		_hideoutImage.SetNativeSize();
		_background.enabled = true;
		_background.color = new Color(0f, 0f, 0f, 255f);
		_hideoutIcon.rotation = Quaternion.Euler(0f, 0f, 240f);
		_centerLogo.gameObject.SetActive(value: true);
		_centerLogo.SetUnlockStatus(value: true);
		_topLeftLogo.SetUnlockStatus(value: false);
		_hideoutIcon.DORotate(new Vector3(0f, 0f, _rotationAngle), _rotationSpeed).SetLoops(-1, LoopType.Incremental).SetUpdate(isIndependentUpdate: true)
			.SetUpdate(UpdateType.Late);
	}

	public void LoadComplete()
	{
		_hideoutLogoAnimation.Play();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		SoftHide(_centerLogo, delegate
		{
			base.Close();
		});
	}

	[CompilerGenerated]
	private void _E000()
	{
		base.Close();
	}
}
