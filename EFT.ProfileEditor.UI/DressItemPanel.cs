using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.UI;
using UnityEngine;

namespace EFT.ProfileEditor.UI;

public sealed class DressItemPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public DressItemPanel _003C_003E4__this;

		public List<ResourceKey> options;

		public Action<_EBDF, ResourceKey> onPartChanged;

		internal void _E000(int arg)
		{
			if (_003C_003E4__this._E076.TryGetValue(options[arg], out var value))
			{
				onPartChanged(value, options[arg]);
			}
		}
	}

	[SerializeField]
	private DropDownBox _dropDown;

	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	private Dictionary<ResourceKey, _EBDF> _E076;

	public void Show(EBodyModelPart part, ResourceKey current, _EBDF[] clothings, Action<_EBDF, ResourceKey> onPartChanged)
	{
		ShowGameObject();
		_caption.text = string.Concat(part, _ED3E._E000(30697));
		_E60E instance = Singleton<_E60E>.Instance;
		List<ResourceKey> options = new List<ResourceKey>(clothings.Length);
		_E076 = new Dictionary<ResourceKey, _EBDF>();
		foreach (_EBDF obj in clothings)
		{
			ResourceKey bundle = instance.GetBundle(obj.Id);
			if (!(bundle == null))
			{
				options.Add(bundle);
				_E076[bundle] = obj;
			}
		}
		_dropDown.Show(options.Select((ResourceKey x) => x.path.Substring(x.path.LastIndexOf('/') + 1).Replace(_ED3E._E000(108691), "")).ToArray());
		_dropDown.UpdateValue(Mathf.Max(options.IndexOf(current), 0));
		_dropDown.Bind(delegate(int arg)
		{
			if (_E076.TryGetValue(options[arg], out var value))
			{
				onPartChanged(value, options[arg]);
			}
		});
	}
}
