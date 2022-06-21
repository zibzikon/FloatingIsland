using System;
using Enums;
using UnityEngine;

namespace Factories.Crafter
{
    public class CrafterBehaviourFactory 
    {
        public ICrafterBehaviour Get(Tier tier)
        {
            return tier switch
            {
                Tier.First => new FirstTierCrafterBehaviour(),
                Tier.Second => new SecondTierCrafterBehaviour(),
                _ => throw new NullReferenceException()
            };
        }
    }
}