package com.soa.stakeholders.service;

public interface BlockSagaGrpcService {
    void executeBlockSync(Long userId, boolean isBlocked);
}