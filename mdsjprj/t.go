package main

import (
	"fmt"
	"time"
)

func main() {

	// 获取当前时间
	currentTime := time.Now()
	// 打印当前时间
	fmt.Println("Current time:", currentTime.Format("2006-01-02 15:04:05"))
	fmt.Println("Current time:")
}
