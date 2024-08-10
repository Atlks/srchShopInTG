package lib

//  7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q

//	"os"

// 检查消息内容是否包含 "今日工作内容"

// 辅助函数：检查字符串是否包含子字符串
func Contains(s, substr string) bool {
	return len(s) > 0 && len(substr) > 0 && s != substr && len(s) >= len(substr) && (s[len(s)-len(substr):] == substr || s[:len(substr)] == substr || Contains(s[1:], substr))
}
