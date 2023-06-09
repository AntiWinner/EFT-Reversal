using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QueuePlayer : MonoBehaviour
{
	private List<AudioSource> m__E000;

	private AudioSource m__E001;

	public _ECEF<AudioClip> Queue = new _ECEF<AudioClip>();

	private Coroutine m__E002;

	public float QueueRefreshTime = 1f;

	private void Awake()
	{
		this.m__E000 = new List<AudioSource>(base.gameObject.GetComponentsInChildren<AudioSource>());
		this.m__E000.ForEach(delegate(AudioSource x)
		{
			x.playOnAwake = false;
		});
		if (this.m__E000.Count > 0)
		{
			this.m__E001 = this.m__E000[0];
			Queue.ItemAdded += _E000;
			Queue.ItemRemoved += _E000;
			Queue.AllItemsRemoved += delegate
			{
				_E000(null);
			};
		}
		else
		{
			base.enabled = false;
		}
	}

	private void _E000(AudioClip _)
	{
		if (this.m__E002 == null && Queue.Count > 0)
		{
			this.m__E002 = StartCoroutine(_E001());
		}
	}

	private IEnumerator _E001()
	{
		while (Queue.Count > 0)
		{
			if (this.m__E000.TrueForAll((AudioSource x) => !x.isPlaying))
			{
				this.m__E000.ForEach(delegate(AudioSource x)
				{
					x.gameObject.SetActive(value: false);
				});
				yield return null;
				AudioClip clip = Queue[0];
				Queue.RemoveAt(0);
				_E002(clip);
			}
			yield return new WaitForSeconds(QueueRefreshTime);
		}
		this.m__E002 = null;
	}

	public void Play(AudioClip clip)
	{
		Queue.Add(clip);
	}

	private void _E002(AudioClip clip)
	{
		foreach (AudioSource item in this.m__E000)
		{
			item.gameObject.SetActive(value: true);
			item.clip = clip;
			item.Play();
		}
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E000(null);
	}
}
