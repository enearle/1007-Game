using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
    // Singleton Manager class that handles spawning of objects like projectiles, enemies, and obstacles
    public static WorldManager Instance;
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private Transform TopSpawn;
    [SerializeField] private Transform BotSpawn;
    [SerializeField] private TMP_Text TMPScore;
    [SerializeField] private TMP_Text TMPHealth;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private GameObject button;
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private SceneChanger sceneChanger;
    public SoundManager sound;

    [SerializeField] private float spawnFreq = 4;
    private float spawnTimer = 0;
    private bool canSpawn = true;
    
    private float obstacleSpawnFreq;
    private float obstacleSpanwtimer = 0;
    private bool canSpawnObstacle = false;
    
    private int Score = 0;
    
    bool botSwitch = true;
    bool gameLost = false;
    
    private Player player;
    private IObjectPool<Missile> missilePool;
    private IObjectPool<Enemy> enemyPool;

    private List<Enemy> aliveEnemies;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance != this)
        {
            Debug.Log("Error: Attempted to instantiate WorldManager singleton");
            Destroy(this);
        }

        Camera c = Camera.main;
        
        
        player = Instantiate(playerPrefab);
        player.SetReferenceToWorld(this);

        aliveEnemies = new List<Enemy>();
        obstacleSpawnFreq = Random.Range(6f, 13f);
        
        missilePool = new ObjectPool<Missile>(CreateMissile,GetMissile,ReleaseMissile);
        enemyPool = new ObjectPool<Enemy>(CreateEnemy, GetEnemy, ReleaseEnemy);
    }

    private void Start()
    {
        sound.Play(0);
        TMPScore.text = "Score: 0";
        UpdateHealth(100);
    }

    // Enemies //////////////////////////////////////////////////////

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
            Application.Quit();
        
        if (spawnFreq < 0.5)
        {
            if(!gameLost)
                WinGame();
            return; 
        }
            

        if (canSpawnObstacle)
        {
            Obstacle o = Instantiate(obstaclePrefab);
            o.transform.position = new Vector3(8.5f, Random.Range(-6f, 6f));
            canSpawnObstacle = false;
            obstacleSpawnFreq = Random.Range(6f, 13f);
        }
        else
        {
            if (obstacleSpanwtimer >= obstacleSpawnFreq)
            {
                canSpawnObstacle = true;
                obstacleSpanwtimer = 0;
            }
            else
            {
                obstacleSpanwtimer += Time.deltaTime;
            }
        }
            
        
        if (canSpawn)
        {
            Enemy e = SpawnEnemy();
            if (botSwitch)
                e.transform.position = BotSpawn.position;
            else
                e.transform.position = TopSpawn.position;
            e.moveUp = botSwitch;
            botSwitch = !botSwitch;
            canSpawn = false;
        }
        else
        {
            if (spawnTimer >= spawnFreq)
            {
                canSpawn = true;
                spawnTimer = 0;
                spawnFreq -= spawnFreq * 0.03f;
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    private void ReleaseEnemy(Enemy obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void GetEnemy(Enemy obj)
    {
        obj.gameObject.SetActive(true);
    }

    private Enemy CreateEnemy()
    {
        Enemy e = Instantiate(enemyPrefab);
        e.SetReferenceToWorld(this);
        return e;
    }

    public Enemy SpawnEnemy()
    {
        Enemy e = enemyPool.Get();
        return e;
    }

    public void DestroyEnemy(Enemy e)
    {
        enemyPool.Release(e);
    }

    
    // Projectiles ///////////////////////////////////////////////////
    
    private void ReleaseMissile(Missile obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void GetMissile(Missile obj)
    {
        sound.Play(3);
        obj.gameObject.SetActive(true);
    }

    private Missile CreateMissile()
    {
        Missile m = Instantiate(missilePrefab);
        return m;
    }
    
    public void DestroyMissile(Missile obj)
    {
        if (obj.isActiveAndEnabled)
        {
            missilePool.Release(obj);
        }
    }
    
    public Missile SpawnMissile(bool isPlayerMissile)
    {
        Missile m = missilePool.Get();
        m.playerOwned = isPlayerMissile;
        return m;
    }

    public void UpdateScore()
    {
        Score++;
        TMPScore.text = $"Score: {Score}";
    }

    public void UpdateHealth(float Health)
    {
        TMPHealth.text = $"Health = {Health} %";
    }
    
    // UI //////////////////////////////////////////////////

    public void LoseGame()
    {
        gameLost = true;
        Destroy(player.gameObject);
        endText.color = Color.red;
        endText.text = "You lose";
        endText.gameObject.SetActive(true);
        button.SetActive(true);
    }
    
    public void WinGame()
    {
        Time.timeScale = 0;
        endText.color = Color.green;
        endText.text = "You win";
        endText.gameObject.SetActive(true);
        button.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        Instantiate(sceneChanger);
    }
    

    // FX //////////////////////////////////////////////////

    // TODO make another object pool
    public void CreateExplosion(Vector3 location, float scale = 1)
    {
        Explosion e = Instantiate(explosionPrefab);
        e.transform.position = location;
        e.transform.localScale = Vector3.one * scale;
    }
    
    
}
