package domain_user

type UserService struct {
	repo UserRepository 
}

func NewUserService(repo UserRepository) *UserService {
	return &UserService{repo: repo}
}

func (s *UserService) FindByEmail(email string) (*UserResponse, error) {
	user, err := s.repo.FindByEmail(email)
	if err != nil {
		return nil, err
	}

	return &UserResponse{
		ID:           user.ID,
		FirstName:    user.FirstName,
		LastName:     user.LastName,
		EmailAddress: user.EmailAddress,
	}, nil
}

func (s *UserService) GetAllUsers() []UserResponse {
	users := s.repo.GetAll()
	responses := make([]UserResponse, len(users))
	for i, user := range users {
		responses[i] = UserResponse{
			ID:           user.ID,
			FirstName:    user.FirstName,
			LastName:     user.LastName,
			EmailAddress: user.EmailAddress,
		}
	}
	return responses
}