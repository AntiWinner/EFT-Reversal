using UnityEngine;

public class SingleFlareController : MonoBehaviour
{
	public MultiFlareLight Light;

	public float Angle;

	public float YShift = 0.075f;

	public float StartWidth;

	private float m__E000;

	private float _E001;

	private const float _E002 = 2f;

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			_E000();
		}
	}

	private void OnDrawGizmosSelected()
	{
	}

	private void _E000()
	{
		if (Light == null)
		{
			Light = GetComponent<MultiFlareLight>();
		}
		if (Light == null)
		{
			if (Application.isPlaying)
			{
				Debug.LogError(base.gameObject.name + _ED3E._E000(88302));
			}
		}
		else
		{
			this.m__E000 = Angle - 1f;
			_E001 = 1f / Angle;
		}
	}

	private void Awake()
	{
		_E000();
	}

	private void OnEnable()
	{
		Light.CurrentAlpha = 0f;
	}

	private void OnDisable()
	{
		Light.CurrentAlpha = 0f;
	}

	private void Update()
	{
		Camera camera = _E8A8.Instance.Camera;
		if (camera == null)
		{
			camera = Camera.main;
		}
		if (!(camera == null) && !(Light == null))
		{
			float num = Mathf.Clamp01(Vector3.Dot(Vector3.Normalize(camera.transform.position + camera.transform.up * YShift - base.transform.position), base.transform.forward) + this.m__E000) * _E001;
			float num2 = Light.DefaultAlpha * num * num;
			float deltaTime = Time.deltaTime;
			if (Light.CurrentAlpha < num2)
			{
				Light.CurrentAlpha += deltaTime * 2f;
				Light.CurrentAlpha = Mathf.Clamp(Light.CurrentAlpha, 0f, num2);
			}
			else
			{
				Light.CurrentAlpha -= deltaTime * 2f;
				Light.CurrentAlpha = Mathf.Clamp(Light.CurrentAlpha, num2, 1f);
			}
		}
	}
}
