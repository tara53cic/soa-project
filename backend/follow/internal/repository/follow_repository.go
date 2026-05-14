package repository

import "follow/internal/dto"

type FollowRepository interface {
	FollowUser(followerUsername string, followingUsername string) error
	IsFollowing(followerUsername string, followingUsername string) (bool, error)
	GetFollowing(userID int64) ([]dto.UserResponse, error)
	GetRecommendations(username string) ([]dto.RecommendationResponse, error)
}
