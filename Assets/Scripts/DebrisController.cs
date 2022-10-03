using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour
{
    [SerializeField] private GameObject _debrisPrefab;
    void Start()
    {
        //if (Random.Range(0, 2) == 0) {
            {
            int angle = Random.Range(0, 12);
            
            int howManyDebris = Random.Range(3, 10);
            for (int i = 0; i < howManyDebris; i++) {

                if (Random.Range(0, 100) < 20)
                    continue;

                var debris = Instantiate(_debrisPrefab);
                debris.transform.position = new Vector3(
                    this.transform.position.x,
                    this.transform.position.y - 5,
                    this.transform.position.z
                );
                debris.transform.parent = this.transform;            
            
                debris.transform.RotateAround(this.transform.position, Vector3.forward, (angle+i) * 30);
            }
        }
    }
}
