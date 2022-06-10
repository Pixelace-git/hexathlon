using UnityEngine;
using TMPro;

public class PowerManager : MonoBehaviour
{
    public TMP_Text textPower;
    public int power;
    GameObject parent;

    void Start()
    {
        parent = transform.parent.gameObject;
        power = parent.GetComponent<Mushroom>().power;       
        textPower.text = power.ToString();
    }
    
}
