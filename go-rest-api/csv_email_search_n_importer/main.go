package main

import (
	"net/http"
	"csv_email_search_n_importer/infrastructure/router"
	"csv_email_search_n_importer/infrastructure/observability"
)

func main() {

	logger.SetupLogger("application.log")

  	logger.GetLogger().Println("Starting application...")
    logger.GetLogger().Printf("Application environment: %s", "development")

	router := router.NewRouter()
	logger.GetLogger().Println("Server running on :8080")
	logger.GetLogger().Fatal(http.ListenAndServe(":8080", router))
}