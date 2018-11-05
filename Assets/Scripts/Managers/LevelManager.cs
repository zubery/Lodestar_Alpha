using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string LevelName;
    public TextAsset LevelMapFile;
    private void Start()
    {
        if (GameManager.instance == null) print("what the literal fuck");
        GameManager.instance.CurrentLevel = this;
        LodestarMap.instance.ReadMapFile(LevelMapFile);
    }
}
