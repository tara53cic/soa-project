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

	if request.FollowerID == request.FollowingID {
		return errors.New("user cannot follow themselves")
	}

	isFollowing, err := s.repository.IsFollowing(
		request.FollowerID,
		request.FollowingID,
	)

	if err != nil {
		return err
	}

	if isFollowing {
		return errors.New("already following this user")
	}

	return s.repository.FollowUser(
		request.FollowerID,
		request.FollowingID,
	)
}

func (s *followService) IsFollowing(followerID int64, followingID int64) (bool, error) {
	return s.repository.IsFollowing(followerID, followingID)
}

func (s *followService) GetFollowing(userID int64) ([]dto.UserResponse, error) {
	return s.repository.GetFollowing(userID)
}

func (s *followService) GetRecommendations(userID int64) ([]dto.RecommendationResponse, error) {
	return s.repository.GetRecommendations(userID)
}
