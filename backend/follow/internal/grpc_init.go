package internal

import (
	"context"
	"log"
	"net"

	"follow/internal/grpc/block_saga"
	"follow/internal/service"

	"google.golang.org/grpc"
)

type BlockSagaGrpcServer struct {
	block_saga.UnimplementedBlockSagaGrpcServiceServer
	followService service.FollowService
}

func StartGrpcServer(followService service.FollowService) {
	listener, err := net.Listen("tcp", ":8084")
	if err != nil {
		log.Fatalf("failed to start listener on port 8084: %v", err)
	}

	grpcServer := grpc.NewServer()

	server := &BlockSagaGrpcServer{
		followService: followService,
	}

	block_saga.RegisterBlockSagaGrpcServiceServer(grpcServer, server)

	go func() {
		log.Println("gRPC server started successfully on port 8084")
		if err := grpcServer.Serve(listener); err != nil {
			log.Fatalf("failed to serve gRPC server: %v", err)
		}
	}()
}

func (s *BlockSagaGrpcServer) SyncUserBlockStatus(
	ctx context.Context,
	req *block_saga.BlockStatusRequest,
) (*block_saga.BlockStatusResponse, error) {

	err := s.followService.SetUserBlockStatus(req.GetUserId(), req.GetIsBlocked())
	if err != nil {
		return &block_saga.BlockStatusResponse{
			Success: false,
			Message: err.Error(),
		}, nil
	}

	return &block_saga.BlockStatusResponse{
		Success: true,
		Message: "Neo4j successfully updated through saga",
	}, nil
}
