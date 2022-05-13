using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveStaff : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Target Positions")]
    [Space(10)]
    public Transform TargetToInitPosition;
    public Transform TargetToChip;
    public Transform TargetToVault;

    [Header("Where To Go")]
    [Space(10)]
    public bool isWalkTowardChip = false;
    public bool isWakTowardVault = false;
    public bool isWalkTowardInitPosition = false;

    [Space(10)]
    public Animator anime;
    public float rotationSmooth = 0.2f;
    private float turnSmoothVelocity;
    private GameManager GameManager;
    private controlStaff ControlStaff;
    private coreSection section;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        ControlStaff = GetComponent<controlStaff>();
        section = ControlStaff.section;
    }

    // Update is called once per frame
    void Update()
    {
        moveAndRotateTowardTarget();
        getChipLocation();
        SETTARGETS();
    }
    void moveAndRotateTowardTarget()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        anime.SetFloat("speed", agent.velocity.magnitude);
        if (ControlStaff.Cart.Count > 0)
        {
            anime.SetBool("hold", true);
        }
        else
        {
            anime.SetBool("hold", false);
        }
    }
    public void getChipLocation()
    {
        if(ControlStaff.Cart.Count <= ControlStaff.maxLimit)
        {
            if (TargetToChip == null && section.chips.Count > 0)
            {
                if (!section.chips[section.chips.Count - 1].GetComponent<controlChipsObjects>().isOccupied)
                {
                    TargetToChip = section.chips[section.chips.Count - 1].transform;
                    section.chips[section.chips.Count - 1].GetComponent<controlChipsObjects>().isOccupied = true;
                }
            }
        }
       

        if (TargetToChip && TargetToChip.GetComponent<controlChipsObjects>().isStacked)
        {
            TargetToChip = null;
        }
        
    }
    void SETTARGETS()
    {
        if (TargetToChip && ControlStaff.Cart.Count <= ControlStaff.maxLimit && !ControlStaff.SellItemInVault)
        {
            isWalkTowardChip = true;
            isWakTowardVault = false;
            isWalkTowardInitPosition = false;
        }
        if(ControlStaff.Cart.Count >= ControlStaff.maxLimit && !ControlStaff.SellItemInVault)
        {
            isWalkTowardChip = false;
            isWakTowardVault = true;
            isWalkTowardInitPosition = false;
        }
        if(!TargetToChip && ControlStaff.Cart.Count <= ControlStaff.maxLimit && section.chips.Count <= 0)
        {
            if(!ControlStaff.SellItemInVault)
            {
                isWalkTowardChip = false;
                isWakTowardVault = false;
                isWalkTowardInitPosition = true;
            }           
        }

        if (isWakTowardVault) agent.SetDestination(new Vector3(TargetToVault.position.x, 0.2f, TargetToVault.position.z));
        if (TargetToChip && isWalkTowardChip) agent.SetDestination(new Vector3(TargetToChip.position.x, 0.2f, TargetToChip.position.z));
        if (isWalkTowardInitPosition) agent.SetDestination(TargetToInitPosition.position);
    }
}
