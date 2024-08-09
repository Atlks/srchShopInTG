package lib

import (
	"errors"
	"strings"
	"time"
)

// NewToken generates a new token with the given UID and expiration time.
func NewToken(uid string, exprtTimeSecsAftr int, key string) (string, error) {
	tkExprt := AddTime(exprtTimeSecsAftr)
	issTime := time.Now().Format(time.RFC3339)
	mrg, err := EncryptAes(uid+"."+tkExprt+"."+issTime, key)
	if err != nil {
		return "", err
	}
	tkOri := uid + "_" + mrg
	return tkOri, nil
}

// IsValidToken checks if the given token is valid.
func IsValidToken(token string, key string) (bool, error) {
	parts := strings.Split(token, "_")
	if len(parts) != 2 {
		return false, errors.New("invalid token format")
	}

	uid := parts[0]
	exprt := parts[1]

	mrgDecd, err := DecryptAes(exprt, key)
	if err != nil {
		return false, err
	}

	a := strings.Split(mrgDecd, ".")
	if len(a) != 3 {
		return false, errors.New("invalid decrypted format")
	}

	uidInMrg := a[0]
	exprtTime := a[1]
	//issTime := a[2]

	if uid != uidInMrg {
		return false, nil
	}

	// Check if the expiration time is valid
	isExprt, err := IsExprt(exprtTime)
	if err != nil {
		return false, err
	}
	if isExprt {
		return false, nil
	}
	return true, nil
}

// IsExprt checks if the given time string represents a time before the current time.
func IsExprt207(timeString string) (bool, error) {
	format := "2006-01-02 15:04:05"
	parsedTime, err := time.Parse(format, timeString)
	if err != nil {
		return false, errors.New("invalid time format")
	}

	currentTime := time.Now()
	return parsedTime.Before(currentTime), nil
}

// AddTime adds a specified number of seconds to the current time and returns the formatted string.
func AddTime(secondsToAdd int) string {
	// 获取当前时间
	currentTime := time.Now()

	// 计算未来时间
	futureTime := currentTime.Add(time.Duration(secondsToAdd) * time.Second)

	// 定义时间格式
	format := "2006-01-02 15:04:05" // Go 语言中时间格式必须用固定的时间 "Mon Jan 2 15:04:05 MST 2006"

	// 返回格式化后的时间字符串
	return futureTime.Format(format)
}

// NewToken generates a new token with the given UID and expiration time.
func NewToken208(uid string, exprtTimeSecsAftr int, key string) (string, error) {
	tkExprt := AddTime(exprtTimeSecsAftr)
	tkExpEnc, err := EncryptAes(tkExprt, key)
	if err != nil {
		return "", err
	}
	tkOri := uid + "_" + tkExpEnc
	return tkOri, nil
}
