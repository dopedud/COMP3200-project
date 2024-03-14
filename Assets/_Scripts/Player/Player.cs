using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour {

    protected MLEnvManager academy;

    [SerializeField] protected LayerMask objectiveMask;

    protected virtual void Awake() {
        academy = transform.root.GetComponent<MLEnvManager>();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if ((1 << other.gameObject.layer | objectiveMask) != objectiveMask) return;
        
        academy.ClearObjective(other.gameObject);
	}

    public virtual void SetDestination(Vector3 position) {}
    public virtual void Respawn(Vector3 position) {}

}
