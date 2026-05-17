package com.soa.stakeholders.service.impl;

import com.soa.stakeholders.service.BlockSagaGrpcService;
import com.soa.stakeholders.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.lang.reflect.Method;

@Service
public class BlockSagaGrpcServiceImpl implements BlockSagaGrpcService {

    @Autowired
    private UserService userService;

    @Override
    public void executeBlockSync(String username, boolean isBlocked) {
        try {
            Long userId = Long.parseLong(username);
            userService.toggleUserBlockAndSync(userId, isBlocked);
        } catch (Exception e) {
            throw new RuntimeException("Failed to update user block status in Postgres: " + e.getMessage());
        }
    }

    public void syncUserBlockStatus(Object request, Object responseObserver) {
        try {
            Method getUsernameMethod = request.getClass().getMethod("getUsername");
            String username = (String) getUsernameMethod.invoke(request);

            Method getIsBlockedMethod = request.getClass().getMethod("getIsBlocked");
            boolean isBlocked = (boolean) getIsBlockedMethod.invoke(request);

            executeBlockSync(username, isBlocked);

            Class<?> responseClass = Class.forName("com.soa.stakeholders.grpc.BlockStatusResponse");
            Method newBuilderMethod = responseClass.getMethod("newBuilder");
            Object builder = newBuilderMethod.invoke(null);

            Method setSuccessMethod = builder.getClass().getMethod("setSuccess", boolean.class);
            builder = setSuccessMethod.invoke(builder, true);

            Method setMessageMethod = builder.getClass().getMethod("setMessage", String.class);
            builder = setMessageMethod.invoke(builder, "Postgres successfully updated");

            Method buildMethod = builder.getClass().getMethod("build");
            Object response = buildMethod.invoke(builder);

            Method onNextMethod = responseObserver.getClass().getMethod("onNext", Object.class);
            onNextMethod.invoke(responseObserver, response);

            Method onCompletedMethod = responseObserver.getClass().getMethod("onCompleted");
            onCompletedMethod.invoke(responseObserver);

        } catch (Exception e) {
            try {
                Class<?> responseClass = Class.forName("com.soa.stakeholders.grpc.BlockStatusResponse");
                Method newBuilderMethod = responseClass.getMethod("newBuilder");
                Object builder = newBuilderMethod.invoke(null);

                Method setSuccessMethod = builder.getClass().getMethod("setSuccess", boolean.class);
                builder = setSuccessMethod.invoke(builder, false);

                Method setMessageMethod = builder.getClass().getMethod("setMessage", String.class);
                builder = setMessageMethod.invoke(builder, "Postgres update failed: " + e.getMessage());

                Method buildMethod = builder.getClass().getMethod("build");
                Object response = buildMethod.invoke(builder);

                Method onNextMethod = responseObserver.getClass().getMethod("onNext", Object.class);
                onNextMethod.invoke(responseObserver, response);

                Method onCompletedMethod = responseObserver.getClass().getMethod("onCompleted");
                onCompletedMethod.invoke(responseObserver);
            } catch (Exception rollbackError) {
                System.out.println("SAGA critical error: " + rollbackError.getMessage());
            }
        }
    }
}