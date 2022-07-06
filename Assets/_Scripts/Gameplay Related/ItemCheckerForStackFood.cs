using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCheckerForStackFood : MonoBehaviour
{
    public static ItemCheckerForStackFood instance;
    
    public List<Transform> stackPositions;
    public Transform foodStack;
    private Vector3 _oldPos, _newPos;
    private List<Transform> _itemPositions;
    private int _itemPosCounter;
    public Transform topItem;
    public List<GameObject> colliders;
    public Transform movePos;
    public List<GameObject> cookedSlices = new List<GameObject>();
    public GameObject customer;
    public GameObject servingPot;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //_oldPos = foodStack.GetChild(0).localPosition;
        //_oldPos = new Vector3(_oldPos.x, _oldPos.y + 0.5f, _oldPos.z);
    }
    private void OnEnable()
    {
        MainController.GameStateChanged += GameManager_OnGameStateChanged;
    }
    private void OnDisable()
    {
        MainController.GameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            UIController.instance.UpdateCookStatus();
            other.GetComponent<Collider>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            
            cookedSlices.Add(other.gameObject);
            other.transform.parent = foodStack;
            other.transform.DOLocalMove(foodStack.GetChild(_itemPosCounter++).localPosition, 0.3f);
            //_newPos = new Vector3(_oldPos.x, _oldPos.y + 0.25f, _oldPos.z);

            if(other.transform.name.Contains("Ommellete")) other.transform.eulerAngles = Vector3.right * -90;
            
            _oldPos = _newPos;
        }
        if (other.gameObject.name.Contains("Bomb"))
        {
            //explosion
            GameObject spawnFx = EffectsController.instance.explosionFx;
            Instantiate(spawnFx, other.transform.position, spawnFx.transform.rotation);
            other.gameObject.SetActive(false);
            UIController.instance.UpdateBombCounter();
            SoundController.instance.PlayClip(SoundController.instance.blast);
        }
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
                StartCoroutine(MoveForServing());
                break;
            case MainController.StateOfGame.Prepared:
                StartCoroutine(FoodPreparedFx());
                break;
        }
    }
    IEnumerator FoodPreparedFx()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(EffectsController.instance.starExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(EffectsController.instance.emojiExplosion, transform.position, Quaternion.identity);
    }
    IEnumerator MoveForServing()
    {
        DOTweenAnimation tweenAnimation = transform.parent.GetComponent<DOTweenAnimation>();
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
        
        PutTheTopBread();
        transform.parent.DOMove(movePos.position, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            Vector3 movepos = new Vector3(transform.localPosition.x + 4, transform.localPosition.y, transform.localPosition.z);
            transform.parent.DOLocalMove(movepos, 1); 
        });

        yield return new WaitForSeconds(1.5f);
        //move the stack
        foodStack.parent = servingPot.transform.parent;
        Vector3 stackFinalMovePos =
            new Vector3(servingPot.transform.localPosition.x, servingPot.transform.localPosition.y, 0.5f);
        foodStack.DOLocalMove(stackFinalMovePos, 1);

        yield return new WaitForSeconds(2f);
        //move the plate to customer
        Transform plateBoard = servingPot.transform.parent;
        plateBoard.DOLocalMove(new Vector3(plateBoard.transform.localPosition.x, 
            plateBoard.transform.localPosition.y, plateBoard.transform.localPosition.z - 1), 1).OnComplete(() =>
        {
            StartCoroutine(customer.GetComponent<Customer>().EatPizza(cookedSlices));
        });
    }
    public void PutTheTopBread()
    {
        topItem.gameObject.SetActive(true);
        topItem.transform.DOLocalMove(foodStack.GetChild(_itemPosCounter).localPosition, 0.3f);
    }
}
