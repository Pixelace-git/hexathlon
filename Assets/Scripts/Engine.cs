using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public GameObject completLevelUI;
    public static GameObject gameOverUI;   

    [SerializeField] public static int playerPower = 4;

    public static int globalSelectedHexNumber;
    public static int globalCurrentHexNumber;
   
    public static Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    static int _nextObjectId = 1;

    public static GameObject enemy;
    public static Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
    static int _nextEnemyId = 2;
    static int _killedEnemy = 0;
    public static List<int> powerList = new List<int>();

    GameObject _gohs; // Game Object Hex Selection
    GameObject _gohc; // Game Object Hex Current

    int _holder;

    void Awake()
    {
        
        _gohs = GameObject.Find("HexSelection");
        _gohs.SetActive(false);
       
        _gohc = GameObject.Find("HexCurrent");
        _gohc.SetActive(true);   
        
        if(enemy == null)        
            enemy = GameObject.Find("Mushrooms");

        enemy.SetActive(true);

        if (completLevelUI == null)
            completLevelUI = GameObject.Find("LevelComplete");

        completLevelUI.SetActive(false);

        if (gameOverUI == null)
            gameOverUI = GameObject.Find("GameOver");

        gameOverUI.SetActive(false);
      
    }

    void Start()
    {
        enemy.SetActive(false);
    }

    public static GameObject GetEnemy(int number)
    {
        GameObject parent = GameObject.Find("Enemies");

        enemy.SetActive(true);

        // Declaration: public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
        GameObject go = Instantiate(
            enemy,                                          // original
            GetObjectWhithId(number).transform.position,    // position
            Quaternion.identity,                            // rotation            
            parent.transform
            );

        int _rnd = Random.Range(1, 4);

        // Set parameters
        go.GetComponent<Mushroom>().id = number;
        go.GetComponent<Mushroom>().power = powerList[number-1];
        go.GetComponent<Mushroom>().type = HexMapGenerate.SetType(_rnd).ToString();
        go.GetComponent<Mushroom>().isVisible = false;
        go.GetComponent<Mushroom>().SetVisibility(false);
        go.GetComponent<Mushroom>().isAlive = true;
        go.GetComponent<Mushroom>().SetMaterial();

        AddEnemy(go);

        return go;       
    }

    public static void AddEnemy(GameObject go)
    {
        enemies.Add(_nextEnemyId, go);
        _nextEnemyId++;
    }

    public static GameObject GetEnemyWhithId(int id)
    {
        if(enemies.ContainsKey(id))
        {
            return enemies[id];
        }
        return null;
    }

    public static void AddObject(GameObject go)
    {
        objects.Add(_nextObjectId, go);
        _nextObjectId++;
    }

    public static GameObject GetObjectWhithId(int id)
    {
        if (objects.ContainsKey(id))
        {
            return objects[id];
        }
        //Debug.LogWarningFormat("Object with id: {0} doesn't exist!", id);
        return null;
    }

    void Update()
    {
        if (GetObjectWhithId(globalSelectedHexNumber) != null)
        { 
            _gohs.SetActive(true);
            _gohs.transform.position = GetObjectWhithId(globalSelectedHexNumber).transform.position;           
        }
        else
        {
            _gohs.SetActive(false);
        }

        if (globalCurrentHexNumber != _holder)
        {
            GameObject newCurrent = GetObjectWhithId(globalCurrentHexNumber);
            _gohc.transform.position = newCurrent.transform.position;
            newCurrent.GetComponent<Hexagon>().SetNeighborActivity();
            _holder = globalCurrentHexNumber;
        }
        CompleteLevel(enemies.Count);
    }

    public static void PowerCallculate(int mapSize)
    {
        int _pieceOfHexagon = objects.Count;        
        int _valueHolder;

        //Debug.Log("Piece of Hexagon: "+ _pieceOfHexagon);

        // Upload
        for (int index = 1; index <= _pieceOfHexagon; index++)
        {            
            powerList.Add(index);
        }
       
        // 2nd place to right position
        _valueHolder = powerList[2];
        powerList[2] = powerList[mapSize];
        powerList[mapSize] = _valueHolder;
        
        /*
        if (_pieceOfHexagon > 4)
        {
            // Mix
            for (int index = 0; index <= mixingNumber; index++)
            {
                if(index == 1)
                {

                }

                // No 1st, 2nd and last place
                int _place1 = Random.Range(2, _pieceOfHexagon - 2);
                _valueHolder = powerList[_place1];
                int _place2 = Random.Range(2, _pieceOfHexagon - 2);
                if(_place1 != _place2)
                {
                    powerList[_place1] = powerList[_place2];
                    powerList[_place2] = _valueHolder;
                }
                else
                {
                    mixingNumber++;
                }
            }
        }*/
    }

    public static void BattleSystem(GameObject enemy)
    {
        int _enemyPower = enemy.GetComponent<Mushroom>().power;
        int _number = enemy.GetComponent<Mushroom>().id;      
        int _calculatedPower = CalculateFinalPower(_number);
       
        if (_calculatedPower < playerPower)
        {
            //playerPower += _enemyPower;
            //playerPower++;

            GameObject _hex = GetObjectWhithId(_number);

            playerPower += _hex.GetComponent<Hexagon>().line;

            enemy.GetComponent<Mushroom>().isAlive = false;
            enemy.GetComponent<Mushroom>().isVisible = false;
            enemy.GetComponent<Mushroom>().SetVisibility(false);
            //Debug.Log("Enemy ID: "+enemy.GetComponent<Mushroom>().id + " is dead!");
            EnemyIsKilled();
        }
        else
        {
            gameOverUI.SetActive(true);
            Debug.Log("You loose! Calulated Power: "+_calculatedPower);
        }
    }

    public static int CalculateFinalPower(int number)
    {
        int _finalPower = 0;

        GameObject _currentHex = GetObjectWhithId(number);
        string _hexType = _currentHex.GetComponent<Hexagon>().type;      
        GameObject _currnetEnemy = GetEnemyWhithId(number);
        string _enemyType = _currnetEnemy.GetComponent<Mushroom>().type;      
        _finalPower = _currnetEnemy.GetComponent<Mushroom>().power;      

        if (_enemyType == "Water")
        {
            if (_hexType == "Water")
                _finalPower *= 2;
            if (_hexType == "Grass")
                _finalPower = Mathf.RoundToInt(_finalPower * 0.75f);
        }

        if (_enemyType == "Fire")
        {
            if (_hexType == "Fire")
                _finalPower *= 2;
            if (_hexType == "Water")
                _finalPower = Mathf.RoundToInt(_finalPower * 0.75f);
        }

        if (_enemyType == "Grass")
        {
            if (_hexType == "Grass")
                _finalPower *= 2;
            if (_hexType == "Fire")
                _finalPower = Mathf.RoundToInt(_finalPower * 0.75f);
        }

        return _finalPower;
    }

    public static void EnemyIsKilled()
    {
        _killedEnemy++;
    }   

    public void CompleteLevel(int all)
    {        
        if(_killedEnemy == all)
        {
            completLevelUI.SetActive(true);         
        }
    }

}
