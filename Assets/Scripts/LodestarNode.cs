using System.Collections.Generic;
using UnityEngine;

public class LodestarNode : MonoBehaviour
{
    public Vector2 GridPosition;
    public List<LodestarActor> OccupyingActors;
    public bool[] IsNeighbor;

    public LodestarNode()
    {
        GridPosition = Vector2.zero;
        OccupyingActors = new List<LodestarActor>();
        IsNeighbor = new bool[4];
    }
}
