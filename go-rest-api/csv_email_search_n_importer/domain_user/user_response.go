package domain_user

type UserResponse struct {
	ID           string `json:"id"`
	FirstName    string `json:"first_name"`
	LastName     string `json:"last_name"`
	EmailAddress string `json:"email_address"`
}