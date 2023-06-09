using System.Collections.Generic;
using EFT;
using EFT.InventoryLogic;
using UnityEngine;

public class TacticalComboVisualController : MonoBehaviour
{
	public const string DISABLED_TRANSFORM_NAME = "mode_000";

	public const string LIGHT_BEAM_TRANSFORM_NAME = "mode_";

	private readonly List<Transform> _E000 = new List<Transform>();

	public LightComponent LightMod;

	private Transform _E001;

	[SerializeField]
	private float _shadowNearPlaneShift = 0.05f;

	private Light[] _E002;

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		_E002 = GetComponentsInChildren<Light>(includeInactive: true);
		if (_E000.Count != 0)
		{
			return;
		}
		List<Transform> list = _E38B._E004(base.transform, _ED3E._E000(63888), false);
		if (list == null || list.Count == 0)
		{
			Debug.LogErrorFormat(base.transform, _ED3E._E000(63886), _ED3E._E000(63888), base.transform.name);
			return;
		}
		_E001 = list.Find((Transform mod) => mod.name == _ED3E._E000(63948));
		if (_E001 == null)
		{
			Debug.LogErrorFormat(base.transform, _ED3E._E000(63909), _ED3E._E000(63948), base.transform.name);
		}
		else
		{
			list.Remove(_E001);
			_E000.AddRange(list);
		}
	}

	private void OnEnable()
	{
		if (_E000.Count > 0)
		{
			UpdateBeams();
		}
	}

	public void UpdateBeams()
	{
		if (LightMod != null)
		{
			if (_E001 != null)
			{
				_E001.gameObject.SetActive(!LightMod.IsActive);
			}
			for (int i = 0; i < _E000.Count; i++)
			{
				_E000[i].gameObject.SetActive(LightMod.IsActive && LightMod.SelectedMode == i);
			}
		}
	}

	public void SetPointOfView(EPointOfView pointOfView)
	{
	}
}
