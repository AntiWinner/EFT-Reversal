using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class MasteringScreen : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MasteringScreen _003C_003E4__this;

		public Profile profile;

		public _EAE6 inventoryController;

		internal void _E000(_E750 newMastering)
		{
			_003C_003E4__this.Close();
			_003C_003E4__this.Show(profile, inventoryController);
		}

		internal void _E001(MasteringList x)
		{
			x.Show(_003C_003E4__this._E194, _003C_003E4__this._sortMethod, _003C_003E4__this._filterMethod, _003C_003E4__this._searchWeaponInputField, inventoryController);
		}

		internal void _E002(MasteringThumbs x)
		{
			x.Show(_003C_003E4__this._E194, _003C_003E4__this._sortMethod, _003C_003E4__this._filterMethod, _003C_003E4__this._searchWeaponInputField, inventoryController);
		}

		internal void _E003()
		{
			_003C_003E4__this._E193.TryHide().HandleExceptions();
		}

		internal void _E004()
		{
			profile.Skills.NewWeaponMastered -= delegate
			{
				_003C_003E4__this.Close();
				_003C_003E4__this.Show(profile, inventoryController);
			};
		}

		internal void _E005()
		{
			_003C_003E4__this._sortMethod.Hide();
			_003C_003E4__this._filterMethod.Hide();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public KeyValuePair<string, _E750> mastering;

		internal KeyValuePair<string, _E750> _E000(string templateId)
		{
			return new KeyValuePair<string, _E750>(templateId, mastering.Value);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public int index;

		internal bool _E000(KeyValuePair<string, _E750> skill)
		{
			return GetItemTemplateText(skill.Key) == ItemTemplates[index - 1];
		}
	}

	[SerializeField]
	private MasteringList _masteringList;

	[SerializeField]
	private MasteringThumbs _masteringThumbs;

	[SerializeField]
	private DropDownBox _sortMethod;

	[SerializeField]
	private DropDownBox _filterMethod;

	[SerializeField]
	private TMP_InputField _searchWeaponInputField;

	[SerializeField]
	private Tab _listTab;

	[SerializeField]
	private Tab _thumbsTab;

	private static List<string> _E192 = new List<string>();

	private _EC67 _E193;

	private KeyValuePair<string, _E750>[] _E194;

	public static List<string> ItemTemplates => _E192.Select(GetItemTemplateText).Distinct().ToList();

	private void Awake()
	{
		_E193 = new _EC67(new Tab[2] { _listTab, _thumbsTab }, _listTab);
	}

	public void Show(Profile profile, _EAE6 inventoryController)
	{
		ShowGameObject();
		_E194 = profile.Skills.Mastering.SelectMany((KeyValuePair<string, _E750> mastering) => mastering.Value.MasteringGroup.Templates.Select((string templateId) => new KeyValuePair<string, _E750>(templateId, mastering.Value))).ToArray();
		_E192 = new List<string>(_E194.Select((KeyValuePair<string, _E750> _) => _.Key));
		_sortMethod._E002();
		_filterMethod._E002();
		_sortMethod.Show(new string[3]
		{
			_ED3E._E000(30808).Localized(),
			_ED3E._E000(258244).Localized(),
			_ED3E._E000(258297).Localized()
		});
		_sortMethod.UpdateValue(0);
		string[] array = new string[1] { _ED3E._E000(258285).Localized() }.Concat(ItemTemplates).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			ref string reference = ref array[i];
			reference = reference + _ED3E._E000(54246) + _E000(i) + _ED3E._E000(27308);
		}
		_filterMethod.Show(array);
		_filterMethod.UpdateValue(0);
		_listTab.Init(new _EC66<MasteringList>(_masteringList, delegate(MasteringList x)
		{
			x.Show(_E194, _sortMethod, _filterMethod, _searchWeaponInputField, inventoryController);
		}));
		_thumbsTab.Init(new _EC66<MasteringThumbs>(_masteringThumbs, delegate(MasteringThumbs x)
		{
			x.Show(_E194, _sortMethod, _filterMethod, _searchWeaponInputField, inventoryController);
		}));
		profile.Skills.NewWeaponMastered += delegate
		{
			Close();
			Show(profile, inventoryController);
		};
		UI.AddDisposable(delegate
		{
			_E193.TryHide().HandleExceptions();
		});
		UI.AddDisposable(delegate
		{
			profile.Skills.NewWeaponMastered -= delegate
			{
				Close();
				Show(profile, inventoryController);
			};
		});
		UI.AddDisposable(delegate
		{
			_sortMethod.Hide();
			_filterMethod.Hide();
		});
		_E193.Show(null);
	}

	private int _E000(int index)
	{
		if (index != 0)
		{
			return _E194.Count((KeyValuePair<string, _E750> skill) => GetItemTemplateText(skill.Key) == ItemTemplates[index - 1]);
		}
		return _E194.Length;
	}

	public static string GetItemTemplateText(string id)
	{
		return (Singleton<_E63B>.Instance.ItemTemplates[id] as WeaponTemplate).MasteringLocalizationKey.Localized();
	}
}
