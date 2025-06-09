package domain_user

import(
    "os"
)

type User struct {
	ID           string `json:"id"`
	FirstName    string `json:"first_name"`
	LastName     string `json:"last_name"`
	EmailAddress string `json:"email_address"`
	CreatedAt    string `json:"created_at"`
	DeletedAt    string `json:"deleted_at"`
	MergedAt     string `json:"merged_at"`
	ParentUserID string `json:"parent_user_id"`
}

type UserRepository interface {
	ImportFromCSV(file *os.File) error 
	Save(user User) error
	FindByEmail(email string) (*User, error)
	GetAll() []User 
}