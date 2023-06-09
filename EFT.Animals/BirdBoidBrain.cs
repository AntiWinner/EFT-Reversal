using System.Collections;
using UnityEngine;

namespace EFT.Animals;

[RequireComponent(typeof(Bird))]
public class BirdBoidBrain : MonoBehaviour
{
	protected const string _birdsLayer = "Water";

	protected Bird _bird;

	protected int _layersMask;

	protected SphereCollider _collider;

	protected Vector3 _cohesion;

	protected Vector3 _separation;

	protected Vector3 _alignment;

	[HideInInspector]
	public Vector3 Velocity;

	[HideInInspector]
	public Transform TransformCache;

	protected BirdBoidsSpawner _swarmBrain;

	public void Init(BirdBoidsSpawner swarmBrain)
	{
		_swarmBrain = swarmBrain;
	}

	protected void Start()
	{
		TransformCache = base.transform;
		_bird = GetComponent<Bird>();
		_collider = base.gameObject.AddComponent<SphereCollider>();
		_collider.isTrigger = true;
		_layersMask = LayerMask.GetMask(_ED3E._E000(60801));
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(60801));
		StartCoroutine(CalculateVelocity());
		StartCoroutine(UpdateBirdDirection());
	}

	protected void Update()
	{
		Vector3 vector = TransformCache.position - _swarmBrain.TransformCache.position;
		if (vector.sqrMagnitude > _swarmBrain.SwarmSqrRadius)
		{
			Velocity -= vector / (_swarmBrain.SwarmSqrRadius / 2f);
		}
		TransformCache.position += Velocity * Time.deltaTime * _swarmBrain.VelocityMult;
	}

	protected IEnumerator CalculateVelocity()
	{
		Velocity = Random.onUnitSphere * _swarmBrain.BaseSpeed;
		while (true)
		{
			_cohesion = Vector3.zero;
			_separation = Vector3.zero;
			_alignment = Vector3.zero;
			Velocity = Vector3.zero;
			Collider[] array = Physics.OverlapSphere(TransformCache.position, _swarmBrain.CohesionRadius, _layersMask);
			if (array.Length >= _swarmBrain.MinBoidsForCohesion)
			{
				int num = 0;
				int num2 = ((array.Length > _swarmBrain.MaxBoidsForCohesion) ? _swarmBrain.MaxBoidsForCohesion : array.Length);
				for (int i = 0; i < num2; i++)
				{
					Collider obj = array[i];
					BirdBoidBrain component = obj.gameObject.GetComponent<BirdBoidBrain>();
					_cohesion += component.TransformCache.position;
					_alignment += component.Velocity;
					Vector3 vector = TransformCache.position - component.TransformCache.position;
					float sqrMagnitude = vector.sqrMagnitude;
					if (obj != _collider && sqrMagnitude < _swarmBrain.SeparationMaxSqrDistance)
					{
						_separation += vector / sqrMagnitude;
						num++;
					}
				}
				_cohesion /= (float)num2;
				_cohesion -= TransformCache.position;
				_cohesion /= (float)num2;
				if (num > 0)
				{
					_separation /= (float)num;
				}
				_separation *= _swarmBrain.SeparationMult;
				Velocity += _cohesion * _swarmBrain.CohesionMult + _separation * _swarmBrain.SeparationMult + _alignment * _swarmBrain.AlignmentMult;
				Velocity = Vector3.ClampMagnitude(Velocity, _swarmBrain.BaseSpeed);
			}
			yield return new WaitForSeconds(_swarmBrain.CalculationVelocityInterval);
		}
	}

	protected IEnumerator UpdateBirdDirection()
	{
		while (true)
		{
			_bird.SetDirection(Velocity.normalized, Velocity.magnitude);
			yield return new WaitForSeconds(_swarmBrain.UpdateDirectionInterval);
		}
	}
}
