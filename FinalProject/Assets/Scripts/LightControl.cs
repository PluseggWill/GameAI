using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProcedureGenerator))]
public class LightControl : MonoBehaviour
{
    private List<GameObject> lights = new List<GameObject>();

    private List<bool> lightsOn = new List<bool>();


    [SerializeField]
    private float speed;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float lightRateScale = 0.8f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float lightRRR = 0.1f;

    [SerializeField]
    private float updateTime = 2.0f;

    private float timer = 0;
    private float timer2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        ProcedureGenerator proGen = GetComponent<ProcedureGenerator>();
        this.lights = proGen.lights;
        this.lightsOn = proGen.lightsOn;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (lights.Count > 0 && timer >= updateTime)
        {
            
            float lightRate = lightRateScale * Mathf.Abs(Mathf.Sin((20 * timer2) / 360 * Mathf.PI));
            Debug.Log("Update! "+ lightRate);

            for (int i = 0; i < lights.Count; i ++)
            {
                lightsOn[i] = Random.Range(0.0f, 1.0f) <= lightRate;
                lights[i].SetActive(lightsOn[i]);
            }

            timer = 0;
        }
    }
}
