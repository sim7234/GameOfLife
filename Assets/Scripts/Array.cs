using UnityEngine;
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

    public int percentAlive = 10;
    int rnd;

    int currentCellX;
    int currentCellY;

    public float waitTime = 1;
    float waitTimeMinimum;

   public int whenStable = 30;
    public int aliveForRoundCount = 2;
    public int deadForRoundCount = -1;

    public Slider generationSpeedSlider;
    new Camera camera;

    bool pause = false;

    Vector2Int mousePos;
    Vector2Int lastMousePos;
    Vector2Int mousePosDelta;
    bool mouseClicked = false;

    public GameObject background;


    bool drawing = false;
    bool erasing = false;

    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        camera = Camera.main;

        generationSpeedSlider = Slider.FindAnyObjectByType<Slider>();
        generationSpeedSlider.value = waitTime;

        waitTimeMinimum = 0.05f;
        
        cellGrid = new Cell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Creates a prefab (default is cube) at the X,Y Cords.
                SpriteRenderer sprite = Instantiate(prefab, new Vector2(x,y), noRotation);
                //Creates a Cell at the X,Y Cords that we will reference later to change prefabs enabled status.
                cellGrid[x, y] = new Cell(0,false,sprite,x,y,0);
            }
            
        }
       randomize(percentAlive);

       GameObject backgroundVariable = Instantiate(background, Vector3.zero, noRotation);

        backgroundVariable.transform.localScale = new Vector3(gridWidth, gridHeight, 0);
        backgroundVariable.transform.position = new Vector3(gridWidth * 0.5f, gridHeight * 0.5f, 0);

        NextGen();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            randomize(percentAlive);
        }

        if (pause == true)
        {
            draw();
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }


        if (mousePosDelta != Vector2.zero)
        {
            mouseClicked = false;
        }

        if (Input.GetMouseButtonDown(0))
            drawing = true;

        if (Input.GetMouseButtonUp(0))
            drawing = false;

        if (Input.GetMouseButtonDown(1))
            erasing = true;

        if (Input.GetMouseButtonUp(1))
            erasing = false;

        //                                                          Vector 3 0.5f is needed because of changed pivot location from center to bottom left in CellPrefab
        mousePos = Vector2Int.RoundToInt(camera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.5f,0.5f,0));
        mousePosDelta = mousePos - lastMousePos;
        
        lastMousePos = Vector2Int.RoundToInt(camera.ScreenToWorldPoint(Input.mousePosition));
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
            

        waitTime = generationSpeedSlider.value;

        if (waitTime < waitTimeMinimum)
            waitTime = waitTimeMinimum;


            Invoke(nameof(NextGen), waitTime);
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
        //Loop through all cells in the grid
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


        //Apply color and turn on or off the sprite renderer

                //Counts if a cell has been alive more then or equal to the amount its been dead.
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
                    cellGrid[x, y].spriteRenderer.color = Color.yellow;
                }


                if (cellGrid[x, y].stableForTime < whenStable / 3)
                {
                    cellGrid[x, y].spriteRenderer.color = Color.red;
                }


                if (cellGrid[x, y].stableForTime > whenStable / 3 && cellGrid[x, y].stableForTime < whenStable)
                {
                    cellGrid[x, y].spriteRenderer.color = new Color(0.9622642f, 0.3393081f, 0, 1);
                }


                //Color fade
                if (cellGrid[x,y].alive == false)
                {
                    if (cellGrid[x, y].stableForTime < whenStable / 3)
                    {
                        cellGrid[x, y].spriteRenderer.color = new Color(1, 0f, 0f, cellGrid[x, y].stableForTime * 0.1f);
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

        if (!(mousePos.x >= gridWidth || mousePos.y >= gridHeight || mousePos.x < 0 || mousePos.y < 0))
        {

            if (drawing == true && mouseClicked == false && (mousePosDelta.x == 0 || mousePosDelta.y == 0))
            {

                cellGrid[mousePos.x, mousePos.y].spriteRenderer.enabled = true;
                cellGrid[mousePos.x, mousePos.y].alive = true;
                cellGrid[mousePos.x, mousePos.y].spriteRenderer.color = Color.white;

                mouseClicked = true;

            }

            if (erasing == true && mouseClicked == false && (mousePosDelta.x == 0 || mousePosDelta.y == 0))

            {

                cellGrid[mousePos.x, mousePos.y].spriteRenderer.enabled = false;
                cellGrid[mousePos.x, mousePos.y].alive = false;
                cellGrid[mousePos.x, mousePos.y].spriteRenderer.color = Color.white;

            }
        }

    }

    void randomize(int percentAlive)
    {
        percentAlive--;

        if (percentAlive < 0)
            percentAlive = 0;

        if (percentAlive > 99)
            percentAlive = 99;



        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                rnd = Random.Range(0, 100);
                

                if (rnd < percentAlive)
                {
                    cellGrid[x, y].alive = true;
                }
                else
                {
                    cellGrid[x, y].alive = false;
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
