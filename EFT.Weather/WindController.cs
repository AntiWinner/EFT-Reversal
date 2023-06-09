using UnityEngine;

namespace EFT.Weather;

public class WindController : MonoBehaviour
{
	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(218677));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(114692));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(218717));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(175287));

	public WiresController Wires;

	public float RainWindMultiplier = 1f;

	public float CloudWindMultiplier = 1f;

	public float BilboardTreesWindMultiplier = 0.4f;

	[SerializeField]
	private Vector2 defaultWindVector = new Vector2(0.05f, 0.1f);

	private Vector2 _E004 = Vector2.zero;

	private void OnValidate()
	{
		Shader.SetGlobalFloat(WindController.m__E000, BilboardTreesWindMultiplier);
	}

	private void Awake()
	{
		SetWind(defaultWindVector);
		Shader.SetGlobalFloat(WindController.m__E000, BilboardTreesWindMultiplier);
	}

	public void SetWind(Vector2 wind)
	{
		_E000(wind);
		Wires.Update(wind);
	}

	private void _E000(Vector2 wind)
	{
		float magnitude = wind.magnitude;
		Vector2 vector = wind.normalized;
		if (magnitude < 0.0001f)
		{
			vector = Vector2.one;
		}
		magnitude = Mathf.Clamp(magnitude, 0.0001f, 0.99999f);
		wind = vector * magnitude;
		Shader.SetGlobalVector(_E001, wind);
		Shader.SetGlobalVector(_E002, _E004);
		Shader.SetGlobalVector(_E003, new Vector4(vector.x, 0f, vector.y, magnitude));
		_E004 = wind;
	}

	private void OnDestroy()
	{
		_E000(defaultWindVector);
	}
}
