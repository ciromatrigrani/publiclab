package domain_user_test

import (
	"csv_email_search_n_importer/domain_user"
	
	"testing"
)

type MockUserRepository_FindByEmail struct {
	UserToReturn *domain_user.User
	ErrorToReturn error
}

func (m *MockUserRepository_FindByEmail) FindByEmail(email string) (*domain_user.User, error) {
	return m.UserToReturn, m.ErrorToReturn
}

func (m *MockUserRepository_FindByEmail) Save(user domain_user.User) error {
	return nil
}


func TestUserService_FindByEmailSuccess(t *testing.T) {
	expectedUser := &domain_user.User{
		ID:           "user123",
		FirstName:    "Test",
		LastName:     "User",
		EmailAddress: "test@example.com",
		CreatedAt:    "2023-01-01",
		DeletedAt:    "",
		MergedAt:     "",
		ParentUserID: "",
	}

	mockRepo := &MockUserRepository_FindByEmail{
		UserToReturn: expectedUser,
		ErrorToReturn: nil,
	}

	service := domain_user.NewUserService(mockRepo)

	userResponse, err := service.FindByEmail("test@example.com")

	if err != nil {
		t.Fatalf("Expected no error, but got: %v", err)
	}

	if userResponse == nil {
		t.Fatal("Expected a user response, but got nil")
	}

	if userResponse.ID != expectedUser.ID {
		t.Errorf("Expected ID %s, got %s", expectedUser.ID, userResponse.ID)
	}
	if userResponse.FirstName != expectedUser.FirstName {
		t.Errorf("Expected FirstName %s, got %s", expectedUser.FirstName, userResponse.FirstName)
	}
	if userResponse.EmailAddress != expectedUser.EmailAddress {
		t.Errorf("Expected EmailAddress %s, got %s", expectedUser.EmailAddress, userResponse.EmailAddress)
	}

}