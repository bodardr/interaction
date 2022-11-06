using UnityEngine;

[AddComponentMenu("Interaction/Interactor and Sensors/Collider Interactor Sensor")]
[RequireComponent(typeof(Interactor))]
public class ColliderInteractorSensor : MonoBehaviour, IInteractorSensor
{
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();

        if (!interactable)
            return;

        interactor.Interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        interactor.Interactables.Remove(interactable);
    }
}