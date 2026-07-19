using Entity;
using UnityEngine;

namespace Core.State
{
    public interface IStateOwner
    {
        Animator ani { get; }
        Rigidbody2D rb { get; }
        EntityStat stat { get; }
    }
}