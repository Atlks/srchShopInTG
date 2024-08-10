package lib

import (
	"fmt"
	"net/http"
	"net/url"
	"strconv"
	"strings"
	"time"
)

// 获取配置文件中的值的函数
func GetFieldAsInt526(config map[string]string, key string, defaultValue int) int {
	if value, exists := config[key]; exists {
		if intValue, err := strconv.Atoi(value); err == nil {
			return intValue
		}
	}
	return defaultValue
}

func CatchPanic() {
	if r := recover(); r != nil {
		fmt.Println("Recovered from panic:", r)
	}
}

// 获取配置字典的函数（模拟）
func GetDicFromIni(filePath string) map[string]string {
	// 模拟读取配置文件的过程
	rzt, _ := ReadIniFile(filePath)
	return rzt
}

// PrintTimestamp 打印时间戳
func PrintTimestamp(message string) {
	fmt.Printf("%s: %s\n", time.Now().Format(time.RFC3339), message)
}

// PrintLog 打印日志
func PrintLog(message string) {
	fmt.Println(message)
}

func Test3GetWbapi(w http.ResponseWriter, r *http.Request) {
	fmt.Print(("....in tet get wapi ()"))
}

// NewDelegate 示例函数，用于创建函数映射
func NewDelegate(funname string) func(http.ResponseWriter, *http.Request) {
	// 示例实现，根据函数名称返回实际的函数
	functions := map[string]func(http.ResponseWriter, *http.Request){
		"Test2GETWbapi": Test2GETWbapi,
	}
	return functions[funname]
}

// DecodeURL 解码 URL 编码的字符串
func DecodeURL(path string) (string, error) {
	decodedPath, err := url.QueryUnescape(path)
	if err != nil {
		return "", err
	}
	return decodedPath, nil
}

// CastToPathReal 处理路径并返回实际路径
func CastToPathReal(path string) (string, error) {
	// 替换双斜杠为单斜杠
	path = strings.ReplaceAll(path, "//", "/")

	// 解码 URL
	decodedPath, err := DecodeURL(path)
	if err != nil {
		return "", err
	}
	return decodedPath, nil
}

// CastPathReal4biz 示例函数，替换为实际的业务逻辑
// CastPathReal4biz 处理路径并返回实际的业务路径
func CastPathReal4biz(path string) string {
	path, _ = CastToPathReal(path)

	//nginxCfg := "D:\\nginx-1.27.0\\conf\\nginx.conf"
	// 解析 Nginx 配置（示例代码，实际需要实现）
	// li := ParseNginxConfigV2(ReadAllText(nginxCfg))

	if strings.HasPrefix(path, "/api/") {
		path = path[4:] // 移除 "/api"
	}
	return path
}

// GetFunFromPathUrl 处理路径并返回处理后的字符串
func GetFunFromPathUrl(path string) string {
	// 替换双斜杠为单斜杠
	path = strings.ReplaceAll(path, "//", "/")
	// 去除路径的第一个字符
	if len(path) > 0 && path[0] == '/' {
		path = path[1:]
	}
	// 替换所有的斜杠为空字符串
	path = strings.ReplaceAll(path, "/", "")
	return path
}
