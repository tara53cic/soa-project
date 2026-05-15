package handler

import (
	"encoding/json"
	"fmt"
	"follow/internal/auth"
	"follow/internal/dto"
	"follow/internal/service"
	"net/http"
)

type FollowHandler struct {
	service service.FollowService
}

func NewFollowHandler(service service.FollowService) *FollowHandler {
	return &FollowHandler{
		service: service,
	}
}

func (h *FollowHandler) FollowUser(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		return
	}

	var request dto.FollowRequest

	err := json.NewDecoder(r.Body).Decode(&request)
	if err != nil {
		http.Error(w, "Invalid request body", http.StatusBadRequest)
		return
	}

	err = h.service.FollowUser(request)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusCreated)
	w.Write([]byte("User followed successfully"))
}

func (h *FollowHandler) IsFollowing(w http.ResponseWriter, r *http.Request) {

	if r.Method != http.MethodGet {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		return
	}

	followerID := r.URL.Query().Get("followerId")
	followingID := r.URL.Query().Get("followingId")

	if followerID == "" || followingID == "" {
		http.Error(w, "Missing query parameters", http.StatusBadRequest)
		return
	}

	var follower string
	var following string

	_, err := fmt.Sscan(followerID, &follower)
	if err != nil {
		http.Error(w, "Invalid followerId", http.StatusBadRequest)
		return
	}

	_, err = fmt.Sscan(followingID, &following)
	if err != nil {
		http.Error(w, "Invalid followingId", http.StatusBadRequest)
		return
	}

	isFollowing, err := h.service.IsFollowing(follower, following)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	response := map[string]bool{
		"isFollowing": isFollowing,
	}

	w.Header().Set("Content-Type", "application/json")

	json.NewEncoder(w).Encode(response)
}

func (h *FollowHandler) GetFollowing(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodGet {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		return
	}

	username := r.URL.Query().Get("username")
	if username == "" {
		http.Error(w, "Missing username parameter", http.StatusBadRequest)
		return
	}

	users, err := h.service.GetFollowing(username)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(users)
}

func (h *FollowHandler) GetRecommendations(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodGet {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		return
	}

	username, err := auth.ExtractUsernameFromToken(r)
	if err != nil {
		http.Error(w, "Unauthorized: "+err.Error(), http.StatusUnauthorized)
		return
	}

	recommendations, err := h.service.GetRecommendations(username)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(recommendations)
}

func (h *FollowHandler) GetAllProfilesWithFollowStatus(w http.ResponseWriter, r *http.Request) {

	loggedUsername := r.URL.Query().Get("username")

	if loggedUsername == "" {
		http.Error(w, "username is required", http.StatusBadRequest)
		return
	}

	profiles, err := h.service.GetAllProfilesWithFollowStatus(loggedUsername)

	if err != nil {
		http.Error(w, "failed to fetch profiles", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(profiles)
}
