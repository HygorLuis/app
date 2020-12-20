using System;
using System.Threading.Tasks;
using PCLStorage;
using PCLStorage.Exceptions;

namespace QueixaAki.Helpers
{
    public static class FileHelper
    {
        public static async Task<Tuple<string, string>> CreateFile(byte[] resultBytes, string fileName)
        {
            try
            {
                var appFolder = App.SpecificPlatform.RootFolder();

                var rootFolder = await FileSystem.Current.GetFolderFromPathAsync(appFolder);

                var newFile = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                using (var streamWriter = await newFile.OpenAsync(FileAccess.ReadAndWrite))
                {
                    streamWriter.Write(resultBytes, 0, resultBytes.Length);
                }

                return new Tuple<string, string>(newFile.Path, "");
            }
            catch (Exception e)
            {
                return new Tuple<string, string>("", e.Message);
            }
        }

        public static async Task<string> FileExists(string fileName)
        {
            try
            {
                var appFolder = App.SpecificPlatform.RootFolder();

                var rootFolder = await FileSystem.Current.GetFolderFromPathAsync(appFolder);

                var fileExists = await rootFolder.GetFileAsync(fileName);

                return fileExists.Path;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
    }
}
