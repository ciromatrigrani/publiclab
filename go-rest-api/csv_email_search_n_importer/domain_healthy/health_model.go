package domain_healthy

import "time"

type Health struct {
	Status    string    `json:"status"`
	Timestamp time.Time `json:"timestamp"`
}

func NewHealthCheck() Health {
	return Health{
		Status:    "active",
		Timestamp: time.Now().UTC(),
	}
}