using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class UICanvasScaler : UIElement
{
	[SerializeField]
	private CanvasScaler _canvasScaler;

	private Action _E215;

	private void OnEnable()
	{
		ShowGameObject();
		_E215 = (Singleton<_E7DE>.Instantiated ? Singleton<_E7DE>.Instance.Graphics.Settings.DisplaySettings.Bind(delegate
		{
			_E000();
		}) : null);
	}

	private void _E000()
	{
		if (base.gameObject.activeSelf)
		{
			_E001();
			_EC74.Register(_canvasScaler);
		}
	}

	private void _E001()
	{
		_EC74.Unregister(_canvasScaler);
	}

	private void OnDisable()
	{
		_E215?.Invoke();
		_E215 = null;
		Close();
	}

	public override void Close()
	{
		_E001();
	}

	[CompilerGenerated]
	private void _E002(_E7E4 x)
	{
		_E000();
	}
}
