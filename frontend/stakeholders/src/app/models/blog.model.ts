export interface Blog {
  id: string;
  title: string;
  description: string;
  descriptionHtml: string;
  createdAt: string;
  authorId: number;
  authorUsername?: string;
}

export interface CreateBlogRequest {
  title: string;
  description: string;
  authorId: number;
  imageUrls: string[];
}