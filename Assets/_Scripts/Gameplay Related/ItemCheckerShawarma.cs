using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCheckerShawarma : MonoBehaviour
{
    public List<GameObject> colliders;
    public Transform movePos;
    public List<GameObject> cookedSlices = new List<GameObject>();
    public GameObject customer;
    public GameObject servingPot;
    public Vector3 servingAngle;
    
    public List<Transform> toppingPositions;
    private int _toppingPosCounter;
    
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
            //item.ReleaseItemOnCurry(transform.parent.parent);
            //other.GetComponent<Collider>().enabled = false;
            cookedSlices.Add(other.gameObject);
            other.transform.parent = transform.parent;
            other.transform.DOScale(other.transform.localScale * 0.65f, 0.5f);
            other.transform.DOLocalMove(toppingPositions[_toppingPosCounter++].localPosition, 0.3f).OnComplete(() =>
            {
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.GetComponent<Collider>().enabled = false;
            });
        }
        if (other.gameObject.name.Contains("Bomb"))
        {
            //explosion
            GameObject spawnFx = EffectsController.instance.explosionFx;
            Instantiate(spawnFx, other.transform.position, spawnFx.transform.rotation);
            other.gameObject.SetActive(false);
            UIController.instance.UpdateBombCounter();
        }
    }
    void GameManager_OnGameStateChanged(MainController.StateOfGame newState, MainController.StateOfGame oldState)
    {
        switch (newState)
        {
            case MainController.StateOfGame.Lose:
                //Add explosion force
                Collider[] colliders = Physics.OverlapSphere(transform.parent.position, 5);
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
                        rb.AddExplosionForce(500, transform.parent.position, 20, 3.0f);
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
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -4);
        Instantiate(EffectsController.instance.emojiExplosion, pos, Quaternion.identity);
    }
    
    IEnumerator MoveForServing()
    {
        GetComponent<Collider>().enabled = false;
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
        transform.parent.DOMove(movePos.position, 2).SetEase(Ease.Linear);
        
        yield return new WaitForSeconds(2.4f);
        //rotate the holders
        transform.parent.DOLocalRotate(servingAngle, 1);
        
        yield return new WaitForSeconds(1f);
        Vector3 movepos = new Vector3(transform.localPosition.x + 4, transform.localPosition.y, transform.localPosition.z);
        transform.parent.DOLocalMove(movepos, 1);
        //activate RB of cookedSlices;
        for (int i = 0; i < cookedSlices.Count; i++)
        {
            cookedSlices[i].GetComponent<Rigidbody>().isKinematic = false;
            cookedSlices[i].GetComponent<Collider>().enabled = true;
        }
        
        for (int i = 0; i < cookedSlices.Count; i++)
        {
            cookedSlices[i].transform.parent = servingPot.transform;
        }

        yield return new WaitForSeconds(2.5f);
        //move the plate to customer
        Transform plateBoard = servingPot.transform.parent;
        //transform.parent.parent = plateBoard;
        plateBoard.DOLocalMove(new Vector3(plateBoard.transform.localPosition.x, 
            plateBoard.transform.localPosition.y, plateBoard.transform.localPosition.z - 1), 1).OnComplete(() =>
        {
            StartCoroutine(customer.GetComponent<Customer>().EatPizza(cookedSlices));
        });
    }
}
