using System.IO;

public static class Rename
{
    public static void RenameFiles(string folderPath)
    {
        // Get a list of all files in the folder
        string[] files = Directory.GetFiles(folderPath);

        // Loop through each file
        for (int i = 0; i < files.Length; i++)
        {
            string oldFilePath = files[i];
            string newFileName = (i + 1) + ".jpg"; // New filename based on your requirement
            string newFilePath = Path.Combine(folderPath, newFileName);

            // Rename the file
            File.Move(oldFilePath, newFilePath);
        }
    }
}
