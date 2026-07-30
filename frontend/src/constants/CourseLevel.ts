export const CourseLevel = {
    Beginner: 0,
    Intermediate: 1,
    Advanced: 2
} as const;

export type CourseLevelValue =
    typeof CourseLevel[keyof typeof CourseLevel];


export const CourseLevelLabels = {
    [CourseLevel.Beginner]: "Nybörjare",
    [CourseLevel.Intermediate]: "Medel",
    [CourseLevel.Advanced]: "Avancerad"
} satisfies Record<CourseLevelValue, string>;

export const getCourseLevelLabel = (level: number): string => {
    return CourseLevelLabels[level as CourseLevelValue] ?? "Okänd nivå";
};
