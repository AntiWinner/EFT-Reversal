using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SplashScreenPanel : UIElement
{
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private Sprite[] _sprites;

	[SerializeField]
	private CanvasGroup _textCanvasGroup;

	[SerializeField]
	private CanvasGroup _imageCanvasGroup;

	[SerializeField]
	private Image _splashScreen;

	private DateTime _E0DA;

	private bool _E189;

	private const float _E18A = 0.3f;

	private const float _E18B = 3f;

	private Sprite _E000
	{
		get
		{
			int num = UnityEngine.Random.Range(0, _sprites.Length - 1);
			return _sprites[num];
		}
	}

	private void Awake()
	{
		ShowGameObject();
		UnityEngine.Object.DontDestroyOnLoad(_canvas.gameObject);
		_E0DA = _E5AD.Now;
		_splashScreen.color = Color.white;
		_splashScreen.sprite = this._E000;
		_textCanvasGroup.alpha = 0f;
		_imageCanvasGroup.alpha = 0f;
		StartCoroutine(_E000(_textCanvasGroup));
		this.WaitSeconds(0.3f, delegate
		{
			StartCoroutine(_E000(_imageCanvasGroup));
		});
	}

	private void Update()
	{
		if ((_E5AD.Now - _E0DA).TotalSeconds >= 3.0 && _E189)
		{
			_E189 = false;
			StartCoroutine(_E001(_imageCanvasGroup));
			this.WaitSeconds(0.3f, delegate
			{
				StartCoroutine(_E001(_textCanvasGroup, Close));
			});
		}
	}

	private IEnumerator _E000(CanvasGroup canvasGroup)
	{
		while (canvasGroup.alpha < 1f)
		{
			canvasGroup.alpha += Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 1f;
	}

	private IEnumerator _E001(CanvasGroup canvasGroup, Action callback = null)
	{
		while (canvasGroup.alpha > 0f)
		{
			canvasGroup.alpha -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0f;
		callback?.Invoke();
	}

	public void Hide()
	{
		_E189 = true;
	}

	public override void Close()
	{
		StopAllCoroutines();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		StartCoroutine(_E000(_imageCanvasGroup));
	}

	[CompilerGenerated]
	private void _E003()
	{
		StartCoroutine(_E001(_textCanvasGroup, Close));
	}
}
