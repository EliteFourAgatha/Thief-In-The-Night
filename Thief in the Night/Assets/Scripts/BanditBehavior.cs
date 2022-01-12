using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditBehavior : MonoBehaviour
{
    GameController gameController;
    public AudioClip banditStealSFX;
    public float banditSpeed = 1f;

    private bool holdingLoot;

    //Array of treasure, populated at start of level
    GameObject[] availableLoot;
    GameObject closestLoot = null;
    private bool canGrabLoot;
    public GameObject lootOnBandit;
    Vector3 banditPosition;

    void Start()
    {
        if(gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        banditPosition = transform.position;
        availableLoot = GameObject.FindGameObjectsWithTag("Loot");
        // Closest loot determined by FindNearestLoot and returned as GameObject
        closestLoot = FindNearestLoot();
    }
    void Update()
    {
        if(closestLoot == null)
        {
            closestLoot = FindNearestLoot();
        }
        if(!holdingLoot)
        {
            canGrabLoot = true;
        }
        else
        {
            canGrabLoot = false;
        }
        MoveToChosenLoot(closestLoot);
    }
    private GameObject FindNearestLoot()
    {
        float lowDistance = Mathf.Infinity;
        for(int i = 0; i < availableLoot.Length; i++)
        {
            float distance = Vector3.Distance(availableLoot[i].transform.position, banditPosition);
            if(distance < lowDistance)
            {
                lowDistance = distance;
                closestLoot = availableLoot[i];
            }
        }
        return closestLoot;
    }
    void MoveToChosenLoot(GameObject closestObj)
    {
        if(!holdingLoot)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestObj.transform.position,
                                                banditSpeed * Time.deltaTime);
        }
        if(closestObj == null)
        {
            closestObj = FindNearestLoot();
        }
    }
    public void BanditPickUpLoot(GameObject pickedUpLoot)
    {
        if(canGrabLoot)
        {
            lootOnBandit.SetActive(true);
            holdingLoot = true;
            pickedUpLoot.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(banditStealSFX, transform.position, 3f);
            Debug.Log("LOOT STOLEN");
            gameController.lootLives --;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Handcuffs")
        {
            Destroy(col.gameObject);
            gameController.IncrementBanditCount();
            gameController.PlayBanditCaughtAudio();
            Destroy(gameObject);
        }
    }
}