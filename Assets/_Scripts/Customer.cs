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
        if (GameController.Instance.dirtyItemAdded)
        {
            _animator.SetTrigger("failEat");
            yield return new WaitForSeconds(1.5f);
            transform.DOLocalMove(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3), 0.5f);
            transform.DOLocalRotate(Vector3.up * 130, 0.5f);
            yield return new WaitForSeconds(1f);
            //vomiting fx
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger("vomit");
            
            yield return new WaitForSeconds(3.5f);
            MainController.instance.SetActionType(MainController.StateOfGame.DirtyAdded);
        }
        else
        {
            _animator.SetTrigger("eat");
            SoundController.instance.PlayClip(SoundController.instance.eating);
            yield return new WaitForSeconds(0.5f);
            eatingFx.SetActive(true);
            PreparingPot.instance.StartCoroutine(PreparingPot.instance.FinishFood());
        
            yield return new WaitForSeconds(3);
            StartCoroutine(WinEffects());
        }
        
    }

    public IEnumerator EatPizza(List<GameObject> pizzaToppings)
    {
        if (GameController.Instance.dirtyItemAdded)
        {
            _animator.SetTrigger("failEat");
            yield return new WaitForSeconds(1.5f);
            transform.DOLocalMove(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3), 0.5f);
            transform.DOLocalRotate(Vector3.up * 130, 0.5f);
            yield return new WaitForSeconds(1f);
            //vomiting fx
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger("vomit");
            SoundController.instance.PlayClip(SoundController.instance.vomit);
            
            yield return new WaitForSeconds(3.5f);
            MainController.instance.SetActionType(MainController.StateOfGame.DirtyAdded);
        }
        else
        {
            _animator.SetTrigger("eat");
            SoundController.instance.PlayClip(SoundController.instance.eating);
            yield return new WaitForSeconds(0.5f);
            eatingFx.SetActive(true);
            float waitTime = 3f / pizzaToppings.Count;
            for (int i = 0; i < pizzaToppings.Count; i++)
            {
                pizzaToppings[i].SetActive(false);
                yield return new WaitForSeconds(waitTime);
            }
        
            //yield return new WaitForSeconds(waitTime * pizzaToppings.Count);
            StartCoroutine(WinEffects());   
        }
    }

    IEnumerator WinEffects()
    {
        eatingFx.SetActive(false);
        _animator.SetTrigger("doneEating");
        MainController.instance.SetActionType(MainController.StateOfGame.EatingDone);
        CameraController.instance.lastFocusCamera.SetActive(true);
        SoundController.instance.PlayClip(SoundController.instance.girlYummy);
        yield return new WaitForSeconds(1);
        SoundController.instance.PlayClip(SoundController.instance.wow);
        wowTextFx.SetActive(true);
        yield return new WaitForSeconds(1);
        happyParticleFx.SetActive(true);
        yield return new WaitForSeconds(2);
        winParticle.SetActive(true);
        SoundController.instance.PlayClip(SoundController.instance.confetti);
        MainController.instance.SetActionType(MainController.StateOfGame.Win);
    }
}
