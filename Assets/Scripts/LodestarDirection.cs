using UnityEngine;


//TODO: FIX THIS ASS CLASS

public class LodestarDirection
{
    public static int Up = 0;
    public static int Right = 1;
    public static int Down = 2;
    public static int Left = 3;

    public int Current;

    public Quaternion CurrentQuaternion
    {
        get
        {
            return ToQuaternion(Current);
        }
    }

    public LodestarDirection()
    {
        Current = Up;
    }

    public LodestarDirection(int _rot)
    {
        Current = _rot % 4;
    }

    public int Rotate(int _numCWQuarterTurns)
    {
        Current = (Current + _numCWQuarterTurns) % 4;
        return Current;
    }

    public int SetRotation(int _rotation)
    {
        Current = _rotation % 4;
        return Current;
    }

    public static Quaternion ToQuaternion(int _rot)
    {
        if (_rot == Up)
            return Quaternion.LookRotation(Vector3.forward);
        else if (_rot == Down)
            return Quaternion.LookRotation(Vector3.back);
        else if (_rot == Left)
            return Quaternion.LookRotation(Vector3.left);
        else
            return Quaternion.LookRotation(Vector3.right);
    }
}

