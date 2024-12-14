using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlamethrowerScript : MonoBehaviour
{
    public int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private GameObject detectionArea; // Reference to the game object with the sphere collider
    [SerializeField] private LayerMask burnable;
    private CapsuleCollider detectionCollider;
    private bool isReady = true;
    [SerializeField] private ParticleSystem flame;
    [SerializeField] private ParticleSystem rainbow;
    [SerializeField] private TextMeshProUGUI tmpAmmo;
    public bool isRainbow = true;
    void Start()
    {
        // Initialize ammo
        if (ammo > maxAmmo)
        {
            ammo = maxAmmo;
        }

        // Get the SphereCollider from the detection area
        if (detectionArea != null)
        {
            detectionCollider = detectionArea.GetComponent<CapsuleCollider>();
            if (detectionCollider == null || !detectionCollider.isTrigger)
            {
                Debug.LogError("The detection area must have a SphereCollider with 'Is Trigger' enabled.");
            }
        }
        else
        {
            Debug.LogError("Detection area is not assigned.");
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && ammo > 0 && isReady) // Left-click to use the flamethrower
        {
            StartCoroutine("UseFlameThrower");
        }
        tmpAmmo.text = ammo.ToString() + "/" + maxAmmo.ToString();

    }

    private IEnumerator UseFlameThrower()
    {
        // Reduce ammo
        ammo--;
        isReady = false;
        if (isRainbow)
            rainbow.Play();
        else
            flame.Play();
        // Detect objects in the detection area
        Collider[] hitColliders = Physics.OverlapSphere(detectionCollider.bounds.center, detectionCollider.radius * detectionArea.transform.localScale.x, burnable);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.CompareTag("Shroom"))
            {
                Debug.Log("Hit flower");
                GrowShroomScript GSS = hit.gameObject.GetComponent<GrowShroomScript>();
                GSS.IncreaseHitCount();
            }

            else if (hit.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy");
                EnemyScript ES = hit.gameObject.GetComponent<EnemyScript>();
                ES.DecraseEnemyHP();
            }
        }
        yield return new WaitForSeconds(0.1f);
        isReady = true;
    }

    public void AddAmmo()
    {
        if (ammo + 10 <= maxAmmo)
        {
            ammo += 10;
        }
        else
        {
            ammo = maxAmmo;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionArea != null && detectionCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionCollider.bounds.center, detectionCollider.radius * detectionArea.transform.localScale.x);
        }
    }
}
