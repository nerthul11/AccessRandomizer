using System.Collections.Generic;

namespace AccessRandomizer.IC;

public class GateObject
{
    public string gate;
    public string sceneName;
    public float x;
    public float y;
    public float backupX;
    public float backupY;
    public string logic;
    public Dictionary<string, string> logicOverrides;
    public Dictionary<string, Dictionary<string, string>> logicSubstitutions;
}