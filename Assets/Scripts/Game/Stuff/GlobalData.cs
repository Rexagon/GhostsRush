using UnityEngine;
using System.Collections;

public enum InputType
{
    Default,
    VR
}

public static class GlobalData
{
    public static InputType inputType = InputType.Default;

    public static string lastMessage;
}
