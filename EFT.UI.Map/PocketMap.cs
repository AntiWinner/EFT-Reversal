using System.Collections;
using System.Collections.Generic;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Map;

public sealed class PocketMap : SimplePocketMap
{
	[SerializeField]
	private Scrollbar _scrollbar;

	[SerializeField]
	private Button _increaseScaleButton;

	[SerializeField]
	private Button _decreaseScaleButton;

	[SerializeField]
	[Range(0f, 1f)]
	private float _scaleStep = 0.1f;

	[SerializeField]
	private float _inputScaleSensitivity = 2f;

	[SerializeField]
	private float _animationSpeed = 0.2f;

	private IEnumerator _scaleAnimation;

	private float MinScale => Map?.ScaleMin ?? 0.1f;

	private float MaxScale => Map?.ScaleMax ?? 1f;

	protected override void Awake()
	{
		base.Awake();
		_scrollbar.onValueChanged.AddListener(SetScaleFromBar);
		_increaseScaleButton.onClick.AddListener(IncreaseScale);
		_decreaseScaleButton.onClick.AddListener(DecreaseScale);
	}

	public override void Show(MapComponent map)
	{
		base.Show(map);
		SetScale(MinScale);
	}

	private void Update()
	{
		float num = Input.GetAxis("Mouse ScrollWheel") * _inputScaleSensitivity;
		if (num != 0f)
		{
			float to = base.transform.localScale.x + num;
			AnimateScaleStart(base.transform.localScale.x, to, _animationSpeed);
		}
	}

	private void SetScale(float scale)
	{
		float x = base.transform.localScale.x;
		scale = Mathf.Clamp(scale, MinScale, MaxScale);
		base.transform.localScale = scale * Vector3.one;
		((RectTransform)base.transform).anchoredPosition *= scale / x;
		UpdateScrollbar();
		CheckTilesForUpdate();
	}

	private IEnumerator AnimateScale(float from, float to, float time)
	{
		float timePass = 0f;
		float progress = 0f;
		while (progress <= 1f)
		{
			progress = timePass / time;
			SetScale(Mathf.Lerp(from, to, progress));
			yield return null;
			timePass += Time.deltaTime;
		}
		_scaleAnimation = null;
	}

	private void AnimateScaleStart(float from, float to, float time)
	{
		AnimateScaleStop();
		_scaleAnimation = AnimateScale(from, to, time);
		StartCoroutine(_scaleAnimation);
	}

	private void AnimateScaleStop()
	{
		if (_scaleAnimation != null)
		{
			StopCoroutine(_scaleAnimation);
			_scaleAnimation = null;
		}
	}

	private void UpdateScrollbar()
	{
		_scrollbar.value = 1f - Mathf.InverseLerp(MinScale, MaxScale, base.transform.localScale.x);
	}

	private void SetScaleFromBar(float value)
	{
		if (_scaleAnimation == null)
		{
			AnimateScaleStop();
			SetScale(Mathf.Lerp(MinScale, MaxScale, 1f - value));
		}
	}

	private void IncreaseScale()
	{
		float to = base.transform.localScale.x + _scaleStep;
		AnimateScaleStart(base.transform.localScale.x, to, _animationSpeed);
	}

	private void DecreaseScale()
	{
		float to = base.transform.localScale.x - _scaleStep;
		AnimateScaleStart(base.transform.localScale.x, to, _animationSpeed);
	}

	public List<PocketMapTile> GetTiles()
	{
		return Tiles;
	}

	public Vector2 GetSize()
	{
		return MapSize;
	}
}
