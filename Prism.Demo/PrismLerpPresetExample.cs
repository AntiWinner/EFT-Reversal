using Prism.Utils;
using UnityEngine;

namespace Prism.Demo;

public class PrismLerpPresetExample : MonoBehaviour
{
	private PrismEffects _E000;

	[Header("This script lerps a PRISM preset based on distance to the camera")]
	[Header("NOTE: This is an example script, you should only have one per scene")]
	public float maxDistance = 500f;

	public float t;

	[Tooltip("The Prism-Preset to lerp TO")]
	public PrismPreset target;

	public AnimationCurve cameraDistanceCurve;

	private void Start()
	{
		_E000 = Camera.main.GetComponent<PrismEffects>();
		if (!_E000)
		{
			Debug.LogWarning(_ED3E._E000(71757));
			base.enabled = false;
		}
	}

	private void Update()
	{
		t = Vector3.Distance(base.transform.position, Camera.main.transform.position);
		t = cameraDistanceCurve.Evaluate(t);
		_E000.LerpToPreset(target, t);
	}
}
