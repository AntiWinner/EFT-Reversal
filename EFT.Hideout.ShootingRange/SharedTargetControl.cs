using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class SharedTargetControl : MonoBehaviour
{
	private _E84D _E000 = new _E84D();

	public async Task Run(_E84C targetScenario)
	{
		await this._E000.Start(targetScenario);
	}

	public async Task Complete()
	{
		await this._E000.Complete();
	}

	public void Cancel()
	{
		this._E000.Cancel();
	}
}
