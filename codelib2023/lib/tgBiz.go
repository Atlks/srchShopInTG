package lib

//  7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q
import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"path/filepath"

	//	"os"
	"time"

	tgbotapi "github.com/go-telegram-bot-api/telegram-bot-api"
)

// https://api.telegram.org/bot7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q/getUpdates
const (
//	botToken = "7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q" // 替换为你的 Telegram Bot Token

)

// 保存消息到文件的函数
func saveMessageToFile(message tgbotapi.Message) error {
	fld, _ := CreateFolderBasedOnDate("dlyrpt")
	// 创建 dlyrpt 文件夹（如果不存在）
	err := os.MkdirAll(fld, os.ModePerm)
	if err != nil {
		return fmt.Errorf("无法创建文件夹: %v", err)
	}

	// 序列化消息为 JSON
	jsonData, err := json.Marshal(message)
	if err != nil {
		return fmt.Errorf("JSON 序列化失败: %v", err)
	}

	// 获取发送人的用户名
	username := message.From.UserName
	if username == "" {
		username = "unknown"
	}

	uname := "uname(" + username + ") frstLastname(" + message.From.FirstName + message.From.LastName + ")"
	// 确定文件路径
	fname := ConvertToValidFileName(uname)

	filePath := filepath.Join(fld, fname+".json")

	// 保存 JSON 文件
	err = os.WriteFile(filePath, jsonData, 0644)
	if err != nil {
		return fmt.Errorf("无法写入文件: %v", err)
	}

	return nil
}

// 处理接收到的消息
func handleMessage(bot *tgbotapi.BotAPI, update tgbotapi.Update) {

	message := update.Message
	if message == nil || message.Text == "" {
		return
	}

	//------------------------today wk rpt
	// 检查消息内容是否包含 "今日工作内容"
	if Contains(message.Text, "今日工作内容") {
		err := saveMessageToFile(*message)
		if err != nil {
			fmt.Println("错误:", err)
		} else {
			fmt.Println("消息已保存")
		}

		// 创建要发送的回复消息
		fld, _ := CreateFolderBasedOnDate("dlyrpt")
		alreadySendUsers, _ := getFileNamesAsJSON(fld)
		messageContent := "接受到日报消息." + "\n目前已经发送的人员如下：\n" + alreadySendUsers

		reply := tgbotapi.NewMessage(update.Message.Chat.ID, messageContent)
		// 设置回复的消息 ID
		messageID := update.Message.MessageID
		reply.ReplyToMessageID = messageID
		// 发送回复
		_, err2 := bot.Send(reply)
		if err2 != nil {
			log.Printf("Failed to send message: %v", err2)
		}
	}
}

func Main4daylyRpt() {

	StartTg(handleMessage)

	// 设置每天 5:30 执行任务
	go scheduleDailyTask(14, 21, sendMessage4daylyrpt)
	go scheduleDailyTask(5, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(6, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(8, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(11, 30, sendMessage4daylyrpt)

}

func scheduleDailyTask(hour, minute int, task func()) {

	CreateFolderBasedOnDate("dlyrpt")
	for {
		now := time.Now()
		// 获取本地时间
		loc := time.Local
		nextRun := time.Date(now.Year(), now.Month(), now.Day(), hour, minute, 0, 0, loc)

		// 如果现在时间已经超过了计划时间，则将任务计划在第二天
		if now.After(nextRun) {
			nextRun = nextRun.Add(24 * time.Hour)
		}

		duration := nextRun.Sub(now)
		time.Sleep(duration)

		task()
	}
}

// 获取文件夹中的所有文件名并序列化为 JSON 字符串
func getFileNamesAsJSON(dirPath string) (string, error) {
	// 打开目录
	files, err := ioutil.ReadDir(dirPath)
	if err != nil {
		return "", fmt.Errorf("无法读取目录: %v", err)
	}

	// 创建一个存储文件名的切片
	var fileNames []string

	// 遍历目录中的所有文件
	for _, file := range files {
		if !file.IsDir() { // 只获取文件，不包括子目录
			fileNames = append(fileNames, file.Name())
		}
	}

	// 将文件名切片序列化为 JSON 字符串
	jsonData, err := json.Marshal(fileNames)
	if err != nil {
		return "", fmt.Errorf("JSON 序列化失败: %v", err)
	}

	return string(jsonData), nil
}

func sendMessage4daylyrpt() {
	bot, err := tgbotapi.NewBotAPI(botToken)
	if err != nil {
		log.Fatalf("Failed to create bot: %v", err)
	}
	messageContent := "时间到了发日报了"
	fld, _ := CreateFolderBasedOnDate("dlyrpt")
	alreadySendUsers, _ := getFileNamesAsJSON(fld)
	messageContent = messageContent + "\n目前已经发送的人员如下：\n" + alreadySendUsers
	msg := tgbotapi.NewMessageToChannel(chatID, messageContent)
	_, err = bot.Send(msg)
	if err != nil {
		log.Fatalf("Failed to send message: %v", err)
	}

	log.Println("Message sent successfully")
}
