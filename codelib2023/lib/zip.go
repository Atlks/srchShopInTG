package lib

import (
	"archive/zip"
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"os"
)

// AppendSortedListToZip 将 SortedList 追加到 ZIP 文件中
func AppendSortedListToZip(sortedList map[string]interface{}, zipFilePath, entryName string) error {
	// 创建一个临时文件来保存新的 ZIP 文件
	tempFilePath := zipFilePath + ".tmp"

	// 序列化 SortedList 为 JSON
	var buffer bytes.Buffer
	encoder := json.NewEncoder(&buffer)
	encoder.SetIndent("", "  ") // 美化 JSON 输出
	if err := encoder.Encode(sortedList); err != nil {
		return fmt.Errorf("failed to serialize sortedList: %v", err)
	}

	// 创建或打开临时 ZIP 文件
	tempFile, err := os.Create(tempFilePath)
	if err != nil {
		return fmt.Errorf("failed to create temp file: %v", err)
	}
	defer tempFile.Close()

	// 创建新的 ZIP 文件
	zipWriter := zip.NewWriter(tempFile)
	defer zipWriter.Close()

	// 复制现有的 ZIP 文件条目到新的 ZIP 文件中
	if _, err := os.Stat(zipFilePath); err == nil {
		originalZipFile, err := os.Open(zipFilePath)
		if err != nil {
			return fmt.Errorf("failed to open existing zip file: %v", err)
		}
		defer originalZipFile.Close()

		originalZipReader, err := zip.NewReader(originalZipFile, fileSize(originalZipFile))
		if err != nil {
			return fmt.Errorf("failed to create zip reader: %v", err)
		}

		for _, entry := range originalZipReader.File {
			zipEntryWriter, err := zipWriter.Create(entry.Name)
			if err != nil {
				return fmt.Errorf("failed to create zip entry: %v", err)
			}

			entryReader, err := entry.Open()
			if err != nil {
				return fmt.Errorf("failed to open zip entry: %v", err)
			}
			defer entryReader.Close()

			if _, err := io.Copy(zipEntryWriter, entryReader); err != nil {
				return fmt.Errorf("failed to copy zip entry: %v", err)
			}
		}
	}

	// 添加新的条目到 ZIP 文件
	newEntryWriter, err := zipWriter.Create(entryName)
	if err != nil {
		return fmt.Errorf("failed to create new zip entry: %v", err)
	}

	if _, err := io.Copy(newEntryWriter, &buffer); err != nil {
		return fmt.Errorf("failed to write new zip entry: %v", err)
	}

	// 替换原始 ZIP 文件
	if err := os.Remove(zipFilePath); err != nil {
		return fmt.Errorf("failed to remove original zip file: %v", err)
	}

	if err := os.Rename(tempFilePath, zipFilePath); err != nil {
		return fmt.Errorf("failed to rename temp file: %v", err)
	}

	return nil
}

// 文件大小计算函数
func fileSize(file *os.File) int64 {
	stat, err := file.Stat()
	if err != nil {
		return 0
	}
	return stat.Size()
}

func main1024() {
	// 创建一个示例 SortedList
	sortedList := map[string]interface{}{
		"id":   1,
		"name": 888,
	}

	// 指定 ZIP 文件路径和条目名称
	zipFilePath := "db1share1.zip"
	entryName := "id_1.json"

	// 将 SortedList 追加到 ZIP 文件中
	if err := AppendSortedListToZip(sortedList, zipFilePath, entryName); err != nil {
		fmt.Printf("Error: %v\n", err)
		return
	}

	fmt.Println("SortedList has been appended to the ZIP file.")
}
