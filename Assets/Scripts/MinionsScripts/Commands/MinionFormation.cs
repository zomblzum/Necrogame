using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFormation : MonoBehaviour
{
    private MinionFormationPoint[] positions;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        positions = GetComponentsInChildren<MinionFormationPoint>();
    }

    void Update()
    {
        if(target)
        {
            transform.position = target.transform.position;
            transform.LookAt(target.transform);
        }
    }

    public GameObject GetPosition(int i)
    {
        if(i < positions.Length)
        {
            return positions[i].gameObject;
        }
        else
        {
            return positions[0].gameObject;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
