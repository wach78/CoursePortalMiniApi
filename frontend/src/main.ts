import { fetchCourses, fetchCourse } from "./service/courseService";

import type { CourseResponseDto } from "./interfaces/CourseResponseDto";
import { CourseLevelLabels } from "./constants/CourseLevel";

type CourseSortBy = "price" | "level" | "name" | "date";
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
const contactForm = getRequiredElement<HTMLFormElement>("contact-form");
const modal = getRequiredElement<HTMLDialogElement>("modal");
const modalClose = getRequiredElement<HTMLButtonElement>("closeModal");

contactForm.addEventListener("submit", (event) => {
    event.preventDefault();
    const formData = new FormData(contactForm);
    console.log(formData.get("name"));
    console.log("Formulär skickat!");
});

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
            "relative",
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
        detailsButton.classList.add(
            "text-blue-700",
            "underline",
            "hover:cursor-pointer",
            "text-xl",
        );
        detailsButton.textContent = "Läs mer";

        detailsButton.addEventListener("click", async () => {
            const selectedCourse = await fetchCourse(course.id);
            document
                .getElementById("course-view")
                ?.scrollIntoView({ behavior: "smooth" });
            showCourseDetails(selectedCourse);
        });

        const newBadge = document.createElement("span");
        newBadge.textContent = "NYHET!";
        newBadge.classList.add(
            "bg-orange-500",
            "rotate-25",
            "absolute",
            "rounded-full",
            "w-1/4",
            "top-1",
            "-right-3",
            "z-50",
        );

        const creationDate = new Date(course.createdAt);
        const today = new Date();

        const monthInMilli = 30 * 24 * 60 * 60 * 1000;
        const age = today.getTime() - creationDate.getTime();

        if (age <= monthInMilli) {
            console.log(course.createdAt);
            courseInfo.append(courseName, newBadge, infoDiv, detailsButton);
        } else {
            courseInfo.append(courseName, infoDiv, detailsButton);
        }

        coursesList.appendChild(courseInfo);
    });

    showCourseDetails(courses[0]);
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
    infoDiv.classList.add("flex", "gap-5", "justify-center");

    infoDiv.append(spanStart, spanLength, spanLevel, spanPrice);

    const applyButton = document.createElement("button");

    applyButton.type = "button";
    applyButton.textContent = "Anmäl dig nu";
    applyButton.addEventListener("click", () => {
        modal.showModal();
        modal.classList.add(
            "bg-gray-800",
            "rounded-lg",
            "flex",
            "flex-col",
            "justify-between",
            "text-white",
            "text-center",
            "top-1/2",
            "left-1/2",
            "-translate-y-1/2",
            "-translate-x-1/2",
            "w-100",
            "h-50",
            "p-5",
            "shadow-lg",
        );
        const modalHeading =
            getRequiredElement<HTMLParagraphElement>("modalHeading");
        const modalTextBody =
            getRequiredElement<HTMLParagraphElement>("modalTextBody");
        modalHeading.textContent = course.name;
        modalTextBody.textContent = "Nu är du anmäld till kursen!";
        // alert("Du är anmäld till kurs: " + course.name);
    });
    applyButton.classList.add(
        "p-3",
        "mx-auto",
        "bg-gray-950",
        "text-white",
        "rounded-lg",
        "hover:bg-gray-900",
        "hover:cursor-pointer",
        "lg:w-1/5",
        "w-1/2",
    );

    courseDetails.append(heading, description, infoDiv, applyButton);
};

await loadCourses("date", "desc");

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

modalClose.addEventListener("click", () => {
    modal.close();
    modal.classList.remove(
        "bg-gray-800",
        "rounded-lg",
        "flex",
        "flex-col",
        "justify-between",
        "text-white",
        "text-center",
        "top-1/2",
        "left-1/2",
        "-translate-y-1/2",
        "-translate-x-1/2",
        "w-100",
        "h-50",
        "p-5",
        "shadow-lg",
    );
});
const btn = getRequiredElement<HTMLButtonElement>("easterEgg");
btn.addEventListener("click", () => {
    const sections = document.querySelectorAll("section");

    sections.forEach((section) => {
        section.classList.add("animate-spin");
    });
});

function isCourseSortBy(value: string): value is CourseSortBy {
    return (
        value === "price" ||
        value === "level" ||
        value === "name" ||
        value === "date"
    );
}

function isSortDirection(value: string): value is SortDirection {
    return value === "asc" || value === "desc";
}
