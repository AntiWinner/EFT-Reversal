using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Diz.DependencyManager;

public class TestLoadable : MonoBehaviour, _ED0F
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TestLoadable _003C_003E4__this;

		public Button tokenButton;

		internal void _E000()
		{
			_003C_003E4__this.m__E001[tokenButton].Release();
			UnityEngine.Object.Destroy(tokenButton.gameObject);
		}
	}

	[SerializeField]
	private Button _loadButton;

	[SerializeField]
	private Image _mainImage;

	[SerializeField]
	private Text _refCountLabel;

	[SerializeField]
	private Transform _tokensContainer;

	[SerializeField]
	private Button _tokenTemplate;

	private _ECF5<ELoadState> m__E000 = new _ECF5<ELoadState>(ELoadState.Unloaded);

	public TestLoadable[] Dependencies;

	public Action<TestLoadable> OnClicked;

	public Action<TestLoadable> OnRightClicked;

	private readonly Dictionary<Button, _ED0E<TestLoadable>._E002> m__E001 = new Dictionary<Button, _ED0E<TestLoadable>._E002>();

	public _ECF5<ELoadState> LoadState => this.m__E000;

	public string Key => base.name;

	public IEnumerable<string> DependencyKeys => Dependencies.Select((TestLoadable x) => x.Key);

	public float Progress => 0f;

	private void Awake()
	{
		_E000();
		_loadButton.onClick.AddListener(delegate
		{
			OnClicked(this);
		});
	}

	private void _E000()
	{
		switch (this.m__E000.Value)
		{
		case ELoadState.Loaded:
			_mainImage.color = Color.green;
			break;
		case ELoadState.Loading:
			_mainImage.color = Color.yellow;
			break;
		case ELoadState.Unloaded:
			_mainImage.color = Color.grey;
			break;
		case ELoadState.Failed:
			_mainImage.color = Color.red;
			break;
		case ELoadState.Unloading:
			break;
		}
	}

	public void AddToken(_ED0E<TestLoadable>._E002 token)
	{
		Button tokenButton = UnityEngine.Object.Instantiate(_tokenTemplate);
		this.m__E001[tokenButton] = token;
		tokenButton.gameObject.SetActive(value: true);
		tokenButton.transform.SetParent(_tokensContainer, worldPositionStays: false);
		tokenButton.onClick.AddListener(delegate
		{
			this.m__E001[tokenButton].Release();
			UnityEngine.Object.Destroy(tokenButton.gameObject);
		});
	}

	public void Load()
	{
		StartCoroutine(Loading());
	}

	public IEnumerator Load(IProgress<float> progress, CancellationToken ct)
	{
		return Loading();
	}

	public IEnumerator Loading()
	{
		Debug.Log(_ED3E._E000(245656) + base.name);
		this.m__E000.Value = ELoadState.Loading;
		_E000();
		yield return new WaitForSeconds(10f);
		this.m__E000.Value = ELoadState.Loaded;
		_E000();
	}

	public void Unload()
	{
		this.m__E000.Value = ELoadState.Unloaded;
		_E000();
	}

	public void SetRefCount(int rc)
	{
		_refCountLabel.text = rc.ToString();
	}

	[CompilerGenerated]
	private void _E001()
	{
		OnClicked(this);
	}
}
