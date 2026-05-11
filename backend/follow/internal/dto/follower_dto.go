package dto

type UserResponse struct {
	ID       int64  `json:"id"`
	Username string `json:"username"`
}

type RecommendationResponse struct {
	ID                int64  `json:"id"`
	Username          string `json:"username"`
	MutualConnections int64  `json:"mutualConnections"`
}

type FollowRequest struct {
	FollowerID  int64 `json:"followerId"`
	FollowingID int64 `json:"followingId"`
}
