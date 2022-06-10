using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TMP_Text textLevel;
    public int level;
    
    void Start()
    {        
        textLevel.text = "Level " + level.ToString();
    }
    
    void Update()
    {
        textLevel.text = "Level " + level.ToString();
    }
}
