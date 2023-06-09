using System.Linq;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public class CompositeArmorComponent : ArmorComponent
{
	[CanBeNull]
	public readonly TogglableComponent Togglable;

	private readonly _E9D3[] _E009;

	private readonly _E9D0 _E000;

	public EHeadSegment[] HeadSegments => _E000.HeadSegments;

	public CompositeArmorComponent(Item item, _E9D0 template, RepairableComponent repairable, TogglableComponent togglable, BuffComponent buff)
		: base(item, template, repairable, buff)
	{
		_E000 = template;
		Togglable = togglable;
		_E009 = new _E9D3[template.HeadSegments.Length];
		for (int i = 0; i < template.HeadSegments.Length; i++)
		{
			_E009[i] = _E9D3.Values[template.HeadSegments[i]];
		}
	}

	public override bool ShotMatches(EBodyPart bodyPartType, int pitch = 60, int yaw = 0)
	{
		if (!base.ArmorZone.Contains(bodyPartType))
		{
			return false;
		}
		if (Togglable != null && !Togglable.On)
		{
			return false;
		}
		if (_E009.Length == 0)
		{
			return true;
		}
		_E9D3[] array = _E009;
		for (int i = 0; i < array.Length; i++)
		{
			_E9D3 obj = array[i];
			if ((float)yaw >= obj.Yaw.x && (float)yaw <= obj.Yaw.y && (float)pitch >= obj.Pitch.x && (float)pitch <= obj.Pitch.y)
			{
				return true;
			}
		}
		return false;
	}
}
