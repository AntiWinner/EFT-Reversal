using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ColorView : MonoBehaviour
{
	[SerializeField]
	private GameObject _selectBorder;

	[SerializeField]
	private Image _colorImage;

	[CompilerGenerated]
	private EventHandler _E000;

	public Color Color => _colorImage.color;

	public event EventHandler OnSelected
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = _E000;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E000, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = _E000;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _E000, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public void Show(Color color)
	{
		_colorImage.color = color;
	}

	public void SelectionChanged(bool selected)
	{
		_selectBorder.SetActive(selected);
	}

	public void OnClick()
	{
		if (_E000 != null)
		{
			_E000(this, EventArgs.Empty);
		}
	}
}
