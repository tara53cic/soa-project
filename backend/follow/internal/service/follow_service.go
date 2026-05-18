package service

import "follow/internal/dto"

type FollowService interface {
	FollowUser(request dto.FollowRequest) error
	IsFollowing(followerUsername string, followingUsername string) (bool, error)
	GetFollowing(username string) ([]dto.UserResponse, error)
	GetRecommendations(username string) ([]dto.RecommendationResponse, error)
	GetAllProfilesWithFollowStatus(loggedUsername string) ([]dto.ProfileFollowStatusResponse, error)
	SetUserBlockStatus(serId int64, isBlocked bool) error
}
