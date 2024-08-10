package lib

import (
	"errors"
	"os"
	"time"
)

// FileExists 判断文件是否存在
func FileExists(filePath string) bool {
	_, err := os.Stat(filePath)
	if os.IsNotExist(err) {
		return false
	}
	return true
}

// IsExprt checks if the given time string represents a time before the current time.
// It returns true if the time is before the current time, and false otherwise.
// It returns an error if the time string is in an invalid format.
func IsExprt(timeString string) (bool, error) {
	// 定义时间格式
	format := "2006-01-02 15:04:05" // Go 使用固定的时间值来定义时间格式

	// 尝试解析时间字符串
	parsedTime, err := time.Parse(format, timeString)
	if err != nil {
		return false, errors.New("Invalid time format.")
	}

	// 获取当前时间
	currentTime := time.Now()

	// 比较时间
	return parsedTime.Before(currentTime), nil
}

// IsNotExprt checks if the given time string represents a time that is not before the current time.
// It returns true if the time is not before the current time, and false otherwise.
func IsNotExprt(timeString string) (bool, error) {
	isExprt, err := IsExprt(timeString)
	if err != nil {
		return false, err
	}
	return !isExprt, nil
}
