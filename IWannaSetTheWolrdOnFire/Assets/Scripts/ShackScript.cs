using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ShackScript : MonoBehaviour
{
    [SerializeField] private Volume vol;
    private bool playerIN=false;
    private IEnumerator corutine;
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovementScript;
    [SerializeField] private GameObject canvasForE;
    [SerializeField] private GameObject blackoutPanel;
    [SerializeField] private GameObject fightPanel;
    [SerializeField] private AudioSource drop;
    [SerializeField] private List<GameObject> playerSpawnList;
    [SerializeField] private SpawnEnemies spawnEnemiesScript;
    [SerializeField] private SpawnShrooms spawnShroomsScript;
    [SerializeField] private FlamethrowerScript flamethrowerScript;

    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject rainbowThrower;

    private void Start()
    {
        playerMovementScript = player.GetComponent<PlayerMovement>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIN = true;
            canvasForE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIN = false;
            canvasForE.SetActive(false);

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIN)
        {
            playerMovementScript.canMove = false;
            StartCoroutine("StartTransition");
        }
    }

    IEnumerator StartTransition()
    {

        drop.Play();

        blackoutPanel.SetActive(true);
        flamethrowerScript.isRainbow = false;
        rainbowThrower.SetActive(false);
        flameThrower.SetActive(true);
        if (playerSpawnList == null || playerSpawnList.Count == 0)
        {
            Debug.LogError("Player spawn list is empty or null.");
            yield break;
        }

        // Select a random spawn point
        int rand = Random.Range(0, playerSpawnList.Count);
        Vector3 spawnPosition = playerSpawnList[rand].transform.position;

        Debug.Log($"Moving player to spawn point {rand}: {spawnPosition}");

        player.transform.position = spawnPosition;
        spawnShroomsScript.DeleteShrooms();
        spawnEnemiesScript.StartSpawning();

        yield return new WaitForSeconds(17);
        playerMovementScript.canMove = true;
        blackoutPanel.SetActive(false);
        fightPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fightPanel.SetActive(false);
        corutine = ChangeWeight(0, 1, 0.05f);
        StartCoroutine(corutine);

    }


    IEnumerator ChangeWeight(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            vol.weight = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        vol.weight = v_end;
    }
}
