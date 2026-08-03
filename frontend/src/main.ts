
import {
    fetchCourses,
    fetchCourse
} from "./service/courseService";

import type { CourseResponseDto } from "./interfaces/CourseResponseDto";

type CourseSortBy = "price" | "level";
type SortDirection = "asc" | "desc";

const getRequiredElement = <T extends HTMLElement>(id: string): T => {
    const element = document.getElementById(id);

    if (!element) {
        throw new Error(`Elementet #${id} hittades inte.`);
    }

    return element as T;
};



const coursesList = getRequiredElement<HTMLUListElement>("coursesList");
const courseDetails = getRequiredElement<HTMLDivElement>("course-details");
const courseSortSelect = getRequiredElement<HTMLSelectElement>("course-sort");


const loadCourses = async (
    sortBy?: CourseSortBy,
    direction: SortDirection = "asc"
): Promise<void> => {
    const courses = await fetchCourses(sortBy, direction);

    coursesList.replaceChildren();

    courses.forEach(course => {

        const courseInfo = document.createElement("li");
        courseInfo.classList.add('bg-gray-200', 'text-black', 'p-5', 'rounded-full', 'flex', 'flex-col');
        courseInfo.textContent = course.name;

        const detailsButton = document.createElement("button");
        detailsButton.type = "button";
        detailsButton.classList.add("text-blue-500", "underline");
        detailsButton.textContent = "Läs mer";

        detailsButton.addEventListener("click", async () => {
            const selectedCourse = await fetchCourse(course.id);

            showCourseDetails(selectedCourse);
        });

        courseInfo.appendChild(detailsButton);
        coursesList.appendChild(courseInfo);
    });

};

const showCourseDetails = (course: CourseResponseDto): void => {
    courseDetails.replaceChildren();

    const heading = document.createElement("h2");
    heading.textContent = course.name;

    const description = document.createElement("p");
    description.textContent = course.description;

    const applyButton = document.createElement("button");

    applyButton.type = "button";
    applyButton.textContent = "Anmäl dig nu";

    applyButton.classList.add(
        "p-3",
        "bg-gray-950",
        "text-white",
        "rounded-lg",
        "hover:bg-gray-900",
        "hover:cursor-pointer",
        "max-w-[20%]"
    );

    courseDetails.append(
        heading,
        description,
        applyButton
    );
};

await loadCourses();


courseSortSelect.addEventListener("change", async () => {
    const selectedValue = courseSortSelect.value;

    if (selectedValue === "") {
        await loadCourses();
        return;
    }

    const [sortBy, direction] = selectedValue.split(":");

    await loadCourses(
        sortBy as CourseSortBy,
        direction as SortDirection
    );
});
