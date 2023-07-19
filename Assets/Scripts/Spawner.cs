using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameContext context;
    public void Init(GameContext _context)
    {
        context = _context;
    }

    public void SpawnEnemy(EnemyItem parameters)
    {
        EnemyController enemy = Instantiate(Resources.Load<EnemyController>("Prefabs/Enemy"), transform);
        enemy.Init(context, parameters);
        context.EnemiesOnField.Add(enemy);
        Vector2 rndPos = Random.insideUnitCircle.normalized * Random.Range(5, 20);
        enemy.transform.position = new Vector3(rndPos.x, 0.6f, rndPos.y);

        context.UpdateEnemiesCountUI();
    }
}
