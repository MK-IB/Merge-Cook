using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PreparingPot : MonoBehaviour
{
    public static PreparingPot instance;

    public GameObject holdingHand;
    public List<GameObject> colliders;
    public Transform movePos;
    public List<GameObject> cookedSlices = new List<GameObject>();
    public GameObject customer;
    public GameObject servingPot;
    public Vector3 servingAngle;
    public Transform foodStack;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        MainController.GameStateChanged += GameManager_OnGameStateChanged;
    }
    private void OnDisable()
    {
        MainController.GameStateChanged -= GameManager_OnGameStateChanged;
    }

    void GameManager_OnGameStateChanged(MainController.StateOfGame newState, MainController.StateOfGame oldState)
    {
        switch (newState)
        {
            case MainController.StateOfGame.Lose:
                //Add explosion force
                Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
                for (int i = 0; i < colliders.Length; i++)
                {
                    Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                    if(rb == null) colliders[i].AddComponent<Rigidbody>();

                    if (colliders[i].GetComponent<MeshCollider>())
                    {
                        colliders[i].GetComponent<MeshCollider>().convex = true;
                    }
                    if (rb == null && colliders[i].GetComponent<MeshCollider>())
                    {
                        colliders[i].AddComponent<Rigidbody>();
                        colliders[i].GetComponent<MeshCollider>().convex = true;
                    }
                    colliders[i].GetComponent<Rigidbody>().isKinematic = false;

                    if (rb != null)
                    {
                        rb.AddExplosionForce(500, transform.position, 20, 3.0f);
                    }
                }
                break;
            case MainController.StateOfGame.Decoration:
                StartCoroutine(MoveForDecoration());
                break;
        }
    }

    public IEnumerator MoveForDecoration()
    {
        transform.parent.parent = null;
        if(holdingHand != null)
        {
            Vector3 handMovePosFrom = new Vector3(holdingHand.transform.position.x, holdingHand.transform.position.y,
                holdingHand.transform.position.z + 5);
            holdingHand.transform.DOLocalMove(handMovePosFrom, 0.5f).From();
        }
        
        DOTweenAnimation tweenAnimation = GetComponent<DOTweenAnimation>();
        if(tweenAnimation) tweenAnimation.enabled = false;
        DOTween.Kill(gameObject);

        if (colliders.Count > 0)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].SetActive(false);
            }
        }

        //move and rotate to decoration pos
        Collider collider = GetComponent<Collider>();
        if(collider) collider.enabled = false;
        transform.DOMove(movePos.position, 2).SetEase(Ease.Linear);
        
        //moving and serving tweens
        yield return new WaitForSeconds(2.4f);
        transform.DOLocalRotate(servingAngle, 1).OnComplete(() =>
        {
            Vector3 movepos = new Vector3(transform.localPosition.x + 4, transform.localPosition.y, transform.localPosition.z);
            transform.DOLocalMove(movepos, 1);
        });
        for (int i = 0; i < cookedSlices.Count; i++)
        {
            cookedSlices[i].transform.parent = servingPot.transform;
        }
        servingPot.GetComponent<ServedPlate>().MakeCurryLevel();
        
        yield return new WaitForSeconds(2f);
        //move the plate towards customer
        Transform plateBoard = servingPot.transform.parent;
        plateBoard.DOLocalMove(new Vector3(plateBoard.transform.localPosition.x, 
            plateBoard.transform.localPosition.y, plateBoard.transform.localPosition.z - 1), 1).OnComplete(() =>
        {
            StartCoroutine(customer.GetComponent<Customer>().Eat());
        });
    }

    public IEnumerator MoveStackForDecoration()
    {
        DOTweenAnimation tweenAnimation = GetComponent<DOTweenAnimation>();
        if(tweenAnimation) tweenAnimation.enabled = false;
        DOTween.Kill(gameObject);

        if (colliders.Count > 0)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].SetActive(false);
            }
        }
        Collider collider = GetComponent<Collider>();
        if(collider) collider.enabled = false;
        //put the top bread
        ItemCheckerForStackFood.instance.PutTheTopBread();
        yield return new WaitForSeconds(1f);
        transform.DOMove(movePos.position, 2).SetEase(Ease.Linear);
        
        yield return new WaitForSeconds(2.2f);
        foodStack.parent = null;
        Vector3 movepos = new Vector3(transform.localPosition.x + 4, transform.localPosition.y, transform.localPosition.z);
        transform.DOLocalMove(movepos, 1);
        
        Transform plateBoard = servingPot.transform.parent;
        foodStack.parent = plateBoard;
        foodStack.DOLocalMove(new Vector3(0,0,0.46f), 0.5f);
        plateBoard.DOLocalMove(new Vector3(plateBoard.transform.localPosition.x, 
            plateBoard.transform.localPosition.y, plateBoard.transform.localPosition.z - 1), 1).OnComplete(() =>
        {
            StartCoroutine(customer.GetComponent<Customer>().Eat());
        });
    }
    

    public IEnumerator FinishFood()
    {
        float waitTime = 3f / cookedSlices.Count;
        for (int i = 0; i < cookedSlices.Count; i++)
        {
            cookedSlices[i].SetActive(false);
            yield return new WaitForSeconds(waitTime);
        }
    }
}