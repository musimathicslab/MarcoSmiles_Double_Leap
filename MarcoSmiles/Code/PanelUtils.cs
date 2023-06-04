using SFB;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contains methods for managing panels to change, import and export system configurations.
/// </summary>
public class PanelUtils{
    /// <summary>
    /// Opens a panel to select a dataset for export to any directory on the PC.
    /// </summary>
    public static void OpenExportPanel(){

        // Go to datasets folder
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        //  Open panel
        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);

        //  If dataset exist, you can export the dataset
        if (Directory.Exists(path)){
            var expPath = StandaloneFileBrowser.OpenFolderPanel("Choose the location to export to", tmp_path, false);
            var finalPath = expPath.Last() + "\\" + paths.Last().Split('\\').ToList().Last();

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);
            FileUtils.Export(path, finalPath);
        }
    }

    /// <summary>
    /// Opens a panel for selecting a dataset to be imported into the MyDataset folder.
    /// </summary>
    public static void OpenImportPanel(){

        // Go to datasets folder
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        //  Open panel
        var paths = StandaloneFileBrowser.OpenFolderPanel("Export Dataset", tmp_path, false);
        var path = String.Join("/", paths);

        //  If dataset exist, you can export the dataset
        if (Directory.Exists(path)){
            var newdirName = tmp_path + paths.Last().Split('\\').ToList().Last();
            FileUtils.Import(paths.Last(), newdirName);
        }
        // Check existence of files needed to play
        FileUtils.CheckForDefaultFiles();
    }

    /// <summary>
    /// Opens a panel to select the configuration (and dataset) to be used in the MyDataset folder.
    /// </summary>
    public static void OpenPanel(){
        //  // Go to datasets folder
        var tmp = FileUtils.GeneratePath().Split('/').ToList();
        tmp.Remove(tmp.Last());
        tmp.Remove(tmp.Last());

        var tmp_path = "";
        foreach (var item in tmp)
            tmp_path += item + '/';

        // Open panel
        var paths = StandaloneFileBrowser.OpenFolderPanel("Change Dataset", tmp_path, false);
        if ( paths.Length > 0 ) {
            var path = paths.Last().Split('\\').ToList().Last();

            if (path.Length != 0){
                FileUtils.selectedDataset = paths.Last().Split('\\').ToList().Last();

                // Populate matrix of the neural network with the new configuration
                TestML.Populate();
            }
        }
        // Check existence of files needed to play
        FileUtils.CheckForDefaultFiles();
    }
}

