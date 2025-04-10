using SimpleFileBrowser;
using System.IO;
using TMPro;
using UnityEngine;

public class CSVSetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boardCSV;
    [SerializeField] private TextMeshProUGUI cardCSV;

    void Start()
    {
        UpdateCSV();
    }

    /// <summary>
    /// Sets the CSV path for the <paramref name="which"/>
    /// </summary>
    /// <param name="which">The CVS to set</param>
    /// <param name="result">The path to set.</param>
    private void SetCSV(string which, string result)
    {
        PlayerPrefs.SetString(which, result);
        UpdateCSV();
    }

    /// <summary>
    /// Update the CSV path display on the UI
    /// </summary>
    private void UpdateCSV()
    {
        boardCSV.text = PlayerPrefs.HasKey("Board") ? Path.GetFileName(PlayerPrefs.GetString("Board")) : "None selected";
        cardCSV.text = PlayerPrefs.HasKey("Card") ? Path.GetFileName(PlayerPrefs.GetString("Card")) : "None selected";
    }

    /// <summary>
    /// Open a file dialog to select the CSV for <paramref name="which"/>
    /// </summary>
    /// <param name="which"></param>
    private void BrowseCSV(string which)
    {
        FileBrowser.SetFilters(false, ".csv");
        FileBrowser.ShowLoadDialog(r => SetCSV(which, r[0]), null, FileBrowser.PickMode.Files, initialPath: Path.GetFullPath("."), title: $"Load {which} CSV");
    }

    /// <summary>
    /// Open a file dialog to select the BoardCSV
    /// </summary>
    public void BrowseBoardCSV()
    {
        BrowseCSV("Board");
    }

    /// <summary>
    /// Open a file dialog to select the CardCSV
    /// </summary>
    public void BrowseCardCSV()
    {
        BrowseCSV("Card");
    }
}
