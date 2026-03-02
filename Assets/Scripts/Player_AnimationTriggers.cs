using System;
using UnityEngine;

public class Player_AnimationTriggers : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void CurrentStateTrigger()
    {
        _player.CallCurrentStateTrigger();
    }
}