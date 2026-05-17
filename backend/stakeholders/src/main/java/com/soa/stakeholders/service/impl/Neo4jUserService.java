package com.soa.stakeholders.service.impl;

import com.soa.stakeholders.model.User;
import com.soa.stakeholders.repository.UserRepository;
import org.neo4j.driver.Driver;
import org.neo4j.driver.Session;
import org.neo4j.driver.Values;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class Neo4jUserService {
    private final Driver driver;
    private final UserRepository userRepository;

    public Neo4jUserService(Driver driver, UserRepository userRepository) {
        this.driver = driver;
        this.userRepository = userRepository;
    }

    @EventListener(ApplicationReadyEvent.class)
    public void syncUsersToNeo4j() {
        List<User> users = userRepository.findAll();
        try (Session session = driver.session()) {
            for (User user : users) {
                session.executeWrite(tx -> {
                    tx.run("MERGE (u:User {username: $username})", 
                            Values.parameters("username", user.getUsername()));
                    return null;
                });
            }
        } catch (Exception e) {
            System.err.println("Failed to synchronize users to Neo4j: " + e.getMessage());
        }
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
