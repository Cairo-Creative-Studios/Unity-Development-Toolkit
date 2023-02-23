using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// The Component Collision Interface allows the Standard Object Manager to perform collision related Events, and
    /// interface with the Collision system externally.
    /// </summary>
    public interface IComponentCollision
    {
        /// <summary>
        /// This object's collider
        /// </summary>
        public Collider Collider { get; set; }
    }
}