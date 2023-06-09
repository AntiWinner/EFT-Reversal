using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public abstract class QTEAction : MonoBehaviour
{
	public abstract Task<bool> StartAction(QuickTimeEvent quickTimeEvent);
}
