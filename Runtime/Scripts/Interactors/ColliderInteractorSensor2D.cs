using UnityEngine;

[AddComponentMenu("Interaction/Interactors/Collider Interactor Sensor 2D")]
[RequireComponent(typeof(Interactor))]
public class ColliderInteractorSensor2D : MonoBehaviour, IInteractorSensor
{
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();

        if (!interactable)
            return;

        interactor.Interactables.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();
        interactor.Interactables.Remove(interactable);
    }
}