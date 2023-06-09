using UnityEngine;

namespace EFT.Particles;

public class BasicParticleSystemMediator : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem[] _particleSystems;

	private int _E000;

	public void Emit(Vector3 position, Quaternion rotation)
	{
		if (_particleSystems.Length != 0)
		{
			ParticleSystem obj = _particleSystems[_E000];
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.Play(withChildren: true);
			_E000 = (_E000 + 1) % _particleSystems.Length;
		}
	}
}
