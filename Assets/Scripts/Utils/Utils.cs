using UnityEngine;

public static class Utils
{
    public static bool CompareLayers(LayerMask layerMask, int layerIndex)
    {
        return (layerMask & (1 << layerIndex)) != 0;
    }
}
