using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coreSectionUnlock : MonoBehaviour
{
    [Header("Locked")]
    [Space(10)]
    public GameObject LockedSection;
    public GameObject unlockPartical;
    public Transform unlockParticalPosition;
    public string Camera;
    public float delayInUnlock = 1.5f;
    public int levelToUnlock = 5;
    public bool isLocked = true;

    [Header("Unlock")]
    [Space(5)]
    public GameObject unlockedSection;

    [Header("Processing")]
    [Space(5)]
    public Animator cameraAnimator;
    private GameManager GameManager;
    private AudioManager AudioManager;
    private bool isUnlocked;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
    }

    
    void Update()
    {
        Unlock();
    }

    void Unlock()
    {
        if(isLocked && GameManager.Level >= levelToUnlock)
        {
            StartCoroutine(unlockSection(delayInUnlock));
        }
        if (!isLocked)
        {
            LockedSection.SetActive(false);
            unlockedSection.SetActive(true);
        }
        if(!isLocked && !isUnlocked)
        {
            FindObjectOfType<GameManager>().SectionCount += 1;
            isUnlocked = true;
        }
    }
    IEnumerator unlockSection(float t)
    {
        cameraAnimator.Play(Camera);
        yield return new WaitForSeconds(t);
        if (!unlockedSection.activeSelf)
        {
            Destroy(Instantiate(unlockPartical, unlockParticalPosition.position, Quaternion.Euler(-90f,0,0)), 5);
            AudioManager.source.PlayOneShot(AudioManager.areaUnlock);
        }
        LockedSection.SetActive(false);
        unlockedSection.SetActive(true);
        isLocked = false;        
    }
}
