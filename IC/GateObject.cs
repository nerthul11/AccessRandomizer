using System.Collections.Generic;

namespace AccessRandomizer.IC;

public class GateObject
{
    public string gate;
    public string sceneName;
    public string objectName;
    public float x;
    public float y;
    public string mapScene;
    public float mapX;
    public float mapY;
    public string logic;
    public Dictionary<string, string> logicOverrides;
    public Dictionary<string, Dictionary<string, string>> logicSubstitutions;
}