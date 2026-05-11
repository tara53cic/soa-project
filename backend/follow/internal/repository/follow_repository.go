package repository

import "follow/internal/dto"

type FollowRepository interface {
	FollowUser(followerID int64, followingID int64) error
	IsFollowing(followerID int64, followingID int64) (bool, error)
	GetFollowing(userID int64) ([]dto.UserResponse, error)
	GetRecommendations(userID int64) ([]dto.RecommendationResponse, error)
}
