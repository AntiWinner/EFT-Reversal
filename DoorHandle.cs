using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class DoorHandle : MonoBehaviour
{
	private const int _E000 = 14;

	public Quaternion OpenRotation;

	public Quaternion DefaultRotation;

	public AnimationCurve OpenAnimation;

	public AnimationCurve LockedAnimation;

	public bool pos;

	public Vector3 OpenPosition;

	public Vector3 DefaultPosition;

	public AudioClip[] DownSound;

	public AudioClip[] UpSound;

	private float _E001;

	[ContextMenu("Open Position")]
	public void OpenPos()
	{
		base.transform.localRotation = Quaternion.Slerp(DefaultRotation, OpenRotation, 1f);
		if (pos)
		{
			base.transform.localPosition = Vector3.Lerp(DefaultPosition, OpenPosition, 1f);
		}
	}

	[ContextMenu("Default Position")]
	public void DefPos()
	{
		base.transform.localRotation = Quaternion.Slerp(OpenRotation, DefaultRotation, 1f);
		if (pos)
		{
			base.transform.localPosition = Vector3.Lerp(OpenPosition, DefaultPosition, 1f);
		}
	}

	public void Awake()
	{
		if (pos)
		{
			base.transform.localPosition = DefaultPosition;
			base.transform.localRotation = DefaultRotation;
		}
	}

	public void Open()
	{
		StartCoroutine(OpenCoroutine());
	}

	public IEnumerator OpenCoroutine()
	{
		if (DownSound != null && DownSound.Length != 0)
		{
			AudioClip audioClip = DownSound[Random.Range(0, DownSound.Length)];
			if (audioClip == null)
			{
				Debug.LogError(string.Format(_ED3E._E000(55380), base.transform.parent));
			}
			else
			{
				MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, audioClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, 14, 0.8f, EOcclusionTest.Fast);
			}
		}
		bool flag = false;
		if (OpenAnimation == null || OpenAnimation.length == 0)
		{
			Debug.LogErrorFormat(this, _ED3E._E000(55405) + base.transform.GetFullPath(withSceneName: true));
			yield break;
		}
		float num = 0f;
		do
		{
			num += Time.deltaTime;
			float t = OpenAnimation.Evaluate(num);
			base.transform.localRotation = Quaternion.Slerp(DefaultRotation, OpenRotation, t);
			if (pos)
			{
				base.transform.localPosition = Vector3.Lerp(DefaultPosition, OpenPosition, t);
			}
			if (UpSound.Length != 0 && !flag && num > OpenAnimation[OpenAnimation.length - 1].time / 2f)
			{
				AudioClip audioClip2 = UpSound[Random.Range(0, UpSound.Length)];
				if (audioClip2 == null)
				{
					Debug.LogError(string.Format(_ED3E._E000(55380), base.transform.parent));
				}
				else
				{
					MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, audioClip2, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, 14, 0.8f, EOcclusionTest.Fast);
				}
				flag = true;
			}
			yield return null;
		}
		while (num < OpenAnimation[OpenAnimation.length - 1].time);
	}
}
