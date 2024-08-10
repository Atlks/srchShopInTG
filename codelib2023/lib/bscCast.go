package lib

import (
	"net/url"
	"strings"
)

// ConvertToValidFileName 将非法字符转换为 URL 编码格式的有效字符
func ConvertToValidFileName(input string) string {
	invalidChars := `\/|:"*?<>`
	var encodedBuilder strings.Builder

	for _, c := range input {
		if strings.ContainsRune(invalidChars, c) {
			// 如果字符为非法字符，则使用 URL 编码替换
			encoded := url.QueryEscape(string(c))
			encodedBuilder.WriteString(encoded)
		} else {
			// 如果字符为合法字符，则直接添加到结果中
			encodedBuilder.WriteRune(c)
		}
	}

	return encodedBuilder.String()
}
