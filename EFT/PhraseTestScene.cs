using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;

namespace EFT;

public class PhraseTestScene : MonoBehaviour
{
	private const string m__E000 = "assets/content/audio/prefabs/phrases/bear/bearsashaphrasesounds.bundle";

	[SerializeField]
	private Camera _camera;

	private void Awake()
	{
		_E000();
	}

	private async void _E000()
	{
		_E8A8.Instance.SetCamera(_camera);
		IOperation<_EC35> operation = await _E5DC.CreateAssetsManager();
		if (operation.Failed)
		{
			Debug.Log(_ED3E._E000(191640));
			return;
		}
		_E5DB.Manager = operation.Result;
		await _E001();
	}

	private async Task _E001()
	{
		await _E5DB.Manager.LoadBundlesAsync(new string[1] { _ED3E._E000(191675) });
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Release(Singleton<BetterAudio>.Instance);
		}
		Singleton<BetterAudio>.Create(new GameObject(_ED3E._E000(88578)).AddComponent<BetterAudio>());
		await Singleton<BetterAudio>.Instance.PreloadCoroutine();
	}
}
