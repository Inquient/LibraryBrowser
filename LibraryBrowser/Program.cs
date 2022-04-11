using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DirectoryContentTool;

namespace LibraryBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstDirectoryPath = @"D:\Testdata\LibraryFirst";
            var secondDirectoryPath = @"D:\Testdata\LibrarySecond";

            var firstDirName = Regex.Match(firstDirectoryPath, @"((?:.*\\)?)(.*)").Groups[2].Value;
            var secondDirName = Regex.Match(secondDirectoryPath, @"((?:.*\\)?)(.*)").Groups[2].Value;

            var firstRootPath = Regex.Match(firstDirectoryPath, @"^.*\\").Value;
            var secondRootPath = Regex.Match(secondDirectoryPath, @"^.*\\").Value;

            var (firstLibraryFiles, firsLibraryDirectories) = ReadFilesFromDirectory(firstDirectoryPath);
            var (secondLibraryFiles, secondLibraryDirectories) = ReadFilesFromDirectory(secondDirectoryPath);

            if (firstLibraryFiles == secondLibraryFiles && firsLibraryDirectories == secondLibraryDirectories)
            {
                Console.WriteLine("Директории уже синхронизированы");
                return;
            }

            var firstLibraryClone = new HashSet<string>(firstLibraryFiles);

            firstLibraryFiles.ExceptWith(secondLibraryFiles);
            secondLibraryFiles.ExceptWith(firstLibraryClone);



            foreach (var directory in firsLibraryDirectories)
            {
                var pathToDirectory = Regex.Replace(directory, @"^.*\\"+ firstDirName, "");
                var expectedDirectory = secondDirectoryPath + pathToDirectory;
                if (!Directory.Exists(expectedDirectory))
                {
                    Directory.CreateDirectory(expectedDirectory);
                }
            }

            foreach (var directory in secondLibraryDirectories)
            {
                var pathToDirectory = Regex.Replace(directory, @"^.*\\"+ secondDirName, "");
                var expectedDirectory = firstDirectoryPath + pathToDirectory;
                if (!Directory.Exists(expectedDirectory))
                {
                    Directory.CreateDirectory(expectedDirectory);
                }
            }

            foreach (var file in firstLibraryFiles)
            {
                var source = firstRootPath + firstDirName + file;
                var destination = secondRootPath + secondDirName + file;
                if (!File.Exists(destination))
                {
                    File.Copy(source, destination);
                }
            }

            foreach (var file in secondLibraryFiles)
            {
                var source = secondRootPath + secondDirName + file;
                var destination = firstRootPath + firstDirName + file;
                if (!File.Exists(destination))
                {
                    File.Copy(source, destination);
                }
            }
        }

        public static (HashSet<string> filesHash, List<string> directories) ReadFilesFromDirectory(string directoryPath)
        {
            var rootDirectory = Regex.Replace(directoryPath, @"^.*\\", "");
            var (files, directories) = DirectoryContentReader.ReadDirectory(directoryPath);

            var filesNamesHash = new HashSet<string>();

            foreach (var file in files)
            {
                var fileNameAndExtension = Regex.Replace(file, @"^.*\\", ""); // Обрезать путь и оставить только имя файла
                var match = Regex.Match(fileNameAndExtension, @"(.*)\.([\w-]*)$");

                var fileName = match.Groups[1].Value;
                var fileExtension = match.Groups[2].Value;

                var filePathFromRootDirectory = Regex.Match(file, rootDirectory + @"(.*)").Groups[1].Value; ;

                filesNamesHash.Add(filePathFromRootDirectory);
            }

            return (filesNamesHash, directories);
        }
    }
}
