using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Update()
    {
        transform.Translate(Vector3.back * _speed * Time.deltaTime);

        if (transform.position.z - 5.5 < Camera.main.transform.position.z)
        {
            Destroy(this.gameObject, 0f);
        }
    }
}
