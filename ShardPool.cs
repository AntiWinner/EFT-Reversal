using UnityEngine;

public class ShardPool : MonoBehaviour
{
	private static ShardPool m__E000;

	[HideInInspector]
	[SerializeField]
	private Shard[] shards;

	[SerializeField]
	private int maxPoolSize;

	private int _E001;

	public static int ShardsUsed
	{
		get
		{
			CheckPool();
			return ShardPool.m__E000._E001;
		}
	}

	public bool IsPopulated => shards != null;

	public static Shard NextShard
	{
		get
		{
			CheckPool();
			if (_E002)
			{
				return ShardPool.m__E000.shards[ShardPool.m__E000._E001++];
			}
			return null;
		}
	}

	private static bool _E002 => ShardPool.m__E000._E001 + 1 < ShardPool.m__E000.shards.Length;

	private void Awake()
	{
		ShardPool.m__E000 = this;
		PopulatePool();
	}

	public static void CheckPool()
	{
		if (ShardPool.m__E000 == null)
		{
			_E000();
		}
		ShardPool.m__E000.PopulatePool();
	}

	private static void _E000()
	{
		ShardPool.m__E000 = _E3AA.FindUnityObjectOfType(typeof(ShardPool)) as ShardPool;
		if (!(ShardPool.m__E000 != null))
		{
			ShardPool.m__E000 = new GameObject(_ED3E._E000(85652)).AddComponent<ShardPool>();
		}
	}

	public void PopulatePool()
	{
		if (IsPopulated && shards.Length == maxPoolSize)
		{
			return;
		}
		if (shards != null)
		{
			Shard[] array = shards;
			for (int i = 0; i < array.Length; i++)
			{
				Object.DestroyImmediate(array[i].gameObject);
			}
		}
		shards = new Shard[maxPoolSize];
		for (int j = 0; j < maxPoolSize; j++)
		{
			shards[j] = new GameObject(_ED3E._E000(85640)).AddComponent<Shard>();
			shards[j].transform.parent = base.transform;
		}
	}
}
