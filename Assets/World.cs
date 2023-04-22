using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Chunk
{
    public int Diff = 0;
    public bool Up = false;
    public bool Down = false;
    public bool Left = false;
    public bool Right = false;
    public bool WalkerVisited = false;
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}

[System.Serializable]
public class Level
{
    public int diff;
    public int top;
    public int down;
    public int left;
    public int right;
    public string level;
}

[System.Serializable]
public class LevelsJSON
{
    //[System.Serializable]


    public Level[] levels;
    public List<int> magrit;
    public string name;

    public static LevelsJSON CreateFromJSON(string jsonString = "chunks")
    {
        string targetFile = Resources.Load<TextAsset>(jsonString).ToString();

        //remove every space and newline
        targetFile = targetFile.Replace("\r", "").Replace("\n", "").Replace(" ","");

        Debug.Log("AMOONGUS " + targetFile);

        //return new LevelsJSON();

        //var ret = new LevelsJSON();
        //ret.levels = new List<LevelJSON>();

        return JsonUtility.FromJson<LevelsJSON>(targetFile);
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

    List<List<Chunk>> chunks;

    private void GenerateWorld()
    {
        chunks = new List<List<Chunk>>();

        for (int y = 0; y < depth; y++)
        {
            chunks.Add(new List<Chunk>());
            for (int x = 0; x < 4; x++)
            {
                chunks[y].Add(new Chunk());
            }
        }

        Vector2Int startingChunk = new Vector2Int(Random.Range(0, 3), 0);

        chunks[startingChunk.y][startingChunk.x].WalkerVisited = true;

        //while not every chunk has been visited
        while (chunks.Any(y => y.Any(x => x.WalkerVisited == false)))
        {
            //pick a random chunk
            Vector2Int currentChunk = new Vector2Int(Random.Range(0, 3), Random.Range(0, depth - 1));

            //if it has been visited, skip it
            if (chunks[currentChunk.y][currentChunk.x].WalkerVisited)
                continue;

            //if it hasn't been visited, mark it as visited
            chunks[currentChunk.y][currentChunk.x].WalkerVisited = true;

            //pick a random direction
            int direction = Random.Range(0, 4);

            //if the direction is up
            if (direction == 0)
            {
                //if the chunk is at the top, skip it
                if (currentChunk.y == depth - 1)
                    continue;

                //if the chunk is not at the top, mark it as up
                chunks[currentChunk.y][currentChunk.x].Up = true;

                //mark the chunk above it as down
                chunks[currentChunk.y + 1][currentChunk.x].Down = true;
            }

            //if the direction is down
            if (direction == 1)
            {
                //if the chunk is at the bottom, skip it
                if (currentChunk.y == 0)
                    continue;

                //if the chunk is not at the bottom, mark it as down
                chunks[currentChunk.y][currentChunk.x].Down = true;

                //mark the chunk below it as up
                chunks[currentChunk.y - 1][currentChunk.x].Up = true;
            }

            //if the direction is left
            if (direction == 2)
            {
                //if the chunk is at the left, skip it
                if (currentChunk.x == 0)
                    continue;

                //if the chunk is not at the left, mark it as left
                chunks[currentChunk.y][currentChunk.x].Left = true;

                //mark the chunk to the left of it as right
                chunks[currentChunk.y][currentChunk.x - 1].Right = true;
            }

            //if the direction is right
            if (direction == 3)
            {
                //if the chunk is at the right, skip it
                if (currentChunk.x == 3)
                    continue;

                //if the chunk is not at
            }

            //Load JSON
            var Levels = LevelsJSON.CreateFromJSON();
            Debug.Log(Levels.name + " " + Levels.magrit);

            int chunkID = 0;

            for (int y = 0; y < depth; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Vector2Int chunkPos = new Vector2Int(x, y);
                    DrawChunk(chunkPos, Levels.levels[chunkID]);
                }
            }

        }
    }

    public List<TileBase> tiles;

    void DrawChunk(Vector2Int chunkPos, Level level)
    {
        Vector2Int worldChunkPos = chunkPos * 6;

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                tilemap.SetTile((Vector3Int)(worldChunkPos + new Vector2Int(x, y)), tiles[0]);
            }
        }
    }

}
