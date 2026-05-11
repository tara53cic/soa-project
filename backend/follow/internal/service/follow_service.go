package service

import "follow/internal/dto"

type FollowService interface {
	FollowUser(request dto.FollowRequest) error
	IsFollowing(followerID int64, followingID int64) (bool, error)
	GetFollowing(userID int64) ([]dto.UserResponse, error)
	GetRecommendations(userID int64) ([]dto.RecommendationResponse, error)
}
