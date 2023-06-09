using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Ballistics;

public sealed class BallisticCalculatorPrewarmer : MonoBehaviour
{
	private const float _E000 = 1f;

	private const float _E001 = 1f;

	private BallisticsCalculator _E002;

	public _EC26 SimulateShot()
	{
		_E002 = BallisticsCalculator.Create(base.gameObject, 0, delegate
		{
		});
		_EC26 obj = _E002.CreateShot(new _EA12(string.Empty, new AmmoTemplate
		{
			casingSounds = string.Empty
		}), base.transform.position, base.transform.forward, 1, null, null);
		_E002.PreWarmerSimulateShotNoPool(obj, 1f, 1f);
		return obj;
	}

	private void OnDestroy()
	{
		Object.Destroy(_E002);
		_E002 = null;
	}
}
