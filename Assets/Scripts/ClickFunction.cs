using System.Reflection;
using UnityEngine;

public class ClickFunction : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    int _number;

    void Awake()
    {
        if (prefab == null)
        {
           prefab = this.GetComponent<Hexagon>().prefab;
        }

        _number = prefab.GetComponent<Hexagon>().number;
    }

    void OnMouseDown()
    {
        GameObject _enemy = prefab.GetComponent<Hexagon>().enemy;
        // Hex is free?
        if (_enemy != null)
        {
            bool isFree = _enemy.GetComponent<Mushroom>().isAlive;            
            if (isFree)
            {
                //Debug.Log("Is NOT free!");
                Engine.BattleSystem(_enemy);   
            }
            else
            {
                Debug.Log("Is free!");
            }
        }
        else
        {
            Debug.Log("Is starter hex");
        }
        Engine.globalCurrentHexNumber = _number;
    }

    void OnMouseEnter()
    {
        if (this.GetComponent<Hexagon>().isActive)
        {
            if (this.GetComponent<Hexagon>().number != Engine.globalCurrentHexNumber)
            {
                Engine.globalSelectedHexNumber = _number;
                //Debug.Log(_number);
            }
        }
    }

    void OnMouseExit()
    {
        Engine.globalSelectedHexNumber = -1;
    }

}
