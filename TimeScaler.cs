using UnityEngine;

public class TimeScaler : MonoBehaviour
{
	public bool IgnoreFixedUpdates = true;

	private float _E000;

	public Vector2 TimeScaleRange = new Vector2(0.01f, 1f);

	[Range(0.01f, 1f)]
	public float TimeScale = 1f;

	private void Awake()
	{
		_E000 = Time.fixedDeltaTime;
	}
}
