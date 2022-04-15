using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    private float lifeTime = 5;

    private Rigidbody rb;

    private Material mat;
    private Color originalCol;
    private float fadePercent;
    private float deathtime;
    private bool fading;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        originalCol = mat.color;
        deathtime = Time.time + lifeTime;

        StartCoroutine("Fade");
	}
	
    IEnumerator Fade()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            if (fading)
            {
                fadePercent += Time.deltaTime;
                mat.color = Color.Lerp(originalCol, Color.clear, fadePercent);

                if(fadePercent >= 1)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if(Time.time > deathtime)
                {
                    fading = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Ground")
        {
            rb.Sleep();
        }
    }
}
