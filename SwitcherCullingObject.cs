using EFT.Interactive;
using UnityEngine;

[ExecuteInEditMode]
public class SwitcherCullingObject : DisablerCullingObject
{
	[SerializeField]
	[Header("Синяя стрелка (ось Z) коллайдеров должна указывать на вход в помещение")]
	[Header("Первая проверка делается по радиусу, дальше по коллайдерам")]
	private float _firstCheckRadius = 10f;

	private bool _E008 = true;

	private ColliderReporter _E009;

	private Vector3 _E00A;

	private bool _E00B;

	public override bool HasEntered => _E00B;

	public override void ManualUpdate(float dt)
	{
		base.ManualUpdate(dt);
		if (_E008 && _E000())
		{
			_E008 = false;
		}
	}

	private bool _E000()
	{
		Camera camera = _E001();
		if (camera != null)
		{
			float num = Vector3.Distance(camera.transform.position, base.transform.position);
			_E00B = num < _firstCheckRadius;
			UpdateComponentsStatusOnUpdate();
			return true;
		}
		return false;
	}

	protected override void OnTriggerEnterEvent(ColliderReporter colliderReporter, Collider collider)
	{
		if (_E009 == null && IsRightCollider(collider))
		{
			Vector3 position = collider.transform.position;
			_E009 = colliderReporter;
			_E00A = position;
		}
	}

	protected override void OnTriggerExitEvent(ColliderReporter colliderReporter, Collider collider)
	{
		if (_E009 != null && IsRightCollider(collider))
		{
			bool flag = Vector3.Dot(collider.transform.position - _E00A, _E009.transform.forward) > 0f;
			if (flag != _E00B)
			{
				_E00B = flag;
				UpdateComponentsStatusOnUpdate();
			}
			_E009 = null;
		}
	}

	private Camera _E001()
	{
		Camera result = null;
		if (_E8A8.Instance.Camera != null)
		{
			return _E8A8.Instance.Camera;
		}
		return result;
	}
}
