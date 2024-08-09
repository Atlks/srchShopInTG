package lib

import (
	"fmt"
	"io"
	"os"
	"path/filepath"
)

// ObjectInfo 结构体定义
type ObjectInfo struct {
	BucketName string // 公共字段
	ObjectName string // 公共字段
	Name       string // 公共字段
}

// NewObjectInfo 创建一个新的 ObjectInfo 实例
func NewObjectInfo(bucketName, objectName string) *ObjectInfo {
	return &ObjectInfo{
		BucketName: bucketName,
		ObjectName: objectName,
		Name:       objectName,
	}
}

// UploadObjectToStorageClient simulates uploading a file to a specific location
func UploadObjectToStorageClient(bucketName, objectName string, fileStream *os.File) (ObjectInfo, error) {
	destinationFilePath := filepath.Join("bkss", bucketName, objectName)

	// Create the destination file
	destinationFile, err := os.Create(destinationFilePath)
	if err != nil {
		return ObjectInfo{}, fmt.Errorf("failed to create destination file: %w", err)
	}
	defer destinationFile.Close()

	// Copy the content from the fileStream to the destination file
	_, err = io.Copy(destinationFile, fileStream)
	if err != nil {
		return ObjectInfo{}, fmt.Errorf("failed to copy file content: %w", err)
	}

	return ObjectInfo{BucketName: bucketName, ObjectName: objectName}, nil
}
