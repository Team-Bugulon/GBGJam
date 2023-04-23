using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Newtonsoft.Json;

public class Chunk
{
    public int Diff = 0;
    public bool Up = false;
    public bool Down = false;
    public bool Left = false;
    public bool Right = false;
    public bool WalkerVisited = false;
    public bool FlowerChunk = false;
}

public class Level
{
    public int diff;
    public int top;
    public int down;
    public int left;
    public int right;
    public string level = ":)";
    public bool flipped = false;
    public int flower;
}

public class LevelsJSON
{
    public List<Level> levels;
    public List<int> magrit;
    public string name;

    public static LevelsJSON CreateFromJSON(string jsonString = "chunks")
    {
        string targetFile = Resources.Load<TextAsset>(jsonString).ToString();

        //remove every space and newline
        targetFile = targetFile.Replace("\r", "").Replace("\n", "").Replace(" ", "");

        Debug.Log("AMOONGUS " + targetFile);

        LevelsJSON lvl = JsonConvert.DeserializeObject<LevelsJSON>(targetFile);

        //for each level add their flipped counter part
        List<Level> flippedLayouts = new List<Level>();
        foreach(var layout in lvl.levels)
        {
            var flippedLayout = new Level();
            flippedLayout.diff = layout.diff;
            flippedLayout.top = layout.top;
            flippedLayout.down = layout.down;
            flippedLayout.left = layout.right;
            flippedLayout.right = layout.left;
            flippedLayout.level = layout.level;
            flippedLayout.flipped = true;
            flippedLayout.flower = layout.flower;

            flippedLayouts.Add(flippedLayout);
        }

        foreach(var fl in flippedLayouts)
        {
            lvl.levels.Add(fl);
        }

        return lvl;
    }
}

public class World : MonoBehaviour
{
    [InspectorButton("GenerateWorld")]
    public bool generate;

    [Header("Geometry")]
    public int depth = 3;
    public int difficulty = 0;

    [Header("References")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] Transform groundAnchor;
    [SerializeField] Transform bgContainer;
    [SerializeField] Transform boat;
    [SerializeField] Transform sky;
    [SerializeField] Transform waves;
    [SerializeField] Transform whateverBG;

    [Header("Prefabs")]
    [SerializeField] GameObject BG;
    [SerializeField] List<Sprite> BGSprites;

    [Header("Tiles")]
    [SerializeField] List<TileBase> tiles;

    [Header("ColorTabs")]
    [SerializeField] List<Color> colorDay;
    [SerializeField] List<Color> colorNight;
    [SerializeField] List<Color> colorEvening;
    [SerializeField] List<Sprite> boatSprites;
    [SerializeField] List<Sprite> skySprites;

    List<List<Chunk>> chunks;
    Vector2Int startingChunk;

    private void Start()
    {
        difficulty = TransitionManager.i.Level;
        if (difficulty == 0)
        {
            depth = 2;
        }
        else if (difficulty <= 5)
        {
            depth = 1 + 3 * difficulty;
        } 
        else if (difficulty <= 10)
        {
            depth = 4 * difficulty;
        } 
        else
        {
            depth = 5 * difficulty;
        }
        
        switch ((Mathf.FloorToInt(TransitionManager.i.Level/2) % 3))
        {
            case (0):
                GameManager.i.timeOfDay = GameManager.DayTime.Day;
                break;
            case (1):
                GameManager.i.timeOfDay = GameManager.DayTime.Evening;
                break;
            case (2):
                GameManager.i.timeOfDay = GameManager.DayTime.Night;
                break;
        }
        
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        switch (GameManager.i.timeOfDay)
        {
            case (GameManager.DayTime.Day):
                boat.GetComponent<SpriteRenderer>().sprite = boatSprites[0];
                sky.GetComponent<SpriteRenderer>().sprite = skySprites[0];
                waves.GetComponent<Animator>().Play("water_idle1");
                whateverBG.GetComponent<SpriteRenderer>().color = colorDay[0];
                break;
            case (GameManager.DayTime.Evening):
                boat.GetComponent<SpriteRenderer>().sprite = boatSprites[1];
                sky.GetComponent<SpriteRenderer>().sprite = skySprites[1];
                waves.GetComponent<Animator>().Play("water_idle2");
                whateverBG.GetComponent<SpriteRenderer>().color = colorEvening[0];
                break;
            case (GameManager.DayTime.Night):
                boat.GetComponent<SpriteRenderer>().sprite = boatSprites[2];
                sky.GetComponent<SpriteRenderer>().sprite = skySprites[2];
                waves.GetComponent<Animator>().Play("water_idle3");
                whateverBG.GetComponent<SpriteRenderer>().color = colorNight[0];
                break;
        }
        foreach (var txt in UIManager.i.LevelCounter.GetComponentsInChildren<TMPro.TextMeshPro>())
        {
            txt.text = (TransitionManager.i.Level + 1).ToString();
        }

        chunks = new List<List<Chunk>>();

        for (int y = 0; y < depth; y++)
        {
            chunks.Add(new List<Chunk>());
            for (int x = 0; x < 4; x++)
            {
                chunks[y].Add(new Chunk());
            }
        }

        Walker();

        //Load JSON
        var Levels = LevelsJSON.CreateFromJSON();

        //Clear tilemap
        tilemap.ClearAllTiles();

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                //DrawChunk(x, y, Levels.levels[Random.Range(0, Levels.levels.Count)]);
                DrawChunk(x, y, Levels);

            }
        }

        WorldBorders();

        var collider = tilemap.GetComponent<TilemapCollider2D>();
        collider.usedByComposite = false;
        collider.usedByComposite = true;

        //Empty BG Container
        foreach (Transform child in bgContainer)
        {
            DestroyImmediate(child.gameObject);
        }

        //Set BG
        for (int y = 0; y < Mathf.RoundToInt(depth/2) + 1; y++)
        {
            var bg = Instantiate(BG, bgContainer);
            bg.transform.position = new Vector3(0, -y * 6 * 2 - 8, 0);
            var sr = bg.GetComponentsInChildren<SpriteRenderer>();
            sr[1].sprite = BGSprites[Random.Range(0, BGSprites.Count)];
            Color cooler = Color.cyan;
            switch (GameManager.i.timeOfDay)
            {
                case (GameManager.DayTime.Day):
                    cooler = colorDay[Mathf.Min(y + 1, colorDay.Count - 1)];
                    break;
                case (GameManager.DayTime.Evening):
                    cooler = colorEvening[Mathf.Min(y + 1, colorEvening.Count - 1)];
                    break;
                case (GameManager.DayTime.Night):
                    cooler = colorNight[Mathf.Min(y + 1, colorNight.Count - 1)];
                    break;
            }
            sr[0].color = cooler;
            sr[1].color = cooler;
            sr[0].sortingOrder += y;
            sr[1].sortingOrder += y;
        }

        if (GameObject.FindGameObjectsWithTag("Mearl").Length < 7) GenerateWorld();
        
        TransitionManager.i.TransiOut(1f);

        switch (GameManager.i.timeOfDay)
        {
            case (GameManager.DayTime.Day):
                SoundManager.i.PlayMusic("Day");
                break;
            case (GameManager.DayTime.Evening):
                SoundManager.i.PlayMusic("Evening");
                break;
            case (GameManager.DayTime.Night):
                SoundManager.i.PlayMusic("Night");
                break;
        }

    }

    void WorldBorders()
    {
        for (int y = -4; y < depth*6; y++)
        {
            tilemap.SetTile((Vector3Int)(new Vector2Int(-6 * 2 - 1, -y - 4)), tiles[0]);
            tilemap.SetTile((Vector3Int)(new Vector2Int(6 * 2, -y - 4)), tiles[0]);
        }
        for (int x = - 6 * 2 - 1; x < 6*2 + 1; x++)
        {
            tilemap.SetTile((Vector3Int)(new Vector2Int(x, -depth * 6 - 4)), tiles[0]);
        }

        groundAnchor.position = new Vector3(0, -depth * 6 - 4 + 1, 0);


    }

    void DrawChunk(int chunkPosX, int chunkPosY, LevelsJSON levels)
    {
        if (startingChunk.x == chunkPosX && startingChunk.y == chunkPosY) return;
        var filteredChunk = new List<Level>();
        foreach (var lvl in levels.levels)
        {
            bool consider = true;
            if (chunks[chunkPosY][chunkPosX].Up && lvl.top == 0)
            {
                consider = false;
            }
            
            if (chunks[chunkPosY][chunkPosX].Down && lvl.down == 0)
            {
                consider = false;
            }

            if (chunks[chunkPosY][chunkPosX].Left && lvl.left == 0)
            {
                consider = false;
            }

            if (chunks[chunkPosY][chunkPosX].Right && lvl.right == 0)
            {
                consider = false;
            }

            if (Mathf.FloorToInt(chunkPosY/2) + difficulty < lvl.diff)
            {
                consider = false;
            }

            if (chunks[chunkPosY][chunkPosX].FlowerChunk && lvl.flower == 0)
            {
                consider = false;
            }

            if (consider)
            {
                filteredChunk.Add(lvl);
            }
        }

        Level selectedLevel = filteredChunk[Random.Range(0, filteredChunk.Count)];

        Vector2Int worldChunkPos = new Vector2Int(chunkPosX * 6, - chunkPosY * 6) +  Vector2Int.left * 6*2 + Vector2Int.down * 4;

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                int readX = x;
                if (selectedLevel.flipped)
                {
                    readX = 5 - x;
                }

                if (selectedLevel.level[readX + y * 6] == '#')
                {
                    if (chunkPosY < 4)
                    {
                        tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[1]);
                    } else if (chunkPosY > 6)
                    {
                        tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[7]);
                    } else
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[1]);
                        } else
                        {
                            tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[7]);
                        }

                    }


                } else if (selectedLevel.level[readX + y * 6] == 'o')
                {
                    tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[2]);
                }
                else if (selectedLevel.level[readX + y * 6] == 'x')
                {
                    tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[3]);
                }
                else if (selectedLevel.level[readX + y * 6] == 'y' && (chunks[chunkPosY][chunkPosX].FlowerChunk || Random.Range(0, 10) == 0) && TransitionManager.i.Level != 0)
                {
                    tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[4]);
                }
                else if (selectedLevel.level[readX + y * 6] == 'u')
                {
                    tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[5]);
                }
                else if (selectedLevel.level[readX + y * 6] == 'g')
                {
                    tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, -y)), tiles[6]);
                }
            }
        }
    }
    
    void Walker()
    {
        startingChunk = new Vector2Int(Random.Range(0, 3), 0);
        chunks[startingChunk.y][startingChunk.x].WalkerVisited = true;

        int visitedOnLine = 1;

        for (int y = 0; y<depth; y++)
        {
            while (visitedOnLine < 4)
            {
                int randomX = Random.Range(0, 4);
                if (chunks[y][randomX].WalkerVisited == false)
                {
                    //check for neighbors
                    bool neiLeft = false;
                    bool neiRight = false;
                    bool neiTop = false;
                    List<string> dirs = new List<string>();

                    //top
                    if (y != 0)
                    {
                        if (chunks[y-1][randomX].WalkerVisited == true)
                        {
                            neiTop = true;
                            dirs.Add("Top");
                        }
                    }

                    //left
                    if (randomX > 0)
                    {
                        if (chunks[y][randomX - 1].WalkerVisited == true)
                        {
                            neiLeft = true;
                            dirs.Add("Left");
                            //dirs.Add("Left");
                        }
                    }

                    //right
                    if (randomX < 3)
                    {
                        if (chunks[y][randomX + 1].WalkerVisited == true)
                        {
                            neiRight = true;
                            dirs.Add("Right");
                            //dirs.Add("Right");
                        }
                    }

                    if (dirs.Count > 0)
                    {
                        string dir = dirs[Random.Range(0, dirs.Count)];

                        if (dir == "Top")
                        {
                            chunks[y][randomX].Up = true;
                            chunks[y - 1][randomX].Down = true;

                        }
                        else if (dir == "Left")
                        {
                            chunks[y][randomX].Left = true;
                            chunks[y][randomX - 1].Right = true;
                        }
                        else if (dir == "Right")
                        {
                            chunks[y][randomX].Right = true;
                            chunks[y][randomX + 1].Left = true;
                        }

                        if (visitedOnLine == 0 && y > 2 && y % 3 == 0)
                        {
                            chunks[y][randomX].FlowerChunk = true;
                        }

                        visitedOnLine += 1;
                        chunks[y][randomX].WalkerVisited = true;
                    }

                }
            }
            visitedOnLine = 0;
        }

    }
}
