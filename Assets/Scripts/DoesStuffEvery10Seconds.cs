using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DoesStuffEvery10Seconds : MonoBehaviour
{
	private float timer;

    [Space]

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI planetCounter;
    public static int planetCount = 0;

    [Space]

	public Transform planetManager;
	public GameObject planetPrefab;
	public List<int> spawnCounter;
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

    public List<Material> planetMaterials;

    public AudioClip spawnPlanetSound;

    [Space]

    public Transform enemyManager;
    public GameObject enemyPrefab;

    public float enemySizeAverage = 2.0f;
    public float enemySizeVariance = 0.5f;

    public float enemySpeedBase = 10.0f;
    public float enemySpeedVariance = 5.0f;

    public float enemyHealth = 25;
    
    void Start()
    {
        // initailize our timer from 0
        timer = 0;

        // initialize the spawn counter with no planets in play
        spawnCounter = new List<int>();
        spawnCounter.Add(0);

		planetCount = 0;

		// spawn a couple planets to start with
		for (int i = 0; i < startingPlanetCount; i++)
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

        timerText.text = "" + (int)(timer % 10);
        if(timer % 10 < 1)
        {
            timerText.text = "10";
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    private void Every10Seconds()
    {
        // do something every 10 seconds! this will usually be spawning a planet,
        // but sometimes something extra as well
        SpawnNewPlanet();
        SpawnNewEnemies();

        // zoom out the camera slightly
        Camera.main.transform.position += Vector3.back * (1.0f/6.0f);
    }

    public void SpawnNewPlanet()
    {
        planetCount++;
        planetCounter.text = planetCount + " Planets In The System";

        // get the ring number to spawn this planet under
        int ringNumber = GetNextRingNumber();

        // spawn the planet at that ring, with the distance being randomly shifted closer or farther
        GameObject newPlanet = Instantiate(planetPrefab, planetManager);
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
        newPlanet.GetComponent<MeshRenderer>().material = planetMaterials[Random.Range(0, planetMaterials.Count)];

        // play a sound
        AudioSource.PlayClipAtPoint(spawnPlanetSound, newPlanet.transform.position);
    }

    private void SpawnNewEnemies()
    {
        // start by spawning 1 enemy every time this funciton gets called, increasing by 1 every 30 seconds
        for(int i = 0; i < (int)(timer/30) + 1; i++)
        {
            // spawn a new enemy, with the distance away being the farthest ring
            GameObject newEnemy = Instantiate(enemyPrefab, enemyManager);
			newEnemy.transform.position = Vector2.up * (distanceFromSun + (distanceBetweenRingsAverage * spawnCounter.Count) + Random.Range(-distanceBetweenRingsOffset, distanceBetweenRingsOffset));

			// randomly rotate the enemy around the center of the sun to give it a unique starting point
			newEnemy.transform.RotateAround(transform.position, Vector3.forward, Random.Range(0.0f, 360.0f));

            // set the enemy's health
            newEnemy.GetComponent<IsEnemy>().health = enemyHealth;

            // randomize the enemy's size
            newEnemy.transform.localScale = Vector3.one * (enemySizeAverage + Random.Range(-enemySizeVariance, enemySizeVariance));

			// randomize the enemy's speed
			newEnemy.GetComponent<IsEnemy>().movementSpeed = enemySpeedBase + Random.Range(-enemySpeedVariance, enemySpeedVariance);

            // set the enemy's time: melee or ranged
            newEnemy.GetComponent<IsEnemy>().SetType(Random.Range(0, 2) == 1);
		}

        // increase the base health of each enemy exponentially with time
        enemyHealth *= 1.08f;
        enemyHealth += 2;
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
