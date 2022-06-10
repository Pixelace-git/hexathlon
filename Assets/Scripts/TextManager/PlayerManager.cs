using UnityEngine;
using TMPro;
public class PlayerManager : MonoBehaviour
{
    public TMP_Text textPower;   

    void Update()
    {       
        textPower.text = Engine.playerPower.ToString();
    }
}
