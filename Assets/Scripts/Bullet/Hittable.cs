using FishNet.Object;

namespace Survival.Ingame.Bullet
{
    public abstract class Hittable : NetworkBehaviour
    {
        public virtual void Hit(int damage)
        {

        }
    }

}

