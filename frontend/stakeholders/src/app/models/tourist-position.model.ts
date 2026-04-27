export interface TouristPosition {
    id?: number;
    touristId: number;
    latitude: number;
    longitude: number;
    lastActivity?: Date;
}