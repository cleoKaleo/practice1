using System;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace practice1
{
    class Singer
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Band { get; set; }
        public string Style { get; set; }
    }
    class Program
    {
        static void showDiskInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Свободное пространство: {drive.AvailableFreeSpace}");
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}\n");
                }
            }
        }
        public static void creatFillFile()
        {
            string path = @"C:\Users\DNS\Desktop\СиШарп\helloWorld.txt";
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter fl = File.CreateText(path))
            {
                Console.Write("Enter string to write into file: ");
                string str = Console.ReadLine();
                fl.WriteLineAsync(str);
                fl.Close();
            }
            using (StreamReader fa = File.OpenText(path))
            {
                string s;
                while ((s = fa.ReadLine()) != null)
                    Console.WriteLine(s);
            }
            FileInfo fileInfo = new FileInfo(path);
            Console.WriteLine("   Имя файла: {0};", fileInfo.Name);
            Console.WriteLine("   Время создания: {0};\n", fileInfo.CreationTime);

            deleteFile(path);
        }
        static async Task jsonFilesTask()
        {
            using (FileStream fs = new FileStream("singer.json", FileMode.OpenOrCreate))
            {
                Singer andy = new Singer() { Name = "Andy", Age = 29, Band = "Black veil brides", Style = "Rock" };
                await JsonSerializer.SerializeAsync<Singer>(fs, andy);
            }
            using (FileStream fs = new FileStream("singer.json", FileMode.OpenOrCreate))
            {
                Singer restoredAndy = await JsonSerializer.DeserializeAsync<Singer>(fs);
                Console.WriteLine($"Name: {restoredAndy.Name}; Age: {restoredAndy.Age}; Band: {restoredAndy.Band}; Style: {restoredAndy.Style}");
            }
            string path = @"C:\Users\DNS\Desktop\СиШарп\practice1\practice1\bin\Debug\netcoreapp3.1\singer.json";
            deleteFile(path);
        }
        public static void xmlTask()
        {
            XDocument xdoc = new XDocument();

            XElement singer = new XElement("singer");
            Console.Write("Enter singer's name: ");
            string str = Console.ReadLine();

            XAttribute singerNameAttr = new XAttribute("name", str);
            Console.Write("Enter singer's band: ");
            str = Console.ReadLine();

            XElement singerBandElem = new XElement("band", str);
            Console.Write("Enter singer's age: ");
            str = Console.ReadLine();

            XElement singerAgeElem = new XElement("age", str);
            singer.Add(singerNameAttr);
            singer.Add(singerBandElem);
            singer.Add(singerAgeElem);

            XElement singers = new XElement("singers");
            singers.Add(singer);
            xdoc.Add(singers);

            xdoc.Save("singers.xml");

            foreach (XElement s in xdoc.Element("singers").Elements("singer"))
            {
                XAttribute nameAttribute = s.Attribute("name");
                XElement bandElement = s.Element("band");
                XElement ageElement = s.Element("age");

                if (nameAttribute != null && bandElement != null && ageElement != null)
                {
                    Console.WriteLine($"   Name: {nameAttribute.Value}");
                    Console.WriteLine($"   Band: {bandElement.Value}");
                    Console.WriteLine($"   Age: {ageElement.Value}");
                }
                Console.WriteLine();
            }
            deleteFile("singers.xml");
        }
        public static void zipTask()
        {
            string path = @"C:\Users\DNS\Desktop\СиШарп\practice1\test\hey.txt";
            string sourceFolder = @"C:\Users\DNS\Desktop\СиШарп\practice1\test";
            string zipFile = @"C:\Users\DNS\Desktop\СиШарп\practice1\test.zip";
            string targetFolder = @"C:\Users\DNS\Desktop\СиШарп\practice1\newtest";

            DirectoryInfo dirInf = new DirectoryInfo(sourceFolder);
            if (!dirInf.Exists) dirInf.Create();
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter fl = File.CreateText(path))
            {
                Console.Write("Enter string to write into file: ");
                string str = Console.ReadLine();
                fl.WriteLineAsync(str);
                fl.Close();
            }
            using (StreamReader fa = File.OpenText(path))
            {
                string s;
                while ((s = fa.ReadLine()) != null)
                    Console.WriteLine(s);
            }
            
            FileInfo fileInfo = new FileInfo(path);
            Console.WriteLine("Имя файла: {0}", fileInfo.Name);
            Console.WriteLine("Время создания: {0}", fileInfo.CreationTime);
            Console.WriteLine("Размер: {0}\n", fileInfo.Length);

            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            ZipFile.ExtractToDirectory(zipFile, targetFolder);

            FileInfo fileInfo1 = new FileInfo(zipFile);
            Console.WriteLine("Имя файла: {0}", fileInfo1.Name);
            Console.WriteLine("Время создания: {0}", fileInfo1.CreationTime);
            Console.WriteLine("Размер: {0}\n", fileInfo1.Length);

            deleteFile(zipFile);
            deleteFile(path);
            deleteFile(targetFolder + @"\hey.txt");
            deleteFolder(sourceFolder, "test");
            deleteFolder(targetFolder, "newtest");
        }
        static void Main(string[] args)
        {
            bool f = false;
            Console.WriteLine("Create and fill file press........1");
            Console.WriteLine("See discs' info press.............2");
            Console.WriteLine("Serialize object press............3");
            Console.WriteLine("Make xml file press...............4");
            Console.WriteLine("Compress & decompress file press..5");
            Console.WriteLine("To exit...........................6");
            while (!f)
            {
                Console.Write("   Enter number: ");
                int x = Convert.ToInt32(Console.ReadLine());
                switch (x)
                {
                    case 1:
                        creatFillFile();
                        break;
                    case 2:
                        showDiskInfo();
                        break;
                    case 3:
                        var task3 = jsonFilesTask();
                        task3.Wait();
                        break;
                    case 4:
                        xmlTask();
                        break;
                    case 5:
                        zipTask();
                        break;
                    case 6:
                        f = true;
                        break;
                    default:
                        Console.WriteLine("Error, try again!!");
                        break;
                }
            }
        }
        static void deleteFile(string path)
        {
            Console.WriteLine("Удалить файл?[y/n]");
            string str = Console.ReadLine();
            if (str.CompareTo("y") == 0)
            {
                FileInfo fileinfo = new FileInfo(path);
                if (fileinfo.Exists)
                {
                    fileinfo.Delete();
                }
                Console.WriteLine("Your file is deleted\n");
            }
        }
        static void deleteFolder(string path, string name)
        {
            Console.WriteLine($"Удалить папку {name}?[y/n]");
            string str = Console.ReadLine();
            if (str.CompareTo("y") == 0)
            {
                DirectoryInfo dirinf = new DirectoryInfo(path);
                if (dirinf.Exists)
                {
                    dirinf.Delete();
                }
                Console.WriteLine($"Your {name} folder is deleted\n");
            }
        }
    }
}
