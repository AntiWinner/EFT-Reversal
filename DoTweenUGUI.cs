using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoTweenUGUI : MonoBehaviour
{
	public Image dotweenLogo;

	public Image circleOutline;

	public Text text;

	public Text relativeText;

	public Text scrambledText;

	public Slider slider;

	private void Start()
	{
		dotweenLogo.DOFade(0f, 1.5f).SetAutoKill(autoKillOnCompletion: false).Pause();
		circleOutline.DOColor(_E000(), 1.5f).SetEase(Ease.Linear).Pause();
		circleOutline.DOFillAmount(0f, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo)
			.OnStepComplete(delegate
			{
				circleOutline.fillClockwise = !circleOutline.fillClockwise;
				circleOutline.DOColor(_E000(), 1.5f).SetEase(Ease.Linear);
			})
			.Pause();
		text.DOText(_ED3E._E000(40337), 2f).SetEase(Ease.Linear).SetAutoKill(autoKillOnCompletion: false)
			.Pause();
		relativeText.DOText(_ED3E._E000(40361), 2f).SetRelative().SetEase(Ease.Linear)
			.SetAutoKill(autoKillOnCompletion: false)
			.Pause();
		scrambledText.DOText(_ED3E._E000(40440), 2f, richTextEnabled: true, ScrambleMode.All).SetEase(Ease.Linear).SetAutoKill(autoKillOnCompletion: false)
			.Pause();
		slider.DOValue(1f, 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo)
			.Pause();
	}

	public void StartTweens()
	{
		DOTween.PlayAll();
	}

	public void RestartTweens()
	{
		DOTween.RestartAll();
	}

	private Color _E000()
	{
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
	}

	[CompilerGenerated]
	private void _E001()
	{
		circleOutline.fillClockwise = !circleOutline.fillClockwise;
		circleOutline.DOColor(_E000(), 1.5f).SetEase(Ease.Linear);
	}
}
