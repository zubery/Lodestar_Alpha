using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class LodestarMap : MonoBehaviour
{
    public static LodestarMap instance;
    private List<List<LodestarNode>> Layout;
    private void Awake()
    {
        #region Singleton Architecture
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
        #endregion
    }

    public LodestarNode GetNodeFromLayout(int _x, int _y)
    {
        if (_x >= 0 && _x < Layout[0].Count && _y >= 0 && _y < Layout.Count)
            return Layout[_y][_x];
        return null;
    }

    public LodestarNode GetNodeFromLayout(LodestarTransform _trans)
    {
        if (_trans == null) return null;
        int _x = _trans.x;
        int _y = _trans.y;
        if (_x >= 0 && _x < Layout[0].Count && _y >= 0 && _y < Layout.Count)
            return Layout[_trans.y][_trans.x];
        return null;
    }

    public void ReadMapFile(TextAsset _mapFile)
    {
        if (_mapFile)
        {
            Layout = MapGenerator.instance.GenerateMapFromFile(_mapFile);
        }
    }



    public bool IsTerrifier(LodestarTransform _GridTransform)
    {
        LodestarNode _target = GetNodeFromLayout(_GridTransform);
        if (!_target) return false;
        for (int i = 0; i < _target.OccupyingActors.Count; ++i)
        {
            if (_target.OccupyingActors[i] && _target.OccupyingActors[i] is LA_Terrifier)
                return true;
        }
        return false;
    }

    public bool IsAttractor(LodestarTransform _GridTransform)
    {
        LodestarNode _target = GetNodeFromLayout(_GridTransform);
        if (!_target) return false;
        for (int i = 0; i < _target.OccupyingActors.Count; ++i)
        {
            if (_target.OccupyingActors[i] && _target.OccupyingActors[i] is LA_Attractor)
                return true;
        }
        return false;
    }

    public LodestarTransform GetConnectedNeighborTransform(LodestarTransform _CurrentGridTransform, int _NeighborDirection)
    {
        LodestarNode _currNode = GetNodeFromLayout(_CurrentGridTransform);
        if (_currNode.IsNeighbor[_NeighborDirection])
        {
            return new LodestarTransform(_currNode.GridPosition + LodestarTransform.GetDeltaPosition(_NeighborDirection));
        }
        return null;
    }
}
