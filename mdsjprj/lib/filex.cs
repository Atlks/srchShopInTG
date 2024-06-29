global using static prj202405.lib.filex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types.ReplyMarkups;
using static mdsj.lib.encdCls;
// prj202405.lib.filex
namespace prj202405.lib
{
    internal class filex
    {
        public static void Copy2024(string sourceFilePath, string destination_newFileName)
        {
            filex.mkdir_forFile(destination_newFileName);

            // 构造目标文件的完整路径
            // string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destination_newFileName, true);
        }


        public static string InsertCurrentTimeToFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            // 获取文件名和扩展名
            string nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string extension = System.IO.Path.GetExtension(fileName);

            // 获取当前时间并格式化
            string formattedTime = DateTime.Now.ToString("yyMMdd_HHmmss_fff");

            // 构造新的文件名
            string newFileName = $"{nameWithoutExtension}_{formattedTime}{extension}";

            return newFileName;
        }

        public static void CopyAndRenameFile(string sourceFilePath, string destinationFolderPath, string newFileName)
        {
            // 如果目标文件夹不存在，则创建它
            Directory.CreateDirectory(destinationFolderPath);

            // 构造目标文件的完整路径
            string destinationFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);

            // 复制并重命名文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
        }

        public static void CopyFileToFolder(string sourceFilePath, string targetFolderPath)
        {
            // 检查目标文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
                Console.WriteLine($"Created directory: {targetFolderPath}");
            }

            // 获取源文件名
            string fileName = System.IO.Path.GetFileName(sourceFilePath);

            // 构建目标文件路径
            string destinationFilePath = System.IO.Path.Combine(targetFolderPath, fileName);

            // 复制文件
            System.IO.File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }

        public static string GetBaseFileName(string filePath)
        {
            // 获取文件名（带扩展名）
            string fileName = Path.GetFileName(filePath);

            // 去掉扩展名，获取基本名字
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);

            return baseFileName;
        }
        public static string GetFilePathsCommaSeparated(string directoryPath)
        {
            // 获取目录下的所有文件路径
            string[] filePaths = Directory.GetFiles(directoryPath);

            // 将文件路径数组转换为逗号分割的字符串
            string result = string.Join(",", filePaths);

            return result;
        }
      //  WriteAllText
        //我们首先使用 System.IO.Path.GetInvalidFileNameChars 方法获取操作系统支持的非法文件名字符数组
        /*
         
         我们遍历输入的文本，并检查每个字符是否是非法字符。如果字符是非法字符，则使用 HttpUtility.UrlEncode 方法对字符进行 URL 编码，然后将编码后的结果添加到结果字符串中。最后，返回处理后的结果字符串。
         
         */
        public static string ConvertToValidFileName2024(string input)
        {
            // URL 编码非法字符
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            StringBuilder encodedBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (invalidChars.Contains(c))
                {
                    // 如果字符为非法字符，则使用 URL 编码替换
                    string encoded = HttpUtility.UrlEncode(c.ToString());
                    encodedBuilder.Append(encoded);
                }
                else
                {
                    // 如果字符为合法字符，则直接添加到结果中
                    encodedBuilder.Append(c);
                }
            }
            return encodedBuilder.ToString();
        }

        /// <summary>
        /// 创建一个新的目录，如果目录已存在，则不执行任何操作。
        /// </summary>
        /// <param name="directoryPath">要创建的目录的路径</param>
        public static void mkdir(string directoryPath)
        {
            try
            {
                // 使用 Directory.CreateDirectory 创建目录
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void mkdir_forFile(string filePath )
        {
            // 获取文件目录
            string dir = System.IO.Path.GetDirectoryName(filePath);

            // 检查目录是否存在，如果不存在，则创建目录
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
          //  File.WriteAllText(filePath, txt);


        }
        public static string GetAbsolutePath(string relativePath)
        {
            // 获取当前工作目录
            string currentDirectory = Environment.CurrentDirectory;

            // 将相对路径转换为绝对路径
            string absolutePath = Path.GetFullPath(Path.Combine(currentDirectory, relativePath));

            return absolutePath;
        }
        /// <summary>
        /// 读取指定文件的内容，并返回字符串形式的内容。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容的字符串</returns>
        public static string file_get_contents(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                else
                {
                    throw new FileNotFoundException("The specified file does not exist.", filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Ensures that the directory for the specified file path exists. If it does not exist, create it.
        /// </summary>
        /// <param name="filePath">The file path for which to ensure the directory exists.</param>
        public static void EnsureDirectoryExists(string filePath)
        {
            // Get the directory path from the file path
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                // Create the directory and any parent directories
                Directory.CreateDirectory(directoryPath);
            }
        }
        public static void wrtLgTypeDate(string logdir, object o)
        {
            // 创建目录
            Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"{logdir}/{timestamp}.json";
            file_put_contents(fileName, json_encode(o), false);

        }

        public static string filenameBydtme( )
        {
            // 创建目录
          //  Directory.CreateDirectory(logdir);
            // 获取当前时间并格式化为文件名
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            return timestamp;

        }
        /// <summary>
        /// Removes consecutive extra newline characters from the input string.
        /// </summary>
        /// <param name="input">The input string with potential extra newlines.</param>
        /// <returns>A string with extra newlines removed.</returns>

        /// <summary>
        /// 将指定内容写入到文件中，如果文件不存在则创建文件。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">要写入的内容</param>
        /// <param name="append">是否追加内容到文件末尾，默认为 false</param>
        public static void file_put_contents(string filePath, string content, bool append = false)
        {
            EnsureDirectoryExists(filePath);
            try
            {
                // 根据 append 参数确定写入模式
                FileMode mode = append ? FileMode.Append : FileMode.Create;

                // 使用 FileStream 写入内容到文件
                using (FileStream fs = new FileStream(filePath, mode, FileAccess.Write))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static HashSet<string> ReadWordsFromFile(string filePath)
        {
            var words = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 拆分行中的单词，按空格和回车拆分
                        var splitWords = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in splitWords)
                        {
                          var  word1= word.Trim();
                            words.Add(word1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

            return words;
        }
        public static ArrayList rdWdsFromFileSplitComma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);


            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }

                    }

                }


            }
            return wordList;
        }

        public static ArrayList rdWdsFromFile(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }
                      
                    }

                }

                
            }
            return wordList;
        }

        public static InlineKeyboardButton[][] wdsFromFileRendrToBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<InlineKeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<InlineKeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        // { CallbackData = $"Merchant?id={guid}" }
                        InlineKeyboardButton btn = new InlineKeyboardButton(word) { CallbackData = $"cmd={word}" };
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenuBycomma(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }

                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }

        public static KeyboardButton[][] wdsFromFileRendrToTgBtmBtnmenu(string filePath)
        {
            // 创建一个 ArrayList 来存储所有的单词
            ArrayList wordList = new ArrayList();

            // 读取文件中的所有行
            string[] lines = System.IO.File.ReadAllLines(filePath);

            List<KeyboardButton[]> btnTable = [];
            // 遍历每一行
            foreach (string line in lines)
            {
                // 按空格分割行，得到单词数组
                string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyboardButton> lineBtnArr = [];
                // 将单词添加到 ArrayList 中
                foreach (string word in words)
                {
                    if (word.Trim().Length > 0)
                    {
                        wordList.Add(word);
                        KeyboardButton btn = new KeyboardButton(word);
                        lineBtnArr.Add(btn);
                    }
                       
                }

                btnTable.Add(lineBtnArr.ToArray());
            }
            return btnTable.ToArray();
        }
        static string GetParentDirectory(string filePath)
        {
            // 使用Path.GetDirectoryName方法获取文件路径的上一级目录
            // Path.GetDirectoryName方法返回目录路径字符串，如果路径无效，则返回null。
            string parentDirectory = Path.GetDirectoryName(filePath);

            if (parentDirectory != null)
            {
                // 使用Path.GetFileName获取上一级目录的名称
                string parentDirectoryName = Path.GetFileName(parentDirectory);
                return parentDirectoryName;
            }
            else
            {
                // 处理无效路径的情况
                return "无效路径";
            }
        }
        public   static HashSet<string> GetNotepadFilePaths(string rootDirectory, int depth)
        {
            HashSet<string> notepadFilePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetNotepadFilePathsRecursive(rootDirectory, depth, notepadFilePaths);
            return notepadFilePaths;
        }
        public static void GetNotepadFilePathsRecursive(string currentDirectory, int remainingDepth, HashSet<string> notepadFilePaths)
        {
            if (remainingDepth == 0)
                return;

            try
            {
                // 获取当前文件夹中的所有文件
                string[] files = Directory.GetFiles(currentDirectory, "*.txt");

                // 将记事本文件的路径添加到HashSet中
                foreach (string file in files)
                {
                    notepadFilePaths.Add(file);
                }

                // 获取当前文件夹中的所有子文件夹
                string[] subdirectories = Directory.GetDirectories(currentDirectory);

                // 递归调用每个子文件夹
                foreach (string subdirectory in subdirectories)
                {
                    GetNotepadFilePathsRecursive(subdirectory, remainingDepth - 1, notepadFilePaths);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理无权访问的文件夹
                Console.WriteLine($"Access to {currentDirectory} is denied.");
            }
            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
