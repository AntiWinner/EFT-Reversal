using System;
using UnityEngine;

namespace EFT.UI;

public class TagColorsPanel : MonoBehaviour
{
	[SerializeField]
	private GameObject _cellTemplate;

	private ColorView[] m__E000;

	private Action<ColorView, int> _E001;

	public void Show(Color[] colors, int selectedIndex, Action<ColorView, int> OnSelected)
	{
		if (colors == null || colors.Length == 0)
		{
			return;
		}
		if (this.m__E000 != null)
		{
			for (int i = 0; i < this.m__E000.Length; i++)
			{
				this.m__E000[i].OnSelected -= _E000;
				if (i >= colors.Length)
				{
					UnityEngine.Object.Destroy(this.m__E000[i].gameObject);
				}
			}
			if (this.m__E000.Length != colors.Length)
			{
				Array.Resize(ref this.m__E000, colors.Length);
			}
		}
		else
		{
			this.m__E000 = new ColorView[colors.Length];
		}
		for (int j = 0; j < colors.Length; j++)
		{
			this.m__E000[j] = this.m__E000[j] ?? UnityEngine.Object.Instantiate(_cellTemplate, base.transform).GetComponent<ColorView>();
			this.m__E000[j].gameObject.SetActive(value: true);
			this.m__E000[j].Show(colors[j]);
			this.m__E000[j].SelectionChanged(j == selectedIndex);
			this.m__E000[j].OnSelected += _E000;
		}
		_E001 = OnSelected;
		if (_E001 != null)
		{
			_E001(this.m__E000[selectedIndex], selectedIndex);
		}
	}

	public void Hide()
	{
		for (int i = 0; i < this.m__E000.Length; i++)
		{
			this.m__E000[i].OnSelected -= _E000;
		}
	}

	private void _E000(object sender, EventArgs args)
	{
		ColorView colorView = sender as ColorView;
		if (!(colorView == null) && _E001 != null)
		{
			_E001(colorView, Array.IndexOf(this.m__E000, colorView));
		}
	}
}
