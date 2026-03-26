using UnityEngine;

public class KeyFollowAndDeposit : MonoBehaviour
{
    [Header("Keys & Player")]
    public GameObject keyOne;
    public GameObject keyTwo;
    public Transform player;

    [Header("Follow Settings")]
    public Vector3 offsetOne = new Vector3(0.1f, 0.8f, 0f);
    public Vector3 offsetTwo = new Vector3(-0.2f, 0.8f, 0f);
    public float followSpeed = 5f;

    [Header("Statues")]
    public ChiefCellActivationState chiefCell;
    public ParietalCellActivationState parietalCell;
    public MucusCellActivationState mucusCell;
    public GCellActivationState gCell;

    private bool followPlayer = false;
    private bool mucusTriggered = false;

    void Start()
    {
        keyOne.SetActive(false);
        keyTwo.SetActive(false);
    }

    /// Give keys to the player
    public void ActivateKey()
    {
        keyOne.SetActive(true);
        keyTwo.SetActive(true);

        keyOne.transform.position = player.position + offsetOne;
        keyTwo.transform.position = player.position + offsetTwo;

        followPlayer = true;

    }

    /// Deposit key 1
    public void DepositKey1()
    {
        if (!keyOne.activeSelf) return;

        keyOne.SetActive(false);
        Debug.Log("Key One has been used.");

        chiefCell.UpdateState();
    }

    /// Deposit key 2
    public void DepositKey2()
    {
        if (!keyTwo.activeSelf) return;

        keyTwo.SetActive(false);
        Debug.Log("Key Two has been used.");

        parietalCell.UpdateState();
    }

    void Update()
    {
        if (!followPlayer) return;

        if (keyOne.activeSelf)
        {
            keyOne.transform.position = Vector3.Lerp(
                keyOne.transform.position,
                player.position + offsetOne,
                followSpeed * Time.deltaTime
            );
        }

        if (keyTwo.activeSelf)
        {
            keyTwo.transform.position = Vector3.Lerp(
                keyTwo.transform.position,
                player.position + offsetTwo,
                followSpeed * Time.deltaTime
            );
        }

        // Stop following if both keys gone
        if (!keyOne.activeSelf && !keyTwo.activeSelf)
        {
            followPlayer = false;
        }

        // Activate mucus once
        if (!mucusTriggered &&
            chiefCell.currentState == StatueActivationBase.StatueState.PostActivation &&
            parietalCell.currentState == StatueActivationBase.StatueState.PostActivation)
        {
            mucusTriggered = true;
            mucusCell.ActivateStatue();
        }
    }
}