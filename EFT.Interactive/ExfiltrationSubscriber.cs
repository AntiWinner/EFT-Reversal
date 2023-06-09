using System.Collections;
using UnityEngine;

namespace EFT.Interactive;

public class ExfiltrationSubscriber : MonoBehaviour
{
	public ExfiltrationPoint Subscribee;

	public float Delay;

	public virtual void Start()
	{
		if (Subscribee != null)
		{
			Subscribee.OnStatusChanged += OnStatusChangedHandler;
		}
	}

	protected virtual void OnStatusChangedHandler(ExfiltrationPoint point, EExfiltrationStatus prevStatus)
	{
		if (point.Status == EExfiltrationStatus.NotPresent)
		{
			if (prevStatus == EExfiltrationStatus.Pending)
			{
				Play(force: true);
			}
			else
			{
				StartCoroutine(Wait());
			}
		}
	}

	public IEnumerator Wait()
	{
		yield return new WaitForSeconds(Delay);
		Play();
	}

	public virtual void Play(bool force = false)
	{
	}

	public virtual void Dispose()
	{
		if (Subscribee != null)
		{
			Subscribee.OnStatusChanged -= OnStatusChangedHandler;
		}
	}
}
