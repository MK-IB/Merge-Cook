using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private Animator _animator;
    public Transform movePos;
    public GameObject happyParticleFx;
    public GameObject eatingFx;
    public GameObject wowTextFx;
    public GameObject winParticle;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        MainController.GameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        MainController.GameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(MainController.StateOfGame newState, MainController.StateOfGame oldState)
    {
        if(newState == MainController.StateOfGame.Decoration)
            StartCoroutine(ApproachToCounter());
    }
    IEnumerator ApproachToCounter()
    {
        _animator.SetTrigger("walk");
        transform.DOMove(movePos.position, 1f).OnComplete(() => { _animator.SetTrigger("idle");});
        yield return new WaitForSeconds(1);
        
    }

    public IEnumerator Eat()
    {
        _animator.SetTrigger("eat");
        yield return new WaitForSeconds(0.5f);
        eatingFx.SetActive(true);
        PreparingPot.instance.StartCoroutine(PreparingPot.instance.FinishFood());
        
        yield return new WaitForSeconds(3);
        eatingFx.SetActive(false);
        _animator.SetTrigger("doneEating");
        MainController.instance.SetActionType(MainController.StateOfGame.EatingDone);
        CameraController.instance.lastFocusCamera.SetActive(true);
        yield return new WaitForSeconds(1);
        wowTextFx.SetActive(true);
        yield return new WaitForSeconds(1);
        happyParticleFx.SetActive(true);
        yield return new WaitForSeconds(2);
        winParticle.SetActive(true);
    }
}
