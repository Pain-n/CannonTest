using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private GameContext context;

    public CannonProjectile CannonProjectilePrefab;
    public Transform CannonBarrel;

    private List<CannonProjectile> projectilesPull;
    public int ProjectilesPullCapacity;
    public int Damage;
    public float AttackCooldown;
    public float RotationSpeed;
    private bool canShoot;
    public void Init(GameContext _context)
    {
        CursorDisable();

        context = _context;

        canShoot = true;
        projectilesPull = new List<CannonProjectile>(ProjectilesPullCapacity);

        for(int i = 0; i < ProjectilesPullCapacity; i++)
        {
            CannonProjectile item = Instantiate(CannonProjectilePrefab);
            item.Init(context);
            projectilesPull.Add(item);
            item.gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        if(Cursor.visible == false)
        {
            transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, Time.fixedDeltaTime * RotationSpeed).eulerAngles.y, 0);
            CannonBarrel.transform.rotation = Quaternion.Euler(Quaternion.Slerp(CannonBarrel.transform.rotation, Camera.main.transform.rotation, Time.fixedDeltaTime * RotationSpeed).eulerAngles.x - 1,
                                                               Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation, Time.fixedDeltaTime * RotationSpeed).eulerAngles.y,
                                                               0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Cursor.visible == false)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            CursorEnable();
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            CursorDisable();
        }
    }

    private void Shoot()
    {
        if (!canShoot) return;

        for (int i = 0; i< projectilesPull.Capacity; i++)
        {
            if (!projectilesPull[i].gameObject.activeSelf)
            {
                projectilesPull[i].gameObject.SetActive(true);
                projectilesPull[i].transform.position = CannonBarrel.transform.position;
                projectilesPull[i].rb.velocity = CannonBarrel.transform.forward * 50;

                canShoot = false;
                StartCoroutine(ShootCooldown());
                break;
            }
        }
    }

    private void CursorEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void CursorDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canShoot = true;
    }
}
