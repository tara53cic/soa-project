package com.soa.stakeholders.service.impl;

import com.soa.stakeholders.service.BlockSagaGrpcService;
import com.soa.stakeholders.service.UserService;
import org.springframework.stereotype.Service;

@Service
public class BlockSagaGrpcServiceImpl implements BlockSagaGrpcService {

    private final UserService userService;

    public BlockSagaGrpcServiceImpl(UserService userService) {
        this.userService = userService;
    }

    @Override
    public void executeBlockSync(Long userId, boolean isBlocked) {
        userService.toggleUserBlockAndSync(userId, isBlocked);
    }
}