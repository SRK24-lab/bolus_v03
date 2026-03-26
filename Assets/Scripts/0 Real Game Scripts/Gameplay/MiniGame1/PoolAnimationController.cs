using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class PoolAnimationController : MonoBehaviour
{
    public Animator animator;

    public GameStage stageToTrigger;

    private float delay = 4f;

    private int lastCount = -1;



    void Update()
    {
        int currentCount = StatueActivationBase.postActivatedStatueCount;

        // Only update if the value changed
        if (currentCount != lastCount)
        {
            lastCount = currentCount;

            if (currentCount == 2)
            {
                animator.SetTrigger("fluid_level_one");
                Debug.Log("Two statues post-active. Fluid level 1");
            }
            else if (currentCount == 3)
            {
                animator.SetTrigger("fluid_level_two");
                Debug.Log("Three statues post-active. Fluid level 2");

                

                StartCoroutine(ActivationSequence());

                



            }
            else if (currentCount == 4)
            {
                
                animator.SetTrigger("fluid_level_three");
                Debug.Log("Four statues post-active. Fluid level 3");
                
            }
        }
    }

    IEnumerator ActivationSequence()
    {
        yield return new WaitForSeconds(delay);

        GameEvents.TriggerStage(stageToTrigger);
    }
}