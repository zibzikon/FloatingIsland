
public class ContainerInteractBehaviour:IInteractionBehaviour,IOpenable
{
    public void OnInteract(IInteractor sender)
    {
        Open(sender);
    }

    public void Open(IInteractor sender)
    {
    }
}
