using System;
using System.Collections.Generic;
using EFT.UI;
using UnityEngine;

public sealed class TabSizeController : UIElement
{
	[SerializeField]
	private float _gap;

	private List<TabElement> _E005 = new List<TabElement>();

	public void UpdateLayout()
	{
		float num = 0f;
		foreach (TabElement item in _E005)
		{
			num = Math.Max(num, item.Width);
		}
		foreach (TabElement item2 in _E005)
		{
			item2.SetWidth(num, _gap);
		}
	}

	private void Awake()
	{
		foreach (Transform item in base.transform)
		{
			TabElement component = item.gameObject.GetComponent<TabElement>();
			if (!(component == null))
			{
				_E005.Add(component);
				UI.AddDisposable(component.OnLayoutUpdatedEvent.Subscribe(UpdateLayout));
			}
		}
	}

	private void OnDestroy()
	{
		Dispose();
	}
}
