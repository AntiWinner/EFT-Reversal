using UnityEngine;

public class BouncingObjectLauncher : MonoBehaviour
{
	[SerializeField]
	private LayerMask _castMask;

	[SerializeField]
	private int _maxCastCount;

	[SerializeField]
	private float _deltaTimeStep;

	[SerializeField]
	private float _jumpSpeed;

	[SerializeField]
	private float _playMult;

	[SerializeField]
	private float _randomReboundSpread;

	[SerializeField]
	private float _bounceSpeedMult;

	[SerializeField]
	private float _radius;

	private GameObject m__E000;

	public void Launch()
	{
		_E000(base.transform);
	}

	private void _E000(Transform from)
	{
		if (this.m__E000 != null)
		{
			Object.DestroyImmediate(this.m__E000);
		}
		this.m__E000 = new GameObject(_ED3E._E000(56098));
		this.m__E000.AddComponent<BouncingObject>().Init(from.position, from.forward * _jumpSpeed, _radius, _playMult, _castMask, _maxCastCount, _deltaTimeStep, _randomReboundSpread, _bounceSpeedMult, showDebug: true);
	}
}
