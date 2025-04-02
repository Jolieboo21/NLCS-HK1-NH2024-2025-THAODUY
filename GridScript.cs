using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform[,] grid;
    public int width=10,height=20;
    // Start is called before the first frame update
    
    void Start()
    {
        grid = new Transform[width, height];
    }

    public void UpdateGrid(Transform tetromino)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino)
                    {
                        grid[x, y] = null;
                    }
                }

            }
        }
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    


    }

    public bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x<width && (int)pos.y >= 0 && (int) pos.y<height;
    }

    public Transform GetTransformArGridPosition(Vector2 pos)
    {
        if(pos.y > height - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }


    public bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);
            if (!IsInsideBorder(pos))
            {
                return false;
            }
            if(GetTransformArGridPosition(pos) != null &&  GetTransformArGridPosition(pos).parent != tetromino)
            {
                return false;
            }
        }
        return true;
    }

    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public int CheckForLines()
    {
        int linesCleared = 0;
        for (int y=0; y< height; y++)
        {
            if (LineIsFull(y))
            {
                DeleteLine(y);
                DecreaseRowAbove(y+1);
                y--;
                linesCleared++;
            }
        }
        return linesCleared;
    }
    bool LineIsFull(int y)
    {
        for (int x=0; x< width; x++)
        {
            if (grid[x,y] == null)
            {
                return false;   
            }
        }
        return true;    
    }

    void DeleteLine(int y)
    {
        for (int x=0; x< width; x++)
        {
            Destroy(grid[x,y].gameObject);
            grid[x,y] = null;
        }
    }

    public bool IsGameOver()
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)  // Nếu có khối nào ở hàng trên cùng
            {
                return true;
            }
        }
        return false;
    }

    void DecreaseRowAbove(int startRow)
    {
        for (int y = startRow; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x,y] != null)
                {
                    grid[x,y-1] = grid[x,y];
                    grid[x, y] = null;
                    grid[x, y-1].position += Vector3.down;


                }
            }
        }
    }
}
