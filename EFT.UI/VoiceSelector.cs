using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;

namespace EFT.UI;

public sealed class VoiceSelector : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public EPlayerSide side;

		internal bool _E000(_EBE9 x)
		{
			return x.Side.Contains(side);
		}
	}

	[SerializeField]
	private DropDownBox _dropdown;

	[SerializeField]
	private DefaultUIButton _button;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private CanvasGroup _changeVoiceCanvas;

	private readonly List<(string name, string localizationKey)> m__E000 = new List<(string, string)>();

	private int m__E001;

	private int m__E002;

	private _EC5F m__E003;

	private Func<string, Task<bool>> m__E004;

	private void Awake()
	{
		_button.OnClick.AddListener(delegate
		{
			_E002().HandleExceptions();
		});
	}

	public void Initialize(string currentVoice, EPlayerSide side, Func<string, Task<bool>> onVoiceChanged)
	{
		this.m__E004 = onVoiceChanged;
		_EBE9[] array = (from x in Singleton<_E60E>.Instance.Voices
			where x.Side.Contains(side)
			orderby x.Name
			select x).ToArray();
		this.m__E001 = 0;
		this.m__E000.Clear();
		_EBE9[] array2 = array;
		foreach (_EBE9 obj in array2)
		{
			this.m__E000.Add((obj.Name, obj.NameLocalizationKey));
			if (obj.Prefab == currentVoice)
			{
				this.m__E001 = this.m__E000.Count - 1;
			}
		}
	}

	public void Show(bool profileIsFree)
	{
		this.m__E003 = new _EC5F();
		_dropdown.Bind(_E000);
		_changeVoiceCanvas.SetUnlockStatus(profileIsFree);
		_dropdown.Show(() => this.m__E000.Select(((string name, string localizationKey) x) => x.localizationKey.Localized()).ToArray());
		_dropdown.UpdateValue(this.m__E001, sendCallback: false);
		_E001(currentSelected: true);
	}

	private void _E000(int currentIndex)
	{
		this.m__E002 = currentIndex;
		_E001(this.m__E002 == this.m__E001);
		this.m__E003.PlayVoice(this.m__E000[this.m__E002].name).HandleExceptions();
	}

	private void _E001(bool currentSelected)
	{
		_button.Interactable = !currentSelected;
	}

	private async Task _E002()
	{
		if (this.m__E004 != null)
		{
			_E003(status: true);
			int num = this.m__E002;
			if (await this.m__E004(this.m__E000[num].name))
			{
				this.m__E001 = num;
				_E001(this.m__E002 == this.m__E001);
			}
			_E003(status: false);
		}
	}

	private void _E003(bool status)
	{
		_button.gameObject.SetActive(!status);
		_loader.SetActive(status);
	}

	public void Hide()
	{
		_dropdown.Hide();
		this.m__E003 = null;
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E002().HandleExceptions();
	}

	[CompilerGenerated]
	private IEnumerable<string> _E005()
	{
		return this.m__E000.Select(((string name, string localizationKey) x) => x.localizationKey.Localized()).ToArray();
	}
}
