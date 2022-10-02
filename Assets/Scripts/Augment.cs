using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

public class Augment : MonoBehaviour
{
	public static List<Sprite> augmentSprites;
	public static List<Object> augmentFunctions;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class AugmentPair
{
    public Object function;
}