using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpCompress.Archives;
using System.IO;
using SSL.Models;
using System.Xml.Linq;

namespace SSL
{
    public class Decompressor
    {
        public Comics Convert(HttpPostedFileBase file)
        {
            var comic = new Comics();

            try
            {
                var fileName = Path.GetFileName(file.FileName);
                var fileNameWithoutExension = Path.GetFileNameWithoutExtension(file.FileName);
                string cbzFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Media/Comics"), fileName);
                string extractionPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Media/turnjs4/samples/docs"), fileNameWithoutExension + "/");

                string readOnlinePath = fileNameWithoutExension;
                string downloadPath = "~/Media/Comics/" + fileName;

                comic.ReadOnline = readOnlinePath;
                comic.Download = downloadPath;

                if (!Directory.Exists(extractionPath))
                {
                    Directory.CreateDirectory(extractionPath);
                }

               
                using (var archive = ArchiveFactory.Open(cbzFilePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            if (entry.Key.EndsWith(".xml"))
                            {
                                using (var xmlStream = entry.OpenEntryStream())
                                {
                                    XDocument doc = XDocument.Load(xmlStream);
                                    comic.Name = doc.Root.Element("Series").Value;
                                    comic.Summary = doc.Root.Element("Summary").Value;
                                    comic.Pages = System.Convert.ToInt32(doc.Root.Element("PageCount").Value);
                                    comic.Publisher = doc.Root.Element("Publisher").Value;
                                    comic.Genre = doc.Root.Element("Genre").Value;
                                    comic.DateAdded = DateTime.Now;
                                }
                            }

                            string entryFilePath = Path.Combine(extractionPath, entry.Key);

                            
                            if (entry.Key.EndsWith(".jpg"))
                            {
                                HandleImageFile(entry, entryFilePath, extractionPath);
                            }
                        }
                    }
                }

                Console.WriteLine("Extraction and processing completed.");
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error)
                Console.WriteLine($"Error processing comic: {ex.Message}");
            }

            return comic;
        }

        private void HandleImageFile(IArchiveEntry entry, string entryFilePath, string extractionPath)
        {
            try
            {
                // Get the existing image files in the destination directory
                string[] existingFiles = Directory.GetFiles(extractionPath, "*.jpg");

                // Find the maximum image counter from existing files
                int maxImageCounter = existingFiles
                    .Select(filePath => Path.GetFileNameWithoutExtension(filePath))
                    .Where(fileName => int.TryParse(fileName, out _))
                    .Select(fileName => int.Parse(fileName))
                    .DefaultIfEmpty(0)
                    .Max();

                // Increment the counter for each image file
                int imageCounter = maxImageCounter + 1;

                // Generate the new file name using the counter
                string newFileName = $"{imageCounter}.jpg";
                string newFilePath = Path.Combine(extractionPath, newFileName);

                // Check if the destination file exists
                while (File.Exists(newFilePath))
                {
                    // If it exists, increment the counter and generate a new file name
                    imageCounter++;
                    newFileName = $"{imageCounter}.jpg";
                    newFilePath = Path.Combine(extractionPath, newFileName);
                }

                // Ensure that the target directory exists
                string targetDirectory = Path.GetDirectoryName(newFilePath);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Now copy the source file to the destination (overwriting if it exists)
                using (var outputStream = File.Create(newFilePath))
                {
                    entry.WriteTo(outputStream);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error)
                Console.WriteLine($"Error processing image file: {ex.Message}");
            }
        }

    }
}