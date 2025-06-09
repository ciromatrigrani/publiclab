package persistance

import (
	"encoding/csv"
	"errors"
	"fmt" 
	"os"
	"sync"
	"csv_email_search_n_importer/domain_user"
)

type UserRepository struct {
	users map[string]domain_user.User 
	mu    sync.RWMutex                
}

func NewUserRepository() *UserRepository {
	return &UserRepository{
		users: make(map[string]domain_user.User),
	}
}

func (r *UserRepository) ImportFromCSV(file *os.File) error {
	r.mu.Lock() 
	defer r.mu.Unlock()

	reader := csv.NewReader(file)

	records, err := reader.ReadAll()
	if err != nil {
		return fmt.Errorf("failed to read CSV records: %w", err)
	}

	for i, record := range records {
		if i == 0 { 
			continue
		}

		if len(record) < 8 {
			return fmt.Errorf("malformed CSV record at line %d: expected 8 fields, got %d", i+1, len(record))
		}

		user := domain_user.User{
			ID:           record[0],
			FirstName:    record[1],
			LastName:     record[2],
			EmailAddress: record[3],
			CreatedAt:    record[4],
			DeletedAt:    record[5],
			MergedAt:     record[6],
			ParentUserID: record[7],
		}
		r.users[user.EmailAddress] = user // Armazena usando EmailAddress como chave
	}
	return nil
}

func (r *UserRepository) Save(user domain_user.User) error {
	r.mu.Lock()
	defer r.mu.Unlock()
	r.users[user.EmailAddress] = user // Salva usando EmailAddress como chave
	return nil
}

func (r *UserRepository) FindByEmail(email string) (*domain_user.User, error) {
	r.mu.RLock() // Bloqueia para leitura
	defer r.mu.RUnlock()

	user, ok := r.users[email]
	if !ok {
		return nil, errors.New("user not found")
	}
	return &user, nil
}

func (r *UserRepository) GetAll() []domain_user.User {
	r.mu.RLock()
	defer r.mu.RUnlock()

	allUsers := make([]domain_user.User, 0, len(r.users))
	for _, user := range r.users {
		allUsers = append(allUsers, user)
	}
	return allUsers
}