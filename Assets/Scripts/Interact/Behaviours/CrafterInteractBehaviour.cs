public class CrafterInteractBehaviour:IInteractionBehaviour, IOpenable
{
    public void OnInteract(IInteractor sender)
    {
        Open(sender);
    }

    public void Open(IInteractor sender)
    {
    }
}
