using UnityEngine;
using System.Collections;

public enum InputType
{
    DEFAULT,
    VR
}

public enum NetworkType
{
    HOST,
    CLIENT
}

public static class GlobalData
{
    public static InputType inputType = InputType.DEFAULT;

    public static string lastMessage;

    public static NetworkType networkType;

    public static string connectionAddress;
    public static int connectionPort = 7742;
}
