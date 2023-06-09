using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarButtons : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Scrollbar scrollbar;

		internal void _E000()
		{
			scrollbar.value += 0.1f;
		}

		internal void _E001()
		{
			scrollbar.value -= 0.1f;
		}
	}

	[SerializeField]
	private Button _positiveButton;

	[SerializeField]
	private Button _negativeButton;

	private void Start()
	{
		Scrollbar scrollbar = GetComponent<Scrollbar>();
		_positiveButton.onClick.AddListener(delegate
		{
			scrollbar.value += 0.1f;
		});
		_negativeButton.onClick.AddListener(delegate
		{
			scrollbar.value -= 0.1f;
		});
	}
}
