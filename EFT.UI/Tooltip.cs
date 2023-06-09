using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class Tooltip : UIElement
{
	[SerializeField]
	private RectTransform _mainTransform;

	[SerializeField]
	private RectTransform _boundsTransform;

	private Coroutine _E1AC;

	private Vector2 _E1AD;

	[CompilerGenerated]
	private bool _E1AE;

	public bool Displayed
	{
		[CompilerGenerated]
		get
		{
			return _E1AE;
		}
		[CompilerGenerated]
		private set
		{
			_E1AE = value;
		}
	}

	protected virtual void Awake()
	{
		if (_boundsTransform == null)
		{
			_boundsTransform = _mainTransform;
		}
	}

	protected virtual void Update()
	{
		_E000(Input.mousePosition);
	}

	protected void OnDisable()
	{
		Close();
	}

	protected void Show(Vector2 offset = default(Vector2), float delay = 0f)
	{
		_E1AD = offset;
		StaticManager.KillCoroutine(_E1AC);
		if (delay > 0f)
		{
			_E1AC = StaticManager.Instance.WaitSeconds(delay, Display);
		}
		else
		{
			Display();
		}
		Displayed = true;
	}

	public override void Display()
	{
		base.Display();
		_E000(Input.mousePosition);
	}

	public override void Close()
	{
		base.Close();
		if (_E1AC != null)
		{
			StaticManager.KillCoroutine(_E1AC);
		}
		_E1AC = null;
		Displayed = false;
	}

	protected void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && Displayed)
		{
			Close();
		}
	}

	private void _E000(Vector2 position)
	{
		_mainTransform.position = position + _E1AD.Multiply((Vector2)_mainTransform.lossyScale);
		_mainTransform.CorrectPositionResolution(_boundsTransform);
	}
}
