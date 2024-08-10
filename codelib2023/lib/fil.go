package lib

import (
	"fmt"
	"os"
	"path/filepath"
	"time"
)

// GetRealPath 返回指定路径的绝对路径
func GetRealPath(folder string) string {
	// 获取文件夹的绝对路径
	realPath, err := filepath.Abs(folder)
	if err != nil {
		//fmt.Print((err))
		//，%v 是格式化指令的一部分，用于格式化值为其默认的字符串表示形式
		fmt.Printf("Error getting real path: %v\n", err)
	}
	return realPath
}

// CreateFolderBasedOnDate 创建一个以当前月份和日期为名称的文件夹
// CreateFolderBasedOnDate 创建一个以当前月份和日期为名称的文件夹，并返回文件夹名称
func CreateFolderBasedOnDate(prefix string) (string, error) {
	// 获取当前时间
	now := time.Now()

	// 格式化文件夹名称为 "MM-DD"
	folderName := now.Format("01-02")

	// 创建文件夹
	err := os.MkdirAll(prefix+folderName, os.ModePerm)
	if err != nil {
		return "", fmt.Errorf("failed to create folder: %w", err)
	}

	return prefix + folderName, nil
}
