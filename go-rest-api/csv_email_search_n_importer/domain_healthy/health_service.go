package domain_healthy

type HealthService struct{}

func (s *HealthService) Check() Health {
	return NewHealthCheck()
}
