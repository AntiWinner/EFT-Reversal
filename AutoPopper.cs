using EFT.Ballistics;
using UnityEngine;

public class AutoPopper : MonoBehaviour
{
	public Transform MovingPart;

	public BallisticCollider Target;

	public Vector3 DefaultRotation;

	public Vector3 HitRotation = new Vector3(90f, 0f, 0f);

	public float HitTime = 3f;

	private float m__E000;

	private void Start()
	{
		Target.OnHitAction += _E000;
	}

	private void _E000(_EC23 obj)
	{
		MovingPart.localRotation = Quaternion.Euler(HitRotation);
		this.m__E000 = HitTime;
	}

	private void Update()
	{
		if (this.m__E000 > 0f)
		{
			this.m__E000 -= Time.deltaTime;
			if (this.m__E000 <= 0f)
			{
				MovingPart.localRotation = Quaternion.Euler(DefaultRotation);
			}
		}
	}
}
