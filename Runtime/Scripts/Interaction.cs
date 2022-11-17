using System.ComponentModel;
using UnityEngine;

[AddComponentMenu("Interaction/Interaction")]
public abstract class Interaction : MonoBehaviour, INotifyPropertyChanged
{
    private string text;
    protected Interactor interactor;

    [SerializeField]
    private string initialText = "Interact";

    public string Text
    {
        get => text;
        set
        {
            if (text == value)
                return;
            
            text = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void Start()
    {
        Text = initialText;
    }
    
    public virtual bool CanInteract(Interactor interactor) => true;
    
    /// <summary>
    /// Fires the interaction
    /// </summary>
    /// <returns>Returns true if it interrupts the interactor's actions, false if it is a single, uninterrupted interaction</returns>
    public abstract bool Interact(Interactor interactor);

    public void LiberateInteractor() => interactor.FreeFromInteraction();
}