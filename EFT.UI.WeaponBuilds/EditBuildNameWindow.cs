using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI.WeaponBuilds;

public sealed class EditBuildNameWindow : Window<_EC7C>
{
	[SerializeField]
	private ValidationInputField _inputField;

	[SerializeField]
	private DefaultUIButton _saveButtonSpawner;

	private Action<string> m__E000;

	private Action m__E001;

	protected override void Awake()
	{
		base.Awake();
		_saveButtonSpawner.OnClick.AddListener(delegate
		{
			this.m__E000?.Invoke(_inputField.text);
			Close();
		});
	}

	public void Show(string savedName, Action<string> onNameSet)
	{
		Show(Close);
		this.m__E000 = onNameSet;
		_inputField.text = savedName;
		this.m__E001 = _inputField.HasError.Bind(_E000);
	}

	private void _E000(bool hasError)
	{
		_saveButtonSpawner.Interactable = !hasError;
	}

	public override void Close()
	{
		base.Close();
		this.m__E001?.Invoke();
		this.m__E001 = null;
	}

	[CompilerGenerated]
	private void _E001()
	{
		this.m__E000?.Invoke(_inputField.text);
		Close();
	}
}
