package lib

//  7061706802:AAFeczJuLwyTo1iN2FBiKQZyz7HyQwWxG_Q
import (
	"fmt"
	"log"
	//	"os"

	tgbotapi "github.com/go-telegram-bot-api/telegram-bot-api"
)

var botToken string = "" // 替换为你的 Telegram Bot Token
var chatID string = ""   // 替换为你的聊天 ID 或频道 ID

var botClient *tgbotapi.BotAPI

// 获取机器人的信息 获取Telegram机器人的信息
func I获取机器人的信息() {
	defer func() {
		if r := recover(); r != nil {
			fmt.Printf("An error occurred: %v\n", r)
		}
	}()

	me, err := botClient.GetMe()
	if err != nil {
		log.Printf("An error occurred: %v\n", err)
		return
	}

	fmt.Printf("Bot ID: %d\n", me.ID)
	fmt.Printf("Bot Name: %s\n", me.FirstName)
	fmt.Printf("Bot Username: %s\n", me.UserName)
}

func StartTg(handleMessage func(*tgbotapi.BotAPI, tgbotapi.Update)) {
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

	if token == "" {
		log.Fatal("TELEGRAM_BOT_TOKEN environment variable is not set")
	}

	// 创建新的 Telegram 机器人实例
	bot, err := tgbotapi.NewBotAPI(token)
	if err != nil {
		log.Fatalf("Failed to create bot: %v", err)
	}
	botClient = bot
	I获取机器人的信息()
	// 设置机器人的日志级别
	bot.Debug = true
	log.Printf("Authorized on account %s", bot.Self.UserName)

	//----------------------------rcv msg---------
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
	//------------end rcv msg
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
