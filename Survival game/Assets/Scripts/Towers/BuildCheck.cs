using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    public GameObject range;
    private PlayerController playerController;
    public List<Transform> container;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(container.Count > 0)
        {
            playerController.mayPlace = false;
            range.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            playerController.mayPlace = true;
            range.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
    private void OnTriggerEnter(Collider _inRange)
    {
        if (_inRange.transform.tag == "Tower" || _inRange.transform.tag == "Wall" || _inRange.transform.tag == "Shop")
        {
            container.Add(_inRange.transform);
        }
    }
    private void OnTriggerExit(Collider _inRange)
    {
        if (_inRange.transform.tag == "Tower" || _inRange.transform.tag == "Wall")
        {
            container.Remove(_inRange.transform);
        }
    }
}
