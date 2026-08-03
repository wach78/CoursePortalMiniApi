import './style.css'


import {
    fetchCourses,
    fetchCourse
} from "./service/courseService";

type CourseSortBy = "price" | "level";
type SortDirection = "asc" | "desc";

const getRequiredElement = <T extends HTMLElement>(id: string): T => {
    const element = document.getElementById(id);

    if (!element) {
        throw new Error(`Elementet #${id} hittades inte.`);
    }

    return element as T;
};

const loadCourses = async (
    sortBy?: CourseSortBy,
    direction: SortDirection = "asc"
): Promise<void> => {
    const courses = await fetchCourses(sortBy, direction);

    courses.forEach(course => {



    });

};

/*

const getCourse = async (): Promise<void> => {

};

*/
/*
const courseSortSelect =
    document.querySelector<HTMLSelectElement>("#course-sort");

if (!courseSortSelect) {
    throw new Error("Course sorting element was not found.");
}

const selectedSort = courseSortSelect.value;

if (selectedSort === "") {
    await loadCourses();
} else {
    const [sortBy, direction] = selectedSort.split(":");

    await loadCourses(
        sortBy as CourseSortBy,
        direction as SortDirection
    );
}
*/
