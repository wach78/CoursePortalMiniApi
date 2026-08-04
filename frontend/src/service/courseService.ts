import type { CourseRequestDto } from "../interfaces/CourseRequestDto";
import type { CourseResponseDto } from "../interfaces/CourseResponseDto";
import { API_URL, HttpMethod, JSON_HEADERS } from "../constants/constants";

type CourseSortBy = "price" | "level" | "name" | "date";
type SortDirection = "asc" | "desc";

export const fetchCourses = async (
    sortBy?: CourseSortBy,
    direction: SortDirection = "asc"
): Promise<CourseResponseDto[]> => {
    const url = new URL(API_URL);

    if (sortBy) {
        url.searchParams.set("sortBy", sortBy);
        url.searchParams.set("direction", direction);
    }

    const response = await fetch(url);

    if (!response.ok) {
        throw new Error(
            `Failed to fetch courses: ${response.status}`
        );
    }

    return await response.json() as CourseResponseDto[];
};

export const fetchCourse = async (id: number): Promise<CourseResponseDto> => {
    const response = await fetch(`${API_URL}/${id}`);

    if (!response.ok) {
        throw new Error(`Kunde inte hämta kursen: ${response.status}`);
    }

    return await response.json() as CourseResponseDto;
};

export const createCourse = async (
    newCourse: CourseRequestDto
): Promise<CourseResponseDto> => {
    const response = await fetch(API_URL, {
        method: HttpMethod.POST,
        headers: JSON_HEADERS,
        body: JSON.stringify(newCourse)
    });

    if (!response.ok) {
        throw new Error(`Kunde inte skapa kursen: ${response.status}`);
    }

    return await response.json() as CourseResponseDto;
};

export const deleteCourse = async (id: number): Promise<void> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: HttpMethod.DELETE
    });

    if (!response.ok) {
        throw new Error(`Kunde inte ta bort kursen: ${response.status}`);
    }
};

export const updateCourse = async (
    id: number,
    editCourse: CourseRequestDto
): Promise<number> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: HttpMethod.PUT,
        headers: JSON_HEADERS,
        body: JSON.stringify(editCourse)
    });

    if (!response.ok) {
        throw new Error(`Kunde inte uppdatera kursen: ${response.status}`);
    }

    return response.status;
};
