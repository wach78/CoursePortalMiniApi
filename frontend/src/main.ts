import { fetchCourses, fetchCourse } from "./service/courseService";

import type { CourseResponseDto } from "./interfaces/CourseResponseDto";
import { CourseLevelLabels } from "./constants/CourseLevel";

type CourseSortBy = "price" | "level" | "name";
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
    direction: SortDirection = "asc",
): Promise<void> => {
    const courses = await fetchCourses(sortBy, direction);

    coursesList.replaceChildren();

    coursesList.classList.add(
        "flex",
        "flex-wrap",
        "gap-5",
        "p-2",
        "bg-gray-800",
    );

    courses.forEach((course) => {
        const courseInfo = document.createElement("li");
        courseInfo.classList.add(
            "bg-gray-200",
            "text-black",
            "p-5",
            "w-90",
            "rounded-lg",
            "flex",
            "text-center",
            "justify-center",
            "flex-col",
            "align-items",
        );

        const courseName = document.createElement("h2");
        courseName.textContent = course.name;
        courseName.classList.add("text-xl", "font-bold", "mb-2");

        const infoDiv = document.createElement("div");
        infoDiv.classList.add("flex", "gap-5", "mx-auto");

        const spanLevel = document.createElement("span");
        const spanPrice = document.createElement("span");

        spanLevel.textContent = `Nivå: ${CourseLevelLabels[course.level]}`;
        spanPrice.textContent = `Pris: ${String(course.price)} kr`;

        spanLevel.classList.add("text-sm");
        spanPrice.classList.add("text-sm");
        infoDiv.append(spanLevel, spanPrice);

        const detailsButton = document.createElement("button");
        detailsButton.type = "button";
        detailsButton.classList.add("text-blue-700", "underline");
        detailsButton.textContent = "Läs mer";

        detailsButton.addEventListener("click", async () => {
            const selectedCourse = await fetchCourse(course.id);

            showCourseDetails(selectedCourse);
        });

        courseInfo.append(courseName, infoDiv, detailsButton);
        coursesList.appendChild(courseInfo);
    });
};

const showCourseDetails = (course: CourseResponseDto): void => {
    courseDetails.replaceChildren();

    courseDetails.classList.add("m-2");

    const heading = document.createElement("h2");
    heading.textContent = course.name;
    heading.classList.add("font-bold", "text-2xl");

    const description = document.createElement("p");
    description.textContent = course.description;

    const spanStart = document.createElement("span");
    const spanLength = document.createElement("span");
    const spanLevel = document.createElement("span");
    const spanPrice = document.createElement("span");

    spanStart.textContent = `Start: ${course.startDate}`;
    spanLength.textContent = `Längd: ${String(course.durationInWeeks)} veckor`;
    spanLevel.textContent = `Nivå: ${CourseLevelLabels[course.level]}`;
    spanPrice.textContent = `Pris: ${String(course.price)} kr`;

    spanStart.classList.add("font-bold");
    spanLength.classList.add("font-bold");
    spanLevel.classList.add("font-bold");
    spanPrice.classList.add("font-bold");

    const infoDiv = document.createElement("div");
    infoDiv.classList.add("flex", "gap-5");

    infoDiv.append(spanStart, spanLength, spanLevel, spanPrice);

    const applyButton = document.createElement("button");

    applyButton.type = "button";
    applyButton.textContent = "Anmäl dig nu";
    applyButton.addEventListener("click", () => {
        alert("Du är anmäld till kurs: " + course.name);
    });
    applyButton.classList.add(
        "p-3",
        "bg-gray-950",
        "text-white",
        "rounded-lg",
        "hover:bg-gray-900",
        "hover:cursor-pointer",
        "max-w-[20%]",
    );

    courseDetails.append(heading, description, infoDiv, applyButton);
};

await loadCourses("name", "asc");

courseSortSelect.addEventListener("change", async () => {
    const selectedValue = courseSortSelect.value;

    if (selectedValue === "") {
        await loadCourses();
        return;
    }

    const parts = selectedValue.split(":");

    if (parts.length !== 2) {
        console.error("Invalid sorting format:", selectedValue);
        await loadCourses();
        return;
    }

    const [sortBy, direction] = parts;

    if (!isCourseSortBy(sortBy) || !isSortDirection(direction)) {
        console.error("Invalid sorting value:", selectedValue);
        await loadCourses();
        return;
    }

    await loadCourses(sortBy, direction);
});

function isCourseSortBy(value: string): value is CourseSortBy {
    return value === "price" || value === "level" || value === "name";
}

function isSortDirection(value: string): value is SortDirection {
    return value === "asc" || value === "desc";
}
