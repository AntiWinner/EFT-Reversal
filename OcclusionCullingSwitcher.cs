using UnityEngine;

[ExecuteInEditMode]
public class OcclusionCullingSwitcher : DisablerCullingObjectBase
{
	private bool _E000 => _E8A8.Instance.IsOcclusionCullingAllowed;

	protected override void SetComponentsEnabled(bool hasEntered)
	{
		_E000();
	}

	private void _E000()
	{
		Camera camera = _E001();
		if (camera != null)
		{
			bool flag = this._E000 && HasEntered;
			if (camera.useOcclusionCulling != flag)
			{
				camera.useOcclusionCulling = flag;
				Debug.Log(_ED3E._E000(95197) + camera.useOcclusionCulling);
			}
		}
	}

	private Camera _E001()
	{
		return _E8A8.Instance.Camera;
	}
}
