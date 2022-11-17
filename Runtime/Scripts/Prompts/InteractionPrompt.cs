using System.ComponentModel;
using Bodardr.UI;
using UnityEngine;

[RequireComponent(typeof(UIView))]
[AddComponentMenu("Interaction/Prompts/Interaction Prompt")]
public class InteractionPrompt : MonoBehaviour, INotifyPropertyChanged
{
    private Transform target;
    private Interaction interaction;

    private UIView uiView;
    protected bool IsHidden => uiView.IsHidden;

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Target)));
        }
    }

    public Interaction Interaction
    {
        get => interaction;
        set
        {
            interaction = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Interaction)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void Awake()
    {
        uiView = GetComponent<UIView>();
    }

    public void Show()
    {
        uiView.Show();
    }

    public void Hide()
    {
        uiView.Hide();
    }

    public void Clear()
    {
        Target = null;
        Interaction = null;
    }
}