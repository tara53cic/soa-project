package repository

import "follow/internal/dto"

type FollowRepository interface {
	FollowUser(followerUsername string, followingUsername string) error
	IsFollowing(followerUsername string, followingUsername string) (bool, error)
	GetFollowing(username string) ([]dto.UserResponse, error)
	GetRecommendations(username string) ([]dto.RecommendationResponse, error)
	GetAllProfilesWithFollowStatus(loggedUsername string) ([]dto.ProfileFollowStatusResponse, error)
	SetUserBlockStatus(serId int64, isBlocked bool) error
}
