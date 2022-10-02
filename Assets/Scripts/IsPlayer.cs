using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml.XPath;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IsPlayer : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    public JumpsOnClick jumpsOnClick;
    public FiresOnClick firesOnClick;
    public DoesStuffEvery10Seconds doesStuffEvery10Seconds;
    public GameObject enemyManager;

    public int level = 1;
    public int xp = 0;

    public int xpPerLevel = 5;

    private int option1, option2, option3;
    public Button button1, button2, button3;
    public List<Sprite> augmentSprites;
    public GameObject augmentUI;

    public AudioClip levelUpSound;
    public AudioClip upgradeSelectedSound;

    private void Start()
    {
        // hide the augment UI
        augmentUI.SetActive(false);

		// update our stat UI
		xpText.text = xp + "/" + xpPerLevel * level + " Kills To Level";
		levelText.text = "Level " + level;
	}

    public void AddXP(int xpIn)
    {
        xp += xpIn;

        if(xp >= xpPerLevel * level)
        {
            xp -= xpPerLevel * level;
            LevelUp();
        }

        // update our stat UI
        xpText.text = xp + "/" + xpPerLevel * level + " Kills To Level";
	}

	public void LevelUp()
    {
        level++;

        // pick 3 unique numbers and set options1/2/3 to them
        SetRandomOptions();

        // set the images of the UI buttons to them
        button1.image.sprite = augmentSprites[option1];
		button2.image.sprite = augmentSprites[option2];
		button3.image.sprite = augmentSprites[option3];

		// pause time so the player has time to pick an upgrade
		Time.timeScale = 0.0f;

		// reveal the augment UI
		augmentUI.SetActive(true);

		// update our stat UI
		levelText.text = "Level " + level;

        // play a sound
        AudioSource.PlayClipAtPoint(levelUpSound, transform.position);
    }

    private void SetRandomOptions()
    {
        // build a list of random numbers to pull from
        List<int> randomNumsList = new List<int>();
        for(int i = 0; i < augmentSprites.Count; i++)
        {
            randomNumsList.Add(i);
        }

        int randomNum1 = Random.Range(0, randomNumsList.Count);
		int randomIndex1 = randomNumsList[randomNum1];
        randomNumsList.RemoveAt(randomNum1);
        option1 = randomIndex1;

		int randomNum2 = Random.Range(0, randomNumsList.Count);
		int randomIndex2 = randomNumsList[randomNum2];
		randomNumsList.RemoveAt(randomNum2);
		option2 = randomIndex2;

		int randomNum3 = Random.Range(0, randomNumsList.Count);
		int randomIndex3 = randomNumsList[randomNum3];
		randomNumsList.RemoveAt(randomNum3);
		option3 = randomIndex3;
    }

    public void Button1Selected()
    {
        ApplyUpgrade(option1);
    }

	public void Button2Selected()
	{
		ApplyUpgrade(option2);
	}

	public void Button3Selected()
	{
		ApplyUpgrade(option3);
	}

	private void ApplyUpgrade(int upgradeIndex)
    {
		// hide the augment UI
		augmentUI.SetActive(false);

		// restore time
		Time.timeScale = 1.0f;

        // there's probably a much cleaner way to do this, but nothing comes to mind
        // because of late night game jam brain :)
        switch(upgradeIndex)
        {
            case 0:
                // player damage up
                firesOnClick.bulletDamage *= 1.20f;
                break;
			case 1:
                // extra bullet
                firesOnClick.shotsToFire++;
				break;
            case 2:
				// attack cooldown decreased
				firesOnClick.fireCooldown *= 0.80f;
                break;
            case 3:
                // jump speed increased
                jumpsOnClick.movementSpeed *= 1.2f;
                break;
			case 4:
                // piercing +1
                firesOnClick.piercingCounter++;
				break;
			case 5:
                // bullet size
                firesOnClick.bulletSizeScalar *= 1.4f;
				break;
			case 6:
                // spawn extra planets
                doesStuffEvery10Seconds.SpawnNewPlanet();
				doesStuffEvery10Seconds.SpawnNewPlanet();
				doesStuffEvery10Seconds.SpawnNewPlanet();
				break;
			case 7:
				// kill all enemies
                foreach(Transform enemy in enemyManager.transform)
                {
                    Destroy(enemy.gameObject);
                }
				break;
			case 8:
                // bullet speed increased
                firesOnClick.bulletSpeed *= 1.2f;
                break;
			default:
                break;
        }

        // play a sound
        AudioSource.PlayClipAtPoint(upgradeSelectedSound, transform.position);
    }
}
