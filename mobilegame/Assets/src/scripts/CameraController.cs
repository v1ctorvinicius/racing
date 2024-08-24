using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Quaternion lockedRotation;
    
    
    void Start() { lockedRotation = transform.rotation; }
    void Update() {
        transform.rotation = lockedRotation;
        Debug.Log(transform.rotation);
    }
}
