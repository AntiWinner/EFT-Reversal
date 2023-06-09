using System;
using System.Collections.Generic;
using EFT.Weather;
using Unity.Jobs;
using UnityEngine;

[ExecuteInEditMode]
public class CullingManager : MonoBehaviour
{
	public struct _E000
	{
		public _E42A CullingObject;

		public float CullingDistanceSqr;

		public _E001 VisibilityData;

		public bool JobVisibilityFlag;

		public void Reset()
		{
			CullingObject = null;
			CullingDistanceSqr = 0f;
			VisibilityData.Reset();
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(94963), CullingDistanceSqr, CullingObject.IsVisible) + VisibilityData.ToString();
		}
	}

	public struct _E001
	{
		public bool InOpticFructum;

		public bool InFpsFrustum;

		public bool IsCulledByDistance;

		public float CurrentCameraDistanceSqr;

		public bool IsAimingOn;

		public bool CullingByDistanceOnly;

		public bool IsObjectVisible()
		{
			if (CullingByDistanceOnly)
			{
				return !IsCulledByDistance;
			}
			if (IsCulledByDistance || !InFpsFrustum)
			{
				return InOpticFructum;
			}
			return true;
		}

		public void Reset()
		{
			InOpticFructum = false;
			InFpsFrustum = false;
			IsCulledByDistance = true;
			CurrentCameraDistanceSqr = 0f;
			IsAimingOn = false;
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(95032), InOpticFructum, InFpsFrustum, IsCulledByDistance, CurrentCameraDistanceSqr, IsAimingOn, CullingByDistanceOnly, IsObjectVisible());
		}
	}

	private sealed class _E002
	{
		public _E7DD CameraFrustrum = new _E7DD();

		public Camera CameraInstance;

		public bool IsCameraEnabled;

		public Vector3 CameraPosition;

		public bool IsOpticCamera;

		public void Clear()
		{
			CameraFrustrum = null;
			CameraInstance = null;
		}

		public bool UpdateParameters()
		{
			if (CameraInstance == null)
			{
				return false;
			}
			CameraPosition = CameraInstance.transform.position;
			IsCameraEnabled = CameraInstance.enabled;
			IsOpticCamera = _E8A8.Instance?.OpticCameraManager?.Camera == CameraInstance;
			CameraFrustrum.Update(CameraInstance.transform.position, CameraInstance.transform.rotation, CameraInstance.fieldOfView, CameraInstance.nearClipPlane, CameraInstance.farClipPlane, CameraInstance.aspect);
			return true;
		}
	}

	private struct _E003 : IJob
	{
		public static readonly Dictionary<int, _E002> JobParameters = new Dictionary<int, _E002>();

		public int JobId;

		public void Execute()
		{
			if (JobParameters != null && JobParameters.ContainsKey(JobId))
			{
				_E000(JobParameters[JobId]);
			}
		}

		private void _E000(_E002 parameters)
		{
			try
			{
				if (Instance == null || Instance.m__E006 == null || Instance.m__E004 == null || Instance.m__E005 == null || !parameters.IsCameraEnabled)
				{
					return;
				}
				_E7DD cameraFrustrum = parameters.CameraFrustrum;
				_E000[] array = Instance.m__E006;
				BoundingSphere[] array2 = Instance.m__E004;
				List<int> list = Instance.m__E005;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					int num = list[i];
					if (array != null && array.Length > num)
					{
						float sqrMagnitude = (parameters.CameraPosition - array[num].CullingObject.SafeMultithreadedPosition).sqrMagnitude;
						bool flag = cameraFrustrum.IntersectsSphere(ref array2[num]);
						array[num].VisibilityData.InOpticFructum = array[num].VisibilityData.InOpticFructum || (flag && parameters.IsOpticCamera);
						array[num].VisibilityData.InFpsFrustum = flag;
						array[num].VisibilityData.CurrentCameraDistanceSqr = sqrMagnitude;
						array[num].CullingObject.SqrCameraDistance = sqrMagnitude;
						array[num].VisibilityData.IsCulledByDistance = sqrMagnitude > array[num].CullingDistanceSqr;
						array[num].JobVisibilityFlag = array[num].VisibilityData.IsObjectVisible() && array[num].CullingObject.IsAutocullVisible;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public static CullingManager _instance;

	[SerializeField]
	private Camera _debugCamera;

	private const int m__E000 = 4500;

	private int m__E001;

	private readonly Dictionary<Camera, CullingGroup> m__E002 = new Dictionary<Camera, CullingGroup>();

	private readonly Queue<int> m__E003 = new Queue<int>(4500);

	private readonly BoundingSphere[] m__E004 = new BoundingSphere[4500];

	private readonly List<int> m__E005 = new List<int>(4500);

	private readonly _E000[] m__E006 = new _E000[4500];

	private static readonly List<_E42A> m__E007 = new List<_E42A>();

	private Camera m__E008;

	private bool m__E009;

	private List<JobHandle> m__E00A;

	public static CullingManager Instance => _instance;

	private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogWarning(_ED3E._E000(94855), base.gameObject);
			if (Application.isPlaying)
			{
				Debug.LogError(_ED3E._E000(94855), base.gameObject);
				UnityEngine.Object.DestroyImmediate(_instance);
			}
		}
		_instance = this;
		_E000();
		_E001();
	}

	private void Update()
	{
		int count = this.m__E005.Count;
		for (int i = 0; i < count; i++)
		{
			int num = this.m__E005[i];
			this.m__E006[num].CullingObject.SetVisibility(this.m__E006[num].JobVisibilityFlag);
			this.m__E006[num].VisibilityData.InOpticFructum = false;
			if (this.m__E006[num].CullingObject.IsAutocullVisible)
			{
				this.m__E006[num].CullingObject.CustomUpdate();
			}
		}
		_E00A();
	}

	private void LateUpdate()
	{
		_E00B();
	}

	private void _E000()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E004));
		_E003();
		_E007();
	}

	private void _E001()
	{
		for (int i = 0; i < CullingManager.m__E007.Count; i++)
		{
			CullingManager.m__E007[i].Register();
		}
		CullingManager.m__E007.Clear();
	}

	public int Register(_E42A cullableObject)
	{
		int num;
		if (this.m__E003.Count > 0)
		{
			num = this.m__E003.Dequeue();
		}
		else
		{
			num = this.m__E001;
			this.m__E001++;
			foreach (KeyValuePair<Camera, CullingGroup> item in this.m__E002)
			{
				item.Value.SetBoundingSphereCount(this.m__E001);
			}
		}
		this.m__E005.Add(num);
		UpdateSphere(cullableObject, num);
		_E002(cullableObject, num);
		return num;
	}

	public static void AddEarlyObject(_E42A cullableObject)
	{
		if (!CullingManager.m__E007.Contains(cullableObject))
		{
			CullingManager.m__E007.Add(cullableObject);
		}
	}

	public static void RemoveEarlyObject(_E42A cullableObject)
	{
		CullingManager.m__E007.Remove(cullableObject);
	}

	private void _E002(_E42A cullableObject, int index)
	{
		this.m__E006[index].CullingDistanceSqr = cullableObject.CullDistanceSqr;
		this.m__E006[index].CullingObject = cullableObject;
		this.m__E006[index].VisibilityData.CullingByDistanceOnly = cullableObject.CullByDistanceOnly;
		if (this.m__E002.Count == 0)
		{
			this.m__E006[index].CullingObject.SetVisibility(isVisible: false);
			return;
		}
		foreach (KeyValuePair<Camera, CullingGroup> item in this.m__E002)
		{
			Camera key = item.Key;
			_E7DD cameraFrustrum = CullingManager._E003.JobParameters[key.GetInstanceID()].CameraFrustrum;
			if (!(key == null) && key.enabled && key.gameObject.activeInHierarchy)
			{
				_ = item.Value;
				bool flag = key.name.Contains(_ED3E._E000(94881));
				float num = Vector3.SqrMagnitude(key.transform.position - this.m__E006[index].CullingObject.ClearTransformPosition);
				bool flag2 = cameraFrustrum.IntersectsSphere(ref this.m__E004[index]);
				this.m__E006[index].VisibilityData.IsCulledByDistance = !flag && num > this.m__E006[index].CullingDistanceSqr;
				if (flag)
				{
					this.m__E006[index].VisibilityData.InOpticFructum = flag2;
				}
				else
				{
					this.m__E006[index].VisibilityData.InFpsFrustum = flag2;
				}
				this.m__E006[index].CullingObject.SetVisibility(this.m__E006[index].VisibilityData.IsObjectVisible() && this.m__E006[index].CullingObject.IsAutocullVisible);
				if (key == _E8A8.Instance.Camera)
				{
					this.m__E006[index].CullingObject.SqrCameraDistance = num;
				}
			}
		}
	}

	public void Unregister(_E42A o)
	{
		if (CullingManager.m__E007.Contains(o))
		{
			CullingManager.m__E007.Remove(o);
		}
		if (!this.m__E003.Contains(o.Index))
		{
			this.m__E003.Enqueue(o.Index);
		}
		this.m__E006[o.Index].Reset();
		this.m__E005.Remove(o.Index);
	}

	private void _E003()
	{
		this.m__E003.Clear();
		this.m__E005.Clear();
		_E006();
		Array.Clear(this.m__E004, 0, this.m__E004.Length);
		Array.Clear(this.m__E006, 0, this.m__E006.Length);
		this.m__E008 = null;
		this.m__E001 = 0;
	}

	public void Reload()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E004));
		_E003();
		_E000();
	}

	private void _E004(Camera cam)
	{
		if (this.m__E009 || cam == null)
		{
			return;
		}
		bool flag = cam == _E8A8.Instance.Camera;
		bool flag2 = false;
		bool flag3 = false;
		if (WeatherController.Instance != null && WeatherController.Instance.PlayerCamera != null)
		{
			flag3 = cam == WeatherController.Instance.PlayerCamera;
		}
		bool flag4 = cam == _debugCamera;
		if (flag3 || flag || flag4 || flag2)
		{
			if (flag2)
			{
				this.m__E008 = cam;
			}
			if (!this.m__E002.ContainsKey(cam))
			{
				CullingGroup cullingGroup = new CullingGroup();
				cullingGroup.targetCamera = cam;
				cullingGroup.SetBoundingSpheres(this.m__E004);
				cullingGroup.SetBoundingSphereCount(this.m__E001);
				cullingGroup.enabled = false;
				_E008(cam);
				this.m__E002.Add(cam, cullingGroup);
			}
		}
	}

	private void _E005(Camera cam, CullingGroup cullingGroup, bool isOpticCam)
	{
		for (int i = 0; i < this.m__E005.Count; i++)
		{
			int num = this.m__E005[i];
			bool isVisible = this.m__E006[num].CullingObject.IsVisible;
			this.m__E006[num].VisibilityData.InOpticFructum &= this.m__E008 != null && this.m__E008.enabled;
			float num2 = Vector3.SqrMagnitude(cam.transform.position - this.m__E006[num].CullingObject.ClearTransformPosition);
			bool flag = cullingGroup.IsVisible(num);
			if (isOpticCam)
			{
				this.m__E006[num].VisibilityData.InOpticFructum = flag;
			}
			else
			{
				this.m__E006[num].VisibilityData.InFpsFrustum = flag;
				this.m__E006[num].VisibilityData.CurrentCameraDistanceSqr = num2;
				this.m__E006[num].CullingObject.SqrCameraDistance = num2;
			}
			this.m__E006[num].VisibilityData.IsCulledByDistance = !isOpticCam && num2 > this.m__E006[num].CullingDistanceSqr;
			bool flag2 = this.m__E006[num].VisibilityData.IsObjectVisible() && this.m__E006[num].CullingObject.IsAutocullVisible;
			if (isVisible != flag2)
			{
				this.m__E006[num].CullingObject.SetVisibility(flag2);
			}
		}
	}

	public float GetCameraDistanceSqr(int index)
	{
		if (this.m__E006 == null)
		{
			return 0f;
		}
		return this.m__E006[index].VisibilityData.CurrentCameraDistanceSqr;
	}

	public bool IsOpticEnabled()
	{
		if (this.m__E008 != null)
		{
			return this.m__E008.enabled;
		}
		return false;
	}

	public void UpdateSphere(_E42A cullingObject, int index)
	{
		this.m__E004[index].radius = cullingObject.Radius;
		this.m__E004[index].position = cullingObject.Position;
	}

	public void UpdateSphere(_E42A cullingObject)
	{
		UpdateSphere(cullingObject, cullingObject.Index);
	}

	private void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E004));
		_E006();
	}

	private void OnDestroy()
	{
		_E003();
		_instance = null;
	}

	private void _E006()
	{
		foreach (KeyValuePair<Camera, CullingGroup> item in this.m__E002)
		{
			if (item.Value != null)
			{
				item.Value.Dispose();
			}
		}
		this.m__E002.Clear();
		_E009();
	}

	public void LockState(bool lockState)
	{
		this.m__E009 = lockState;
	}

	public void ForceEnable(bool enable)
	{
		for (int i = 0; i < this.m__E005.Count; i++)
		{
			int num = this.m__E005[i];
			this.m__E006[num].CullingObject.SetVisibility(enable);
		}
	}

	private void _E007()
	{
		this.m__E00A = new List<JobHandle>();
	}

	private void _E008(Camera cam)
	{
		_E002 obj = new _E002();
		obj.CameraInstance = cam;
		CullingManager._E003.JobParameters[cam.GetInstanceID()] = obj;
		Debug.Log(string.Format(_ED3E._E000(94943), cam.gameObject.name, CullingManager._E003.JobParameters.Count));
	}

	private void _E009()
	{
		if (CullingManager._E003.JobParameters == null)
		{
			return;
		}
		foreach (KeyValuePair<int, _E002> jobParameter in CullingManager._E003.JobParameters)
		{
			jobParameter.Value.Clear();
		}
		CullingManager._E003.JobParameters.Clear();
	}

	private void _E00A()
	{
		if (CullingManager._E003.JobParameters == null || CullingManager._E003.JobParameters.Count == 0)
		{
			return;
		}
		_E003 jobData = default(_E003);
		foreach (KeyValuePair<int, _E002> jobParameter in CullingManager._E003.JobParameters)
		{
			if (jobParameter.Value.UpdateParameters())
			{
				jobData.JobId = jobParameter.Value.CameraInstance.GetInstanceID();
				this.m__E00A.Add(jobData.Schedule());
			}
		}
		JobHandle.ScheduleBatchedJobs();
	}

	private void _E00B()
	{
		foreach (JobHandle item in this.m__E00A)
		{
			item.Complete();
		}
		this.m__E00A.Clear();
	}

	private void _E00C()
	{
		int count = this.m__E005.Count;
		for (int i = 0; i < count; i++)
		{
			_ = this.m__E005[i];
		}
	}
}
