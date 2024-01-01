using System;
using System.Collections;

using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField]
    //if more than one curve make as array
    Transform[] routes;
    public Transform[] routesAssigned;
    //while hold the index of the next array to follow in routes
    int routetoGo;

    public bool isPenny;

    //Current step
    float t;

    [SerializeField]
    Vector2 POS;
    [SerializeField]
    public float speed;

    //prevents coroutine from running whilst running a coroutine already
    bool coroutineAllowed;

    void Start()
    {












        //for when the enemy spawner sets an enemy on a path
        if (routesAssigned.Length > 0)
        {
            routes = routesAssigned;
        }
        routetoGo = 0;
        t = 0f;
        
        coroutineAllowed = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed)
            StartCoroutine(Travel(routetoGo));
    }
    IEnumerator Travel(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        while (t < 1)
        {
            if (routes[routeNum].gameObject.GetComponent<Target_Path_Script>().OtherlineType == true)
            {
                
                t += Time.deltaTime* speed;
                POS= Vector2.Lerp(p0, p1, t);
                transform.position = POS;
                yield return null;

            }
            else
            {
                t += Time.deltaTime * speed;
                POS = Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2
                    + Mathf.Pow(t, 3) * p3;
                transform.position = POS;
                yield return new WaitForEndOfFrame();
            }
        }

        t = 0f;
        routetoGo += 1;

        if (routetoGo > routes.Length - 1)
        {
            /*change this part so it doesnt start at beginning*/
            // doesnt index out of array;
            if (isPenny) {
                gameObject.SetActive(false);
                gameObject.GetComponent<CollisionDamageNoLimit>().SentToStartResponse();
               
                routetoGo = 0;
                gameObject.SetActive(true);
            
            
            }
           
                routetoGo = 0;
                if (GetComponent<move>() != null)
                {
                    GetComponent<move>().AttackTower();
                }

         
           
        }

        coroutineAllowed = true;
        yield break;




    }
}
