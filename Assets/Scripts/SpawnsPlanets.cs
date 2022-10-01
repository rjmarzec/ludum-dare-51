using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnsPlanets : MonoBehaviour
{
	public Transform planetManager;
	public GameObject planetPrefab;
	public int startingPlanetCount = 5;
	public int extraPlanetsPerRing = 3;

    public float distanceFromSun = 2.0f;
    public float distanceBetweenRingsAverage = 6.0f;
    public float distanceBetweenRingsOffset = 1.0f;

	public float orbitSpeedAverage = 20.0f;
	public float orbitSpeedVariance = 10.0f;

	public float rotationSpeedAverage = 40.0f;
    public float rotationSpeedVariance = 20.0f;

    public float sizeAverage = 3.0f;
    public float sizeVariance = 2.0f;

    public List<Material> materials;

	private float timer;
    public List<int> spawnCounter;
    
    void Start()
    {
        // initailize our timer from 0
        timer = 0;

        // initialize the spawn counter with no planets in play
        spawnCounter = new List<int>();
        spawnCounter.Add(0);

        // spawn a couple planets to start with
        for(int i = 0; i < startingPlanetCount; i++)
        {
            SpawnNewPlanet();
        }
    }

    void Update()
    {
        // incerement our timer
        timer += Time.deltaTime;

        // did another set of 10 seconds pass?
        if(timer % 10 < (timer - Time.deltaTime) % 10)
        {
            Every10Seconds();
        }
    }

    private void Every10Seconds()
    {
        // do something every 10 seconds! this will usually be spawning a planet,
        // but sometimes something extra as well
        SpawnNewPlanet();

        // zoom out the camera
    }

    private void SpawnNewPlanet()
    {
        // get the ring number to spawn this planet under
        int ringNumber = GetNextRingNumber();

        // spawn the planet at that ring, with the distance being randomly shifted closer or farther
        GameObject newPlanet = Instantiate(planetPrefab);
        newPlanet.transform.position = Vector2.up * (distanceFromSun + (distanceBetweenRingsAverage * ringNumber) + Random.Range(-distanceBetweenRingsOffset, distanceBetweenRingsOffset));

        // randomly rotate the planet around the center of the sun to give it a unique starting point
        newPlanet.transform.RotateAround(transform.position, Vector3.forward, Random.Range(0.0f, 360.0f));

        // make the planet rotate around this object (the sun)
        RotatesAround newPlanetRA = newPlanet.GetComponent<RotatesAround>();
        newPlanetRA.SetNewTarget(transform, orbitSpeedAverage + Random.Range(-orbitSpeedVariance, orbitSpeedVariance));

        // give the planet some constant rotation
        Rotates newPlanetRotates = newPlanet.GetComponent<Rotates>();
        newPlanetRotates.degreesPerSecond = rotationSpeedAverage + Random.Range(-rotationSpeedVariance, rotationSpeedVariance);

        // give the planet a random size
        newPlanet.transform.localScale = Vector3.one * (sizeAverage + Random.Range(-sizeVariance, sizeVariance));

        // give the planet a random material
        newPlanet.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Count)];
    }

    private int GetNextRingNumber()
    {
        // given how long spawnCounter is, how many total rings should we have?
        int totalRings = spawnCounter.Count - 1;

        // the total number of planets per ring should be n*3, where n is the n-th ring.
        // knowing this, count the max number of rings we could support right now, plus 1
        int maximumPlanets = 1;
        for(int i = 1; i <= totalRings; i ++)
        {
            maximumPlanets += i * extraPlanetsPerRing;
        }

        // count how many planets we've actually spawned so far
        int numPlanetsSpawned = 0;
        foreach(int i in spawnCounter)
        {
            numPlanetsSpawned += i;
        }

        // generate a random number between the maximum planets we can support and
        // the number of planets we already have spawned. 
        int randomPlanetNumber = Random.Range(0, maximumPlanets - numPlanetsSpawned);

        // loop over all the missing planet idicies and find the ring of the m-th missing
        // planet, where m is the random number found in the previous line. if there is
        // no corresponding ring, add a new ring to our solar system by growing
        // spawnCounter and return the index of that new ring
        int missingPlanetsCounter = 0;
		for (int i = 1; i <= totalRings; i++)
		{
            // the number of missing planets per ring the number of planets that should be
            // on that ring minus the number that actually are there
            missingPlanetsCounter += (i * extraPlanetsPerRing) - spawnCounter[i];

            // if the number of missing planets we've counted has become greater than the m-th index
            // we're looking for, then the m-th missing planet must be on the current ring
            if(missingPlanetsCounter > randomPlanetNumber)
            {
                spawnCounter[i]++;
				return i;
			}
		}

        // if the m-th planet has not been found yet, it must spawn on the next ring. expand
        // spawnCounter to account for a new ring, and return the index of that new ring
        spawnCounter.Add(1);
        return spawnCounter.Count;
    }
}
