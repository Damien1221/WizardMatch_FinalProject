using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class EnemySpell : MonoBehaviour
{
    public GameObject _evilRing;
    public GameObject _closeEvilRing;
    public bool isEvilRingActive = false;
    public bool isWeakPointActive = false;
    public WeakPoint weakPoint;

    private GameObject activeEvilRing;
    private Animator animator;
    private SoundManager _soundManager;
    private PlayerHealth _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _soundManager = FindObjectOfType<SoundManager>();
        weakPoint = FindObjectOfType<WeakPoint>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateEvilRing()
    {
        if (!gameObject.activeInHierarchy) return; // prevent animator bug

        animator.Play("EnemyCircle_Activate");
        SpawnEvilRing();
        isEvilRingActive = true;

        // 50% chance to activate WeakPoint
        if (weakPoint != null && UnityEngine.Random.value < 0.5f)
        {
            weakPoint.WeakPointActivate();
            isWeakPointActive = true;
        }
    }

    public void CloseEvilRing()
    {
        isEvilRingActive = false;
        Vector3 newPosition = new Vector3(3.35f, -3.35f, -9.91f);
        Quaternion rotation = Quaternion.Euler(-106.915f, 0, 0);

        animator.Play("EnemyCircle_Close");
        Instantiate(_closeEvilRing, newPosition, rotation);
    }

    IEnumerator CloseWeakPoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isWeakPointActive)
        {
            weakPoint.WeakPointClose();
            isWeakPointActive = false;
        }
    }

    public void WeakPointDestroy()
    {
        if (!isWeakPointActive)
        {
            weakPoint.WeakPointDestroy();
            isWeakPointActive = false;
        }
    }

    public void RingGetDestroy() //player defence
    {
        isEvilRingActive = false;
        _soundManager.PlayBreakMagicRing();
        animator.Play("EnemyCircle_GetDestroy");

        if (activeEvilRing != null)
        {
            Destroy(activeEvilRing);
            activeEvilRing = null;
        }
    }

    public void SpawnEvilRing() //GetDestroy
    {
        StartCoroutine(CloseWeakPoint(5.5f));
        Vector3 newPosition = new Vector3(3.38f, -3.24f, 0);
        activeEvilRing = Instantiate(_evilRing, newPosition, Quaternion.identity); // Store the reference

        FindObjectOfType<LowHealthEffect>().TriggerLowHealthEffect();
        activeEvilRing.transform.DOScale(0.25f, 5f)
            .SetDelay(1f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                Destroy(activeEvilRing);
                //player get attack
                _playerHealth.DamagePlayer();
                CloseEvilRing();
            });
    }
}
