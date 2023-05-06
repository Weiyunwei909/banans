using System;
using System.Collections;
using System.Collections.Generic;
using GPUInstancer.CrowdAnimations;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{

    public GameObject parent;

    public GameObject resobj;

    public AnimationClip idle;
    public AnimationClip run;
    private GameObject _gameObject;
    private GPUICrowdPrefab go;
    private NavMeshAgent agent;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
          _gameObject = GameObject.Instantiate(resobj);
          go = _gameObject.GetComponent<GPUICrowdPrefab>();
          agent = go.GetComponent<NavMeshAgent>();
          GPUICrowdAPI.StartAnimation(go,idle);
          for (int i = 0; i < 10; i++)
          {
              _gameObject = GameObject.Instantiate(resobj);
              go = _gameObject.GetComponent<GPUICrowdPrefab>();
              agent = go.GetComponent<NavMeshAgent>();
              GPUICrowdAPI.StartAnimation(go,idle);
          }
         
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if ( Physics.Raycast(myRay, out hit) )
        //     {
        //         agent.SetDestination(hit.point);
        //         GPUICrowdAPI.StartAnimation(go,run);
        //     }
        //    
        //     
        // }
    }
}

public class GameObj
{
    public GameObject _gameObject;
    public NavMeshAgent ai;
    public GPUICrowdPrefab _crowdPrefab;
    public string name;
}
