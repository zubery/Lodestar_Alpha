using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodestarTransform
{
    public Vector2 Position;
    public LodestarDirection Rotation;


    public override string ToString()
    {
        string _rtn = "Pos: (" + Position.x + ", " + Position.y + ")\n";
        _rtn += "Rot: ";
        if (Rotation.Current == LodestarDirection.Up)
            _rtn += "Up";
        else if (Rotation.Current == LodestarDirection.Down)
            _rtn += "Down";
        else if (Rotation.Current == LodestarDirection.Left)
            _rtn += "Left";
        else
            _rtn += "Right";       
        return _rtn;
    }

    public int x
    {
        get
        {
            return (int)Position.x;
        }
    }
    public int y
    {
        get
        {
            return (int)Position.y;
        }
    }

    #region Constructors
    public LodestarTransform()
    {
        Position = Vector2.zero;
        Rotation = new LodestarDirection(0);
    }

    public LodestarTransform(Vector2 _pos, LodestarDirection _dir)
    {
        Position = _pos;
        Rotation = _dir;
    }

    public LodestarTransform(Vector2 _pos, int _rot = 0)
    {
        Position = _pos;
        Rotation = new LodestarDirection(_rot);
    }
    #endregion

    #region Position Methods
    // NOTE: UNSAFE -> DO ERROR CHECKING!
    public Vector2 Advance()
    {
        if (Rotation.Current == LodestarDirection.Up)
            Position.y -= 1;
        else if (Rotation.Current == LodestarDirection.Down)
            Position.y += 1;
        else if (Rotation.Current == LodestarDirection.Left)
            Position.x -= 1;
        else
            Position.x += 1;
        return Position;
    }

    public static Vector2 GetDeltaPosition(int _direction)
    {
        Vector2 _deltaPos = Vector2.zero;
        if (_direction == LodestarDirection.Up)
            _deltaPos.y = -1;
        else if (_direction == LodestarDirection.Down)
            _deltaPos.y = 1;
        else if (_direction == LodestarDirection.Left)
            _deltaPos.x = -1;
        else
            _deltaPos.x = 1;
        return _deltaPos;
    }


    public Vector2 SetPosition(Vector2 _newPos)
    {
        Position = _newPos;
        return Position;
    }
    #endregion

    #region Rotation Methods
    public int TurnLeft()
    {
        return Rotation.Rotate(3);
    }

    public int TurnAround()
    {
        return Rotation.Rotate(2);
    }

    public int TurnRight()
    {
        return Rotation.Rotate(1);
    }


    public void SetRotation(LodestarDirection _newRot)
    {
        Rotation = _newRot;
    }

    public void SetRotation(int _newRot)
    {
        Rotation.SetRotation(_newRot);
    }

    public Quaternion GetRotationAsQuaternion()
    {
        return LodestarDirection.ToQuaternion(Rotation.Current);
    }
    #endregion

    public Vector3 GetWorldSpacePosition()
    {
        if (MapGenerator.instance)
            return MapGenerator.instance.MapScalar * new Vector3(Position.x, 0, -Position.y) + MapGenerator.instance.MapOffset;
        else
        {
            Debug.LogError("No map generator present");
            return Vector3.zero;
        }
    }
}
