using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Streamer : MonoBehaviour
{
	[Serializable]
	public class Scene
	{
		public string Name;

		public string Path;

		public Chunk[] Chunks;

		public const float DefaultDistance = 150f;

		public float Distance = 150f;

		public GameObject[] LODs;

		[HideInInspector]
		public Bounds Bounds;

		[HideInInspector]
		public LoadingState State = LoadingState.NotLoaded;

		[HideInInspector]
		public GameObject[] LODsInstances;

		public Vector2 Distances { get; set; }

		public Scene()
		{
		}

		public Scene(UnityEngine.SceneManagement.Scene scene, Bounds bounds, float distance)
		{
			Name = scene.name;
			Path = scene.path;
			Bounds = bounds;
			Distance = distance;
		}
	}

	[Serializable]
	public class Chunk
	{
		public string Name;

		public string Path;

		[HideInInspector]
		public LoadingState State;

		public Chunk()
		{
		}

		public Chunk(UnityEngine.SceneManagement.Scene scene)
		{
			Name = scene.name;
			Path = scene.path;
		}
	}

	public enum LoadingState
	{
		Loading,
		Loaded,
		NotLoaded
	}

	public Transform Player;

	[Range(0f, 0.5f)]
	public float Backlash = 0.3f;

	public int MaxPerformanceRaiting = 8000;

	public Scene[] Scenes;

	private readonly Queue<Scene> m__E000 = new Queue<Scene>();

	private readonly HashSet<Scene> m__E001 = new HashSet<Scene>();

	private bool m__E002;

	private void Awake()
	{
		Scene[] scenes = Scenes;
		foreach (Scene scene in scenes)
		{
			scene.Distances = new Vector2(1f - Backlash, 1f + Backlash) * scene.Distance;
			scene.State = LoadingState.NotLoaded;
			scene.LODsInstances = new GameObject[scene.LODs.Length];
			for (int j = 0; j < scene.LODs.Length; j++)
			{
				scene.LODsInstances[j] = UnityEngine.Object.Instantiate(scene.LODs[j]);
				scene.LODsInstances[j].transform.SetParent(base.transform, worldPositionStays: true);
				scene.LODsInstances[j].SetActive(value: true);
			}
		}
	}

	private void Update()
	{
		_E8A8.Instance.SetCamera(Camera.main);
		if (_E8A8.Instance.Camera != null)
		{
			Vector3 position = _E8A8.Instance.Camera.transform.position;
			Scene[] scenes = Scenes;
			foreach (Scene scene in scenes)
			{
				if (scene.State == LoadingState.Loading)
				{
					continue;
				}
				Bounds bounds = scene.Bounds;
				float val = Math.Abs(bounds.center.x - position.x) - bounds.extents.x;
				float val2 = Math.Abs(bounds.center.z - position.z) - bounds.extents.z;
				float num = Math.Max(val, 0f) + Math.Max(val2, 0f);
				if (scene.State == LoadingState.Loaded)
				{
					if (num > scene.Distances.y)
					{
						StartCoroutine(_E000(scene));
					}
				}
				else if (num < scene.Distances.x)
				{
					_E001(scene);
				}
			}
		}
		if (this.m__E000.Count > 0 && !this.m__E002)
		{
			Scene scene2 = this.m__E000.Dequeue();
			StartCoroutine(_E002(scene2));
		}
	}

	private IEnumerator _E000(Scene scene)
	{
		Debug.Log(_ED3E._E000(97405) + scene.Name);
		if (scene.State == LoadingState.NotLoaded)
		{
			yield break;
		}
		if (scene.State == LoadingState.Loading)
		{
			this.m__E001.Add(scene);
			yield break;
		}
		AsyncOperation[] source = scene.Chunks.Select((Chunk chunk) => SceneManager.UnloadSceneAsync(chunk.Name)).ToArray();
		while (source.Any((AsyncOperation operation) => !operation.isDone))
		{
			yield return null;
		}
		scene.State = LoadingState.NotLoaded;
		GameObject[] lODsInstances = scene.LODsInstances;
		for (int i = 0; i < lODsInstances.Length; i++)
		{
			lODsInstances[i].SetActive(value: true);
		}
	}

	private void _E001(Scene scene)
	{
		Debug.Log(_ED3E._E000(97362) + scene.Name);
		if (scene.State == LoadingState.NotLoaded)
		{
			this.m__E000.Enqueue(scene);
			scene.State = LoadingState.Loading;
		}
	}

	private IEnumerator _E002(Scene scene)
	{
		this.m__E002 = true;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		Chunk[] chunks = scene.Chunks;
		GameObject[] lODsInstances;
		foreach (Chunk chunk in chunks)
		{
			if (this.m__E001.Contains(scene))
			{
				Chunk[] chunks2 = scene.Chunks;
				foreach (Chunk chunk2 in chunks2)
				{
					yield return SceneManager.UnloadSceneAsync(chunk2.Name);
				}
				this.m__E001.Remove(scene);
				this.m__E002 = false;
				scene.State = LoadingState.NotLoaded;
				lODsInstances = scene.LODsInstances;
				for (int k = 0; k < lODsInstances.Length; k++)
				{
					lODsInstances[k].SetActive(value: true);
				}
				yield break;
			}
			yield return StartCoroutine(_E003(chunk));
		}
		lODsInstances = scene.LODsInstances;
		for (int k = 0; k < lODsInstances.Length; k++)
		{
			lODsInstances[k].SetActive(value: false);
		}
		if (this.m__E001.Contains(scene))
		{
			chunks = scene.Chunks;
			foreach (Chunk chunk3 in chunks)
			{
				yield return SceneManager.UnloadSceneAsync(chunk3.Name);
			}
			this.m__E001.Remove(scene);
			scene.State = LoadingState.NotLoaded;
			lODsInstances = scene.LODsInstances;
			for (int k = 0; k < lODsInstances.Length; k++)
			{
				lODsInstances[k].SetActive(value: true);
			}
		}
		else
		{
			scene.State = LoadingState.Loaded;
		}
		this.m__E002 = false;
	}

	private IEnumerator _E003(Chunk chunk)
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(chunk.Name, LoadSceneMode.Additive);
		asyncOperation.priority = 0;
		asyncOperation.allowSceneActivation = false;
		while (asyncOperation.progress < 0.9f)
		{
			yield return null;
		}
		yield return null;
		asyncOperation.allowSceneActivation = true;
		yield return asyncOperation;
		Debug.Log(_ED3E._E000(97398) + chunk.Name);
	}
}
