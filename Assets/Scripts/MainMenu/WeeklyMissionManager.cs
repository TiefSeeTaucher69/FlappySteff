using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Mission
{
    public string description;
    public string id; // z. B. "collect_50_coins"
    public int goal;
    public int current;
    public bool isCompleted;
}

public class WeeklyMissionManager : MonoBehaviour
{
    public List<Mission> allPossibleMissions;
    public List<Mission> activeMissions;

    private void Start()
    {
        LoadMissions();
    }

    public void LoadMissions()
    {
        DateTime now = DateTime.Now;
        DateTime thisMonday = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);

        if (!PlayerPrefs.HasKey("WeeklyMissionStartTime"))
        {
            GenerateNewWeeklyMissions(thisMonday);
        }
        else
        {
            long savedTime;
            if (long.TryParse(PlayerPrefs.GetString("WeeklyMissionStartTime"), out savedTime))
            {
                DateTime savedMonday = DateTime.FromBinary(savedTime);
                if (thisMonday > savedMonday)
                {
                    GenerateNewWeeklyMissions(thisMonday);
                }
                else
                {
                    LoadMissionsFromPrefs();
                }
            }
            else
            {
                Debug.LogWarning("WeeklyMissionStartTime ungültig – neue Missionen werden generiert.");
                GenerateNewWeeklyMissions(thisMonday);
            }
        }
    }

    public void GenerateNewWeeklyMissions(DateTime weekStart)
    {
        activeMissions = new List<Mission>();

        var shuffled = allPossibleMissions.OrderBy(x => UnityEngine.Random.value).ToList();
        var usedIds = new HashSet<string>();
        int added = 0;

        foreach (var mission in shuffled)
        {
            if (!usedIds.Contains(mission.id))
            {
                usedIds.Add(mission.id);

                activeMissions.Add(new Mission
                {
                    id = mission.id,
                    description = mission.description,
                    goal = mission.goal,
                    current = 0,
                    isCompleted = false
                });

                added++;
                if (added >= 3)
                    break;
            }
        }

        PlayerPrefs.SetString("WeeklyMissionStartTime", weekStart.ToBinary().ToString());
        SaveMissionsToPrefs();
    }

    public void SaveMissionsToPrefs()
    {
        string json = JsonUtility.ToJson(new MissionWrapper { missions = activeMissions });
        PlayerPrefs.SetString("WeeklyMissions", json);
        PlayerPrefs.Save();
    }

    public void LoadMissionsFromPrefs()
    {
        if (!PlayerPrefs.HasKey("WeeklyMissions")) return;

        try
        {
            string json = PlayerPrefs.GetString("WeeklyMissions");
            activeMissions = JsonUtility.FromJson<MissionWrapper>(json).missions;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Fehler beim Laden der Weekly Missions: " + e.Message);
            activeMissions = new List<Mission>();

            // Fallback: Neue Missionen generieren
            DateTime thisMonday = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
            GenerateNewWeeklyMissions(thisMonday);
        }
    }

    public void UpdateMission(string id, int amount)
    {
        foreach (var m in activeMissions)
        {
            if (m.id == id && !m.isCompleted)
            {
                m.current += amount;
                if (m.current >= m.goal)
                {
                    m.current = m.goal;
                    m.isCompleted = true;
                    Debug.Log("Mission abgeschlossen: " + m.description);
                }
            }
        }
        SaveMissionsToPrefs();
    }

    [System.Serializable]
    private class MissionWrapper
    {
        public List<Mission> missions;
    }

    public float GetMissionProgress(string id)
    {
        var m = activeMissions.FirstOrDefault(x => x.id == id);
        return m != null ? Mathf.Clamp01((float)m.current / m.goal) : 0f;
    }

}
