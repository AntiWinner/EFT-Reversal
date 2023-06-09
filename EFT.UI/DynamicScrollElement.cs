using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public sealed class DynamicScrollElement : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _one;

	[CompilerGenerated]
	private int _E1BD;

	public new RectTransform Transform => base.transform as RectTransform;

	public float Height => 70f;

	public int ScrollIndex
	{
		[CompilerGenerated]
		get
		{
			return _E1BD;
		}
		[CompilerGenerated]
		set
		{
			_E1BD = value;
		}
	}

	public void Perform(int index)
	{
		_one.text = index.ToString();
	}
}
