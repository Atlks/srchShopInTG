package main

import (
	"fmt"
	"modname2024/codelib2023/lib"
)

func main() {
	//------------------ 设置全局异常处理
	defer lib.CatchPanic()
	fmt.Print(111)
	fmt.Print(22)
	lib.Main1()
	//lib.StartWebapiV2()

	lib.Evtboot(func() {
		// Call the function to get bot information

	})

	lib.Main4daylyRpt()
	fmt.Print(333)

	// 阻塞主线程
	select {}
}
