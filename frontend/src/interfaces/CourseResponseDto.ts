import type { CourseLevelValue } from "../constants/CourseLevel";

export interface CourseResponseDto {
    id: number;
    name: string;
    description: string;
    startDate: string;
    durationInWeeks: number;
    price: number;
    level: CourseLevelValue;
}
