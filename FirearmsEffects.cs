using UnityEngine;

public class FirearmsEffects : MonoBehaviour
{
	protected MuzzleManager _muzzleManager;

	public GameObject[] Jets
	{
		get
		{
			if (!(_muzzleManager != null))
			{
				return new GameObject[0];
			}
			return _muzzleManager.MuzzleJets;
		}
	}

	public void StartFireEffects(bool isVisible, float sqrCameraDistance)
	{
		if (base.enabled && EFTHardSettings.Instance.SHOT_EFFECTS_ENABLED)
		{
			_muzzleManager.Shot(isVisible, sqrCameraDistance);
		}
	}

	public void StartLauncherEffects()
	{
		if (base.enabled && EFTHardSettings.Instance.SHOT_EFFECTS_ENABLED)
		{
			_muzzleManager.LauncherShot();
		}
	}

	public void Init(Transform hierarchy)
	{
		_muzzleManager = base.gameObject.transform.GetComponentInChildrenActiveIgnoreFirstLevel<MuzzleManager>();
		if (_muzzleManager != null)
		{
			_muzzleManager.Hierarchy = hierarchy;
			if (_muzzleManager.JetMaterial != null)
			{
				_muzzleManager.JetMaterial = new Material(_muzzleManager.JetMaterial);
			}
			else
			{
				Debug.LogError(_ED3E._E000(45336));
			}
		}
		UpdateMuzzle();
	}

	public void UpdateMuzzle()
	{
		if (base.enabled && !(_muzzleManager == null))
		{
			_muzzleManager._E004();
			_muzzleManager.UpdateJetsAndFumes();
		}
	}
}
