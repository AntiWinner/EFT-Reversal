namespace EFT.InventoryLogic;

public class KeycardComponent : _EB19
{
	public readonly _E9DF Template;

	public readonly KeyComponent Key;

	public KeycardComponent(Item item, _E9DF template)
		: base(item)
	{
		Key = item.GetItemComponent<KeyComponent>();
		Template = template;
	}
}
