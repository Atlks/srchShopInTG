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
	if containsTodayWorkContent(message.Text) {
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

// 检查消息内容是否包含 "今日工作内容"
func containsTodayWorkContent(text string) bool {
	return contains(text, "今日工作内容")
}

// 辅助函数：检查字符串是否包含子字符串
func contains(s, substr string) bool {
	return len(s) > 0 && len(substr) > 0 && s != substr && len(s) >= len(substr) && (s[len(s)-len(substr):] == substr || s[:len(substr)] == substr || contains(s[1:], substr))
}

var botToken string = "" // 替换为你的 Telegram Bot Token
var chatID string = ""   // 替换为你的聊天 ID 或频道 ID
func Main4daylyRpt() {
	fmt.Print(" GetRealPath=>" + GetRealPath("cfg117.ini"))
	config := GetDicFromIni("cfg117.ini")
	if !FileExists("cfg117.ini") {
		fmt.Print(` GetDicFromIni D:\0prj\mdsj\codelib2023\cfg117.ini`)
		config = GetDicFromIni(`D:\0prj\mdsj\codelib2023\cfg117.ini`)
	}

	// 从环境变量中获取 Telegram 机器人令牌
	token := (config["token"])
	botToken = config["token"]
	chatID = config["chatID"] //"-1002155607657"
	//time.Sleep(duration)
	SendMessage("start...")
	// 设置每天 5:30 执行任务
	go scheduleDailyTask(14, 21, sendMessage4daylyrpt)
	go scheduleDailyTask(5, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(6, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(8, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(11, 30, sendMessage4daylyrpt)

	if token == "" {
		log.Fatal("TELEGRAM_BOT_TOKEN environment variable is not set")
	}

	// 创建新的 Telegram 机器人实例
	bot, err := tgbotapi.NewBotAPI(token)
	if err != nil {
		log.Fatalf("Failed to create bot: %v", err)
	}

	// 设置机器人的日志级别
	bot.Debug = true
	log.Printf("Authorized on account %s", bot.Self.UserName)

	// 创建更新通道
	u := tgbotapi.NewUpdate(0)
	u.Timeout = 60

	// 获取更新通道
	updates, err := bot.GetUpdatesChan(u)
	if err != nil {
		log.Fatalf("Failed to get updates channel: %v", err)
	}

	for update := range updates {
		if update.Message != nil { // 如果有消息
			// 打印消息内容
			log.Printf("Received message from chat ID %d: %s", update.Message.Chat.ID, update.Message.Text)

			// 你可以在这里处理消息，例如发送响应
			//	msg := tgbotapi.NewMessage(update.Message.Chat.ID, "消息已接收")
			handleMessage(bot, update)

		}
	}
	// 阻塞主线程
	select {}
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

func SendMessage(messageContent string) {
	bot, err := tgbotapi.NewBotAPI(botToken)
	if err != nil {
		log.Fatalf("Failed to create bot: %v", err)
	}

	msg := tgbotapi.NewMessageToChannel(chatID, messageContent)
	_, err = bot.Send(msg)
	if err != nil {
		log.Fatalf("Failed to send message: %v", err)
	}

	log.Println("Message sent successfully")
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
