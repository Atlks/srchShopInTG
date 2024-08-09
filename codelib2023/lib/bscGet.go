package lib

import (
	"bufio"
	"os"
	"strings"

)

// ReadIniFile 读取 INI 文件并返回一个 map
func ReadIniFile(filePath string) (map[string]string, error) {
	iniData := make(map[string]string)

	// 打开 INI 文件
	file, err := os.Open(filePath)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	// 创建一个读取器
	scanner := bufio.NewScanner(file)

	// 按行读取 INI 文件内容
	for scanner.Scan() {
		line := scanner.Text()
		trimmedLine := strings.TrimSpace(line)

		// 忽略空行和注释行
		if trimmedLine == "" || strings.HasPrefix(trimmedLine, ";") || strings.HasPrefix(trimmedLine, "#") {
			continue
		}

		// 处理键值对行
		equalIndex := strings.Index(trimmedLine, "=")
		if equalIndex > 0 {
			key := strings.TrimSpace(trimmedLine[:equalIndex])
			value := strings.TrimSpace(trimmedLine[equalIndex+1:])
			iniData[key] = value
		}
	}

	if err := scanner.Err(); err != nil {
		return nil, err
	}

	return iniData, nil
}
