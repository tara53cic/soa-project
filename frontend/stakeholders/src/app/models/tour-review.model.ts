export interface TourReview {
    id?: number;
    grade: number;
    comment: string;
    touristId: number;
    tourId: number;
    attendanceDate: string;
    reviewDate?: string;
    images?: string[];
    imageFiles?: File[];
    touristUsername?: string;
}