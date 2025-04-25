using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using PDollarGestureRecognizer;


public class DrawingSystem : MonoBehaviour
{
    public Transform gestureOnScreenPrefab; // For drawing gestures
    public BalloonSpawner balloonSpawner; // Reference to the BalloonSpawner
    public Collider2D drawingAreaCollider;
    public FlyingBall flying_Ball;

    public GameObject _enemyBallVFX;
    public GameObject _closeRing;

    internal ManaBar _manabar;

    public float usedmana = 6f;
    private float usedUltimate = 20f;

    private List<Gesture> trainingSet = new List<Gesture>(); // List of saved gestures
    private List<Point> points = new List<Point>(); // Collected points for drawing
    private int strokeId = -1; // Tracks strokes in a single gesture
    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>(); // Line renderers for drawing
    private LineRenderer currentGestureLineRenderer;
    private SoundManager _soundManager;
    private LightingSpell _lightingSpell;
    private EnemyHealh _enemyhealth;
    private AnimationManager _anim;
    private EnemySpawner _enemySpawner;
    private EnemySpell _enemySpell;

    private Vector3 virtualKeyPosition = Vector2.zero; // Stores mouse position
    private bool recognized = false;
    private bool canClickToDestroy = false;

    private bool isOnCooldown = false;
    public float cooldownTime = 10f;

    void Start()
    {
        _manabar = FindObjectOfType<ManaBar>();
        _anim = FindObjectOfType<AnimationManager>();
        flying_Ball = FindObjectOfType<FlyingBall>();
        _soundManager = FindObjectOfType<SoundManager>();
        _lightingSpell = FindObjectOfType<LightingSpell>();
        _enemyhealth = FindObjectOfType<EnemyHealh>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _enemySpell = FindObjectOfType<EnemySpell>();

        LoadGestures();
    }

    void Update()
    {
        if (_manabar.currentMana >= 6 || _manabar.currentGreenMana >= 6 || _manabar.currentBlueMana >= 6)
        {
            HandleInput();
        }

        if (canClickToDestroy && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("EnemyBall"))
            {
                //Instantiate(_enemyBallVFX, hit.transform.position, Quaternion.identity);
                Destroy(hit.collider.gameObject); // Destroy enemy ball
            }
        }
    }

    void LoadGestures()
    {
        // Load built-in gestures from Resources folder
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
        foreach (TextAsset gestureXml in gesturesXml)
        {
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
        }

        // Load custom gestures saved by the player
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
        }

        Debug.Log($"Loaded {trainingSet.Count} gestures.");
    }

    void HandleInput()
    {
        if (isOnCooldown) return; // Prevent drawing during cooldown

        if (Input.GetMouseButtonDown(0))
        {
            StartNewGesture();
        }
        if (Input.GetMouseButton(0))
        {
            AddPointToGesture();
        }
        if (Input.GetMouseButtonUp(0))
        {
            RecognizeGesture();
        }
    }

    void StartNewGesture()
    {
        if (recognized)
        {
            ResetGesture();
        }

        strokeId++;
        points.Clear();

        // Create new LineRenderer at the cursor position
        Transform tmpGesture = Instantiate(gestureOnScreenPrefab, Vector3.zero, Quaternion.identity);
        currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
        gestureLinesRenderer.Add(currentGestureLineRenderer);
        currentGestureLineRenderer.positionCount = 0;
    }

    void AddPointToGesture()
    {
        virtualKeyPosition = Input.mousePosition; // Get cursor position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10));

        if (drawingAreaCollider != null && !drawingAreaCollider.OverlapPoint(worldPosition))
        {
            return; // Ignore points outside the area
        }

        points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId)); // Add to recognizer

        // Draw gesture
        currentGestureLineRenderer.positionCount++;
        currentGestureLineRenderer.SetPosition(currentGestureLineRenderer.positionCount - 1, worldPosition);
    }

    void RecognizeGesture()
    {
        recognized = true;
        Gesture candidate = new Gesture(points.ToArray());
        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        Debug.Log($"Recognized: {gestureResult.GestureClass} with score: {gestureResult.Score}");

        if (gestureResult.Score > 0.4f && gestureResult.GestureClass == "Lighting" && _manabar.currentMana >= 6
            && _enemySpell != null && _enemySpell.isWeakPointActive)
        {
            //Stop enemy attack action and drawing action and swapping hand && Destroy Evil Ring
            _enemySpawner.StopEnemyAttack();
            FindObjectOfType<SwapHand>().DisableSwapping();

            _soundManager.CorrectSpellSFX();
            _manabar.UsedMana(usedmana);
            //Destroy Evil Ring
            if (_enemySpell != null && _enemySpell.isEvilRingActive)
            {
                StartCoroutine(RingDestroy());
            }
            if (_enemySpell != null && _enemySpell.isWeakPointActive)
            {
                _enemySpell.weakPoint.WeakPointCorrectSpell();
                _enemySpell.isWeakPointActive = false;
            }
            //attack enemy & play sound effect
            StartCoroutine(ThunderComing());
            _anim.ThunderEffect();
            _lightingSpell.ActivateLighting();
            _enemyhealth.ChangeSprite();
            _enemyhealth.DamageEnemy();

            _enemySpell.isWeakPointActive = false;

            //add a Delay & Lighting Effect & CountDown Close Ring
            StartCoroutine(CloseMagicRing());

            StartCoroutine(DrawingCooldown());
        }
        else if (gestureResult.Score > 0.4f && gestureResult.GestureClass == "Lighting" && !_enemySpell.isWeakPointActive)
        {
            _soundManager.WrongSpellSFX();
            Debug.Log("Lighting spell only works when Weak Point is active!");
        }

        else if(gestureResult.Score > 0.4f && gestureResult.GestureClass == "Circle" && _manabar.currentGreenMana >= 6)
        {
            _soundManager.CorrectSpellSFX();
            _manabar.UsedGreenMana(usedmana);
            //Destroy Evil Ring
            if (_enemySpell != null && _enemySpell.isEvilRingActive)
            {
                StartCoroutine(RingDestroy());
            }

            //slow flying ball
            FlyingBall[] allFlyingBalls = FindObjectsOfType<FlyingBall>();
            foreach (FlyingBall ball in allFlyingBalls)
            {
                ball.SlowDownForDrawing();
                ball.SetChangeDirectionTime(5f);
            }
        }
        else if (gestureResult.Score > 0.4f && gestureResult.GestureClass == "five point star" && _manabar.currentBlueMana >= 6)
        {
            //Play sound
            _soundManager.CorrectSpellSFX();

            _manabar.UsedBlueMana(usedmana);
            //destroy enemy ball
            EnemyBall[] allEnemyBalls = FindObjectsOfType<EnemyBall>();
            foreach (EnemyBall enemy in allEnemyBalls)
            {
                enemy.SpawnEffect();
            }

            canClickToDestroy = true;
            StartCoroutine(DisableClickAfterTime(5f));
        }
        else
        {
            _soundManager.WrongSpellSFX();
        }
        //else if (gestureResult.Score > 0.4f) // Minimum confidence threshold
        //{
        //    balloonSpawner.DestroyBalloonByShape(gestureResult.GestureClass); // Destroy balloon matching shape
        //    _manabar.UsedGreenMana(usedmana);
        //}
        ResetGesture();
    }

    private IEnumerator RingDestroy()
    {
        yield return new WaitForSeconds(1f);
        _enemySpell.RingGetDestroy();
    }

    private IEnumerator DrawingCooldown()
    {
        isOnCooldown = true;
        Debug.Log("Drawing is now on cooldown.");
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        Debug.Log("Drawing cooldown ended.");
    }


    private IEnumerator DisableClickAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        canClickToDestroy = false; // Disable clicking after time expires
    }

    private IEnumerator ThunderComing()
    {
        yield return new WaitForSeconds(2f);
        _soundManager.PlayThunder();
    }

    private IEnumerator CloseMagicRing()
    {
        Vector3 newPosition = new Vector3(3.35f, -3.35f, -9.91f );
        Quaternion rotation = Quaternion.Euler(-106.915f, 0, 0);

        yield return new WaitForSeconds(6.5f);
        FindObjectOfType<SwapHand>().EnableSwapping();

        _enemySpell.WeakPointDestroy();
        _enemySpawner.ContinueEnemyAttack();
        _lightingSpell.CloseMagicRing();
        Instantiate(_closeRing, newPosition, rotation);
    }

    void ResetGesture()
    {
        recognized = false;
        strokeId = -1;
        points.Clear();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {
            Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
    }

   
}
