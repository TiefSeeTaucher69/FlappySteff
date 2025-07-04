using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MissionType
{
    CollectBlatt,
    CollectInOneRun,
    TotalScore,
    TimeInOneRun,
    TotalRuns,
    TotalTime,
    TotalJumps,
    CollectStreak,
    TimeStreak
}

[System.Serializable]
public class Mission
{
    public string description;
    public string id;
    public int goal;
    public int current;
    public bool isCompleted;
    public MissionType type;
}

public class WeeklyMissionManager : MonoBehaviour
{
    public static WeeklyMissionManager Instance { get; private set; }

    public event Action OnMissionsLoaded;
    public WeeklyMissionRewardScript weeklyMissionRewardScript;

    public List<Mission> allPossibleMissions;
    public List<Mission> activeMissions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMissions();
        CheckCompletedMissions();
    }

    public void LoadMissions()
    {
        DateTime now = DateTime.Now;
        DateTime thisMonday = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);

        if (!PlayerPrefs.HasKey("WeeklyMissionStartTime"))
        {
            Debug.Log("Keine gespeicherten Missionen gefunden - generiere neue");
            GenerateNewWeeklyMissions(thisMonday);
        }
        else
        {
            if (long.TryParse(PlayerPrefs.GetString("WeeklyMissionStartTime"), out long savedTime))
            {
                DateTime savedMonday = DateTime.FromBinary(savedTime);
                if (thisMonday > savedMonday)
                {
                    Debug.Log("Woche ist vorbei - generiere neue Missionen");
                    GenerateNewWeeklyMissions(thisMonday);
                }
                else
                {
                    Debug.Log("Lade bestehende Missionen");
                    LoadMissionsFromPrefs();
                }
            }
            else
            {
                Debug.LogWarning("WeeklyMissionStartTime ungültig – neue Missionen werden generiert.");
                GenerateNewWeeklyMissions(thisMonday);
            }
        }

        OnMissionsLoaded?.Invoke();
        CheckCompletedMissions();
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
                    isCompleted = false,
                    type = mission.type
                });

                added++;
                if (added >= 3)
                    break;
            }
        }

        Debug.Log($"Neue Missionen generiert: {activeMissions.Count}");
        foreach (var mission in activeMissions)
        {
            Debug.Log($"Mission: {mission.description}, Type: {mission.type}, Goal: {mission.goal}");
        }

        PlayerPrefs.SetString("WeeklyMissionStartTime", weekStart.ToBinary().ToString());
        SaveMissionsToPrefs();

        // Clear all reward collected flags for new missions
        ClearAllRewardCollectedFlags();
    }

    public void SaveMissionsToPrefs()
    {
        string json = JsonUtility.ToJson(new MissionWrapper { missions = activeMissions });
        PlayerPrefs.SetString("WeeklyMissions", json);
        PlayerPrefs.Save();
        Debug.Log("Missionen gespeichert: " + json);
    }

    public void LoadMissionsFromPrefs()
    {
        if (!PlayerPrefs.HasKey("WeeklyMissions")) return;

        try
        {
            string json = PlayerPrefs.GetString("WeeklyMissions");
            Debug.Log("Lade Missionen aus PlayerPrefs: " + json);

            MissionWrapper wrapper = JsonUtility.FromJson<MissionWrapper>(json);

            if (wrapper == null || wrapper.missions == null)
            {
                Debug.LogWarning("Weekly Missions JSON ist leer oder ungültig. Neue Missionen werden generiert.");
                DateTime thisMonday = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
                GenerateNewWeeklyMissions(thisMonday);
                return;
            }

            activeMissions = wrapper.missions;
            FixMissionTypes();

            Debug.Log($"Missionen erfolgreich geladen: {activeMissions.Count}");
            foreach (var mission in activeMissions)
            {
                Debug.Log($"Geladene Mission: {mission.description}, Type: {mission.type}, Current: {mission.current}, Goal: {mission.goal}");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Fehler beim Laden der Weekly Missions: " + e.Message);
            activeMissions = new List<Mission>();

            DateTime thisMonday = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
            GenerateNewWeeklyMissions(thisMonday);
        }
    }

    private void FixMissionTypes()
    {
        foreach (var activeMission in activeMissions)
        {
            if (activeMission.type == 0)
            {
                var originalMission = allPossibleMissions.FirstOrDefault(m => m.id == activeMission.id);
                if (originalMission != null)
                {
                    activeMission.type = originalMission.type;
                    Debug.Log($"Type korrigiert für Mission {activeMission.id}: {activeMission.type}");
                }
            }
        }
    }

    public void UpdateMission(MissionType type, int amount)
    {
        Debug.Log($"UpdateMission aufgerufen: Type={type}, Amount={amount}");

        bool anyUpdated = false;

        foreach (var m in activeMissions)
        {
            if (m.type == type && !m.isCompleted)
            {
                Debug.Log($"Updating mission: {m.description}, Current: {m.current}, Goal: {m.goal}");

                int oldCurrent = m.current;

                switch (type)
                {
                    case MissionType.CollectInOneRun:
                    case MissionType.TimeInOneRun:
                        if (amount > m.current)
                            m.current = amount;
                        break;
                    case MissionType.CollectStreak:
                    case MissionType.TimeStreak:
                        m.current = amount > 0 ? m.current + 1 : 0;
                        break;
                    default:
                        m.current += amount;
                        break;
                }
                if (m.current >= m.goal)
                {
                    m.current = m.goal;
                    m.isCompleted = true;
                    Debug.Log("Mission abgeschlossen: " + m.description);

                    if (!RewardAlreadyCollected(m.id))
                    {
                        if (weeklyMissionRewardScript != null)
                        {
                            Debug.Log($"ShowReward wird aufgerufen für Mission: {m.description}");
                            weeklyMissionRewardScript.ShowReward(m.description, m.id);
                        }
                        else
                        {
                            Debug.LogError("weeklyMissionRewardScript ist NULL beim Versuch ShowReward aufzurufen!");
                        }
                    }
                }

                if (oldCurrent != m.current)
                {
                    anyUpdated = true;
                    Debug.Log($"Mission updated: {m.description} - {oldCurrent} -> {m.current}");
                }
            }
        }

        if (anyUpdated)
        {
            SaveMissionsToPrefs();
            // UI wird jetzt über Event aktualisiert
        }
        else
        {
            Debug.Log($"Keine Mission für Type {type} gefunden oder alle bereits abgeschlossen");
        }
    }

    public void CheckCompletedMissions()
    {
        Debug.Log("🔍 CheckCompletedMissions aufgerufen");

        if (activeMissions == null || activeMissions.Count == 0)
        {
            Debug.LogWarning("Keine aktiven Missionen gefunden.");
            return;
        }

        foreach (var mission in activeMissions)
        {
            Debug.Log($"Mission: {mission.description}, Current: {mission.current}, Goal: {mission.goal}, isCompleted: {mission.isCompleted}");

            if (mission.isCompleted && !RewardAlreadyCollected(mission.id))
            {
                Debug.Log("Mission abgeschlossen erkannt (Belohnung noch nicht eingesammelt): " + mission.description);

                if (weeklyMissionRewardScript != null)
                {
                    Debug.Log("Zeige Belohnungspanel für abgeschlossene Mission: " + mission.description);
                    weeklyMissionRewardScript.ShowReward(mission.description, mission.id);
                }
                else
                {
                    Debug.LogWarning("weeklyMissionRewardScript ist null!");
                }
            }
        }
    }

    public bool RewardAlreadyCollected(string missionId)
    {
        return PlayerPrefs.GetInt($"MissionRewardCollected_{missionId}", 0) == 1;
    }

    public void MarkRewardCollected(string missionId)
    {
        PlayerPrefs.SetInt($"MissionRewardCollected_{missionId}", 1);
        PlayerPrefs.Save();
        Debug.Log($"Belohnung für Mission {missionId} als eingesammelt markiert.");
    }

    private void ClearAllRewardCollectedFlags()
    {
        foreach (var mission in activeMissions)
        {
            PlayerPrefs.DeleteKey($"MissionRewardCollected_{mission.id}");
        }
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class MissionWrapper
    {
        public List<Mission> missions;
    }

    public void NotifyMissionsLoaded()
    {
        OnMissionsLoaded?.Invoke();
    }

    public void ReloadMissions()
    {
        LoadMissions();
    }

    /// <summary>
    /// Wird von WeeklyMissionRewardScript aufgerufen, wenn Belohnung eingesammelt wurde.
    /// </summary>
    /// <param name="missionId"></param>
    public void OnRewardCollected(string missionId)
    {
        MarkRewardCollected(missionId);
    }
}
