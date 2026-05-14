package dto

type UserResponse struct {
	ID       int64  `json:"id"`
	Username string `json:"username"`
}

type RecommendationResponse struct {
	Username          string `json:"username"`
	MutualConnections int64  `json:"mutualConnections"`
}

type FollowRequest struct {
	FollowerUsername  string `json:"followerUsername"`
	FollowingUsername string `json:"followingUsername"`
}
