using UnityEngine;

using System.Collections.Generic;

using System.IO;



public class MapGenerator : MonoBehaviour

{

    const string ChaserKey = "C";
    const string WatchtowerKey = "W";

    const string AttractorKey = "A";

    const string TerrifierKey = "T";
    const string PlayerKey = "P";
    const string EndpointKey = "E";

    const string NodeKey = "N";

    const string BridgeKey = "B";



    [Header("Actor Prefabs")]

    [SerializeField] GameObject ChaserPrefab;
    [SerializeField] GameObject WatchtowerPrefab;
    [SerializeField] GameObject AttractorPrefab;
    [SerializeField] GameObject TerrifierPrefab;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EndpointPrefab;
    [SerializeField] GameObject NodePrefab;
    [SerializeField] GameObject BridgePrefab;

    //public TextAsset IFile;

    public GameObject GeneratedMapParent { get; private set; }

    public GameObject GeneratedNodes { get; private set; }

    public GameObject GeneratedActorObjects { get; private set; }

    public List<LodestarActor> GeneratedActorScripts { get; private set; }



    [Header("Map Options")]

    public float MapScalar = 1;

    public Vector3 MapOffset;

    [SerializeField] bool ShouldAutoCenter = false;

    bool IsEndPointGenerated = false, IsPlayerGenerated = false;

    public bool IsMapGenerationSuccess { get; private set; }

    #region Singleton Architecture

    public static MapGenerator instance;

    private void Awake()
    {
        if (instance) Destroy(this);

        else instance = this;

        GeneratedActorScripts = new List<LodestarActor>();

    }
    #endregion



    public List<List<LodestarNode>> GenerateMapFromFile(TextAsset IFile)

    {

        // Check if file exists

        if (!IFile)

        {

            Debug.LogError("Map generation error: Input file for map generator does not exist", this);

            return null;

        }

        // Returnable map

        // Break up file into lists

        string[] _inputRows = IFile.text.Split(new char[] { '\n' });

        // Create readable grid

        List<List<string>> _charMap = new List<List<string>>();

        for (int i = 0; i < _inputRows.Length; ++i)

        {

            _charMap.Add(new List<string>(_inputRows[i].ToUpper().Split(new char[] { ',' })));

        }


        // Create hierarchy
        GeneratedMapParent = new GameObject("GeneratedMapParent");

        GeneratedNodes = new GameObject("GeneratedNodes");

        GeneratedNodes.transform.parent = GeneratedMapParent.transform;

        GeneratedActorObjects = new GameObject("GeneratedActors");

        GeneratedActorObjects.transform.parent = GeneratedMapParent.transform;



        List<List<LodestarNode>> _generatedMap = new List<List<LodestarNode>>();

        Vector2 _edgeNodePositions = -Vector2.one;

        int _numRows = _charMap.Count;

        for (int y = 0; y < _numRows; y += 4)

        {
            List<LodestarNode> _tempNodeList = new List<LodestarNode>();

            int _currMaxCol = _charMap[y].Count;

            for (int x = 0; x < _currMaxCol; x += 4)

            {

                Vector2 _currGridPos = new Vector2(x / 4, y / 4);

                LodestarNode _newNode = null;
                if (_charMap[y][x].Contains(NodeKey))
                {

                    #region NODE SPAWNING

                    GameObject _instantiatedNode = Instantiate(

                        NodePrefab

                        , MapScalar * new Vector3(_currGridPos.x, 0, -_currGridPos.y)

                        , Quaternion.identity
                        , GeneratedNodes.transform

                    );

                    _newNode = _instantiatedNode.GetComponent<LodestarNode>();

                    _newNode.GridPosition = _currGridPos;
                    #endregion

                    #region BRIDGE SPAWNING
                    // Check down
                    if (y + 2 < _numRows && _charMap[y + 2][x].Contains(BridgeKey))
                    {

                        _newNode.IsNeighbor[LodestarDirection.Down] = true;
                        Instantiate(
                            BridgePrefab
                            , MapScalar * new Vector3(_currGridPos.x, 0, -(_currGridPos.y + 0.5f))
                            , Quaternion.identity
                            , GeneratedNodes.transform
                        );
                    }
                    // Check right

                    if (x + 2 < _currMaxCol && _charMap[y][x + 2].Contains(BridgeKey))

                    {

                        _newNode.IsNeighbor[LodestarDirection.Right] = true;

                        Instantiate(

                            BridgePrefab

                            , MapScalar * new Vector3(_currGridPos.x + 0.5f, 0, -_currGridPos.y)

                            , Quaternion.LookRotation(Vector3.right)

                            , GeneratedNodes.transform

                        );

                    }


                    // Check up
                    if (y - 2 >= 0 && _charMap[y - 2][x].Contains(BridgeKey))
                    {
                        _newNode.IsNeighbor[LodestarDirection.Up] = true;
                    }



                    // Check left

                    if (x - 2 >= 0 && _charMap[y][x - 2].Contains(BridgeKey))

                    {

                        _newNode.IsNeighbor[LodestarDirection.Left] = true;

                    }
                    #endregion

                    #region ACTOR SPAWNING
                    // Check for occupying actors
                    if (y + 1 < _numRows)

                    {
                        _newNode.OccupyingActors.AddRange(GenerateActorsFromString(_charMap[y + 1][x], new LodestarTransform(_currGridPos, LodestarDirection.Down)));

                    }

                    if (y - 1 >= 0)

                    {

                        _newNode.OccupyingActors.AddRange(GenerateActorsFromString(_charMap[y - 1][x], new LodestarTransform(_currGridPos, LodestarDirection.Up)));

                    }

                    if (x + 1 < _currMaxCol)

                    {

                        _newNode.OccupyingActors.AddRange(GenerateActorsFromString(_charMap[y][x + 1], new LodestarTransform(_currGridPos, LodestarDirection.Right)));

                    }

                    if (x - 1 >= 0)

                    {

                        _newNode.OccupyingActors.AddRange(GenerateActorsFromString(_charMap[y][x - 1], new LodestarTransform(_currGridPos, LodestarDirection.Left)));

                    }
                    #endregion

                }

                _tempNodeList.Add(_newNode);

            }

            _generatedMap.Add(_tempNodeList);

        }

        if (!IsPlayerGenerated)
        {

            Debug.LogError("Map generation error: No player declared in map file", this);

        }
        if (!IsEndPointGenerated)

        {

            Debug.LogError("Map generation error: No endpoint declared in map file", this);

        }



        // Make map

        if (ShouldAutoCenter)
        {

            MapOffset = MapScalar / 2.0f * new Vector3(-_generatedMap[0].Count, 0, _generatedMap.Count);

        }

        GeneratedMapParent.transform.SetPositionAndRotation(MapOffset, Quaternion.identity);

        IsMapGenerationSuccess = true;

        return _generatedMap;

    }



    private LodestarActor InstantiateActor(GameObject _basePrefab, LodestarTransform _ltrans)
    {
        GameObject _instantiatedActor;
        _instantiatedActor = Instantiate(
            _basePrefab
            , MapScalar * new Vector3(_ltrans.x, 0, -_ltrans.y)
            , _ltrans.GetRotationAsQuaternion()
            , GeneratedActorObjects.transform
          );

        LodestarActor _rtnActor = _instantiatedActor.GetComponent<LodestarActor>();

        _rtnActor.SetGridTransform(_ltrans);

        GeneratedActorScripts.Add(_rtnActor);
        return _rtnActor;

    }



    private List<LodestarActor> GenerateActorsFromString(string _input, LodestarTransform _ltrans)
    {

        List<LodestarActor> _actorList = new List<LodestarActor>();

        // TODO: Transition to dictionary

        if (_input.Contains(ChaserKey))

        {

            _actorList.Add(InstantiateActor(ChaserPrefab, _ltrans));

        }

        if (_input.Contains(WatchtowerKey))

        {
            _actorList.Add(InstantiateActor(WatchtowerPrefab, _ltrans));

        }
        if (_input.Contains(AttractorKey))

        {
            _actorList.Add(InstantiateActor(AttractorPrefab, _ltrans));

        }
        if (_input.Contains(TerrifierKey))

        {
            _actorList.Add(InstantiateActor(TerrifierPrefab, _ltrans));

        }
        if (_input.Contains(PlayerKey))
        {

            if (IsPlayerGenerated)

                Debug.LogError("Map generation error: Multiple endpoints declared in map file.", this);

            else

            {
                _actorList.Add(InstantiateActor(PlayerPrefab, _ltrans));

                IsPlayerGenerated = true;

            }
        }

        if (_input.Contains(EndpointKey))

        {

            if (IsEndPointGenerated)

                Debug.LogError("Map generation error: Multiple endpoints declared in map file.", this);

            else
            {
                _actorList.Add(InstantiateActor(EndpointPrefab, _ltrans));

                IsEndPointGenerated = true;

            }

        }
        return _actorList;

    }

    public bool DestroyActor(LodestarActor _actor)
    {
        LodestarActor _temp;
        _temp = GeneratedActorScripts.Find(x => x == _actor);
        if (_temp != null)
        {
            Destroy(_temp);
            return true;
        }
        return false;
    }

}


