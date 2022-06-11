using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexMapGenerate : MonoBehaviour
{
    [SerializeField, Min(3)] int mapSize = 10;
    [SerializeField, Range(1,30)] int GapChance;
    public List<GameObject> HexagonTypeList;

    GameObject HexPrefab;
    public static List<GameObject> globalHexPrefabList;
       
    public enum HexTypes { Normal, Fire, Grass, Water };

    void Awake()
    {
        if (HexagonTypeList.Count == 0)
        {           
            //in a folder called "Resources" like this "Assets/Resources/Prefabs" 
            HexagonTypeList = Resources.LoadAll<GameObject>("Prefabs").ToList();
        }

        globalHexPrefabList = new List<GameObject>();
        globalHexPrefabList.Clear();

        Engine.ClearObjects();
        GenerateMap();       
        HexagonConnect();        
        Engine.PowerCallculate(mapSize);
        SetEnemies();
        SetToCurrent();
    }

    private void SetEnemies()
    {
        foreach (GameObject currentObj in globalHexPrefabList)
        {
            int _number = currentObj.GetComponent<Hexagon>().number;
            if (_number == 1)
                continue;               

            GameObject newEnemy = Engine.GetEnemy(_number);
            currentObj.GetComponent<Hexagon>().enemy = newEnemy;            
        }
    }

    public void GenerateMap()
    {
        int _number = 1;        

        for (int column = 0; column < mapSize; column++)
        {
            for (int row = 0; row < mapSize; row++)
            {                    
                if (_number == 1)
                {
                    SetHexagonValues(_number, column, row);                        
                }
                else
                {
                    int rnd = Random.Range(0, HexagonTypeList.Count);
                    HexPrefab = HexagonTypeList[rnd];
                    SetHexagonValues(_number, column, row, rnd);                    
                }     
                
                _number++; 

                Hex h = new Hex(column, row);

                // Declaration: public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                GameObject go = Instantiate(
                    HexPrefab,              // original
                    h.Position(),           // position
                    Quaternion.identity,    // rotation
                    this.transform          // parent
                    );

                globalHexPrefabList.Add(go);
                Engine.AddObject(go);              
            }
        }      
    }    

    void SetHexagonValues(int serialNumber, int column, int row, int rnd=0)
    {
        HexPrefab = HexagonTypeList[rnd];
        HexPrefab.GetComponent<Hexagon>().type = SetType(rnd).ToString();
        HexPrefab.GetComponent<Hexagon>().prefab = HexagonTypeList[rnd];
        HexPrefab.GetComponent<Hexagon>().number = serialNumber;
        HexPrefab.GetComponent<Hexagon>().line = SetLine(serialNumber, mapSize);
        HexPrefab.GetComponent<Hexagon>().matrixPosition = new Vector2(column, row);
        HexPrefab.GetComponent<Hexagon>().isActive = false;
        HexPrefab.GetComponent<Hexagon>().SetInvisibility(false);
        // HexPrefab.GetComponent<Hexagon>().transform.GetChild(0).gameObject.SetActive(false);        
    }

    void SetToCurrent()
    {
        GameObject obj = Engine.GetObjectWhithId(1);
        Engine.globalCurrentHexNumber = obj.GetComponent<Hexagon>().number;
        obj.GetComponent<Hexagon>().isActive = true;
        obj.GetComponent<Hexagon>().SetInvisibility(true); 
        //obj.GetComponent<Hexagon>().transform.GetChild(0).gameObject.SetActive(true);
        obj.GetComponent<Hexagon>().SetNeighborActivity();
    }
    
    public void HexagonConnect()
    {
        foreach (GameObject currentObj in globalHexPrefabList)
        {
            int currentX = (int)currentObj.GetComponent<Hexagon>().matrixPosition.x;
            int currentY = (int)currentObj.GetComponent<Hexagon>().matrixPosition.y;
            int currentLine = currentObj.GetComponent<Hexagon>().line;  

            foreach (GameObject otherObj in globalHexPrefabList)
            {
                int otherX = (int)otherObj.GetComponent<Hexagon>().matrixPosition.x;
                int otherY = (int)otherObj.GetComponent<Hexagon>().matrixPosition.y;

                if ((currentX == otherX) && (currentY == otherY))
                {                    
                    continue;
                }
                
                int otherLine = otherObj.GetComponent<Hexagon>().line;

                if (otherLine > currentLine + 1 || otherLine < currentLine - 1)
                {                    
                    continue;
                }

                //összehasonlítás 2 vel nagyobbe
                if (currentX + 1 < otherX || currentY + 1 < otherY)
                    continue;
                if (currentX - 1 > otherX || currentY - 1 > otherY)
                    continue;

                    Hexagon.HexDirections direction = currentObj.GetComponent<Hexagon>().PositionToDir(currentX, currentY, otherX, otherY);
               // Debug.Log("Line:"+currentLine+ " " +currentX+":"+ currentY+" | Line:" + otherLine + " " +otherX+":"+otherY+" Direction: " + direction);
                currentObj.GetComponent<Hexagon>().AddNeighbor(direction, otherObj.GetComponent<Hexagon>());
               
            }
        }
    }

    public static HexTypes SetType(int number)
    {
        HexTypes type = HexTypes.Normal;

        if (number == 0)
            type = HexTypes.Normal;
        else if (number == 1)
            type = HexTypes.Fire;
        else if (number == 2)
            type = HexTypes.Grass;
        else if (number == 3)
            type = HexTypes.Water;
        else
            Debug.Log("Types wrong!");

        return type;
    }
     
    int SetLine(int _number, int _mapsize)
    {        
        int _line = _number % _mapsize;
        if (_line == 0)
            _line = (_number / _mapsize) + (_mapsize -1);
        else
            _line += _number / _mapsize; 
        return _line;
    }  
}
