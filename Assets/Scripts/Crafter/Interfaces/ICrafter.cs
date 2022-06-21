using Enums;

public interface ICrafter
{
    ICrafterBehaviour CrafterBehaviour { get; }
    Tier Tier { get; }
}
