using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextPanelWithManualLayout : MonoBehaviour
{
	private static char[] m__E000 = new char[1] { '\n' };

	[SerializeField]
	private Text _dataLineTemplate;

	private readonly List<Text> _E001 = new List<Text>();

	private int _E002;

	private RectTransform _E003;

	public RectTransform RectTransform => _E003 ?? (_E003 = base.transform.RectTransform());

	private float _E004 => (float)_E002 * _dataLineTemplate.rectTransform.sizeDelta.y * _dataLineTemplate.rectTransform.localScale.y;

	public void Start()
	{
		_dataLineTemplate.gameObject.SetActive(value: false);
	}

	public void UpdateText(string text)
	{
		string[] array = text?.Split(DebugTextPanelWithManualLayout.m__E000, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
		if (array != null)
		{
			_E002 = array.Length;
			for (int i = 0; i < array.Length; i++)
			{
				Text text2 = ((_E001.Count <= i) ? _E000() : _E001[i]);
				if (!text2.gameObject.activeSelf)
				{
					text2.gameObject.SetActive(value: true);
				}
				text2.text = array[i];
			}
		}
		else
		{
			_E002 = 0;
		}
		for (int j = _E002; j < _E001.Count; j++)
		{
			if (_E001[j].gameObject.activeSelf)
			{
				_E001[j].gameObject.SetActive(value: false);
			}
		}
		RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _E004);
	}

	private Text _E000()
	{
		Text text = UnityEngine.Object.Instantiate(_dataLineTemplate, base.transform);
		text.rectTransform.anchoredPosition = text.rectTransform.anchoredPosition.WithY((float)(-1 * _E001.Count) * text.rectTransform.sizeDelta.y * text.rectTransform.localScale.y);
		_E001.Add(text);
		return text;
	}

	private void OnDestroy()
	{
		_E001.Clear();
	}
}
