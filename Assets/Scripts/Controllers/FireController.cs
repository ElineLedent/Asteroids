using UnityEngine;

public class FireController
{
    public void FireBullet(ObjectPool pool, Transform muzzleTransform, Vector3 shipVelocity)
    {
        // Get available bullet from bullet pool
        GameObject bullet = pool.GetFreePoolObject();

        // Set start location on bullet
        bullet.transform.position = muzzleTransform.position;

        Projectile projectileScript = bullet.GetComponent<Projectile>();
        if (DebugUtilities.Verify(projectileScript != null, "Projectile script not found"))
        {
            // Set direction on projectile through projectile script
            projectileScript.ProjectileDirection = muzzleTransform.forward;
            //Set start velocity of projectile through projectile script
            projectileScript.StartVelocity = shipVelocity;
        }

        bullet.SetActive(true);
    }
}