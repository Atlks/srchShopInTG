package lib

import (
	"fmt"
	"io"
	"log"
	"os"
	"time"
)

var originalStdout *os.File

// 创建一个复合写入器，写入到控制台和日志文件
func SetLogHr() {
	// 获取当前时间
	now := time.Now()
	formattedDate := now.Format("2006-01-02_15") // Go 中的时间格式化

	// 创建日志文件
	logFile, err := os.OpenFile(fmt.Sprintf("log1037_%s.log", formattedDate), os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0666)
	if err != nil {
		log.Fatalf("error opening log file: %v", err)
	}
	defer logFile.Close()

	// 保存原始输出
	originalStdout := os.Stdout

	// 创建一个多写入器
	multiWriter := io.MultiWriter(originalStdout, logFile)

	// 设置日志输出到多写入器
	log.SetOutput(multiWriter)
}

// 设置日志并启动日志轮换
func RunSetRollLogFileV2() {
	SetLogHr()
	RunSetRollLogFile()
}

// 轮换日志文件
func RunSetRollLogFile() {
	go func() {
		for {
			time.Sleep(5 * time.Second)
			now := time.Now()
			if now.Minute() == 1 {
				SetLogHr()
				time.Sleep(70 * time.Second)
			}
		}
	}()
}
