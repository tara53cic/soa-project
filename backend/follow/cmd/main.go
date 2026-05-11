package main

import (
	"context"
	"follow/internal/config"
	"follow/internal/db"
	"log"
	"net/http"
)

func main() {
	cfg := config.LoadConfig()

	neo4jDriver, err := db.NewNeo4jDriver(cfg)
	if err != nil {
		log.Fatal("Failed to create Neo4j driver: ", err)
	}
	defer neo4jDriver.Close(context.Background())

	err = neo4jDriver.VerifyConnectivity(context.Background())
	if err != nil {
		log.Fatal("Failed to connect to Neo4j: ", err)
	}

	log.Println("Connected to Neo4j successfully")

	http.HandleFunc("/health", func(w http.ResponseWriter, r *http.Request) {
		w.WriteHeader(http.StatusOK)
		w.Write([]byte("Follow service is running"))
	})

	log.Println("Follow service started on port " + cfg.Port)

	err = http.ListenAndServe(":"+cfg.Port, nil)
	if err != nil {
		log.Fatal(err)
	}
}
