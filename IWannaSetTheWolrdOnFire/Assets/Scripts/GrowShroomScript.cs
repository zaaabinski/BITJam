using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShroomScript : MonoBehaviour
{
    [SerializeField] private bool fullyGrown = false;
    [SerializeField] private int hitCount = 0;

    [SerializeField] private int maxHits = 10;

    [SerializeField] private float targetScale = 2f;

    [SerializeField] private float scaleIncreasePerHit = 0.1f;
     public int shroomType;
   // [SerializeField] private GameObject gameManagerHolder;
     private GameManager gm;
    private bool isAdded=false;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        float scaleFactor = Mathf.Min(1f + hitCount * scaleIncreasePerHit, targetScale);

        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        if (fullyGrown && !isAdded)
            AddToList();
    }

    private void AddToList()
    {
        isAdded = true;
        gm.grownShrooms.Add(this.gameObject);
    }

    public void IncreaseHitCount()
    {
        hitCount++;

        if (hitCount > maxHits)
        {
            hitCount = maxHits;
            fullyGrown = true;
        }
    }
}
