using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimation : MonoBehaviour
{
    [SerializeField] Transform arm;
    [SerializeField] Transform foreArm;
    [SerializeField] Transform digitUpperBase;
    [SerializeField] Transform digitLowerBase;
    [SerializeField] Transform digitUpperEnd;
    [SerializeField] Transform digitLowerEnd;
    [SerializeField] Transform shoulder;
    [SerializeField] Transform elbow;
    [SerializeField] Transform wristUpper;
    [SerializeField] Transform wristLower;
    [SerializeField] Transform knuckleUpper;
    [SerializeField] Transform knuckleLower;

    // Start is called before the first frame update
    void Start()
    {
        arm.localScale = (new Vector3(2,1,1));
        foreArm.localScale = (new Vector3(2,1,1));
        digitUpperBase.localScale = (new Vector2(1,0.5f));
        digitUpperEnd.localScale = (new Vector2(1,0.5f));
        digitLowerBase.localScale = (new Vector2(1,0.5f));
        digitLowerEnd.localScale = (new Vector2(1,0.5f));
        arm.localPosition = (new Vector2(1,-0.5f));
        foreArm.localPosition = (new Vector2(1,-0.5f));
        digitUpperBase.localPosition = (new Vector2(0.5f,-0.25f));
        digitUpperEnd.localPosition = (new Vector2(0.5f,0.25f));
        digitLowerBase.localPosition = (new Vector2(0.5f,0.25f));
        digitLowerEnd.localPosition = (new Vector2(0.5f,-0.25f));
        shoulder.localPosition = (new Vector2(0,0));
        elbow.localPosition = (new Vector2(2,0));
        wristUpper.localPosition = (new Vector2(2,0));
        wristLower.localPosition = (new Vector2(2,-1));
        knuckleUpper.localPosition = (new Vector2(1,-0.5f));
        knuckleLower.localPosition = (new Vector2(1,0.5f));
        shoulder.localEulerAngles = Vector3.forward*(-20);
        elbow.localEulerAngles = Vector3.forward*(40);
        wristUpper.localEulerAngles = Vector3.forward*(45);
        knuckleUpper.localEulerAngles = Vector3.forward*(-75);
        wristLower.localEulerAngles = Vector3.forward*(-45);
        knuckleLower.localEulerAngles = Vector3.forward*(70);
    }

    // Update is called once per frame
    void Update()
    {
        elbow.localEulerAngles += Vector3.forward * Time.deltaTime * 60;
        shoulder.localEulerAngles -= Vector3.forward * Time.deltaTime * 80;

    }
}
