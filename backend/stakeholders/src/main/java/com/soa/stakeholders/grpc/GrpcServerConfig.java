package com.soa.stakeholders.grpc;

import io.grpc.Server;
import io.grpc.ServerBuilder;
import org.springframework.context.annotation.Configuration;

import jakarta.annotation.PostConstruct;
import jakarta.annotation.PreDestroy;

import java.io.IOException;

@Configuration
public class GrpcServerConfig {

    private Server server;

    private final BlockSagaGrpcController blockSagaGrpcController;

    public GrpcServerConfig(BlockSagaGrpcController blockSagaGrpcController) {
        this.blockSagaGrpcController = blockSagaGrpcController;
    }

    @PostConstruct
    public void start() throws IOException {
        server = ServerBuilder
                .forPort(9090)
                .addService(blockSagaGrpcController)
                .build()
                .start();

        System.out.println("gRPC server started on port 9090");
    }

    @PreDestroy
    public void stop() {
        if (server != null) {
            server.shutdown();
        }
    }
}