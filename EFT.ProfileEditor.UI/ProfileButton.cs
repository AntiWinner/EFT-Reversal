using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using UnityEngine;

namespace EFT.ProfileEditor.UI;

public class ProfileButton : UIElement
{
	[SerializeField]
	private DefaultUIButton _inventoryButton;

	[SerializeField]
	private DefaultUIButton _dressRoomButton;

	private Action _E077;

	private Action _E078;

	private void Awake()
	{
		_inventoryButton.OnClick.AddListener(delegate
		{
			_E077();
		});
		_dressRoomButton.OnClick.AddListener(delegate
		{
			_E078();
		});
		_dressRoomButton.SetRawText(string.Empty, 42);
	}

	public void Show(string profileName, Action onInventoryClick, Action onDressRoomClick)
	{
		ShowGameObject();
		_inventoryButton.SetRawText(profileName, 42);
		_E077 = onInventoryClick;
		_E078 = onDressRoomClick;
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E077();
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E078();
	}
}
