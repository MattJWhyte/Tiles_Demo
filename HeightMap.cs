using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMap : MonoBehaviour
{
    public static int numTiles = 100;
    public GameObject tile;
    public int poolDensity = 10;
    public int poolRange = 10;
    public float aveHeight = 10.0f;
    public float aveRadius = 10.0f;
    public Material glassMat, fireMat;
    float maxHeight, radius;
    int xPos, yPos;
    public float twoSigSuared = 1.0f;
    bool[] glass = new bool[numTiles * numTiles];
    float[] contours = new float[numTiles * numTiles];
    public float waveAmplitude = 1.0f;
    float t;
    public float grow = 1.0f;
    public float growCycle = 30.0f;
    public GameObject orb, player;
    GameObject newOrb;
    //public int SlatsPerWavelength = 10;
    //public float frequency = 1.0f;
    //public float Damping = 1.0f;
    //public float AngFreq = 1.0f;
    //public float A = 1.0f;
    //float[] ChangeTimeAmp = new float[numTiles * numTiles];

    GameObject[] tiles = new GameObject[numTiles * numTiles];
    GameObject[] tilesPos = new GameObject[numTiles * numTiles];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numTiles; i++)
        {
            for (int j = 0; j < numTiles; j++)
            {
                tiles[i * numTiles + j] = Instantiate(tile, transform);
                tiles[i * numTiles + j].transform.position = new Vector3(i, 0, j);
            }
        }
        resetVals();
        

    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime;
        for (int i = 0; i < numTiles; i++)
        {
            for (int j = 0; j < numTiles; j++)
            {

                float amplitude = waveAmplitude * Mathf.Sin(0.5f * (t - i));
                float ampGrow = contours[i * numTiles + j];
                //float amplitude = -0.5f*dist;
                tiles[i * numTiles + j].transform.position = new Vector3(i, tiles[i * numTiles + j].transform.position.y + amplitude, j);
                if (t%growCycle<growCycle/2)
                {
                    tiles[i * numTiles + j].transform.position = new Vector3(i, contours[i * numTiles + j]*((t%growCycle)/(growCycle/2)), j);
                    
                }
                else
                {
                    tiles[i * numTiles + j].transform.position = new Vector3(i, contours[i * numTiles + j] *(2- ((t % growCycle) / (growCycle / 2))), j);
                }
                /*if ((tiles[i * numTiles + j].transform.position.y < contours[i * numTiles + j]))
                {
                    tiles[i * numTiles + j].transform.position = new Vector3(i, tiles[i * numTiles + j].transform.position.y + grow*Time.deltaTime, j);
                }*/
            }

        }
        if (t%growCycle<0.1f)
        {
            Destroy(newOrb);
            resetVals();
        }
    }

    private void resetVals()
    {
        player.transform.position = new Vector3(player.transform.position.x, 30.0f, player.transform.position.z);
        float maxOrb = 0.0f;
        Vector3 orbPos = new Vector3(0, 0, 0);
        

        int numPeaks = Random.Range(100, 300);
        for (int k = 0; k < numPeaks; k++)
        {

            maxHeight = Random.Range(aveHeight * 0.5f, aveHeight * 1.5f);
            radius = Random.Range(aveRadius * 0.5f, aveRadius * 1.5f);
            xPos = Random.Range(0, numTiles);
            yPos = Random.Range(0, numTiles);

            for (int i = 0; i < numTiles; i++)
            {
                for (int j = 0; j < numTiles; j++)
                {
                    Vector3 displacement = tiles[xPos * numTiles + yPos].transform.position - tiles[i * numTiles + j].transform.position;
                    float dist = displacement.magnitude;

                    float amplitude = 0.5f * maxHeight * Mathf.Exp(-dist * dist / twoSigSuared) / Mathf.Pow(twoSigSuared * Mathf.PI, 0.5f);
                    contours[i * numTiles + j] = tiles[i * numTiles + j].transform.position.y + amplitude;
                    //float amplitude = -0.5f*dist;

                    tiles[i * numTiles + j].transform.position = new Vector3(i, contours[i * numTiles + j], j);
                    if (tiles[i * numTiles + j].transform.position.y + amplitude > maxOrb)
                    {
                        maxOrb = tiles[i * numTiles + j].transform.position.y + amplitude;
                        orbPos = new Vector3(xPos, maxOrb + 10, yPos);
                    }
                }
            }

        }

        newOrb = Instantiate(orb, transform);
        newOrb.transform.position = orbPos;
        // create glass map
        Vector3 glassCentre = new Vector3(0, 0, 0);
        for (int k = 0; k < 3; k++)
        {
            int randomPool = Random.Range(Mathf.FloorToInt(poolRange * 1.0f), Mathf.FloorToInt(poolRange * 1.5f));
            int xCentrePt = Random.Range(randomPool, numTiles - randomPool);
            int yCentrePt = Random.Range(randomPool, numTiles - randomPool);
            for (int i = 0; i < poolDensity; i++)
            {
                int xPool = Random.Range(xCentrePt - randomPool, xCentrePt + randomPool);
                int yPool = Random.Range(yCentrePt - randomPool, yCentrePt + randomPool);

                if (glassCentre == new Vector3(0, 0, 0))
                {
                    glassCentre = new Vector3(xPool, 0, yPool);
                }
                else if (((new Vector3(xPool, 0, yPool) - glassCentre).magnitude < 50) && ((new Vector3(xPool, 0, yPool) - player.transform.position).magnitude > 30) && ((new Vector3(xPool, 0, yPool) - newOrb.transform.position).magnitude > 30)) 
                {


                    if ((Mathf.Pow(xPool - xCentrePt, 2) + Mathf.Pow(yPool - yCentrePt, 2)) < randomPool * randomPool)
                    {
                        tiles[xPool * numTiles + yPool].GetComponent<MeshRenderer>().material = glassMat;
                        tiles[xPool * numTiles + yPool].tag = "Glass";
                    }
                }

            }
        }
        // create fire map
        Vector3 fireCentre = new Vector3(0, 0, 0);
        for (int k = 0; k < 2; k++)
        {
            int randomPool = Random.Range(Mathf.FloorToInt(poolRange * 0.5f), Mathf.FloorToInt(poolRange * 1.0f));
            int xCentrePt = Random.Range(randomPool, numTiles - randomPool);
            int yCentrePt = Random.Range(randomPool, numTiles - randomPool);
            for (int i = 0; i < poolDensity; i++)
            {
                int xPool = Random.Range(xCentrePt - randomPool, xCentrePt + randomPool);
                int yPool = Random.Range(yCentrePt - randomPool, yCentrePt + randomPool);

                if (fireCentre == new Vector3(0, 0, 0))
                {
                    fireCentre = new Vector3(xPool, 0, yPool);
                }
                else if ((new Vector3(xPool, 0, yPool) - fireCentre).magnitude < 50)
                {


                    if ((Mathf.Pow(xPool - xCentrePt, 2) + Mathf.Pow(yPool - yCentrePt, 2)) < randomPool * randomPool)
                    {
                        tiles[xPool * numTiles + yPool].GetComponent<MeshRenderer>().material = fireMat;
                        tiles[xPool * numTiles + yPool].tag = "Fire";
                    }
                }

            }
        }
    }
    
}

