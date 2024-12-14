using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpPro;
    [SerializeField] PlayerMovement PMScript;
    [SerializeField] private int time = 60;
    [SerializeField] FlamethrowerScript flamethrowerScript;
    private void Start()
    {
        StartCoroutine("Clock");
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1);
        time--;
        
        StartCoroutine("Clock");
        if (time < 0 && flamethrowerScript.isRainbow)
            PMScript.DecreseHp(1);

    }

    private void Update()
    {
        if (time > 0)
            tmpPro.text = time.ToString();
        else if (!flamethrowerScript.isRainbow)
            tmpPro.text = "BURN BURN BURN";
        else
            tmpPro.text = "RUN RUN RUN";
    }

}
