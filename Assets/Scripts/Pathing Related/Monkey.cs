using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    private RaycastHit2D[] targets;
    [SerializeField] private float attackRadius = 20;
   
    public string target_layer;
    private bool attacking = false;
    private string recentTarget_id;
    private GameObject recentTarget;
    public Coroutine active_monkey;
    [SerializeField] private TowerAttackType attackScript;

    public List<GameObject> target_list= new List<GameObject>();

    void Start()
    {
        recentTarget_id = null;
        recentTarget= null;
    }


    /*public IEnumerator Monkey_Behaviour()
    {
        attacking = true;
        yield return new WaitForSeconds(1f);

        recentTarget.GetComponent<move>().hp--;
        if (recentTarget.GetComponent<move>().hp <= 0)
        {
            target_list.RemoveAt(0);
            recentTarget_id = null;
            Destroy(recentTarget);

        }

        attacking = false;
        yield break;
    }*/







    
    void Update()
    {
        targets = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero, attackRadius, 1 << LayerMask.NameToLayer(target_layer));
        foreach (RaycastHit2D hit in targets)
        {
            /*print($"Discovered: {hit.transform.gameObject.GetComponent<move>().see}");*/
            if (!target_list.Contains(hit.transform.gameObject))
            {
                target_list.Add(hit.transform.gameObject);
            }
        }
        //if enemy hp is 0, destroy that enemy and set current target to null
        
        //if there is no current target, pull the first one from the list
        if (recentTarget_id == null)
        {
            if (target_list.Count > 0)
            {
                recentTarget_id = target_list[0].transform.gameObject.GetComponent<move>().see;
                recentTarget = target_list[0].transform.gameObject;
            }
        }
        //if there is a current target...
        else
        {
            //check if its hp is 0 or below; select a new target if it is
            if (recentTarget.GetComponent<move>().HP <= 0)
            {
                target_list.RemoveAt(0);
                recentTarget_id = null;
                Destroy(recentTarget);
                recentTarget = null;
                if (target_list.Count > 0)
                {
                    recentTarget_id = target_list[0].transform.gameObject.GetComponent<move>().see;
                    recentTarget = target_list[0].transform.gameObject;
                }
            }
            //attack current target
            if (recentTarget != null)
            {
                attackScript.TryToAttack(recentTarget);
            }
            //moved to basic attack script
            /*if (!attacking)
                active_monkey = StartCoroutine(Monkey_Behaviour());*/
        }

        bool Did_leave = true;
        //if any of the enemies in the current search radius match the current target,
        //didLeave = false because the current target is still in the radius
        foreach (RaycastHit2D hit in targets)
        {
            if (hit.transform.gameObject == recentTarget)
            {
                Did_leave = false;
            }
        }

        //if the current target isn't in the search radius anymore, then remove them from the list
        if (Did_leave && target_list.Count > 0)
        {
            target_list.RemoveAt(0);
            recentTarget = target_list[0];
            recentTarget_id = target_list[0].GetComponent<move>().see;
        }
        
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
