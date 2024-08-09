package lib

/*
import (
	"fmt"
	"net/http"
	"os"
	"path/filepath"
	"strings"

)

func HttpHdlr(w http.ResponseWriter, r *http.Request, apiPrefix string, httpHdlrApiSpecl func(http.ResponseWriter, *http.Request)) {
	// 获取 URL 和查询字符串
	//url := fmt.Sprintf("%s://%s%s%s", r.URL.Scheme, r.Host, r.URL.Path, r.URL.RawQuery)
	queryString := r.URL.RawQuery
	path := r.URL.Path

	if r.Method == http.MethodPost {
		funname := strings.TrimPrefix(path, "/") + r.Method + "Wbapi"
		// call wbapi_upldPOST path
		// wbapi_upldPost(r, w)
		Callx(funname, r, w)
		return
	}

	// 静态资源处理器映射表
	extNhdlrChoosrMaplist := map[string]string{
		"txt   css js": "TxtHttpHdlr",
		"html htm":     "HtmlHttpHdlrfilTxtHtml",
		"json":         "JsonFLhttpHdlrFilJson",
		"jpg png":      "ImgHhttpHdlrFilImg",
	}

	HttpHdlrFil(r, w, extNhdlrChoosrMaplist)

	// 处理特定API
	httpHdlrApiSpecl(w, r)

	// 设置响应内容类型和编码
	SetRespContentTypeNencode(w, "application/json; charset=utf-8")

	fn := strings.TrimPrefix(path, "/")
	rzt := CallxTryx(apiPrefix+fn, strings.TrimPrefix(queryString, "?"))

	// 发送响应
	SendResp(rzt, w)
}

func HttpHdlrFil(w http.ResponseWriter, r *http.Request, extnameNhdlrChooser map[string]string) {
	// 获取 URL 和查询字符串
	url := fmt.Sprintf("%s://%s%s%s", r.URL.Scheme, r.Host, r.URL.Path, r.URL.RawQuery)
	queryString := r.URL.RawQuery
	path := r.URL.Path
	path = decodeURL(path)

	if strings.Contains(path, "analytics") {
		fmt.Println("Dbg")
	}

	for key, value := range extnameNhdlrChooser {
		exts := strings.Fields(key)
		for _, ext := range exts {
			if strings.HasSuffix(strings.ToUpper(strings.TrimSpace(path)), "."+strings.ToUpper(strings.TrimSpace(ext))) {
				func1 := value
				task := Callx(func1, w, r)
				if task != nil {
					return
				}
			}
		}
	}

	// 获取文件的实际扩展名
	fileExtension := filepath.Ext(path)
	filePath := fmt.Sprintf("%s%s", webrootDir, path)
	filePath = castNormalizePath(filePath)
	if fileHasExtension(filePath) && fileExists(filePath) {
		fileInfo, _ := os.Stat(filePath)
		fileSize := fileInfo.Size()
		if fileSize < 1000*1000 {
			HtmlHttpHdlrfilTxtHtml(w, r)
			return
		} else {
			FildownHttpHdlrFilDown(w, r)
			return
		}
	}

	if fileHasExtension(filePath) && !fileExists(filePath) {
		sendResponseResourceNotFound(w)
		return
	}

	// 处理没有扩展名的文件
	if fileExtension == "" && fileExists(fmt.Sprintf("%s%s", prjdir, path)) {
		HtmlHttpHdlrfilTxtHtml(w, r)
		return
	}
}

// WbapiXqry processes the query string, queries a JSON file, and returns the result as a JSON string
func WbapiXqry(qrystr string) (string, error) {
	qrystrHstb := GetHashtableFromQrystr(qrystr)
	path := qrystrHstb["path"].(string)
	li := ormJSonFLQrySglFL(path + ".json")
	return EncodeJson(li)
}

// GetHashtableFromQrystr parses the query string and returns a map
func GetHashtableFromQrystr(qrystr string) map[string]interface{} {
	values, err := url.ParseQuery(qrystr)
	if err != nil {
		return nil
	}
	qrystrHstb := make(map[string]interface{})
	for key, val := range values {
		qrystrHstb[key] = val[0]
	}
	return qrystrHstb
}

// ormJSonFLQrySglFL is a placeholder for the actual JSON query logic
func ormJSonFLQrySglFL(path string) interface{} {
	// Implement your logic to query the single JSON file
	// This is a placeholder function
	return map[string]string{"data": "sample"}
}

// EncodeJson encodes the data to a JSON string
func EncodeJson(data interface{}) (string, error) {
	jsonData, err := json.Marshal(data)
	if err != nil {
		return "", err
	}
	return string(jsonData), nil
}
func decodeURL(path string) string {
	// 实现URL解码逻辑
	return path
}

func Callx(funcName string, w http.ResponseWriter, r *http.Request) error {
	// 实现Callx逻辑
	fmt.Println("Calling function:", funcName)
	return nil
}

func castNormalizePath(path string) string {
	// 实现路径标准化逻辑
	return path
}

func fileHasExtension(path string) bool {
	// 实现检查文件是否有扩展名的逻辑
	return filepath.Ext(path) != ""
}

func fileExists(path string) bool {
	// 实现检查文件是否存在的逻辑
	_, err := os.Stat(path)
	return err == nil
}

func sendResponseResourceNotFound(w http.ResponseWriter) {
	// 实现发送资源不存在响应的逻辑
	w.WriteHeader(http.StatusNotFound)
	w.Write([]byte("Resource not found"))
}

func HtmlHttpHdlrfilTxtHtml(w http.ResponseWriter, r *http.Request) {
	// 实现HTML处理逻辑
	fmt.Println("Handling HTML response")
}

func FildownHttpHdlrFilDown(w http.ResponseWriter, r *http.Request) {
	// 实现文件下载处理逻辑
	fmt.Println("Handling file download")
}

var (
	webrootDir = "/path/to/webroot"
	prjdir     = "/path/to/project"
)
*/