using DG.Tweening;
using UnityEngine;

public class Materials : MonoBehaviour
{
	public GameObject target;

	public Color toColor;

	private Tween _E000;

	private Tween _E001;

	private Tween _E002;

	private void Start()
	{
		Material material = target.GetComponent<Renderer>().material;
		_E000 = material.DOColor(toColor, 1f).SetLoops(-1, LoopType.Yoyo).Pause();
		_E001 = material.DOColor(new Color(0f, 0f, 0f, 0f), _ED3E._E000(40467), 1f).SetLoops(-1, LoopType.Yoyo).Pause();
		_E002 = material.DOOffset(new Vector2(1f, 1f), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental)
			.Pause();
	}

	public void ToggleColor()
	{
		_E000.TogglePause();
	}

	public void ToggleEmission()
	{
		_E001.TogglePause();
	}

	public void ToggleOffset()
	{
		_E002.TogglePause();
	}
}
