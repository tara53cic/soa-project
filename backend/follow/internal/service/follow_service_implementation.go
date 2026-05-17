package service

import (
	"errors"
	"follow/internal/dto"
	"follow/internal/repository"
)

type followService struct {
	repository repository.FollowRepository
}

func NewFollowService(repository repository.FollowRepository) FollowService {
	return &followService{
		repository: repository,
	}
}

func (s *followService) FollowUser(request dto.FollowRequest) error {

	if request.FollowerUsername == request.FollowingUsername {
		return errors.New("user cannot follow themselves")
	}

	isFollowing, err := s.repository.IsFollowing(
		request.FollowerUsername,
		request.FollowingUsername,
	)

	if err != nil {
		return err
	}

	if isFollowing {
		return errors.New("already following this user")
	}

	return s.repository.FollowUser(
		request.FollowerUsername,
		request.FollowingUsername,
	)
}

func (s *followService) IsFollowing(followerUsername string, followingUsername string) (bool, error) {
	return s.repository.IsFollowing(followerUsername, followingUsername)
}

func (s *followService) GetFollowing(username string) ([]dto.UserResponse, error) {
	return s.repository.GetFollowing(username)
}

func (s *followService) GetRecommendations(username string) ([]dto.RecommendationResponse, error) {
	return s.repository.GetRecommendations(username)
}

func (s *followService) GetAllProfilesWithFollowStatus(loggedUsername string) ([]dto.ProfileFollowStatusResponse, error) {
	return s.repository.GetAllProfilesWithFollowStatus(loggedUsername)
}

func (s *followService) SetUserBlockStatus(userId int64, isBlocked bool) error {
	return s.repository.SetUserBlockStatus(userId, isBlocked)
}
