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



const coursesList = getRequiredElement<HTMLUListElement>("coursesList");

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

        const link = document.createElement("a");
        link.classList.add('text-blue-500', 'underline');
        link.textContent = '[Läs mer]';

        courseInfo.appendChild(link);


        coursesList.appendChild(courseInfo);
    });

};

await loadCourses('level', 'desc');
/*

 <li class="bg-gray-200 text-black p-5 rounded-full flex flex-col">
            Webbutveckling från grunden
            <a class="text-blue-500 underline">[Läs mer]</a>
          </li>
*/

/*

const getCourse = async (): Promise<void> => {

};

*/
/*

<select id="course-sort">
    <option value="">Default sorting</option>
    <option value="price:asc">Price: lowest first</option>
    <option value="price:desc">Price: highest first</option>
    <option value="level:asc">Level: beginner first</option>
    <option value="level:desc">Level: advanced first</option>
</select>

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
