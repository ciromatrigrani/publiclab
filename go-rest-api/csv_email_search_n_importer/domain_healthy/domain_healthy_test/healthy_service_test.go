package domain_healthy_test

import (
	"csv_email_search_n_importer/domain_healthy"
	"testing"
)

func TestHealthService_CheckSuccess(t *testing.T) {

	service := &domain_healthy.HealthService{}
	healthStatus := service.Check()

	if healthStatus.Status != "active" {
		t.Errorf("Expected health status 'active', got '%s'", healthStatus.Status)
	}

	if healthStatus.Timestamp.IsZero() {
		t.Error("Expected timestamp to be not zero")
	}
}