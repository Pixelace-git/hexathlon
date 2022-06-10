using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int id;
    public int power;
    public string type;
    public bool isVisible; 
    public bool isAlive;

    [SerializeField] Material green;
    [SerializeField] Material red;
    [SerializeField] Material blue;
       
    GameObject _currentHex;

    void Start()
    {
        //Check
        if (green == null)
            Debug.Log("Green mushroom material is missing!");
        if (red == null)
            Debug.Log("Red mushroom material is missing!");
        if (blue == null)
            Debug.Log("Blue mushroom material is missing!");
    }

    void Update()
    {
        _currentHex = Engine.GetObjectWhithId(Engine.globalCurrentHexNumber); 
        Vector3 targetPos = _currentHex.transform.position; 

        Vector3 lookDir = targetPos - transform.position;

        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);            
        }       
    }

    public void SetVisibility(bool state)
    {        
        gameObject.SetActive(state);        
    }

    public void SetMaterial()
    {  
        // Mushroom > MushroomMonster > MushroomMon
        Transform child = transform.GetChild(0).GetChild(1);              
       
        if (type == "Grass")       
            child.GetComponent<Renderer>().material = green;
        if (type == "Water")
            child.GetComponent<Renderer>().material = blue;
        if (type == "Fire")
            child.GetComponent<Renderer>().material = red;      
        
    }
}
