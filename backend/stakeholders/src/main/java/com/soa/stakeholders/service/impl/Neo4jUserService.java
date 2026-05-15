package com.soa.stakeholders.service.impl;

import org.neo4j.driver.Driver;
import org.neo4j.driver.Session;
import org.neo4j.driver.Values;
import org.springframework.stereotype.Service;

@Service
public class Neo4jUserService {
    private final Driver driver;

    public Neo4jUserService(Driver driver) {
        this.driver = driver;
    }

    public void createUser(String username) {
        try (Session session = driver.session()) {
            session.executeWrite(tx -> {
                tx.run(
                        "MERGE (u:User {username: $username})",
                        Values.parameters("username", username)
                );
                return null;
            });
        }
    }
}
