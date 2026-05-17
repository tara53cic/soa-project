FROM golang:1.26-alpine AS builder

WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY . .

RUN go build -o follow-service ./cmd/main.go

FROM alpine:latest

WORKDIR /app

COPY --from=builder /app/follow-service .

EXPOSE 8083

CMD ["./follow-service"]