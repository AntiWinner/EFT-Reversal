using EFT.InventoryLogic;
using UnityEngine;

public class SightModVisualControllers : MonoBehaviour
{
	private ScopePrefabCache _E000;

	private SightComponent _E001;

	public SightComponent SightMod
	{
		get
		{
			return _E001;
		}
		set
		{
			_E001 = value;
			UpdateSightMode();
		}
	}

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		if (_E000 == null)
		{
			_E000 = GetComponent<ScopePrefabCache>();
		}
	}

	private void OnEnable()
	{
		if (_E000 != null)
		{
			UpdateSightMode(setupZeroModeAnyway: true);
		}
	}

	public void UpdateSightMode(bool setupZeroModeAnyway = false)
	{
		if (SightMod == null || _E000 == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < SightMod.ScopesCount && i < SightMod.ScopesSelectedModes.Length; i++)
		{
			if (SightMod.GetScopeModesCount(i) == _E000.ModesCount)
			{
				_E000.SetMode(SightMod.ScopesSelectedModes[i]);
				flag = true;
				break;
			}
		}
		if (!flag && setupZeroModeAnyway)
		{
			_E000.SetMode(0);
		}
	}
}
