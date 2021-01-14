using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    public enum GameMode
    {
        NORMAL,
        ATTACK
    }

    public enum State
    {
        IDLE,
        WALK,
        ATTACK,
        DESTROYED
    }

    public enum Direction
    {
        BOTTOM,
        BOTTOM_RIGHT,
        RIGHT,
        TOP_RIGHT,
        TOP,
        TOP_LEFT,
        LEFT,
        BOTTOM_LEFT
    }

    public enum RenderingLayer
    {
        GROUND = 0,
        SHADOW = 1,
        SPRITE = 2
    }
}
