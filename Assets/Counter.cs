using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
    [SerializeField]
    private AudioTick audioTickPrefab;
    [SerializeField]
    private AudioListener listener;
    [SerializeField]
    private Text text;
    [SerializeField]
    private InputField divisions;
    [SerializeField]
    private InputField radius;
    [SerializeField]
    private InputField pitchRange;
    [SerializeField]
    private InputField timeBetweenIncrements;

    private int count;
    private Vector2[] points;

    private int Divisions {
        get {
            return int.Parse(divisions.text);
        }
    }

    private float Radius {
        get {
            return float.Parse(radius.text);
        }
    }

    private float PitchRange {
        get {
            return float.Parse(pitchRange.text);
        }
    }

    private float TimeBetweenIncrements {
        get {
            return float.Parse(timeBetweenIncrements.text);
        }
    }

    private Coroutine cr;

    public void Init() {
        this.points = CalcPoints(listener.transform.position.x, listener.transform.position.y, 100, Divisions);
        ShiftPointsToClockPositions();
        Reset();
        cr = StartCoroutine(StartTimer());
    }

    public void Reset() {
        if (cr != null) {
            StopCoroutine(cr);
        }
        count = 0;
    }

    private void ShiftPointsToClockPositions() {
        for (int i = 0; i < points.Length; i++) {
            points[i] = Quaternion.Euler(0, 0, 90) * points[i];
        }
    }

    private IEnumerator StartTimer() {
        float timer = 0;
        while (count < Divisions) {
            if ((timer += Time.deltaTime) >= TimeBetweenIncrements) {
                Increment();
                timer = 0;
            }
            yield return null;
        }
    }

    private void Increment() {
        AudioTick at = Instantiate<AudioTick>(audioTickPrefab);
        at.Position = points[count];
        at.Play(Mathf.Lerp(1 - PitchRange, 1 + PitchRange, (count + 0.0f) / Divisions));
        count++;
    }

    /// <summary>
    /// Modified from
    /// https://stackoverflow.com/questions/18610350/android-dividing-circle-into-n-equal-parts-and-know-the-coordinates-of-each-divi
    /// </summary>
    private static Vector2[] CalcPoints(float x0, float y0, float r, int noOfDividingPoints) {
        float angle = 0;

        Vector2[] points = new Vector2[noOfDividingPoints];

        for (int i = 0; i < noOfDividingPoints; i++) {
            angle = -i * ((360 + 0.0f) / noOfDividingPoints);

            points[i] = new Vector2((int)(x0 + r * Mathf.Cos(Mathf.Deg2Rad * angle)), (int)(y0 + r * Mathf.Sin(Mathf.Deg2Rad * angle)));
        }

        return points;
    }

    private void Update() {
        text.text = string.Format("{0}/{1} counted.", count, Divisions);
    }
}
