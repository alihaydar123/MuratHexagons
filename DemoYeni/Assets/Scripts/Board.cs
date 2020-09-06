using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width, height;

    public GameObject tilePrefab;

    public GameObject[,] allGons;

    float xOffset = 1.2f;
    float yOffset = 0.348f;

    bool boslukDoldur;

    void Start()
    {
        allGons = new GameObject[width,height];
        Setup();

    }


    private void Setup()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xPos = i * xOffset;
                if (j % 2 == 1)
                {
                    xPos += xOffset / 2;
                }

                Vector2 tempPosition = new Vector2(xPos, j * yOffset);
                GameObject backgroundtile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                backgroundtile.transform.parent = this.transform;

                allGons[i,j] = backgroundtile;
                backgroundtile.name = i + "," +j;
            }
        }
    }
    

    public IEnumerator BoslukDoldur()
    {
        
        yield return new WaitForSeconds(3f);
        if (!boslukDoldur)
        {
            boslukDoldur = true;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (allGons[i, j].transform.childCount == 0)
                    {
                        allGons[i, j].GetComponent<BackgroundTile>().SetUp();
                    }
                    else if (allGons[i,j].transform.childCount > 1)
                    {
                        Destroy(allGons[i, j].transform.GetChild(0).gameObject);
                    }
                }
            }
            boslukDoldur = false;
        }

    }
}
