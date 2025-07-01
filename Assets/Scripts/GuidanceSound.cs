using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceSound : MonoBehaviour
{
    Vector3 tanfanaPos;
    Vector3 chalicePos;
    Vector3 hellhoundPos;

    AudioSource source;
    bool paused = false;

    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        GuidancePointsActions.OnTanfanaSpawned += AssignTanfanaPos;
        GuidancePointsActions.OnChaliceSpawned += AssignChalicePos;
        GuidancePointsActions.OnHellhoundSpawned += AssignHellhoundPos;
        GameStateActions.OnFirstDomeVisit += MoveToChalice;
        GameStateActions.OnChaliceCollected += MoveToTanfana;
        GameStateActions.OnChaliceReturned += MoveToHellhound;
        GameStateActions.OnGamePause += Pause;
    }

    private void OnDisable()
    {
        GuidancePointsActions.OnTanfanaSpawned -= AssignTanfanaPos;
        GuidancePointsActions.OnChaliceSpawned -= AssignChalicePos;
        GuidancePointsActions.OnHellhoundSpawned -= AssignHellhoundPos;
        GameStateActions.OnFirstDomeVisit -= MoveToChalice;
        GameStateActions.OnChaliceCollected -= MoveToTanfana;
        GameStateActions.OnChaliceReturned -= MoveToHellhound;
        GameStateActions.OnGamePause -= Pause;
    }


    private void Pause(bool pause)
    {
        paused = pause;
    }

    void AssignHellhoundPos(Vector3 pos)
    {
        hellhoundPos = pos;
    }

    void AssignTanfanaPos(Vector3 pos)
    {
        Debug.Log("Assigning");
        tanfanaPos = pos;
        if (!GameStateActions.domeVisited)
        {
            Debug.Log("moving to tanfana");
            transform.position = tanfanaPos;
        }
    }

    void AssignChalicePos(Vector3 pos)
    {
        chalicePos = pos;
    }

    void MoveToHellhound()
    {
        if (hellhoundPos != null)
        {
            transform.position = hellhoundPos;
        }
    }

    void MoveToChalice()
    {
        if (chalicePos != null)
        {
            transform.position = chalicePos;
        }
    }

    void MoveToTanfana()
    {
        if (tanfanaPos != null)
        {
            transform.position = tanfanaPos;
        }
    }

    private void Update()
    {
        if (HellhoundActions.hellhoundFightOngoing || paused || GameStateActions.inDialogue)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
            return;
        }

        if (!source.isPlaying)
        {
            source.Play();
        }

    }
}
