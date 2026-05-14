package service

import "follow/internal/dto"

type FollowService interface {
	FollowUser(request dto.FollowRequest) error
	IsFollowing(followerUsername string, followingUsername string) (bool, error)
	GetFollowing(userID int64) ([]dto.UserResponse, error)
	GetRecommendations(username string) ([]dto.RecommendationResponse, error)
}
