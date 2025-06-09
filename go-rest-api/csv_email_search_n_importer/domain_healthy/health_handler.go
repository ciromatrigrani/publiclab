package domain_healthy

import (
	"encoding/json"
	"net/http"
)

type HealthHandler struct {
	service *HealthService
}

func NewHealthHandler(s *HealthService) *HealthHandler {
	return &HealthHandler{service: s}
}

func (h *HealthHandler) Check(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodGet {
		w.WriteHeader(http.StatusMethodNotAllowed)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(h.service.Check())
}