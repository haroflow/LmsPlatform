import { CourseModule } from "./CourseModule";

export interface Course {
	id: string;
	name: string;
	slug: string;
	description: string;
	miniatureImage: string;
	bannerImage: string;
	modules: CourseModule[];
	userEnrolled: boolean;
}