package domain_import

import (
	//"encoding/csv"
	"fmt"
	"io"
	"net/http"
	"os"
	"csv_email_search_n_importer/domain_user"           
	"csv_email_search_n_importer/infrastructure/observability" 
)

type ImportHandler struct {
	repo domain_user.UserRepository 
}

func NewImportHandler(repo domain_user.UserRepository) *ImportHandler {
	return &ImportHandler{repo: repo}
}

func (h *ImportHandler) HandleImport(w http.ResponseWriter, r *http.Request) {
	
	logger.GetLogger().Println("Iniciando importação...")

	if r.Method != http.MethodPost {
		logger.GetLogger().Printf("Método não permitido: %s", r.Method)
		w.WriteHeader(http.StatusMethodNotAllowed)
		return
	}

	err := r.ParseMultipartForm(32 << 20) 
	if err != nil {
		logger.GetLogger().Printf("Erro ao fazer parse do formulário multipart: %v", err)
		http.Error(w, "Falha ao analisar formulário multipart", http.StatusBadRequest)
		return
	}

	file, header, err := r.FormFile("file") 
	if err != nil {
		logger.GetLogger().Printf("Erro ao obter arquivo do formulário: %v", err)
		http.Error(w, "Por favor, envie um arquivo chamado 'file'", http.StatusBadRequest)
		return
	}
	defer file.Close()
	logger.GetLogger().Printf("Arquivo recebido: %s (tamanho: %d bytes)", header.Filename, header.Size)

	tempFile, err := os.CreateTemp("", "upload-*.csv")
	if err != nil {
		logger.GetLogger().Printf("Erro ao criar arquivo temporário: %v", err)
		http.Error(w, "Falha ao processar upload", http.StatusInternalServerError)
		return
	}
	defer os.Remove(tempFile.Name()) 
	defer tempFile.Close()

	if _, err := io.Copy(tempFile, file); err != nil {
		logger.GetLogger().Printf("Erro ao copiar arquivo para temporário: %v", err)
		http.Error(w, "Falha ao salvar arquivo temporário", http.StatusInternalServerError)
		return
	}

	if _, err := tempFile.Seek(0, 0); err != nil {
		logger.GetLogger().Printf("Erro ao rebobinar arquivo temporário: %v", err)
		http.Error(w, "Falha interna do servidor", http.StatusInternalServerError)
		return
	}

	if err := h.repo.ImportFromCSV(tempFile); err != nil {
		logger.GetLogger().Printf("Erro ao importar CSV para o repositório: %v", err)
		http.Error(w, fmt.Sprintf("Erro ao importar dados do CSV: %v", err), http.StatusInternalServerError)
		return
	}

	logger.GetLogger().Println("Importação concluída com sucesso.")
	w.WriteHeader(http.StatusOK)
}