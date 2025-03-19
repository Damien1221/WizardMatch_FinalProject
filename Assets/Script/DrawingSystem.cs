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

    internal AnimationManager _Ultimate;
    internal ManaBar _manabar;

    public float usedmana = 6f;
    private float usedUltimate = 20f;

    private List<Gesture> trainingSet = new List<Gesture>(); // List of saved gestures
    private List<Point> points = new List<Point>(); // Collected points for drawing
    private int strokeId = -1; // Tracks strokes in a single gesture
    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>(); // Line renderers for drawing
    private LineRenderer currentGestureLineRenderer;

    private List<EnemyBall> clickedEnemies = new List<EnemyBall>();
    private Vector3 virtualKeyPosition = Vector2.zero; // Stores mouse position
    private bool recognized = false;
    private bool canClickToDestroy = false;

    void Start()
    {
        _manabar = FindObjectOfType<ManaBar>();
        _Ultimate = FindObjectOfType<AnimationManager>();
        flying_Ball = FindObjectOfType<FlyingBall>();

        LoadGestures();
    }

    void Update()
    {
        if (_manabar.currentMana >= 6 || _manabar.currentGreenMana >= 6)
        {
            HandleInput();
        }

        if (canClickToDestroy && Input.GetMouseButtonDown(0))
        {
            DetectEnemyClick();
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

        if (gestureResult.Score > 0.4f && gestureResult.GestureClass == "Lighting" && _manabar.currentMana >= 6)
        {
            _manabar.UsedMana(usedmana);
            //attack enemy


            //_Ultimate.PlayUltimate();
            //_manabar.UsedMana(usedUltimate);
        }
        else if(gestureResult.Score > 0.4f && gestureResult.GestureClass == "Circle" && _manabar.currentGreenMana >= 6)
        {
            _manabar.UsedGreenMana(usedmana);
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
            _manabar.UsedBlueMana(usedmana);
            //destroy enemy ball
            EnemyBall[] allEnemyBalls = FindObjectsOfType<EnemyBall>();
            foreach (EnemyBall enemy in allEnemyBalls)
            {
                enemy.SpawnEffect();
            }

            canClickToDestroy = true;
            StartCoroutine(DestroyClickedEnemiesAfterTime(5f)); // Start countdown
        }
        //else if (gestureResult.Score > 0.4f) // Minimum confidence threshold
        //{
        //    balloonSpawner.DestroyBalloonByShape(gestureResult.GestureClass); // Destroy balloon matching shape
        //    _manabar.UsedGreenMana(usedmana);
        //}

        ResetGesture();
    }
    void DetectEnemyClick()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

        if (hit != null)
        {
            EnemyBall enemy = hit.GetComponent<EnemyBall>();
            if (enemy != null)
            {
                enemy.OnClick(); // Spawn the click effect
                clickedEnemies.Add(enemy); // Track clicked enemies
            }
        }
    }

    private IEnumerator DestroyClickedEnemiesAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (EnemyBall enemy in clickedEnemies)
        {
            if (enemy != null)
            {
                enemy.DestroyEnemy(); // Destroy enemy and remove spawn effect
            }
        }

        clickedEnemies.Clear(); // Reset the list
        canClickToDestroy = false; // Disable clicking
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
