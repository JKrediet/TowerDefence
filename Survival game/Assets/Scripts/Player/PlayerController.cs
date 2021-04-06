using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{ 
    public float speed, towerCost = 10;
    public Vector3 moveDir;

    private CharacterController controller;
    private GameManager manager;
    private BuildCheck check;

    public bool buildMode, mayPlace, uiIsOn;
    public Transform Selection, tower;
    public LayerMask mask;

    //build cooldown zooi
    private float cooldown = 0.1f, nextMayPlace, distance, nextTimeToSelect, selectCooldown = 0.5f, dashCooldown = 0.6f, dashDuration, nextDash;
    private int thisNumba;
    public GameObject ground;

    private void Awake()
    {
        ground.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (!uiIsOn)
        {
            if(Time.time > dashDuration)
            {
                moveDir = Movement();
            }
            if (Time.time > nextDash)
            {
                if (Input.GetButtonDown("Dash"))
                {
                    nextDash = Time.time + dashCooldown;
                    dashDuration = Time.time + dashCooldown / 2;
                    if (moveDir == Vector3.zero)
                    {
                        moveDir = transform.forward * 2;
                    }
                    else
                    {
                        moveDir *= 2;
                    }
                }
            }
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        //fire bullet
        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextMayPlace)
            {
                nextMayPlace = Time.time + cooldown;
                if (uiIsOn)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return; //nothing!
                    }
                    else
                    {
                        //turn tower stats ui off
                        TurnOffUi();
                    }
                }                
                else
                {
                    if (buildMode)
                    {
                        if (mayPlace)
                        {
                            if (manager.gold >= towerCost)
                            {
                                //place tower
                                Instantiate(tower, TowerLocation(), Selection.rotation);
                                ground.GetComponent<NavMeshSurface>().BuildNavMesh();
                                manager.gold -= towerCost;
                            }
                        }
                        else
                        {
                            if (Time.time > nextTimeToSelect)
                            {
                                nextTimeToSelect = Time.time + selectCooldown;
                                //find nearest tower
                                distance = 10;
                                check = FindObjectOfType<BuildCheck>();
                                for (int i = 0; i < check.container.Count; i++)
                                {
                                    float towerDistance = Vector3.Distance(check.container[i].position, TowerLocation());
                                    if (check.container[i].GetComponentInChildren<Towers>())
                                    {
                                        if (towerDistance < distance)
                                        {
                                            distance = towerDistance;
                                            thisNumba = i;
                                        }
                                    }
                                    if(check.container[i].GetComponentInChildren<Shop>())
                                    {
                                        if (towerDistance < distance)
                                        {
                                            distance = towerDistance;
                                            thisNumba = i;
                                        }
                                    }
                                }   
                                //show nearest tower
                                if (check.container[thisNumba].GetComponentInChildren<Towers>())
                                {
                                    uiIsOn = true;
                                    ToggleBuildMode();
                                    manager.TowerStats(check.container[thisNumba]);
                                }
                                else if(check.container[thisNumba].GetComponentInChildren<Shop>())
                                {
                                    uiIsOn = true;
                                    ToggleBuildMode();
                                    manager.SelectShop(check.container[thisNumba]);
                                }
                            }
                        }
                    }
                    else
                    {
                        //fire bullet
                        FindObjectOfType<Fire>().FireBullet();
                    }
                }
            }
        }
        //buildmode
        if (Input.GetButtonDown("Fire2"))
        {
            if (!uiIsOn)
            {
                ToggleBuildMode();
                if (FindObjectOfType<BuildCheck>())
                {
                    FindObjectOfType<BuildCheck>().container.Clear();
                }
            }
        }
        Selection.gameObject.SetActive(buildMode);
        Selection.position = TowerLocation();
    }
    #region werkt niet aanzitten!
    public void TurnOffUi()
    {
        manager.DeSelectItem();
        uiIsOn = false;
        ToggleBuildMode();
        Selection.gameObject.SetActive(buildMode);
        FindObjectOfType<BuildCheck>().container.Clear();
        manager.DeSelectTower();
    }
    private Vector3 Movement()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //gravity
        //dir.y = -1f;
        return dir;
    }

    private void ToggleBuildMode()
    {
        buildMode = !buildMode;
    }

    private Vector3 TowerLocation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            point = hit.point;
        }
        else
        {
            point = hit.point;
        }
        return point;
    }
    #endregion //<-- region
}
