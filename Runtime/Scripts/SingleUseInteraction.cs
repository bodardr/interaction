using UnityEngine;
using UnityEngine.Events;

public class SingleUseInteraction : Interaction
#if BODARDR_ROOMS
    , ILevelSerializable
#endif
{
    private bool hasTriggered = false;

    [SerializeField]
    private bool interruptsUser = false;

    [SerializeField]
    private UnityEvent OnInteraction;

#if BODARDR_ROOMS
    [Header("Serialization")]
    [SerializeField]
    private bool serializeToLevelData = true;
#endif

    public override bool CanInteract(Interactor interactor) => !hasTriggered;

    public override bool Interact(Interactor interactor)
    {
        this.interactor = interactor;

        OnInteraction.Invoke();

#if BODARDR_ROOMS
        if (serializeToLevelData)
            RoomManager.Instance.LevelData.Serialize(this);
#endif
        hasTriggered = true;

        return interruptsUser;
    }

    public void OnEventLoaded()
    {
        OnInteraction.Invoke();
    }
}