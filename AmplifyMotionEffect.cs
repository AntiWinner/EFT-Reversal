using UnityEngine;

[AddComponentMenu("Image Effects/Amplify Motion")]
[RequireComponent(typeof(Camera))]
public class AmplifyMotionEffect : AmplifyMotionEffectBase
{
	public new static AmplifyMotionEffect FirstInstance => (AmplifyMotionEffect)AmplifyMotionEffectBase.FirstInstance;

	public new static AmplifyMotionEffect Instance => (AmplifyMotionEffect)AmplifyMotionEffectBase.Instance;
}
