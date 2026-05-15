package config

import "os"

type Config struct {
	Port          string
	Neo4jUri      string
	Neo4jUsername string
	Neo4jPassword string
}

func LoadConfig() Config {
	return Config{
		Port:          getEnv("PORT", "8083"),
		Neo4jUri:      getEnv("NEO4J_URI", "neo4j://127.0.0.1:7687"),
		Neo4jUsername: getEnv("NEO4J_USERNAME", "neo4j"),
		Neo4jPassword: getEnv("NEO4J_PASSWORD", "password123"),
	}
}

func getEnv(key string, defaultValue string) string {
	value := os.Getenv(key)

	if value == "" {
		return defaultValue
	}

	return value
}
