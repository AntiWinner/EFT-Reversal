using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Koenigz.PerfectCulling.EFT;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

namespace Koenigz.PerfectCulling;

[RequireComponent(typeof(Camera))]
public class PerfectCullingCamera : MonoBehaviour
{
	public static List<PerfectCullingCamera> AllCameras = new List<PerfectCullingCamera>();

	[CompilerGenerated]
	private bool m__E000 = true;

	[CompilerGenerated]
	private Vector3 m__E001;

	private static int m__E002 = -1;

	private static int m__E003 = -1;

	private static int m__E004 = -1;

	private Camera m__E005;

	private _E4A0<int> _E006 = new _E4A0<int>(4096);

	private _E4A0<int> _E007 = new _E4A0<int>(4096);

	private JobHandle? _E008;

	public bool VisualizeFrustumCulling
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public Vector3 ObservePosition
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	private void Awake()
	{
		this.m__E005 = GetComponent<Camera>();
	}

	private void OnEnable()
	{
		ObservePosition = base.transform.position;
		_E005();
		AllCameras.Add(this);
		RenderPipelineManager.beginCameraRendering += _E000;
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E004));
	}

	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= _E000;
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E004));
		_E005();
		AllCameras.Remove(this);
	}

	private void Update()
	{
		ObservePosition = base.transform.position;
	}

	private void _E000(ScriptableRenderContext context, Camera camera)
	{
		_E004(camera);
	}

	private void _E001(Camera camera)
	{
	}

	internal void _E002()
	{
		_E006 = new _E4A0<int>(4096);
		_E007 = new _E4A0<int>(4096);
	}

	internal void _E003()
	{
		JobHandle? jobHandle = _E008;
		JobHandle valueOrDefault = jobHandle.GetValueOrDefault();
		if (jobHandle.HasValue)
		{
			bool num = PerfectCullingCrossSceneSampler.Instance?.FinishJob(valueOrDefault) ?? true;
			_E008 = null;
			if (num)
			{
				PerfectCullingCamera.m__E003 = PerfectCullingCamera.m__E004;
			}
		}
		_E006.Clear();
		_E007.Clear();
		int frameHashNoOrientation = PerfectCullingCrossSceneVolume.GetFrameHashNoOrientation(ObservePosition, _E006, _E007);
		if (PerfectCullingCamera.m__E003 == frameHashNoOrientation)
		{
			return;
		}
		for (int i = 0; i < _E007.Count; i++)
		{
			PerfectCullingCrossSceneGroup.AllCrossGroups[_E007.Buffer[i]].counterMainThread++;
		}
		if (PerfectCullingCrossSceneSampler.Instance != null)
		{
			PerfectCullingCamera.m__E004 = frameHashNoOrientation;
			_E008 = PerfectCullingCrossSceneSampler.Instance.ScheduleJob(_E006, _E007);
			if (!_E008.HasValue)
			{
				PerfectCullingCamera.m__E003 = PerfectCullingCamera.m__E004;
			}
		}
		else
		{
			PerfectCullingCamera.m__E003 = frameHashNoOrientation;
			_E008 = null;
		}
	}

	private void _E004(Camera camera)
	{
		if (camera != this.m__E005)
		{
			_E001(camera);
		}
		else
		{
			_E003();
		}
	}

	internal void _E005()
	{
		PerfectCullingCamera.m__E003 = -1;
	}
}
