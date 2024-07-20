  using static prjx.lib.corex;
using Mono.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot.Types.ReplyMarkups;
  

// prj202405.lib.filex
namespace prjx.lib
{
    internal class filex
    {

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
        public static string ConvertToValidFileName(string input)
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
               print($"An error occurred: {ex.Message}");
            }
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
               print($"An error occurred: {ex.Message}");
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
               print($"An error occurred: {ex.Message}");
            }
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

        //public static InlineKeyboardButton[][] wdsFromFileRendrToBtnmenu(string filePath)
        //{
        //    // 创建一个 ArrayList 来存储所有的单词
        //    ArrayList wordList = new ArrayList();

        //    // 读取文件中的所有行
        //    string[] lines = System.IO.File.ReadAllLines(filePath);

        //    List<InlineKeyboardButton[]> btnTable = [];
        //    // 遍历每一行
        //    foreach (string line in lines)
        //    {
        //        // 按空格分割行，得到单词数组
        //        string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //        List<InlineKeyboardButton> lineBtnArr = [];
        //        // 将单词添加到 ArrayList 中
        //        foreach (string word in words)
        //        {
        //            if (word.Trim().Length > 0)
        //            {
        //                wordList.Add(word);
        //                // { CallbackData = $"Merchant?id={guid}" }
        //                InlineKeyboardButton btn = new InlineKeyboardButton(word) { CallbackData = $"cmd={word}" };
        //                lineBtnArr.Add(btn);
        //            }

        //        }

        //        btnTable.Add(lineBtnArr.ToArray());
        //    }
        //    return btnTable.ToArray();
        //}
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
               print($"Access to {currentDirectory} is denied.");
            }
            catch (Exception ex)
            {
                // 处理其他异常
               print($"An error occurred: {ex.Message}");
            }
        }
    }
}
