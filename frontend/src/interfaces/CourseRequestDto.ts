import type { CourseLevelValue } from "../constants/CourseLevel";

export interface CourseRequestDto {
    name: string;
    description: string;
    startDate: string;
    durationInWeeks: number;
    price: number;
    level: CourseLevelValue;
}
