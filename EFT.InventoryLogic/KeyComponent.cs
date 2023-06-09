using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class KeyComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public KeyComponent _003C_003E4__this;

		public _E9DF template;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public int maximumUsages;

		public _E000 CS_0024_003C_003E8__locals1;

		internal string _E000()
		{
			return CS_0024_003C_003E8__locals1._003C_003E4__this._E000(maximumUsages);
		}

		internal string _E001()
		{
			int num = CS_0024_003C_003E8__locals1.template.MaximumNumberOfUsage - CS_0024_003C_003E8__locals1._003C_003E4__this.NumberOfUsages;
			bool flag = maximumUsages > 1 && num == 1;
			return string.Format(_ED3E._E000(182604), flag ? string.Format(RedColorFormat, num) : num.ToString(), CS_0024_003C_003E8__locals1.template.MaximumNumberOfUsage);
		}
	}

	private const string _E00B = "keycard_single";

	private const string _E00C = "keycard_reusable";

	private const string _E00D = "keycard_unlimited";

	[_E63C]
	public int NumberOfUsages;

	public readonly _E9DF Template;

	public static readonly string RedColorFormat = _ED3E._E000(215532);

	public static readonly string WhiteColorFormat = _ED3E._E000(215569);

	public static readonly string CyanColorFormat = _ED3E._E000(215606);

	public KeyComponent(Item item, _E9DF template)
		: base(item)
	{
		KeyComponent keyComponent = this;
		Template = template;
		int maximumUsages = template.MaximumNumberOfUsage;
		item.Attributes.Add(new _EB10(EItemAttributeId.Keys)
		{
			Name = EItemAttributeId.Keys.GetName(),
			StringValue = () => keyComponent._E000(maximumUsages),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		if (template.MaximumNumberOfUsage > 0)
		{
			item.Attributes.Add(new _EB10(EItemAttributeId.KeyUses)
			{
				Name = EItemAttributeId.KeyUses.GetName(),
				StringValue = delegate
				{
					int num = template.MaximumNumberOfUsage - keyComponent.NumberOfUsages;
					bool flag = maximumUsages > 1 && num == 1;
					return string.Format(_ED3E._E000(182604), flag ? string.Format(RedColorFormat, num) : num.ToString(), template.MaximumNumberOfUsage);
				},
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
	}

	private string _E000(int usage)
	{
		if (usage != 1)
		{
			if (usage <= 1)
			{
				return string.Format(CyanColorFormat, _ED3E._E000(215458).Localized());
			}
			return _ED3E._E000(215500).Localized();
		}
		return string.Format(RedColorFormat, _ED3E._E000(215549).Localized());
	}
}
