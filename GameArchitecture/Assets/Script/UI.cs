using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public List<GameObject> Roommates;
    private Property target;
    private int targetNum = 0; 

    // Properties
    [SerializeField]
    private float academic;
    [SerializeField]
    private float hunger;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float money;
    [SerializeField]
    private float stress;
    [SerializeField]
    private eStatus status;

    // UI Sliders
    public Slider sAcademic;
    public Slider sHunger;
    public Slider sEnergy;
    public Slider sMoney;
    public Slider sStress;
    public Text tStatus;
    public Text info;
    public Color targetColor;

    void Awake()
    {
        if (Roommates.Count == 0)
        {
            Debug.LogWarning("No Roommates!");
            return;
        }
        target = Roommates[targetNum].GetComponent<Roommate>().property;
        targetColor = Roommates[targetNum].GetComponent<Roommate>().mat.color;
        SetColor();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitSliderValue();
    }

    // Update is called once per frame
    void Update()
    {
        GetProperties();
        UpdateSlider();

        if (Input.GetKeyUp(KeyCode.Q))
        {
            targetNum ++;
            if (targetNum >= Roommates.Count)
            {
                targetNum = 0;
            }
            target = Roommates[targetNum].GetComponent<Roommate>().property;
            targetColor = Roommates[targetNum].GetComponent<Roommate>().mat.color;
            SetColor();
        }
    }

    private void GetProperties()
    {
        academic = target.academic;
        hunger = target.hunger;
        energy = target.energy;
        money = target.money;
        stress = target.stress;
        status = Roommates[targetNum].GetComponent<Roommate>().status;
    }

    private void UpdateSlider()
    {
        sAcademic.value = academic;
        sHunger.value = hunger;
        sEnergy.value = energy;
        sMoney.value = money;
        sStress.value = stress;
        tStatus.text = status.ToString();
    }

    public void InitSliderValue()
    {
        sAcademic.maxValue = 100;
        sHunger.maxValue = 100;
        sEnergy.maxValue = 100;
        sMoney.maxValue = 100;
        sStress.maxValue = 100;
    }

    private void SetColor()
    {
        tStatus.color = targetColor;
        info.color = targetColor;
    }
}

