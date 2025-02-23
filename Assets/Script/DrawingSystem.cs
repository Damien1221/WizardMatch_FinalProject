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

    private List<Gesture> trainingSet = new List<Gesture>(); // List of saved gestures
    private List<Point> points = new List<Point>(); // Collected points for drawing
    private int strokeId = -1; // Tracks strokes in a single gesture
    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>(); // Line renderers for drawing
    private LineRenderer currentGestureLineRenderer;

    private Vector3 virtualKeyPosition = Vector2.zero; // Stores mouse position
    private bool recognized = false;
    private string newGestureName = ""; // Name for custom gestures

    void Start()
    {
        LoadGestures();
    }

    void Update()
    {
        HandleInput();
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

        if (gestureResult.Score > 0.4f) // Minimum confidence threshold
        {
            balloonSpawner.DestroyBalloonByShape(gestureResult.GestureClass); // Destroy balloon matching shape
        }
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

    void SaveGesture(string gestureName)
    {
        string fileName = $"{Application.persistentDataPath}/{gestureName}-{DateTime.Now.ToFileTime()}.xml";

        GestureIO.WriteGesture(points.ToArray(), gestureName, fileName);
        trainingSet.Add(new Gesture(points.ToArray(), gestureName));

        Debug.Log($"Gesture {gestureName} saved!");
    }

    void OnGUI()
    {
        // UI for saving gestures
        GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as:");
        newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

        if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "")
        {
            SaveGesture(newGestureName);
            newGestureName = ""; // Clear after saving
        }
    }
}
