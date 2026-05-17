FROM golang:1.26-alpine AS builder

WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

RUN apk add --no-cache protobuf
RUN go install google.golang.org/protobuf/cmd/protoc-gen-go@v1.33 && go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@v1.3
COPY . .
RUN protoc --go_out=. --go-grpc_out=. protos/block_saga.proto

COPY . .

RUN go build -o follow-service ./cmd/main.go

FROM alpine:latest

WORKDIR /app

COPY --from=builder /app/follow-service .

EXPOSE 8083

CMD ["./follow-service"]