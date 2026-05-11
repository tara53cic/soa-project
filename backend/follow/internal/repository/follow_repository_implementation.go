package repository

import (
	"context"
	"follow/internal/dto"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

type followRepository struct {
	driver neo4j.DriverWithContext
}

func NewFollowRepository(driver neo4j.DriverWithContext) FollowRepository {
	return &followRepository{
		driver: driver,
	}
}

func (r *followRepository) FollowUser(followerID int64, followingID int64) error {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MERGE (follower:User {id: $followerId})
			MERGE (following:User {id: $followingId})
			MERGE (follower)-[:FOLLOWS]->(following)
		`

		params := map[string]any{
			"followerId":  followerID,
			"followingId": followingID,
		}

		_, err := tx.Run(ctx, query, params)
		return nil, err
	})

	return err
}

func (r *followRepository) IsFollowing(followerID int64, followingID int64) (bool, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (follower:User {id: $followerId})-[:FOLLOWS]->(following:User {id: $followingId})
			RETURN count(following) > 0 AS isFollowing
		`

		params := map[string]any{
			"followerId":  followerID,
			"followingId": followingID,
		}

		record, err := tx.Run(ctx, query, params)
		if err != nil {
			return false, err
		}

		if record.Next(ctx) {
			value, _ := record.Record().Get("isFollowing")
			return value.(bool), nil
		}

		return false, nil
	})

	if err != nil {
		return false, err
	}

	return result.(bool), nil
}

func (r *followRepository) GetFollowing(userID int64) ([]dto.UserResponse, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (:User {id: $userId})-[:FOLLOWS]->(following:User)
			RETURN following.id AS id
		`

		params := map[string]any{
			"userId": userID,
		}

		records, err := tx.Run(ctx, query, params)
		if err != nil {
			return nil, err
		}

		users := []dto.UserResponse{}

		for records.Next(ctx) {
			record := records.Record()

			idValue, _ := record.Get("id")

			user := dto.UserResponse{
				ID: idValue.(int64),
			}

			users = append(users, user)
		}

		return users, records.Err()
	})

	if err != nil {
		return nil, err
	}

	return result.([]dto.UserResponse), nil
}

func (r *followRepository) GetRecommendations(userID int64) ([]dto.RecommendationResponse, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (me:User {id: $userId})-[:FOLLOWS]->(:User)-[:FOLLOWS]->(recommended:User)
			WHERE NOT (me)-[:FOLLOWS]->(recommended)
			AND me <> recommended
			RETURN recommended.id AS id, count(recommended) AS mutualConnections
			ORDER BY mutualConnections DESC
		`

		params := map[string]any{
			"userId": userID,
		}

		records, err := tx.Run(ctx, query, params)
		if err != nil {
			return nil, err
		}

		recommendations := []dto.RecommendationResponse{}

		for records.Next(ctx) {
			record := records.Record()

			idValue, _ := record.Get("id")
			mutualValue, _ := record.Get("mutualConnections")

			recommendation := dto.RecommendationResponse{
				ID:                idValue.(int64),
				MutualConnections: mutualValue.(int64),
			}

			recommendations = append(recommendations, recommendation)
		}

		return recommendations, records.Err()
	})

	if err != nil {
		return nil, err
	}

	return result.([]dto.RecommendationResponse), nil
}
