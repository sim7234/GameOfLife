using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class Array : MonoBehaviour
{
    
    public int gridWidth;
    public int gridHeight;

    public SpriteRenderer prefab;
    Cell currentCell;
    Cell[,] cellGrid;

    //only used to instantiate prefabs with no rotation.
    Quaternion noRotation;

    int rnd;

    int currentCellX;
    int currentCellY;

    public float waitTime = 1;
    float waitTimeMinimum;

   public int whenStable = 30;
    public int aliveForRoundCount = 2;
    public int deadForRoundCount = -1;

    Slider slider;
    Camera camera;

    bool pause = false;

    Vector2 mousePos;
    bool drawing = false;


    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        camera = Camera.main;
        slider = Slider.FindAnyObjectByType<Slider>();

        waitTimeMinimum = 0.05f;

        aliveForRoundCount = 2;
        deadForRoundCount = 1;
        

        cellGrid = new Cell[gridWidth, gridHeight];
        

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Creates a prefab (default is cube) at the X,Y Cords.
                SpriteRenderer sprite = Instantiate(prefab, new Vector2(x,y), noRotation);
                //Creates a Cell at the X,Y Cords that we will reference later to change prefabs enabled status.
                cellGrid[x, y] = new Cell(0,true,sprite,x,y,0);

                 rnd = Random.Range(0,10);

               if (rnd == 0)
                {
                    cellGrid[x, y].alive = true;
                    
                }
                else cellGrid[x, y].alive = false;
            }
            
        }
        

    NextGen();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }

        


        if (Input.GetMouseButtonDown(0))
            drawing = true;

        if (Input.GetMouseButtonUp(0))
            drawing = false;

        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

       


    }

    private void NextGen()
    {

        if (pause == false)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    currentCell = cellGrid[x, y];
                    currentCell.numberOfNeighbors = 0;
                    GetNeighbors(x, y);

                }

            }



            ApplyRules();
        }
        else
            Invoke("draw", waitTime);

        waitTime = slider.value;

        if (waitTime < waitTimeMinimum)
            waitTime = waitTimeMinimum;


        


        
            Invoke("NextGen", waitTime);
       

    }

    void GetNeighbors(int x, int y)
    {

        for (int i = 0; i < 3; i++)
         {
             for (int j = 0; j < 3; j++)
             {
                Vector2 currentNeighbor = new Vector2(x + 1 - i, y + 1 - j);
                if (!(currentNeighbor.y >= gridHeight || currentNeighbor.x >= gridWidth || currentNeighbor.x < 0 || currentNeighbor.y < 0))
                {
                    if (cellGrid[x + 1 - i, y + 1 - j].alive)
                    {
                        if (cellGrid[x + 1 - i, y + 1 - j].position != currentCell.position)
                        {
                            currentCell.numberOfNeighbors++;
                        }
                    }
                }
             }
         }

    }
    void ApplyRules()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                currentCell = cellGrid[x, y];

                if (currentCell.numberOfNeighbors < 2)
                    currentCell.alive = false;
                

                if (currentCell.numberOfNeighbors > 3)
                    currentCell.alive = false;

                if (currentCell.numberOfNeighbors == 3 && currentCell.alive == false)
                    currentCell.alive = true;




                

                //Checks if the cells or simulation is stable
                if (currentCell.alive)
                {
                    //add more then we remove to highlight stable patterns as well as stable cells
                    currentCell.stableForTime += aliveForRoundCount;
                }
                else currentCell.stableForTime -= deadForRoundCount;

                if (currentCell.stableForTime < 0)
                    currentCell.stableForTime = 0;


                if (currentCell.stableForTime > whenStable)
                    currentCell.stableForTime = whenStable + 1;



                if (cellGrid[x, y].stableForTime >= whenStable)
                {
                    cellGrid[x, y].spriteRenderer.color = Color.green;
                }


                if (cellGrid[x, y].stableForTime < whenStable / 3)
                {
                    cellGrid[x, y].spriteRenderer.color = Color.red;
                }


                if (cellGrid[x, y].stableForTime > whenStable / 3 && cellGrid[x, y].stableForTime < whenStable)
                {
                    cellGrid[x, y].spriteRenderer.color = Color.magenta;
                }




                //Color fade
                if (cellGrid[x,y].alive == false)
                {
                    if (cellGrid[x, y].stableForTime < whenStable / 3)
                    {
                        cellGrid[x, y].spriteRenderer.color = new Color(1f, 0f, 0f, cellGrid[x, y].stableForTime * 0.1f);
                    }

                    if (cellGrid[x, y].stableForTime <= 0)
                    {
                        cellGrid[x, y].spriteRenderer.enabled = cellGrid[x, y].alive;
                    }
                }
                else
                {
                    cellGrid[x, y].spriteRenderer.enabled = true;
                }

                
                
                
                
                
            }

        }
    }



    
    void draw()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {

                Debug.Log(cellGrid[x, y].position);
               mousePos = new Vector2(Mathf.RoundToInt(mousePos.x),Mathf.RoundToInt(mousePos.y));

                if (cellGrid[x,y].position == mousePos)
                {
                    cellGrid[x, y].alive = false;
                    cellGrid[x, y].spriteRenderer.enabled = cellGrid[x, y].alive;
                }
            }
        }
    }

    


}
    public class Cell
    {
        public int numberOfNeighbors;
        public bool alive;
        public SpriteRenderer spriteRenderer;
        public Vector2 position;
        public int stableForTime;

        public Cell(int neighbors, bool isAlive, SpriteRenderer sprite, int posX, int posY, int aliveTime)
    {
        position = new Vector2(posX, posY);
        numberOfNeighbors = neighbors;
        alive = isAlive;
        spriteRenderer = sprite;
        stableForTime = aliveTime;
    }



}
