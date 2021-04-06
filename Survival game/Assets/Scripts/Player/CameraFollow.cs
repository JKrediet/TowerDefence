using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 player;
    private float cameraHeight;
    private Transform tower;

    private void Start()
    {
        cameraHeight = transform.position.y;
    }
    private void LateUpdate()
    {
        if(tower == null)
        {
            player = FindObjectOfType<PlayerController>().transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.x, cameraHeight, player.z), 0.1f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(tower.position.x + 3, 10, tower.position.z), 0.1f);
        }
    }
    public void ZoomInOnTower(Transform towertje)
    {
        if (tower == null)
        {
            tower = towertje;
        }
        else
        {
            tower = null;
        }
    }
}
