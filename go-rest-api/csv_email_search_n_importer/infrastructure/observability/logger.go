package logger

import (
	"io"
	"log"
	"os"
	"sync" 
)

type Logger interface {
	Print(v ...interface{})
	Printf(format string, v ...interface{})
	Println(v ...interface{})
	Fatal(v ...interface{})
	Fatalf(format string, v ...interface{})
	Fatalln(v ...interface{})
	Panic(v ...interface{})
	Panicf(format string, v ...interface{})
	Panicln(v ...interface{})
}

var loggerInstance *log.Logger
var once sync.Once


func InitLogger(logFilePath string) {
	once.Do(func() {
		file, err := os.OpenFile(logFilePath, os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0666)
		if err != nil {
			log.Fatalf("Failed to open log file: %v", err)
		}

		multiWriter := io.MultiWriter(os.Stdout, file)

		loggerInstance = log.New(multiWriter, "", log.Ldate|log.Ltime|log.Lshortfile)
		loggerInstance.Println("Logger initialized: logging to console and file.")
	})
}

func GetLogger() Logger {
	if loggerInstance == nil {
		return log.Default() 
	}
	return loggerInstance
}

func SetupLogger(logFilePath string) {
	file, err := os.OpenFile(logFilePath, os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0666)
	if err != nil {
		log.Fatalf("Failed to open log file: %v", err)
	}

	multiWriter := io.MultiWriter(file, os.Stdout)
	log.SetOutput(multiWriter)

	log.SetFlags(log.Ldate | log.Ltime | log.Lshortfile)
}

func SetupMetrics(metricsFilePath string) {
	// file, err := os.OpenFile(logFilePath, os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0666)
	// if err != nil {
	// 	log.Fatalf("Failed to open log file: %v", err)
	// }

	// log.SetOutput(file)

	// log.SetFlags(log.Ldate | log.Ltime | log.Lshortfile)
}