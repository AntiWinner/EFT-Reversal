using UnityEngine;

namespace EFT.Interactive;

public class PhotoSensorGroup : MonoBehaviour
{
	public float SunHeihgtThreshold = 0.1f;

	public Turnable[] Items;

	private bool _E000;

	private void Awake()
	{
		if (!(TOD_Sky.Instance == null))
		{
			_E000 = TOD_Sky.Instance.SunDirection.y > SunHeihgtThreshold;
		}
	}

	private void Update()
	{
		if (TOD_Sky.Instance == null)
		{
			return;
		}
		bool flag = TOD_Sky.Instance.SunDirection.y < SunHeihgtThreshold;
		if (flag != _E000)
		{
			_E000 = flag;
			Turnable[] items = Items;
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Switch(flag ? Turnable.EState.On : Turnable.EState.Off);
			}
		}
	}
}
