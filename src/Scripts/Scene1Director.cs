using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class Scene1Director : MonoBehaviour
{
    [Header("Actors")]
    public NavMeshAgent playerAgent; // customized avatar
    public SimpleNPC hostNPC;

    [Header("Markers")]
    public Transform playerStart;
    public Transform hostPodium;
    public Transform meetPoint;
    public Transform tablePoint;

    [Header("UI")]
    public GameObject panelChoices;
    public GameObject panelReframe;
    public TextMeshProUGUI textReframe;

    private bool userMadeChoice = false;
    private bool userClickedContinue = false;

    void Start()
    {
        // Setup initial positions
        playerAgent.gameObject.transform.position = playerStart.position;
        hostNPC.gameObject.transform.position = hostPodium.position;
        //hostNPC.transform.position = hostPodium.position;

        // Hide UI
        panelChoices.SetActive(false);
        panelReframe.SetActive(false);

        // Start the scene sequence
        StartCoroutine(RunScene1());
    }

    IEnumerator RunScene1()
    {
        yield return new WaitForSeconds(2.0f); // Brief pause at start

        // ACTION: Player walks to Host
        playerAgent.SetDestination(meetPoint.position);

        // Wait until player arrives
        while (playerAgent.pathPending || playerAgent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        // TRIGGER: Host looks up and down
        hostNPC.TriggerAnim("doLookUpDn");
        yield return new WaitForSeconds(1.5f); // Let animation play a bit

        // INTERVENTION: Pause and show choices
        Time.timeScale = 0.1f; // Slow down time to focus attention
        panelChoices.SetActive(true);
        userMadeChoice = false;
        // Wait here until a UI button is clicked (handled by public functions below)
        yield return new WaitUntil(() => userMadeChoice);
        panelChoices.SetActive(false);

        // REFRAME: Show the avatar's thought
        panelReframe.SetActive(true);
        userClickedContinue = false;
        yield return new WaitUntil(() => userClickedContinue);
        panelReframe.SetActive(false);
        Time.timeScale = 1.0f; // Resume normal time

        // RESOLUTION: Host gestures and leads
        hostNPC.TriggerAnim("doGesture");
        yield return new WaitForSeconds(1.0f);

        hostNPC.MoveTo(tablePoint);
        yield return new WaitForSeconds(0.5f); // Give host a head start
        playerAgent.SetDestination(tablePoint.position);
    }

    // UI Button Functions
    // Hook these up in the Inspector to the UI Buttons

    public void ChoseNegative()
    {
        // Set the reframe text specifically for the negative choice
        textReframe.text = "Avatar smiles: \"They�re just making sure the table is ready for us.\"";
        userMadeChoice = true;
    }

    public void ChoseBenign()
    {
        // Validate their choice
        textReframe.text = "Avatar nods: \"Exactly. Just checking the reservation.\"";
        userMadeChoice = true;
    }

    public void ClickedContinue()
    {
        userClickedContinue = true;
    }
}