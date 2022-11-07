using System.Collections;
using System.Collections.Generic;
using Bodardr.UI.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Interaction/Interactable")]
public class Interactable : MonoBehaviour
{
    private Interactor interactor;
    private SmartCoroutine updateCoroutine;

    [SerializeField]
    private List<InteractionEntry> primaryInteractions;

    [SerializeField]
    private List<InteractionEntry> secondaryInteractions;

    [FormerlySerializedAs("customPrimaryTarget")]
    [Header("Targets")]
    [SerializeField]
    private Transform customTarget;
    
    [SerializeField]
    private bool customUpdateFrequency = false;

    [ShowIf(nameof(customUpdateFrequency))]
    [SerializeField]
    private float updateFrequency = 0.3f;

    public Transform CustomTarget => customTarget;

    private void Awake()
    {
        updateCoroutine = new SmartCoroutine(this, UpdateInteractionsCoroutine);
    }

    private IEnumerator UpdateInteractionsCoroutine()
    {
        var waitByUpdateFrequency = new WaitForSeconds(updateFrequency);
        while (isActiveAndEnabled)
        {
            primaryInteractions.Sort((x, y) => x.weight.CompareTo(y.weight));
            secondaryInteractions.Sort((x, y) => x.weight.CompareTo(y.weight));

            InteractionEntry primary = null;
            foreach (var x in primaryInteractions)
            {
                if (!x.interaction.CanInteract(interactor))
                    continue;

                primary = x;
                break;
            }

            InteractionEntry secondary = null;
            foreach (var x in secondaryInteractions)
            {
                if (!x.interaction.CanInteract(interactor))
                    continue;

                secondary = x;
                break;
            }

            interactor.Primary = primary?.interaction;
            interactor.Secondary = secondary?.interaction;

            yield return waitByUpdateFrequency;
        }

        updateCoroutine = null;
    }

    public void Enable(Interactor interactor)
    {
        this.interactor = interactor;
        updateCoroutine.Start();
    }

    public void Disable()
    {
        updateCoroutine.Stop();

        interactor.Primary = null;
        interactor.Secondary = null;
    }
}