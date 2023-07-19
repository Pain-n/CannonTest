using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private GameContext context;

    private Animator anim;

    private int HP;
    private int speed;

    public Transform Canvas;
    public Slider HPSlider;

    private Vector3 movePos;
    private Vector3 velocity;
    private bool canMove;
    public void Init(GameContext _context, EnemyItem parameters)
    {
        context = _context;

        anim = GetComponent<Animator>();

        HP = parameters.HP;
        speed = parameters.Speed;
        HPSlider.maxValue = HP;
        HPSlider.value = HP;

        StartCoroutine(MoveCycle());
    }

    private void FixedUpdate()
    {
        transform.LookAt(context.Cannon.transform);
        Canvas.rotation = Quaternion.Euler(0, Quaternion.Slerp(Canvas.rotation, Camera.main.transform.rotation, Time.fixedDeltaTime * 100).eulerAngles.y, 0);

        if (canMove) transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, 3, speed);
    }

    public void TakeDamage(int value)
    {
        HP -= value;
        HPSlider.value = HP;

        if(HP <= 0)
        {
            Death();
        }
    }

    IEnumerator MoveCycle()
    {      
        while (true)
        {
            yield return new WaitForSeconds(3);
            movePos = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
            canMove = true;
            anim.SetBool("IsWalking", canMove);
            yield return new WaitForSeconds(3);
            canMove = false;
            anim.SetBool("IsWalking", canMove);

        }
    }

    public void Death()
    {
        context.EnemiesOnField.Remove(this);
        context.Vallet++;
        context.Score++;
        context.UpdateValletUI();
        context.UpdateEnemiesCountUI();
        context.UpdateScoreUI();
        Destroy(gameObject);
    }
}
