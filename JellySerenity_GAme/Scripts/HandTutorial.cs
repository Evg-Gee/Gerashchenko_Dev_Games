using System.Collections;
using UnityEngine;

public class HandTutorial : MonoBehaviour
{
    [SerializeField] private GameObject handPrefab;
    [SerializeField] private float swipeSpeed = 2f; 
    [SerializeField] private JellySerenityManager jellySystems;
    private Animator animator;

    private GameObject handInstance;
    private bool isAnimating = false;

    void Start()
    {
        if (jellySystems.GetJelliesList().Count < 2)
        {
            Debug.LogError("Нужно минимум две желешки для демонстрации свайпа!");
            return;
        }

        handInstance = Instantiate(handPrefab, jellySystems.GetJelliesList()[0].transform.position, Quaternion.identity);
        animator = handInstance.GetComponent<Animator>();
        handInstance.SetActive(true);

        StartCoroutine(LoopSwipeAnimation());
    }

    private IEnumerator SwipeAnimation()
    {
        isAnimating = true;
        
        Vector3 startPosition = jellySystems.GetJelliesList()[jellySystems.GetJelliesList().Count-1].transform.position;
        Vector3 endPosition = jellySystems.GetJelliesList()[jellySystems.GetJelliesList().Count-2].transform.position;
        handInstance.transform.position = startPosition;
        
        handInstance.SetActive(true);
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.2f);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {            
            elapsedTime += Time.deltaTime * swipeSpeed;
            handInstance.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            yield return null;

            // Проверяем касание
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                isAnimating = false;
                handInstance.SetActive(false);
                yield break;
            }
        }

        isAnimating = false;
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.45f);
        handInstance.SetActive(false);
    }

    public void RestartAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(SwipeAnimation());
        }
    }
    private IEnumerator LoopSwipeAnimation()
    {
        while (!isAnimating)
        {
            yield return StartCoroutine(SwipeAnimation());
            yield return new WaitForSeconds(1.1f);
            Debug.Log("isAnimating " + isAnimating);
        }
    }    
    public IEnumerator HideHand()
    {
        if (handInstance != null)
        {
            StopAllCoroutines();
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(0.24f);
            handInstance.SetActive(false);
            isAnimating = false;
        }
    }
    
}
