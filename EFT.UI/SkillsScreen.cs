using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class SkillsScreen : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Profile profile;

		public SkillsScreen _003C_003E4__this;

		public _E9C4 healthController;

		internal void _E000(SkillList x)
		{
			x.Show(profile.Skills, _003C_003E4__this._sortMethod, _003C_003E4__this._filterMethod, healthController);
		}

		internal void _E001(SkillThumbs x)
		{
			x.Show(profile.Skills, _003C_003E4__this._sortMethod, _003C_003E4__this._filterMethod, healthController);
		}
	}

	[SerializeField]
	private SkillList _skillList;

	[SerializeField]
	private SkillThumbs _skillThumbs;

	[SerializeField]
	private DropDownBox _sortMethod;

	[SerializeField]
	private DropDownBox _filterMethod;

	[SerializeField]
	private Tab _listTab;

	[SerializeField]
	private Tab _thumbsTab;

	private _EC67 _E1AB;

	private void Awake()
	{
		_E1AB = new _EC67(new Tab[2] { _listTab, _thumbsTab }, _listTab);
	}

	public void Show(Profile profile, _E9C4 healthController)
	{
		ShowGameObject();
		_sortMethod._E002();
		_filterMethod._E002();
		_sortMethod.Show(new string[3]
		{
			_ED3E._E000(30808).Localized(),
			_ED3E._E000(258244).Localized(),
			_ED3E._E000(258297).Localized()
		});
		_sortMethod.UpdateValue(0);
		_filterMethod.Show(new string[1] { _ED3E._E000(258285).Localized() }.Concat(((IReadOnlyList<string>)Enum.GetNames(typeof(ESkillClass))).Localized(EStringCase.None)).ToArray());
		_filterMethod.UpdateValue(0);
		_listTab.Init(new _EC66<SkillList>(_skillList, delegate(SkillList x)
		{
			x.Show(profile.Skills, _sortMethod, _filterMethod, healthController);
		}));
		_thumbsTab.Init(new _EC66<SkillThumbs>(_skillThumbs, delegate(SkillThumbs x)
		{
			x.Show(profile.Skills, _sortMethod, _filterMethod, healthController);
		}));
		_E1AB.Show(null);
	}

	public override void Close()
	{
		base.Close();
		_E1AB.TryHide();
		_sortMethod.Hide();
		_filterMethod.Hide();
	}
}
