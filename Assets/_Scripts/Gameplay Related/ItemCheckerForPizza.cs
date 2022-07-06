using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCheckerForPizza : MonoBehaviour
{
    public List<GameObject> colliders;
    public Transform movePos;
    public List<GameObject> cookedSlices = new List<GameObject>();
    public GameObject customer;
    public GameObject servingPot;
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
            other.transform.DOScale(other.transform.localScale * 0.65f, 0.3f);
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
            SoundController.instance.PlayClip(SoundController.instance.blast);
        }
        if (other.gameObject.CompareTag("failItem"))
        {
            GameController.Instance.CheckLevelFailForDirtyItemAddition();
            cookedSlices.Add(other.gameObject);
            UIController.instance.UpdateFailItemCounter();
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
        Collider collider = GetComponent<Collider>();
        if(collider) collider.enabled = false;
        transform.parent.DOMove(movePos.position, 2).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2.5f);
        //move the plate to customer
        Transform plateBoard = servingPot.transform.parent;
        transform.parent.parent = plateBoard;
        plateBoard.DOLocalMove(new Vector3(plateBoard.transform.localPosition.x, 
            plateBoard.transform.localPosition.y, plateBoard.transform.localPosition.z - 1), 1).OnComplete(() =>
        {
            StartCoroutine(customer.GetComponent<Customer>().EatPizza(cookedSlices));
        });
    }
}
