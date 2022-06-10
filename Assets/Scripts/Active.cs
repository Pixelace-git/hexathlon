using UnityEngine;

public class Active : MonoBehaviour
{
    [SerializeField] static GameObject hexActive;
    
    void OnValidate()
    {
        if (hexActive == null)
            hexActive = GameObject.Find("HexActive");
    }

    public static void SetActivePosition()
    {
        /*
        foreach (GameObject go in HexMapGenerate.globalHexPrefabList)
        {
            if (go.GetComponent<Hexagon>().number == Engine.globalActiveHexNumber)
            {
                hexActive.transform.position = go.GetComponent<Hexagon>().transform.position;
            }
        }
        */
    }
}
