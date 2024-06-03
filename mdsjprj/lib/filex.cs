using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prj202405.lib
{
    internal class filex
    {
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
