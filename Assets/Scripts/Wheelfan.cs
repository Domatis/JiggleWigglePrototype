using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gemi tekerleklerinin dönmesinin sağlandığı sınıf.
public class Wheelfan : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update() 
    {
        Vector3 angle = transform.localEulerAngles;
        angle.z += speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(angle);
    }

}
