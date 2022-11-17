using System.Collections.Generic;
using Bodardr.ObjectPooling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[AddComponentMenu("Interaction/Interactors/Interactor", 0)]
public class Interactor : MonoBehaviour
{
    private PoolableComponent<InteractionPrompt> primaryPrompt;
    private PoolableComponent<InteractionPrompt> secondaryPrompt;

    private IInteractorSensor interactorSensor;
    private PlayerInput playerInput;
    private Interactable current;

    private Interaction primary;
    private Interaction secondary;

    private HashSet<Interactable> interactables = new();

    [SerializeField]
    private ScriptableObjectPool promptPool;

    [SerializeField]
    private Transform interactionCanvas = null;

    [SerializeField]
    private UnityEvent onInterruption;

    [SerializeField]
    private UnityEvent onInteractionFreed;

    private Interactable Current
    {
        get => current;
        set
        {
            if (current == value)
                return;

            if (current)
                current.Disable();

            current = value;

            if (current)
                current.Enable(this);
        }
    }

    public UnityEvent OnInterruption => onInterruption;

    public UnityEvent OnInteractionFreed => onInteractionFreed;

    public Interaction Primary
    {
        get => primary;
        set => SetInteraction(ref primary, ref primaryPrompt, value);
    }

    public Interaction Secondary
    {
        get => secondary;
        set => SetInteraction(ref secondary, ref secondaryPrompt, value);
    }

    public HashSet<Interactable> Interactables => interactables;

    private void Awake()
    {
        primaryPrompt = promptPool.Get<InteractionPrompt>(interactionCanvas);
        secondaryPrompt = promptPool.Get<InteractionPrompt>(interactionCanvas);

        interactorSensor = GetComponent<IInteractorSensor>();
        
        if(interactorSensor == null)
            Debug.LogWarning($"No Interactor Sensor has been found on {transform.GetFullPath()}, make sure you've added one!", gameObject);
    }

    private void OnDestroy()
    {
        primaryPrompt.Release();
        secondaryPrompt.Release();
    }

    public void OnInteract()
    {
        if (!Primary)
            return;

        if (Primary.Interact(this))
            OnInterruption?.Invoke();
    }

    public void OnSecondaryInteract()
    {
        if (!Secondary)
            return;

        if (Secondary.Interact(this))
            OnInterruption?.Invoke();
    }

    public void FreeFromInteraction()
    {
        OnInteractionFreed?.Invoke();
    } 

    private void SetInteraction(ref Interaction old, ref PoolableComponent<InteractionPrompt> prompt, Interaction newValue)
    {
        if (old == newValue)
            return;
        
        var wasNull = !old;
        old = newValue;

        var pr = prompt.Content;
        if (!newValue)
        {
            pr.Hide();
            return;
        }
        
        Debug.Log(newValue);

        pr.Interaction = newValue;
        pr.Target = Current.CustomTarget ? Current.CustomTarget : Current.transform;

        if (wasNull)
            pr.Show();
    }
    
    private void Update()
    {
        float minDist = float.MaxValue;
        var pos = transform.position;

        Interactable closestInteractable = null;

        foreach (var i in Interactables)
        {
            var dist = Vector3.Distance(pos, i.transform.position);
            if (dist > minDist)
                continue;

            minDist = dist;
            closestInteractable = i;
        }

        Current = closestInteractable;
    }
}