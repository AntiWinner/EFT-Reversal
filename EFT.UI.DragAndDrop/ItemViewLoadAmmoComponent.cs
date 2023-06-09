using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class ItemViewLoadAmmoComponent : SerializedMonoBehaviour
{
	[SerializeField]
	private Image _loadMagazineLoader;

	[SerializeField]
	private GameObject _loadStopImage;

	[SerializeField]
	private ProgressSpinner _loadProgressSpinner;

	private float m__E000;

	private double _E001;

	private double _E002;

	private double _E003;

	private CancellationTokenSource _E004;

	public void Show(float oneAmmoDuration, int ammoTotal, int ammoDone = 0)
	{
		base.gameObject.SetActive(value: true);
		if (!(_loadMagazineLoader == null) && !(_loadProgressSpinner == null))
		{
			_loadMagazineLoader.gameObject.SetActive(ammoTotal > 1);
			_loadProgressSpinner.transform.rotation = Quaternion.identity;
			_loadProgressSpinner.Show();
			_loadStopImage.SetActive(value: false);
			this.m__E000 = oneAmmoDuration;
			_E003 = this.m__E000 * (float)ammoDone;
			_E001 = (double)(this.m__E000 * (float)ammoTotal) + _E003;
			_E002 = _E5AD.UtcNowUnix - _E003;
			_E000().HandleExceptions();
		}
	}

	private async Task _E000()
	{
		_E004 = new CancellationTokenSource();
		float num2;
		do
		{
			double num = _E5AD.UtcNowUnix - _E002;
			num2 = Mathf.Clamp01((float)(num / _E001));
			double num3 = num % (double)this.m__E000 / (double)this.m__E000;
			_loadMagazineLoader.transform.rotation = Quaternion.Euler(0f, 0f, (float)(-360.0 * num3));
			_loadProgressSpinner.SetProgress(num2);
			await Task.Yield();
		}
		while (!_E004.IsCancellationRequested && (num2 - 1f).Negative());
	}

	public void SetStopButtonStatus(bool active)
	{
		_loadMagazineLoader.color = (active ? Color.red : Color.white);
		_loadProgressSpinner.SetColor(active ? Color.red : Color.white);
		_loadStopImage.SetActive(active);
	}

	public void Destroy()
	{
		_E004?.Cancel();
		Object.Destroy(base.gameObject);
	}
}
