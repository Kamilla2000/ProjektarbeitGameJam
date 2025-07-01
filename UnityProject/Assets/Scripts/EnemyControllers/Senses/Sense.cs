using UnityEngine;

public abstract class Sense : MonoBehaviour
{
    public bool IsDetecting { get; protected set; }

    // wie weit NPC ssehen kann
    public float Range = 1.0f;
    protected GameObject _player;
    protected Vector3 _directionToPlayer;
    public Transform HeadReferenceTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _directionToPlayer = _player.transform.position - HeadReferenceTransform.position;
    }

    public virtual bool IsInRange()
    {
        // Die Distanz zu Spieler ausrechnen: vergleicht die distanz im Quadrat 
        return _directionToPlayer.sqrMagnitude <= Range * Range;
    }
}
