using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotInfoIcon : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Color _trueColor;

	[SerializeField]
	private Color _falseColor;

	[SerializeField]
	private List<Color> _additionalColors = new List<Color>();

	private bool _E000;

	private int _E001;

	public void SetBool(bool value)
	{
		_E000 = value;
		_icon.color = (value ? _trueColor : _falseColor);
	}

	public void SetInt(int value)
	{
		_E001 = value;
		_icon.color = _additionalColors[value % _additionalColors.Count];
	}

	public void SetColor(Color color)
	{
		_icon.color = color;
	}
}
