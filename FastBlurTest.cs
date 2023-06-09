using UnityEngine;

public class FastBlurTest : MonoBehaviour
{
	public float Power = 5f;

	private FastBlur _E000;

	private void Start()
	{
		_E000 = GetComponent<FastBlur>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			_E000.Hit(Power);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			_E000.Die();
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			_E000.Reset();
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			Power = Mathf.Min(Power + 1f, 10f);
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			Power = Mathf.Max(Power - 1f, 1f);
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 100f, 30f), _ED3E._E000(43634) + Power);
	}
}
