/// <summary>
/// Interface which all kinds of assets should implement.
/// </summary>
public interface IAsset
{
    /// <summary>
    /// Get the current value of this asset
    /// </summary>
    /// <returns>The base value of this asset</returns>
    public int GetValue();
}
