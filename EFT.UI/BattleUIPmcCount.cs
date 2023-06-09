using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class BattleUIPmcCount : BattleUIPanel
{
	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _template;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private Sprite _onSprite;

	[SerializeField]
	private Sprite _offSprite;

	private readonly List<Image> _E0A1 = new List<Image>();

	private int _E0A2;

	public void Show(int maxCount, int current)
	{
		ShowGameObject();
		_E000();
		_E0A2 = 0;
		for (int i = 0; i < maxCount; i++)
		{
			Image image = Object.Instantiate(_template, _container);
			image.sprite = _offSprite;
			image.gameObject.SetActive(value: true);
			_E0A1.Add(image);
		}
		for (int j = 0; j < current; j++)
		{
			Enter();
		}
		_backgroundImage.enabled = maxCount > 0;
	}

	public void Enter()
	{
		_E0A1[_E0A2].gameObject.SetActive(value: true);
		_E0A1[_E0A2].sprite = _onSprite;
		_E0A2++;
	}

	public void Exit()
	{
		_E0A2--;
		_E0A1[_E0A2].gameObject.SetActive(value: false);
		_E0A1[_E0A2].sprite = _offSprite;
	}

	private void _E000()
	{
		foreach (Image item in _E0A1)
		{
			Object.Destroy(item.gameObject);
		}
		_E0A1.Clear();
	}

	public override void Close()
	{
		_E000();
		_backgroundImage.enabled = false;
		base.Close();
	}
}
