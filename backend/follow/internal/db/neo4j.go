package db

import (
	"follow/internal/config"

	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

func NewNeo4jDriver(cfg config.Config) (neo4j.DriverWithContext, error) {
	driver, err := neo4j.NewDriverWithContext(
		cfg.Neo4jUri,
		neo4j.BasicAuth(cfg.Neo4jUsername, cfg.Neo4jPassword, ""),
	)

	if err != nil {
		return nil, err
	}

	return driver, nil
}
