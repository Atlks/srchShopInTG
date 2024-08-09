package lib

import (
	"crypto/tls"
	"fmt"
	"log"
	"net/http"
	"strings"
)

// 配置 HTTPS 的函数（模拟）
func CfgHttps(config map[string]string) *tls.Config {
	// 实现 HTTPS 配置
	// 这里是一个示例配置，你需要用你的证书和密钥替换这些路径
	return &tls.Config{
		MinVersion: tls.VersionTLS13,
	}
}

// SendResp 发送响应
func SendResp(w http.ResponseWriter, result string, contentType string) {
	w.Header().Set("Content-Type", contentType)
	w.WriteHeader(http.StatusOK)
	_, err := w.Write([]byte(result))
	if err != nil {
		http.Error(w, "Failed to write response", http.StatusInternalServerError)
	}
}

// HTTP 处理程序 dep
func HttpHdlr(w http.ResponseWriter, r *http.Request) {
	// 实现你的请求处理逻辑
	_, err := w.Write([]byte("Request processed"))
	if err != nil {
		http.Error(w, "Internal Server Error", http.StatusInternalServerError)
	}
}

// http://locaohost:8888/Test2?a=1
func Test2GETWbapi(w http.ResponseWriter, r *http.Request) {
	args931 := strings.TrimPrefix(r.URL.RawQuery, "=")
	fmt.Print("....in tet get wapi ()" + args931)
	// 发送响应
	SendResp(w, "rztttttt", "application/json; charset=utf-8")
}

// HttpHdlrApi 处理 HTTP 请求
func HttpHdlrApi(w http.ResponseWriter, r *http.Request) {
	PrintTimestamp("enter HttpHdlrApi() ")
	PrintLog("request.Path =>" + r.URL.Path)

	path := CastPathReal4biz(r.URL.Path)
	PrintLog("CastPathReal4biz =>" + path)

	// 允许所有域名
	w.Header().Add("Access-Control-Allow-Origin", "*")

	fnName := GetFunFromPathUrl(path) + r.Method + "Wbapi"
	//	args931 := strings.TrimPrefix(r.URL.RawQuery, "=")
	fmt.Print("GetFunFromPathUrl:" + fnName)
	// 使用函数映射调用方法
	f := NewDelegate(fnName)
	if f == nil {
		http.Error(w, "Function not found", http.StatusNotFound)
		return
	}
	f(w, r)

	// 发送响应
	//SendResp(w, result, "application/json; charset=utf-8")
	PrintTimestamp("end fun HttpHdlrApi()")
}

func StartWebapiV2() {
	//apiPrefix := "Wapi"
	// 修复路径分隔符问题
	configPath := `D:\0prj\mdsj\codelib2023\cfg\cfg.ini`
	config := GetDicFromIni(configPath)
	//config := GetDicFromIni("D:\0prj\mdsj\codelib2023\cfg\cfg.ini")
	port := GetFieldAsInt526(config, "wbsvs_port", 8888)
	https := GetFieldAsInt526(config, "https", 0)

	address_wzPort := fmt.Sprintf(":%d", port)
	fmt.Print(" add: http://" + address_wzPort)
	// 配置 HTTPS
	var server *http.Server
	//var serverHttps *http.Server
	if https == 1 {
		tlsConfig := CfgHttps(config)
		server = &http.Server{
			Addr:      address_wzPort,
			Handler:   http.HandlerFunc(HttpHdlr),
			TLSConfig: tlsConfig,
		}
	} else {
		server = &http.Server{
			Addr:    address_wzPort,
			Handler: http.HandlerFunc(HttpHdlrApi),
		}
	}

	// 启动服务器
	if https == 1 {
		// 启动 HTTPS 服务器
		err := server.ListenAndServeTLS("server.crt", "server.key")
		if err != nil {
			log.Fatalf("Failed to start HTTPS server: %v", err)
		}
	} else {
		// 启动 HTTP 服务器
		err := server.ListenAndServe()
		if err != nil {
			log.Fatalf("Failed to start HTTP server: %v", err)
		}
	}
}
