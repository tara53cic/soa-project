package com.soa.stakeholders.service;

public interface BlockSagaGrpcService {
    void executeBlockSync(String username, boolean isBlocked);
}