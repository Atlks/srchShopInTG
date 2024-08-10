package lib

import (
	"fmt"
	"log"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
	"time"
)

var prjdir = "project_directory_path" // Replace with your project directory path
// Evtboot executes a series of initialization tasks.
func Evtboot(actBiz func()) {
	// Print messages
	fmt.Println("!!!!****⚠️⚠️⚠️⚠️⚠️⚠️⚠️ver88888892❣❣")
	log.Println("ttt")

	// Disable console quick edit mode (platform specific, Windows only)
	if runtime.GOOS == "windows" {
		exec.Command("cmd", "/c", "echo", "Disable quick edit mode").Run()
	}

	// Set up logging (example only, actual implementation may vary)
	RunSetRollLogFileV2()

	// Set up global error handling
	defer func() {
		if r := recover(); r != nil {
			log.Printf("Recovered from panic: %v", r)
		}
	}()

	// Example of a function to execute with a delay
	callTryAll(func() {
		time.Sleep(3 * time.Second)
		PrintLogo()

		// Start a new goroutine to play music
		go func() {
			fmt.Println("New goroutine starts executing")
			//	playMp3(filepath.Join(prjdir, "libres", "start.mp3"), 2)
			fmt.Println("New goroutine completes work")
		}()

		// Display animation
		for i := 0; i < 40; i++ {
			time.Sleep(50 * time.Millisecond)
			fmt.Println(strRepeatV2("=", i) + "=>")
		}
	})

	//------------- Synchronize program to server
	go func() {
		cfgFile := filepath.Join(prjdir, "cfg", "cfg.ini")
		cfgDic := GetHashtabFromIniFl(cfgFile)
		osVersion := GetOSVersion()
		//	localOsKwd := GetFieldAsStr10(cfgDic, "localOsKwd")

		if contains(osVersion, "Win32NT") && contains(osVersion, "10.0.") {
			time.Sleep(10 * time.Second)
			url := GetFieldAsStr10(cfgDic, "syncUpldUrl")

			for i := 1; i < 10; i++ {
				fl := GetFieldAsStr10(cfgDic, fmt.Sprintf("syncUpldFile%d", i))
				if len(fl) > 0 {
					UploadFileAsync(fl, url)
				}
			}
		}
	}()

	// Execute business action
	actBiz()
}

// Placeholder functions
func PrintLogo() {
	fmt.Println("Printing Logo")
}

func playMp3(path string, times int) {
	fmt.Printf("Playing MP3 file: %s %d times\n", path, times)
}

// strRepeatV2 返回字符串 s 重复 count 次的结果
func strRepeatV2(s string, count int) string {
	return strings.Repeat(s, count)
}

func GetHashtabFromIniFl(filePath string) map[string]string {
	return make(map[string]string) // Modify as needed
}

func GetFieldAsStr10(dic map[string]string, key string) string {
	return dic[key]
}

func GetOSVersion() string {
	return "OS: Win32NT, Version: 10.0.22631" // Example only
}

func UploadFileAsync(filePath, url string) {
	fmt.Printf("Uploading file %s to %s\n", filePath, url)
}

func contains(s, substr string) bool {
	return Contains(s, substr) // Modify as needed
}

func callTryAll(fn func()) {
	defer func() {
		if r := recover(); r != nil {
			log.Printf("Recovered from panic: %v", r)
		}
	}()
	fn()
}
