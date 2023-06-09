using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.UI;

namespace EFT.Hideout;

public abstract class AbstractPanel<T> : UIElement, _E833, IUIView, IDisposable
{
	[CompilerGenerated]
	private T _E01F;

	[CompilerGenerated]
	private ELevelType _E020;

	[CompilerGenerated]
	private Stage _E021;

	[CompilerGenerated]
	private RelatedData _E022;

	[CompilerGenerated]
	private AreaData _E023;

	[CompilerGenerated]
	private Player _E024;

	[CompilerGenerated]
	private _E796 _E025;

	protected T Info
	{
		[CompilerGenerated]
		get
		{
			return _E01F;
		}
		[CompilerGenerated]
		private set
		{
			_E01F = value;
		}
	}

	protected ELevelType LevelType
	{
		[CompilerGenerated]
		get
		{
			return _E020;
		}
		[CompilerGenerated]
		private set
		{
			_E020 = value;
		}
	}

	protected Stage Stage
	{
		[CompilerGenerated]
		get
		{
			return _E021;
		}
		[CompilerGenerated]
		private set
		{
			_E021 = value;
		}
	}

	protected RelatedData RelatedData
	{
		[CompilerGenerated]
		get
		{
			return _E022;
		}
		[CompilerGenerated]
		private set
		{
			_E022 = value;
		}
	}

	protected AreaData AreaData
	{
		[CompilerGenerated]
		get
		{
			return _E023;
		}
		[CompilerGenerated]
		private set
		{
			_E023 = value;
		}
	}

	protected Player Player
	{
		[CompilerGenerated]
		get
		{
			return _E024;
		}
		[CompilerGenerated]
		private set
		{
			_E024 = value;
		}
	}

	protected _E796 Session
	{
		[CompilerGenerated]
		get
		{
			return _E025;
		}
		[CompilerGenerated]
		private set
		{
			_E025 = value;
		}
	}

	public virtual async Task Show(RelatedData relatedData, Stage stage, ELevelType levelType, AreaData areaData, Player player, _E796 session)
	{
		UI.Dispose();
		Info = ((relatedData.Value is T val) ? val : default(T));
		Stage = stage;
		LevelType = levelType;
		RelatedData = relatedData;
		AreaData = areaData;
		Player = player;
		Session = session;
		UI.AddDisposable(areaData.LevelUpdated.Subscribe(SetInfo));
		UI.AddDisposable(areaData.StatusUpdated.Subscribe(SetInfo));
		SetInfo();
		ShowGameObject();
		await ShowContents();
	}

	public abstract Task ShowContents();

	public abstract void SetInfo();
}
