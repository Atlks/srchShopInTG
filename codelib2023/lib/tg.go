package lib

//  7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q
import (
	"log"
	"time"

	tgbotapi "github.com/go-telegram-bot-api/telegram-bot-api"
)

// https://api.telegram.org/bot7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q/getUpdates
const (
	botToken       = "7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q" // 替换为你的 Telegram Bot Token
	chatID         = "-1002155607657"                                 // 替换为你的聊天 ID 或频道 ID
	messageContent = "时间到了发日报了"
)

func Main4daylyRpt() {

	//time.Sleep(duration)
	SendMessage("start...")
	// 设置每天 5:30 执行任务
	go scheduleDailyTask(5, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(6, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(8, 30, sendMessage4daylyrpt)
	go scheduleDailyTask(11, 30, sendMessage4daylyrpt)
	// 阻塞主线程
	select {}
}

func scheduleDailyTask(hour, minute int, task func()) {
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

func sendMessage4daylyrpt() {
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
