using System;
using System.Collections.Generic;
using UnityEngine;

namespace Container
{
    public abstract class Item:ICraftable,IPickable,IDropable,IPlaceble
    {
        protected abstract ICraftable _craftBehaviour { get; }
        protected abstract IPickable _pickBehaviour { get; }
        protected abstract IDropable _dropBehaviour { get; }
        protected abstract IPlaceble _placeBehaviour { get; }
    
        public void Craft()
        {
            _craftBehaviour.Craft();
        }

        public void Pick()
        {
            _pickBehaviour.Pick();
        }

        public void Drop()
        {
            _dropBehaviour.Drop();
        }

        public void Place()
        {
            _placeBehaviour.Place();
        }
    }  
}

