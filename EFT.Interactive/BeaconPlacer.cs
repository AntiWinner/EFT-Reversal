using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Interactive;

public class BeaconPlacer : MonoBehaviour
{
	[CompilerGenerated]
	private bool m__E000;

	private BoxCollider _E001;

	private DateTime _E002;

	private readonly List<PlaceItemTrigger> _E003 = new List<PlaceItemTrigger>();

	private const float _E004 = 0.2f;

	public bool Available
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	private BoxCollider _E005
	{
		get
		{
			if (!_E001)
			{
				return _E001 = GetComponentInChildren<BoxCollider>();
			}
			return _E001;
		}
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		DateTime dateTime = _E002.AddSeconds(0.20000000298023224);
		if (!(utcNow < dateTime) && !(utcNow == _E002))
		{
			_E002 = utcNow;
			_E000();
		}
	}

	private void _E000()
	{
		List<PlaceItemTrigger> list = new List<PlaceItemTrigger>();
		Collider[] array = Physics.OverlapBox(_E005.center + _E005.transform.position, _E005.size / 2f, _E005.transform.rotation, LayerMask.GetMask(_ED3E._E000(25347)));
		for (int i = 0; i < array.Length; i++)
		{
			PlaceItemTrigger component = array[i].GetComponent<PlaceItemTrigger>();
			if (component != null)
			{
				if (!_E003.Contains(component))
				{
					Debug.Log(_ED3E._E000(205845) + component.name);
					_E003.Add(component);
					Available = true;
				}
				list.Add(component);
			}
		}
		for (int num = _E003.Count - 1; num >= 0; num--)
		{
			PlaceItemTrigger placeItemTrigger = _E003[num];
			if (!list.Contains(placeItemTrigger))
			{
				Debug.Log(_ED3E._E000(205883) + placeItemTrigger.name);
				_E003.Remove(placeItemTrigger);
				Available = false;
			}
		}
	}
}
