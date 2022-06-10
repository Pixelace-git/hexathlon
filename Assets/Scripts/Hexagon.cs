using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public enum HexDirections
    {
        Up = 0,
        UpRight = 1,
        DownRight = 2,
        Down = 3,
        DownLeft = 4,
        UpLeft = 5
    }

    public int number;
    public int line;
    public string type;
    public GameObject prefab;
    public Vector2 matrixPosition;
    public Hexagon[] neighborsArray = new Hexagon[6];
    public bool isActive;

    public GameObject enemy;

    public HexDirections OppositeDirection(HexDirections dir)
    {
        HexDirections hd = HexDirections.Up;

        if (dir == HexDirections.Up) 
            hd = HexDirections.Down;
        if (dir == HexDirections.UpRight) 
            hd = HexDirections.DownLeft;
        if (dir == HexDirections.DownRight) 
            hd = HexDirections.UpLeft;
        if (dir == HexDirections.Down) 
            hd = HexDirections.Up;
        if (dir == HexDirections.DownLeft) 
            hd = HexDirections.UpRight;
        if(dir == HexDirections.UpLeft)
            hd = HexDirections.DownRight;

        return hd;
    }
    
    public void AddNeighbor(HexDirections dir, Hexagon neighbor)
    {
        neighborsArray[(int)dir] = neighbor;
    }

    public HexDirections PositionToDir(int selfX, int selfY, int x, int y)
    {
        HexDirections hd = HexDirections.Up;

        // 0) 1,1 => 1,2  
        if (selfX == x && selfY < y)
            hd = HexDirections.Up;              

        // 1) 1,1 => 2,1
        if (selfX < x && selfY == y)
            hd = HexDirections.UpRight;

        // 2) 1,1 => 2,0
        if (selfX < x && selfY > y)
            hd = HexDirections.DownRight;

        // 3) 1,1 => 1,0
        if (selfX == x && selfY > y)
            hd = HexDirections.Down;

        // 4) 1,1 => 0,1
        if (selfX > x && selfY == y)
            hd = HexDirections.DownLeft;       

        // 5) 1,1 => 0,2
        if (selfX > x && selfY < y)
            hd = HexDirections.UpLeft; 

        return hd;
    }

    public void SetNeighborActivity()
    {
        foreach(Hexagon hex in neighborsArray)
        {
            if (hex != null)
            {
                hex.isActive = true;
                hex.SetInvisibility(true);
                //hex.enemy.GetComponent<Mushroom>().isVisible = true;
            }
        }
    }

    public void SetInvisibility(bool active)
    {       
        prefab.SetActive(active);
        if (enemy != null)
        {
            if (enemy.GetComponent<Mushroom>().isAlive)
            {
                enemy.GetComponent<Mushroom>().isVisible = active;
                enemy.GetComponent<Mushroom>().SetVisibility(active);
            }
        }
    }
}
