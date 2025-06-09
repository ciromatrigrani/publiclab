package router

import (
	"net/http"
	"csv_email_search_n_importer/domain_healthy"
	"csv_email_search_n_importer/domain_import"   
	"csv_email_search_n_importer/domain_user"
	"csv_email_search_n_importer/infrastructure/observability" 
	"csv_email_search_n_importer/infrastructure/persistance"   
)

func NewRouter() *http.ServeMux {
	mux := http.NewServeMux()

	healthSvc := &domain_healthy.HealthService{}
	healthHandler := domain_healthy.NewHealthHandler(healthSvc)
	mux.HandleFunc("GET /healthy", healthHandler.Check)

	userRepo := persistance.NewUserRepository()
	logger.GetLogger().Println("Repositório de usuários inicializado.")

	userSvc := domain_user.NewUserService(userRepo) 
	userHandler := domain_user.NewUserHandler(userSvc)
	mux.HandleFunc("GET /user", userHandler.FindByEmail)

	importHandler := domain_import.NewImportHandler(userRepo) 
	mux.HandleFunc("POST /import", importHandler.HandleImport)

	logger.GetLogger().Println("Configuração do roteador finalizada.")

	return mux
}