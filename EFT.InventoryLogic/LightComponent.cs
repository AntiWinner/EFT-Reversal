namespace EFT.InventoryLogic;

public class LightComponent : _EB19
{
	[_E63C]
	public bool IsActive;

	private readonly _E9E1 _E000;

	private int _E00E;

	[_E63C]
	public int SelectedMode
	{
		get
		{
			return _E00E;
		}
		set
		{
			if (_E000.ModesCount > 0)
			{
				_E00E = value % _E000.ModesCount;
			}
		}
	}

	public LightComponent(Item item, _E9E1 template)
		: base(item)
	{
		_E000 = template;
	}
}
