using System.Threading.Tasks;
using Systems.Effects;
using Comfort.Common;
using UnityEngine;

public class TestEffects : MonoBehaviour
{
	public int ID;

	public Camera Camera;

	public Effects Effects;

	private _E43F _E000;

	private async Task Start()
	{
		if (Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Release(Singleton<Effects>.Instance);
		}
		Singleton<Effects>.Create(Effects);
		_E8A8.Instance.SetCamera(Camera);
		_E5DB.Manager = (await _E5DC.CreateAssetsManager()).Result;
		await _E5DB.Manager.LoadAssetAsync(_ED3E._E000(88562), _ED3E._E000(88584));
		BetterAudio obj = _E3AA.FindUnityObjectOfType<BetterAudio>() ?? new GameObject(_ED3E._E000(88578)).AddComponent<BetterAudio>();
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Release(Singleton<BetterAudio>.Instance);
		}
		Singleton<BetterAudio>.Create(obj);
		obj.Preload();
		this._E000 = new _E43F();
		this._E000.Init();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(_E8A8.Instance.Camera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
		{
			Singleton<Effects>.Instance.TestEmit(ID, hitInfo.point, hitInfo.normal);
		}
	}
}
