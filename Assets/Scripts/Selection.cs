using UnityEngine;

public class Selection : MonoBehaviour
{
    [SerializeField] static GameObject hexSelection;    

    void OnValidate()
    {
        if (hexSelection == null)
            hexSelection = GameObject.Find("HexSelection");
    }

    public static void SetSelectedPosition()
    {
        hexSelection.SetActive(true);
        foreach (GameObject go in HexMapGenerate.globalHexPrefabList)
        {              
            if (go.GetComponent<Hexagon>().number == Engine.globalSelectedHexNumber)
            {  
                hexSelection.transform.position = go.GetComponent<Hexagon>().transform.position;
            }
        }
    }

    public static void ReleseSelectedPosition()
    {
        hexSelection.SetActive(false);
    }

}
