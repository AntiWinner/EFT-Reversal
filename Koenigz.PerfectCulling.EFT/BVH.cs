using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
public class BVH<T> where T : class, ISpatialItem
{
	[Serializable]
	private struct Node
	{
		public int Child1;

		public int Child2;

		public int Height;

		public int Parent;

		public int Next;

		public Bounds AABB;

		public T Object;

		public bool IsLeaf => Child1 == -1;
	}

	private const int EMPTY = -1;

	private const float MARGIN = 0f;

	private int _root = -1;

	private Node[] _nodes;

	private int _nodeCount;

	private int _nodeCapacity;

	private int _freeList;

	private int _inCnt;

	private readonly Stack<int> _growableStack = new Stack<int>(256);

	public void Initialize(int capacity)
	{
		_root = -1;
		_nodeCapacity = capacity;
		_nodeCount = 0;
		_nodes = new Node[_nodeCapacity];
		for (int i = 0; i < _nodeCapacity - 1; i++)
		{
			_nodes[i].Next = i + 1;
			_nodes[i].Height = -1;
		}
		_nodes[_nodeCapacity - 1].Next = -1;
		_nodes[_nodeCapacity - 1].Height = -1;
		_freeList = 0;
		_inCnt = 0;
	}

	public int Add(T obj)
	{
		int num = _E000();
		Bounds spatialItemBounds = obj.SpatialItemBounds;
		spatialItemBounds.Expand(0f);
		_nodes[num].AABB = spatialItemBounds;
		_nodes[num].Height = 0;
		_nodes[num].Object = obj;
		_E002(num);
		return num;
	}

	public int OverlapNoAlloc(Bounds bounds, T[] outputArray)
	{
		int num = 0;
		if (_nodeCount == 0)
		{
			return 0;
		}
		_growableStack.Clear();
		_growableStack.Push(_root);
		while (_growableStack.Count > 0)
		{
			int num2 = _growableStack.Pop();
			if (num2 == -1 || !_nodes[num2].AABB.Intersects(bounds))
			{
				continue;
			}
			if (_nodes[num2].IsLeaf)
			{
				outputArray[num] = _nodes[num2].Object;
				num++;
				if (num == outputArray.Length)
				{
					return num;
				}
			}
			else
			{
				_growableStack.Push(_nodes[num2].Child1);
				_growableStack.Push(_nodes[num2].Child2);
			}
		}
		return num;
	}

	public IEnumerable<T> EnumerateOverlappingLeafNodes(Bounds bounds)
	{
		if (_nodeCount == 0)
		{
			yield break;
		}
		_growableStack.Clear();
		_growableStack.Push(_root);
		while (_growableStack.Count > 0)
		{
			int num = _growableStack.Pop();
			if (num != -1 && _nodes[num].AABB.Intersects(bounds))
			{
				if (_nodes[num].IsLeaf)
				{
					yield return _nodes[num].Object;
					continue;
				}
				_growableStack.Push(_nodes[num].Child1);
				_growableStack.Push(_nodes[num].Child2);
			}
		}
	}

	public T QueryNearest(Vector3 point, float radius)
	{
		if (_nodeCount == 0)
		{
			return null;
		}
		float num = -1f;
		T result = null;
		Bounds bounds = new Bounds(point, Vector3.one * radius);
		foreach (T item in EnumerateOverlappingLeafNodes(bounds))
		{
			if (item.SpatialItemBounds.Contains(point))
			{
				return item;
			}
			float num2 = item.SpatialItemBounds.SqrDistance(point);
			if (num2 < num || num < 0f)
			{
				num = num2;
				result = item;
			}
		}
		return result;
	}

	public T Query(Vector3 point)
	{
		if (_nodeCount == 0)
		{
			return null;
		}
		_growableStack.Clear();
		_growableStack.Push(_root);
		while (_growableStack.Count > 0)
		{
			int num = _growableStack.Pop();
			if (num != -1 && _nodes[num].AABB.Contains(point))
			{
				if (_nodes[num].IsLeaf)
				{
					return _nodes[num].Object;
				}
				_growableStack.Push(_nodes[num].Child1);
				_growableStack.Push(_nodes[num].Child2);
			}
		}
		return null;
	}

	private int _E000()
	{
		if (_freeList == -1)
		{
			Node[] nodes = _nodes;
			_nodeCapacity *= 2;
			_nodes = new Node[_nodeCapacity];
			for (int i = 0; i < nodes.Length; i++)
			{
				_nodes[i] = nodes[i];
			}
			for (int j = _nodeCount; j < _nodeCapacity - 1; j++)
			{
				_nodes[j].Next = j + 1;
				_nodes[j].Height = -1;
			}
			_nodes[_nodeCapacity - 1].Next = -1;
			_nodes[_nodeCapacity - 1].Height = -1;
			_freeList = _nodeCount;
		}
		int freeList = _freeList;
		_freeList = _nodes[freeList].Next;
		_nodes[freeList].Parent = -1;
		_nodes[freeList].Child1 = -1;
		_nodes[freeList].Child2 = -1;
		_nodes[freeList].Height = 0;
		_nodes[freeList].Next = 0;
		_nodes[freeList].Object = null;
		_nodeCount++;
		return freeList;
	}

	private void _E001(int key)
	{
		_nodes[key].Next = _freeList;
		_nodes[key].Height = -1;
		_freeList = key;
		_nodeCount--;
	}

	private void _E002(int leaf)
	{
		_inCnt++;
		if (_root == -1)
		{
			_root = leaf;
			_nodes[_root].Parent = -1;
			return;
		}
		Bounds aABB = _nodes[leaf].AABB;
		int num = _root;
		while (!_nodes[num].IsLeaf)
		{
			int child = _nodes[num].Child1;
			int child2 = _nodes[num].Child2;
			float surfaceArea = _nodes[num].AABB.GetSurfaceArea();
			Bounds aABB2 = _nodes[num].AABB;
			aABB2.Encapsulate(aABB);
			float surfaceArea2 = aABB2.GetSurfaceArea();
			float num2 = 2f * surfaceArea2;
			float num3 = 2f * (surfaceArea2 - surfaceArea);
			float num4;
			if (_nodes[child].IsLeaf)
			{
				Bounds box = aABB;
				box.Encapsulate(_nodes[child].AABB);
				num4 = box.GetSurfaceArea() + num3;
			}
			else
			{
				Bounds box2 = aABB;
				box2.Encapsulate(_nodes[child].AABB);
				float surfaceArea3 = _nodes[child].AABB.GetSurfaceArea();
				num4 = box2.GetSurfaceArea() - surfaceArea3 + num3;
			}
			float num5;
			if (_nodes[child2].IsLeaf)
			{
				Bounds box3 = aABB;
				box3.Encapsulate(_nodes[child2].AABB);
				num5 = box3.GetSurfaceArea() + num3;
			}
			else
			{
				Bounds box4 = aABB;
				box4.Encapsulate(_nodes[child2].AABB);
				float surfaceArea4 = _nodes[child2].AABB.GetSurfaceArea();
				num5 = box4.GetSurfaceArea() - surfaceArea4 + num3;
			}
			if (num2 < num4 && num2 < num5)
			{
				break;
			}
			num = ((!(num4 < num5)) ? child2 : child);
		}
		int num6 = num;
		int parent = _nodes[num6].Parent;
		int num7 = _E000();
		_nodes[num7].Parent = parent;
		_nodes[num7].Object = null;
		Bounds aABB3 = aABB;
		aABB3.Encapsulate(_nodes[num6].AABB);
		_nodes[num7].AABB = aABB3;
		_nodes[num7].Height = _nodes[num6].Height + 1;
		if (parent != -1)
		{
			if (_nodes[parent].Child1 == num6)
			{
				_nodes[parent].Child1 = num7;
			}
			else
			{
				_nodes[parent].Child2 = num7;
			}
			_nodes[num7].Child1 = num6;
			_nodes[num7].Child2 = leaf;
			_nodes[num6].Parent = num7;
			_nodes[leaf].Parent = num7;
		}
		else
		{
			_nodes[num7].Child1 = num6;
			_nodes[num7].Child2 = leaf;
			_nodes[num6].Parent = num7;
			_nodes[leaf].Parent = num7;
			_root = num7;
		}
		for (num = _nodes[leaf].Parent; num != -1; num = _nodes[num].Parent)
		{
			num = _E003(num);
			int child3 = _nodes[num].Child1;
			int child4 = _nodes[num].Child2;
			_nodes[num].Height = 1 + Mathf.Max(_nodes[child3].Height, _nodes[child4].Height);
			Bounds aABB4 = _nodes[child3].AABB;
			aABB4.Encapsulate(_nodes[child4].AABB);
			_nodes[num].AABB = aABB4;
		}
	}

	private int _E003(int iA)
	{
		if (_nodes[iA].IsLeaf || _nodes[iA].Height < 2)
		{
			return iA;
		}
		int child = _nodes[iA].Child1;
		int child2 = _nodes[iA].Child2;
		int num = _nodes[child2].Height - _nodes[child].Height;
		if (num > 1)
		{
			int child3 = _nodes[child2].Child1;
			int child4 = _nodes[child2].Child2;
			_nodes[child2].Child1 = iA;
			_nodes[child2].Parent = _nodes[iA].Parent;
			_nodes[iA].Parent = child2;
			if (_nodes[child2].Parent != -1)
			{
				if (_nodes[_nodes[child2].Parent].Child1 == iA)
				{
					_nodes[_nodes[child2].Parent].Child1 = child2;
				}
				else
				{
					_nodes[_nodes[child2].Parent].Child2 = child2;
				}
			}
			else
			{
				_root = child2;
			}
			if (_nodes[child3].Height > _nodes[child4].Height)
			{
				_nodes[child2].Child2 = child3;
				_nodes[iA].Child2 = child4;
				_nodes[child4].Parent = iA;
				Bounds aABB = _nodes[child].AABB;
				aABB.Encapsulate(_nodes[child4].AABB);
				_nodes[iA].AABB = aABB;
				aABB = _nodes[iA].AABB;
				aABB.Encapsulate(_nodes[child3].AABB);
				_nodes[child2].AABB = aABB;
				_nodes[iA].Height = 1 + Mathf.Max(_nodes[child].Height, _nodes[child4].Height);
				_nodes[child2].Height = 1 + Mathf.Max(_nodes[iA].Height, _nodes[child3].Height);
			}
			else
			{
				_nodes[child2].Child2 = child4;
				_nodes[iA].Child2 = child3;
				_nodes[child3].Parent = iA;
				Bounds aABB2 = _nodes[child].AABB;
				aABB2.Encapsulate(_nodes[child3].AABB);
				_nodes[iA].AABB = aABB2;
				aABB2 = _nodes[iA].AABB;
				aABB2.Encapsulate(_nodes[child4].AABB);
				_nodes[child2].AABB = aABB2;
				_nodes[iA].Height = 1 + Mathf.Max(_nodes[child].Height, _nodes[child3].Height);
				_nodes[child2].Height = 1 + Mathf.Max(_nodes[iA].Height, _nodes[child4].Height);
			}
			return child2;
		}
		if (num < -1)
		{
			int child5 = _nodes[child].Child1;
			int child6 = _nodes[child].Child2;
			_nodes[child].Child1 = iA;
			_nodes[child].Parent = _nodes[iA].Parent;
			_nodes[iA].Parent = child;
			if (_nodes[child].Parent != -1)
			{
				if (_nodes[_nodes[child].Parent].Child1 == iA)
				{
					_nodes[_nodes[child].Parent].Child1 = child;
				}
				else
				{
					_nodes[_nodes[child].Parent].Child2 = child;
				}
			}
			else
			{
				_root = child;
			}
			if (_nodes[child5].Height > _nodes[child6].Height)
			{
				_nodes[child].Child2 = child5;
				_nodes[iA].Child1 = child6;
				_nodes[child6].Parent = iA;
				Bounds aABB3 = _nodes[child2].AABB;
				aABB3.Encapsulate(_nodes[child6].AABB);
				_nodes[iA].AABB = aABB3;
				aABB3 = _nodes[iA].AABB;
				aABB3.Encapsulate(_nodes[child5].AABB);
				_nodes[child].AABB = aABB3;
				_nodes[iA].Height = 1 + Mathf.Max(_nodes[child2].Height, _nodes[child6].Height);
				_nodes[child].Height = 1 + Mathf.Max(_nodes[iA].Height, _nodes[child5].Height);
			}
			else
			{
				_nodes[child].Child2 = child6;
				_nodes[iA].Child1 = child5;
				_nodes[child5].Parent = iA;
				Bounds aABB4 = _nodes[child2].AABB;
				aABB4.Encapsulate(_nodes[child5].AABB);
				_nodes[iA].AABB = aABB4;
				aABB4 = _nodes[iA].AABB;
				aABB4.Encapsulate(_nodes[child6].AABB);
				_nodes[child].AABB = aABB4;
				_nodes[iA].Height = 1 + Mathf.Max(_nodes[child2].Height, _nodes[child5].Height);
				_nodes[child].Height = 1 + Mathf.Max(_nodes[iA].Height, _nodes[child6].Height);
			}
			return child;
		}
		return iA;
	}

	private int _E004(int nodeId)
	{
		if (_nodes[nodeId].IsLeaf)
		{
			return 0;
		}
		int a = _E004(_nodes[nodeId].Child1);
		int b = _E004(_nodes[nodeId].Child2);
		return 1 + Mathf.Max(a, b);
	}

	private void _E005(int index)
	{
		if (index != -1)
		{
			_ = _root;
			int child = _nodes[index].Child1;
			int child2 = _nodes[index].Child2;
			if (!_nodes[index].IsLeaf)
			{
				_E005(child);
				_E005(child2);
			}
		}
	}

	private void _E006(int index)
	{
		if (index != -1)
		{
			int child = _nodes[index].Child1;
			int child2 = _nodes[index].Child2;
			if (!_nodes[index].IsLeaf)
			{
				int height = _nodes[child].Height;
				int height2 = _nodes[child2].Height;
				Mathf.Max(height, height2);
				Bounds aABB = _nodes[child].AABB;
				aABB.Encapsulate(_nodes[child2].AABB);
				_E006(child);
				_E006(child2);
			}
		}
	}
}
