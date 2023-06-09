using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class OperationQueueIndicator : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public OperationQueueIndicator _003C_003E4__this;

		public _E796 session;

		internal void _E000()
		{
			_003C_003E4__this._loader.SetActive(session.IsFlushing || session.QueueStatus == EOperationQueueStatus.AwaitingResponse);
		}
	}

	[SerializeField]
	private GameObject _loader;

	public void Show(_E796 session)
	{
		ShowGameObject();
		UI.BindEvent(session.QueueStatusChanged, delegate
		{
			_loader.SetActive(session.IsFlushing || session.QueueStatus == EOperationQueueStatus.AwaitingResponse);
		});
	}

	private void OnDestroy()
	{
		Close();
	}
}
