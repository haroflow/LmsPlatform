import { Lesson } from "./Lesson";

export interface CourseModule {
	id: string;
	title: string;
	slug: string;
	lessons: Lesson[];
}