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

func (r *followRepository) FollowUser(followerUsername string, followingUsername string) error {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MERGE (follower:User {username: $followerUsername})
			MERGE (following:User {username: $followingUsername})
			MERGE (follower)-[:FOLLOWS]->(following)
		`

		params := map[string]any{
			"followerUsername":  followerUsername,
			"followingUsername": followingUsername,
		}

		_, err := tx.Run(ctx, query, params)
		return nil, err
	})

	return err
}

func (r *followRepository) IsFollowing(followerUsername string, followingUsername string) (bool, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (follower:User {username: $followerUsername})-[:FOLLOWS]->(following:User {username: $followingUsername})
			RETURN count(following) > 0 AS isFollowing
		`

		params := map[string]any{
			"followerUsername":  followerUsername,
			"followingUsername": followingUsername,
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

func (r *followRepository) GetFollowing(username string) ([]dto.UserResponse, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (:User {username: $username})-[:FOLLOWS]->(following:User)
			RETURN following.username AS username
		`

		params := map[string]any{
			"username": username,
		}

		records, err := tx.Run(ctx, query, params)
		if err != nil {
			return nil, err
		}

		users := []dto.UserResponse{}

		for records.Next(ctx) {
			record := records.Record()

			usernameValue, ok := record.Get("username")
			if !ok {
				continue
			}

			usernameString, ok := usernameValue.(string)
			if !ok {
				continue
			}

			users = append(users, dto.UserResponse{
				Username: usernameString,
			})
		}

		if err := records.Err(); err != nil {
			return nil, err
		}

		return users, nil
	})

	if err != nil {
		return nil, err
	}

	return result.([]dto.UserResponse), nil
}

func (r *followRepository) GetRecommendations(username string) ([]dto.RecommendationResponse, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {
		query := `
			MATCH (u:User)
			WHERE u.username <> $username
			  AND NOT EXISTS { MATCH (:User {username: $username})-[:FOLLOWS]->(u) }
			OPTIONAL MATCH (:User {username: $username})-[:FOLLOWS]->(:User)-[r:FOLLOWS]->(u)
			RETURN u.username AS username,
				COUNT(r) AS mutualConnections
			ORDER BY mutualConnections DESC
			LIMIT 10
		`

		params := map[string]any{
			"username": username,
		}

		records, err := tx.Run(ctx, query, params)
		if err != nil {
			return nil, err
		}

		recommendations := []dto.RecommendationResponse{}

		for records.Next(ctx) {
			record := records.Record()

			usernameValue, _ := record.Get("username")
			mutualValue, _ := record.Get("mutualConnections")

			recommendation := dto.RecommendationResponse{
				Username:          usernameValue.(string),
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

func (r *followRepository) GetAllProfilesWithFollowStatus(loggedUsername string) ([]dto.ProfileFollowStatusResponse, error) {
	ctx := context.Background()

	session := r.driver.NewSession(ctx, neo4j.SessionConfig{})
	defer session.Close(ctx)

	result, err := session.ExecuteRead(ctx, func(tx neo4j.ManagedTransaction) (any, error) {

		query := `
			MATCH (u:User)
			WHERE u.username <> $loggedUsername
			RETURN 
				u.username AS username,
				EXISTS { MATCH (:User {username: $loggedUsername})-[:FOLLOWS]->(u) } AS following
			ORDER BY username
		`

		params := map[string]any{
			"loggedUsername": loggedUsername,
		}

		records, err := tx.Run(ctx, query, params)
		if err != nil {
			return nil, err
		}

		var profiles []dto.ProfileFollowStatusResponse

		for records.Next(ctx) {

			record := records.Record()

			username, _ := record.Get("username")
			following, _ := record.Get("following")

			profiles = append(profiles, dto.ProfileFollowStatusResponse{
				Username:  username.(string),
				Following: following.(bool),
			})
		}

		return profiles, records.Err()
	})

	if err != nil {
		return nil, err
	}

	return result.([]dto.ProfileFollowStatusResponse), nil
}
