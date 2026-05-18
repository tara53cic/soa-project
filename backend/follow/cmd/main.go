package main

import (
	"context"
	"follow/internal"
	"follow/internal/config"
	"follow/internal/db"
	"follow/internal/handler"
	"follow/internal/middleware"
	"follow/internal/repository"
	"follow/internal/service"
	"log"
	"net/http"
	"time"
)

func main() {
	cfg := config.LoadConfig()

	neo4jDriver, err := db.NewNeo4jDriver(cfg)
	if err != nil {
		log.Fatal("Failed to create Neo4j driver: ", err)
	}

	defer neo4jDriver.Close(context.Background())

	maxRetries := 15
	for i := 0; i < maxRetries; i++ {
		err = neo4jDriver.VerifyConnectivity(context.Background())
		if err == nil {
			break
		}
		log.Printf("Failed to connect to Neo4j, retrying in 2 seconds... (%d/%d): %v\n", i+1, maxRetries, err)
		time.Sleep(2 * time.Second)
	}

	if err != nil {
		log.Fatal("Failed to connect to Neo4j after retries: ", err)
	}

	log.Println("Connected to Neo4j successfully")

	followRepository := repository.NewFollowRepository(neo4jDriver)

	followService := service.NewFollowService(followRepository)

	followHandler := handler.NewFollowHandler(followService)

	http.HandleFunc("/health", func(w http.ResponseWriter, r *http.Request) {
		w.WriteHeader(http.StatusOK)
		w.Write([]byte("Follow service is running"))
	})

	http.HandleFunc("/follow", middleware.EnableCors(followHandler.FollowUser))

	http.HandleFunc("/is-following", middleware.EnableCors(followHandler.IsFollowing))

	http.HandleFunc("/following", middleware.EnableCors(followHandler.GetFollowing))

	http.HandleFunc("/recommendations", middleware.EnableCors(followHandler.GetRecommendations))

	http.HandleFunc("/profiles", middleware.EnableCors(followHandler.GetAllProfilesWithFollowStatus))

	log.Println("Follow service started on port " + cfg.Port)

	internal.StartGrpcServer(followService)

	err = http.ListenAndServe(":"+cfg.Port, nil)
	if err != nil {
		log.Fatal(err)
	}
}
