    using UnityEngine;
    using UnityEngine.InputSystem;

    public static class RayCast
    {
        public static Transform TrySelectItem(Camera camera)
        {
            var hitedCollider = GetRaycastHitByMousePosition(camera).collider;
            if (hitedCollider == null) return null;       
            var collisionObject = hitedCollider.GetComponent<CollisionObject>();
            return collisionObject != null ? collisionObject.Parent : null;
        }

        public static RaycastHit GetRaycastHitByMousePosition(Camera camera)
        {
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out var hit) ? hit : new RaycastHit();
        }
    }
