using System;
using UnityEngine;

namespace Learning.GUIRef.Base;

public enum E_Alignment_Type
{
    Up,
    Down,
    Left,
    Right,
    Center,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
}

[Serializable]
public class CustomGUIPosition
{
    private Rect m_rect = new(0, 0, 100, 100);
    public E_Alignment_Type screen_alignment_type = E_Alignment_Type.Center;
    public E_Alignment_Type center_alignment_type = E_Alignment_Type.Center;
    public Vector2 offset;
    public float width  = 100;
    public float height = 50;

    private Vector2 ScreenPosByType() => screen_alignment_type switch
    {
        E_Alignment_Type.Up        => new Vector2(Screen.width/2f, 0),
        E_Alignment_Type.UpLeft    => new Vector2(0              , 0),
        E_Alignment_Type.UpRight   => new Vector2(Screen.width   , 0),
        E_Alignment_Type.Down      => new Vector2(Screen.width/2f, Screen.height),
        E_Alignment_Type.DownLeft  => new Vector2(0              , Screen.height),
        E_Alignment_Type.DownRight => new Vector2(Screen.width   , Screen.height),
        E_Alignment_Type.Center    => new Vector2(Screen.width/2f, Screen.height/2f),
        E_Alignment_Type.Left      => new Vector2(0              , Screen.height/2f),
        E_Alignment_Type.Right     => new Vector2(Screen.width   , Screen.height/2f),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    private Vector2 CenterPosByType() => center_alignment_type switch
    {
        E_Alignment_Type.Up        => new Vector2(-width/2f, 0),
        E_Alignment_Type.UpLeft    => new Vector2(0        , 0),
        E_Alignment_Type.UpRight   => new Vector2(-width   , 0),
        E_Alignment_Type.Down      => new Vector2(-width/2f, -height),
        E_Alignment_Type.DownLeft  => new Vector2(0        , -height),
        E_Alignment_Type.DownRight => new Vector2(-width   , -height),
        E_Alignment_Type.Center    => new Vector2(-width/2f, -height/2f),
        E_Alignment_Type.Left      => new Vector2(0        , -height/2f),
        E_Alignment_Type.Right     => new Vector2(-width   , -height/2f),
        _ => throw new ArgumentOutOfRangeException()
    };

    public Rect rect
    {
        get
        {
            m_rect.position = ScreenPosByType() + CenterPosByType() + offset;
            m_rect.width = width;
            m_rect.height = height;
            return m_rect;
        }
    }
}