using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEat : MonoBehaviour
{
    public enum SwitchSizeSkin
    {
        little,
        average,
        big
    }  

    public SwitchSizeSkin switchSkin;
    public float NbEaten = 1f;
    public Vector3 scaler = new Vector3(1,1,1);
    public Mesh meshLittle;
    public Mesh meshAverage;
    public Mesh meshBig;
    public Mesh CurrentMesh;

    private DashCardinalDirection dashCardinalDirection;
    // public bool squashed;

    private void InitializedSize()
    {
        switchSkin = SwitchSizeSkin.little;
        dashCardinalDirection = GetComponent<DashCardinalDirection>();
    }
    private void Start()
    {
         GetComponent<MeshFilter>().mesh = CurrentMesh;
       
    }
    // Update is called once per frame
    void Update()
    {
       scaleEat();

     
    }

    void scaleEat()
    {
        transform.localScale = scaler;
        NbEaten = Mathf.Clamp(NbEaten, 1, 300);

        //juste pour sa soit smooth
        scaler.y = Mathf.Lerp(scaler.y, NbEaten * 1f, .03f);
        scaler.x = Mathf.Lerp(scaler.x, NbEaten * 1f, .03f);
        scaler.z = Mathf.Lerp(scaler.z, NbEaten * 1f, .03f);


        //les different etat du player
        switch (switchSkin)
        {
            case SwitchSizeSkin.little:
                InitializedSize();
                CurrentMesh = meshLittle;
                if (NbEaten >= 100)
                {
                    switchSkin = SwitchSizeSkin.average;
                    GetComponent<MeshFilter>().mesh = meshAverage;
                }
                break;
            case SwitchSizeSkin.average:
                CurrentMesh = meshAverage;
                if (NbEaten < 100)
                {
                    switchSkin = SwitchSizeSkin.little;
                    GetComponent<MeshFilter>().mesh = meshLittle;
                }
                else if (NbEaten >= 200)
                {
                    switchSkin = SwitchSizeSkin.big;
                    GetComponent<MeshFilter>().mesh = meshBig;
                }
                break;
            case SwitchSizeSkin.big:
                CurrentMesh = meshBig;
                // dashCardinalDirection._canDash = true;
                if (NbEaten < 100)
                {
                    switchSkin = SwitchSizeSkin.little;
                    GetComponent<MeshFilter>().mesh = meshLittle;
                }
                else if (NbEaten < 200)
                {
                    switchSkin = SwitchSizeSkin.average;
                    GetComponent<MeshFilter>().mesh = meshAverage;
                }
                break;
        }
    }
}
