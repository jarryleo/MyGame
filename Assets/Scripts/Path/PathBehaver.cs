using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Mechanics
{
    public class PathBehaver : MonoBehaviour
    {
        public PatrolPath path;
        internal PatrolPath.Mover mover;
        public float speed = 1f;

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(speed);
                transform.position = mover.Position;
            }
        }
    }
}
