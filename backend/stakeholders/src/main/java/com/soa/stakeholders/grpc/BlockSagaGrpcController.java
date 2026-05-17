package com.soa.stakeholders.grpc;

import com.soa.stakeholders.service.BlockSagaGrpcService;
import io.grpc.stub.StreamObserver;
import com.soa.stakeholders.grpc.BlockStatusRequest;
import com.soa.stakeholders.grpc.BlockStatusResponse;
import com.soa.stakeholders.grpc.BlockSagaGrpcServiceGrpc;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Service;

@Component
public class BlockSagaGrpcController
        extends BlockSagaGrpcServiceGrpc.BlockSagaGrpcServiceImplBase {

    private final BlockSagaGrpcService blockService;

    public BlockSagaGrpcController(BlockSagaGrpcService blockService) {
        this.blockService = blockService;
    }

    @Override
    public void syncUserBlockStatus(
            BlockStatusRequest request,
            StreamObserver<BlockStatusResponse> responseObserver) {

        try {
            Long userId = request.getUserId();

            blockService.executeBlockSync(
                    userId,
                    request.getIsBlocked()
            );

            BlockStatusResponse response = BlockStatusResponse.newBuilder()
                    .setSuccess(true)
                    .setMessage("OK")
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();

        } catch (Exception e) {

            BlockStatusResponse response = BlockStatusResponse.newBuilder()
                    .setSuccess(false)
                    .setMessage("Error: " + e.getMessage())
                    .build();

            responseObserver.onNext(response);
            responseObserver.onCompleted();
        }
    }
}