using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreSection : MonoBehaviour
{
    public List<GameObject> chips = new List<GameObject>();
    void Start()
    {
        if(!GetComponent<coreSectionUnlock>())
            FindObjectOfType<GameManager>().SectionCount += 1;
       /* try
        {
            coreSectionUnlock c = GetComponent<coreSectionUnlock>();
        }
        catch
        {
            FindObjectOfType<GameManager>().SectionCount += 1;
        }*/
    }

    
    void Update()
    {
        
    }
}
