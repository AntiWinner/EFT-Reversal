using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EFT.Settings.Graphics;

[Serializable]
public static class EftDisplay
{
	private static class _E000
	{
		public struct _E000
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		public struct _E001
		{
			public int x;

			public int y;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		private sealed class _E002
		{
			public int cbSize = Marshal.SizeOf(typeof(_E002));

			public _E000 rcMonitor;

			public _E000 rcWork;

			public int dwFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szDevice;
		}

		private delegate bool _E003(IntPtr hMonitor, IntPtr hdcMonitor, ref _E000 lprcMonitor, IntPtr dwData);

		[CompilerGenerated]
		private sealed class _E004
		{
			public IntPtr monitor;

			public EftDisplay._E001 result;

			public byte index;

			public string[] names;

			internal bool _E000(IntPtr hMonitor, IntPtr hdcMonitor, ref _E000 lprcMonitor, IntPtr dwData)
			{
				_E002 obj = new _E002();
				obj.cbSize = Marshal.SizeOf(obj);
				if (_E00C(hMonitor, obj) && hMonitor == monitor)
				{
					result = _E007(obj, index, names);
					return false;
				}
				index++;
				return true;
			}
		}

		[CompilerGenerated]
		private sealed class _E005
		{
			public List<EftDisplay._E001> result;

			public string[] names;

			internal bool _E000(IntPtr hMonitor, IntPtr hdcMonitor, ref _E000 lprcMonitor, IntPtr dwData)
			{
				_E002 obj = new _E002();
				obj.cbSize = Marshal.SizeOf(obj);
				if (!_E00C(hMonitor, obj))
				{
					return false;
				}
				byte index = (byte)result.Count;
				EftDisplay._E001 item = _E007(obj, index, names);
				result.Add(item);
				return true;
			}
		}

		private const int m__E000 = 2;

		internal static _E001 _E000()
		{
			IntPtr hWnd = _E00A();
			_E001 point = default(_E001);
			if (_E009(hWnd, ref point))
			{
				return point;
			}
			throw new ApplicationException(_ED3E._E000(190950));
		}

		internal static Vector2Int _E001()
		{
			IntPtr hWnd = _E00A();
			_E001 point = default(_E001);
			if (!_E009(hWnd, ref point))
			{
				throw new ApplicationException(_ED3E._E000(190992));
			}
			EftDisplay._E001 obj = _E003(in point);
			_E000 rect = default(_E000);
			if (!_E00D(hWnd, ref rect))
			{
				throw new ApplicationException(_ED3E._E000(190977));
			}
			_E000 rect2 = default(_E000);
			if (!_E00E(hWnd, ref rect2))
			{
				throw new ApplicationException(_ED3E._E000(191031));
			}
			int num = rect.right - rect.left - (rect2.right - rect2.left);
			int num2 = rect.bottom - rect.top - (rect2.bottom - rect2.top);
			int x = obj.MonitorRect.width - num;
			int y = obj.MonitorRect.height - num2;
			return new Vector2Int(x, y);
		}

		internal static EftDisplay._E001 _E002()
		{
			IntPtr hWnd = _E00A();
			_E001 point = default(_E001);
			if (_E009(hWnd, ref point))
			{
				return _E003(in point);
			}
			throw new ApplicationException(_ED3E._E000(191013));
		}

		private static EftDisplay._E001 _E003(in _E001 point)
		{
			EftDisplay._E001 result = EftDisplay._E001.Default;
			byte index = 0;
			IntPtr monitor = _E00F(point, 2);
			string[] names = EftDisplay._E002.GetAllMonitorsFriendlyNames().ToArray();
			_E00B(IntPtr.Zero, IntPtr.Zero, delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref _E000 lprcMonitor, IntPtr dwData)
			{
				_E002 obj = new _E002();
				obj.cbSize = Marshal.SizeOf(obj);
				if (_E00C(hMonitor, obj) && hMonitor == monitor)
				{
					result = _E007(obj, index, names);
					return false;
				}
				index++;
				return true;
			}, IntPtr.Zero);
			return result;
		}

		internal static List<EftDisplay._E001> _E004()
		{
			List<EftDisplay._E001> result = new List<EftDisplay._E001>();
			string[] names = EftDisplay._E002.GetAllMonitorsFriendlyNames().ToArray();
			_E00B(IntPtr.Zero, IntPtr.Zero, delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref _E000 lprcMonitor, IntPtr dwData)
			{
				_E002 obj = new _E002();
				obj.cbSize = Marshal.SizeOf(obj);
				if (!_E00C(hMonitor, obj))
				{
					return false;
				}
				byte index = (byte)result.Count;
				EftDisplay._E001 item = _E007(obj, index, names);
				result.Add(item);
				return true;
			}, IntPtr.Zero);
			return result;
		}

		internal static void _E005(in RectInt rect)
		{
			IntPtr intPtr = _E00A();
			_E001 point = default(_E001);
			if (_E009(intPtr, ref point))
			{
				_E008(intPtr, rect.x, rect.y, rect.width, rect.height, bRepaint: true);
				return;
			}
			throw new ApplicationException(_ED3E._E000(191069));
		}

		private static RectInt _E006(in _E000 rect)
		{
			return new RectInt(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
		}

		private static EftDisplay._E001 _E007(_E002 info, byte index, string[] friendlyNames)
		{
			string szDevice = info.szDevice;
			RectInt monitorRect = _E006(in info.rcWork);
			RectInt workRect = _E006(in info.rcMonitor);
			bool isPrimary = (info.dwFlags & 1) != 0;
			string friendlyName = ((friendlyNames != null && friendlyNames.Length > index) ? friendlyNames[index] : szDevice);
			return new EftDisplay._E001(index, isPrimary, szDevice, friendlyName, monitorRect, workRect);
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "MoveWindow", ExactSpelling = true, SetLastError = true)]
		private static extern void _E008(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "ClientToScreen", ExactSpelling = true, SetLastError = true)]
		private static extern bool _E009(IntPtr hWnd, ref _E001 point);

		[DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
		private static extern IntPtr _E00A();

		[DllImport("user32.dll", EntryPoint = "EnumDisplayMonitors")]
		private static extern bool _E00B(IntPtr hdc, IntPtr lprcClip, _E003 lpfnEnum, IntPtr dwData);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfo")]
		private static extern bool _E00C(IntPtr hMonitor, [In][Out] _E002 info);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetWindowRect", ExactSpelling = true, SetLastError = true)]
		private static extern bool _E00D(IntPtr hWnd, ref _E000 rect);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "GetClientRect", ExactSpelling = true, SetLastError = true)]
		private static extern bool _E00E(IntPtr hWnd, ref _E000 rect);

		[DllImport("user32.dll", EntryPoint = "MonitorFromPoint", ExactSpelling = true)]
		private static extern IntPtr _E00F(_E001 point, int flags);
	}

	public readonly struct _E001 : IEquatable<_E001>
	{
		[CompilerGenerated]
		private readonly byte _E000;

		[CompilerGenerated]
		private readonly bool m__E001;

		[CompilerGenerated]
		private readonly string _E002;

		[CompilerGenerated]
		private readonly string _E003;

		[CompilerGenerated]
		private readonly RectInt _E004;

		[CompilerGenerated]
		private readonly RectInt _E005;

		public static _E001 Default => new _E001(0, isPrimary: true, "", _ED3E._E000(181789), new RectInt(0, 0, Screen.width, Screen.height), new RectInt(0, 0, Screen.width, Screen.height));

		public byte Index
		{
			[CompilerGenerated]
			get
			{
				return _E000;
			}
		}

		public bool IsPrimary
		{
			[CompilerGenerated]
			get
			{
				return m__E001;
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
		}

		public string FriendlyName
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
		}

		public RectInt MonitorRect
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
		}

		public RectInt WorkRect
		{
			[CompilerGenerated]
			get
			{
				return _E005;
			}
		}

		public _E001(byte index, bool isPrimary, string name, string friendlyName, RectInt monitorRect, RectInt workRect)
		{
			_E000 = index;
			m__E001 = isPrimary;
			_E002 = name;
			_E003 = friendlyName;
			_E004 = monitorRect;
			_E005 = workRect;
		}

		public bool Contain(int x, int y)
		{
			if (x < MonitorRect.x || y < MonitorRect.y)
			{
				return false;
			}
			int num = WorkRect.x + WorkRect.width;
			int num2 = WorkRect.y + WorkRect.height;
			if (x < num)
			{
				return y < num2;
			}
			return false;
		}

		public int Distance(int x, int y)
		{
			return 0;
		}

		public bool Equals(_E001 other)
		{
			if (Index == other.Index && IsPrimary == other.IsPrimary && Name == other.Name && FriendlyName == other.FriendlyName && MonitorRect.Equals(other.MonitorRect))
			{
				return WorkRect.Equals(other.WorkRect);
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(191056), Index, IsPrimary, Name, FriendlyName, MonitorRect, WorkRect);
		}
	}

	private static class _E002
	{
		public enum QUERY_DEVICE_CONFIG_FLAGS : uint
		{
			QDC_ALL_PATHS = 1u,
			QDC_ONLY_ACTIVE_PATHS = 2u,
			QDC_DATABASE_CURRENT = 4u
		}

		public enum DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY : uint
		{
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_OTHER = uint.MaxValue,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HD15 = 0u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SVIDEO = 1u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPOSITE_VIDEO = 2u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPONENT_VIDEO = 3u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI = 4u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI = 5u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_LVDS = 6u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_D_JPN = 8u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDI = 9u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL = 10u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED = 11u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EXTERNAL = 12u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EMBEDDED = 13u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDTVDONGLE = 14u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_MIRACAST = 15u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL = 2147483648u,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_SCANLINE_ORDERING : uint
		{
			DISPLAYCONFIG_SCANLINE_ORDERING_UNSPECIFIED = 0u,
			DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE = 1u,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED = 2u,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_UPPERFIELDFIRST = 2u,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_LOWERFIELDFIRST = 3u,
			DISPLAYCONFIG_SCANLINE_ORDERING_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_ROTATION : uint
		{
			DISPLAYCONFIG_ROTATION_IDENTITY = 1u,
			DISPLAYCONFIG_ROTATION_ROTATE90 = 2u,
			DISPLAYCONFIG_ROTATION_ROTATE180 = 3u,
			DISPLAYCONFIG_ROTATION_ROTATE270 = 4u,
			DISPLAYCONFIG_ROTATION_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_SCALING : uint
		{
			DISPLAYCONFIG_SCALING_IDENTITY = 1u,
			DISPLAYCONFIG_SCALING_CENTERED = 2u,
			DISPLAYCONFIG_SCALING_STRETCHED = 3u,
			DISPLAYCONFIG_SCALING_ASPECTRATIOCENTEREDMAX = 4u,
			DISPLAYCONFIG_SCALING_CUSTOM = 5u,
			DISPLAYCONFIG_SCALING_PREFERRED = 128u,
			DISPLAYCONFIG_SCALING_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_PIXELFORMAT : uint
		{
			DISPLAYCONFIG_PIXELFORMAT_8BPP = 1u,
			DISPLAYCONFIG_PIXELFORMAT_16BPP = 2u,
			DISPLAYCONFIG_PIXELFORMAT_24BPP = 3u,
			DISPLAYCONFIG_PIXELFORMAT_32BPP = 4u,
			DISPLAYCONFIG_PIXELFORMAT_NONGDI = 5u,
			DISPLAYCONFIG_PIXELFORMAT_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_MODE_INFO_TYPE : uint
		{
			DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE = 1u,
			DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2u,
			DISPLAYCONFIG_MODE_INFO_TYPE_FORCE_UINT32 = uint.MaxValue
		}

		public enum DISPLAYCONFIG_DEVICE_INFO_TYPE : uint
		{
			DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME = 1u,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2u,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE = 3u,
			DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME = 4u,
			DISPLAYCONFIG_DEVICE_INFO_SET_TARGET_PERSISTENCE = 5u,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_BASE_TYPE = 6u,
			DISPLAYCONFIG_DEVICE_INFO_FORCE_UINT32 = uint.MaxValue
		}

		public struct _E000
		{
			public uint LowPart;

			public int HighPart;
		}

		public struct _E001
		{
			public _E000 adapterId;

			public uint id;

			public uint modeInfoIdx;

			public uint statusFlags;
		}

		public struct _E002
		{
			public _E000 adapterId;

			public uint id;

			public uint modeInfoIdx;

			private DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY _E000;

			private DISPLAYCONFIG_ROTATION _E001;

			private DISPLAYCONFIG_SCALING _E002;

			private _E003 _E003;

			private DISPLAYCONFIG_SCANLINE_ORDERING _E004;

			public bool targetAvailable;

			public uint statusFlags;
		}

		public struct _E003
		{
			public uint Numerator;

			public uint Denominator;
		}

		public struct _E004
		{
			public _E001 sourceInfo;

			public _E002 targetInfo;

			public uint flags;
		}

		public struct _E005
		{
			public uint cx;

			public uint cy;
		}

		public struct _E006
		{
			public ulong pixelRate;

			public _E003 hSyncFreq;

			public _E003 vSyncFreq;

			public _E005 activeSize;

			public _E005 totalSize;

			public uint videoStandard;

			public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
		}

		public struct _E007
		{
			public _E006 targetVideoSignalInfo;
		}

		public struct _E008
		{
			private int _E000;

			private int _E001;
		}

		public struct _E009
		{
			public uint width;

			public uint height;

			public DISPLAYCONFIG_PIXELFORMAT pixelFormat;

			public _E008 position;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct _E00A
		{
			[FieldOffset(0)]
			public _E007 targetMode;

			[FieldOffset(0)]
			public _E009 sourceMode;
		}

		public struct _E00B
		{
			public DISPLAYCONFIG_MODE_INFO_TYPE infoType;

			public uint id;

			public _E000 adapterId;

			public _E00A modeInfo;
		}

		public struct _E00C
		{
			public uint value;
		}

		public struct _E00D
		{
			public DISPLAYCONFIG_DEVICE_INFO_TYPE type;

			public uint size;

			public _E000 adapterId;

			public uint id;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct _E00E
		{
			public _E00D header;

			public _E00C flags;

			public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;

			public ushort edidManufactureId;

			public ushort edidProductCodeId;

			public uint connectorInstance;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string monitorFriendlyDeviceName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string monitorDevicePath;
		}

		public const int ERROR_SUCCESS = 0;

		[DllImport("user32.dll")]
		public static extern int GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

		[DllImport("user32.dll")]
		public static extern int QueryDisplayConfig(QUERY_DEVICE_CONFIG_FLAGS flags, ref uint numPathArrayElements, [Out] _E004[] PathInfoArray, ref uint numModeInfoArrayElements, [Out] _E00B[] ModeInfoArray, IntPtr currentTopologyId);

		[DllImport("user32.dll")]
		public static extern int DisplayConfigGetDeviceInfo(ref _E00E deviceName);

		private static string _E000(_E000 adapterId, uint targetId)
		{
			_E00E obj = default(_E00E);
			obj.header.size = (uint)Marshal.SizeOf(typeof(_E00E));
			obj.header.adapterId = adapterId;
			obj.header.id = targetId;
			obj.header.type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;
			_E00E deviceName = obj;
			int num = DisplayConfigGetDeviceInfo(ref deviceName);
			if (num != 0)
			{
				Debug.LogError(string.Format(_ED3E._E000(191111), num));
				return null;
			}
			return deviceName.monitorFriendlyDeviceName;
		}

		public static IEnumerable<string> GetAllMonitorsFriendlyNames()
		{
			int displayConfigBufferSizes = GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out var numPathArrayElements, out var numModeInfoArrayElements);
			if (displayConfigBufferSizes != 0)
			{
				Debug.LogError(string.Format(_ED3E._E000(191196), displayConfigBufferSizes));
			}
			else
			{
				if (numPathArrayElements == 0)
				{
					yield break;
				}
				_E004[] pathInfoArray = new _E004[numPathArrayElements];
				_E00B[] array = new _E00B[numModeInfoArrayElements];
				displayConfigBufferSizes = QueryDisplayConfig(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, ref numPathArrayElements, pathInfoArray, ref numModeInfoArrayElements, array, IntPtr.Zero);
				if (displayConfigBufferSizes != 0)
				{
					Debug.LogError(string.Format(_ED3E._E000(191226), displayConfigBufferSizes));
					yield break;
				}
				for (int i = 0; i < numModeInfoArrayElements; i++)
				{
					if (array[i].infoType == DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
					{
						yield return _E000(array[i].adapterId, array[i].id);
					}
				}
			}
		}
	}

	public static byte DefaultIndex
	{
		get
		{
			try
			{
				return Infos().FirstOrDefault((_E001 i) => i.IsPrimary).Index;
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}

	public static byte CurrentIndex()
	{
		try
		{
			return _E000._E002().Index;
		}
		catch (Exception)
		{
			return DefaultIndex;
		}
	}

	public static _E001 Current()
	{
		try
		{
			return _E000._E002();
		}
		catch (Exception)
		{
			return _E001.Default;
		}
	}

	public static List<_E001> Infos()
	{
		return _E000._E004();
	}

	public static bool Equal(List<_E001> first, List<_E001> second)
	{
		bool flag = first.Count == second.Count;
		if (!flag)
		{
			return false;
		}
		int num = 0;
		while (flag && num < first.Count)
		{
			_E001 obj = first[num];
			_E001 other = second[num];
			flag = obj.Equals(other);
			num++;
		}
		return flag;
	}

	public static Vector2Int GetWindowMaxResolution()
	{
		try
		{
			return _E000._E001();
		}
		catch (Exception)
		{
			return Current().MonitorRect.size;
		}
	}

	public static Vector2Int GetWindowPosition()
	{
		try
		{
			_E000._E001 obj = _E000._E000();
			return new Vector2Int(obj.x, obj.y);
		}
		catch (Exception)
		{
			return default(Vector2Int);
		}
	}

	public static bool IsValid(byte index)
	{
		if (index == 0)
		{
			return true;
		}
		List<_E001> list = Infos();
		return index < list.Count;
	}

	public static void MoveWindow(byte index, int? width = null, int? height = null)
	{
		List<_E001> list = Infos();
		if (index >= list.Count)
		{
			throw new ArgumentOutOfRangeException(_ED3E._E000(190952));
		}
		RectInt rect = list[index].WorkRect;
		if (width.HasValue && width <= rect.width)
		{
			rect.width = width.Value;
		}
		if (height.HasValue && height <= rect.height)
		{
			rect.height = height.Value;
		}
		_E000._E005(in rect);
	}
}
